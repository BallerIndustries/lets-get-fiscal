using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Lets_Get_Fiscal
{
    public class CollisionManager
    {
        public List<Collision> collisions = new List<Collision>(50);
        //Things to poll
        private Ego ego;
        private List<BadGuy> bad_guys;
        private List<Projectile> projectiles;
        private List<Flourish> flourishes;
        private List<Item> items;
        private TextureManager tm;
        private List<Prop> props;
        private List<BasicGameObject> all_gameobjects;
        private SoundBank soundBank;
        private Camera camera;
        private ProjectileManager pm;
        public HUD hud;
        const int MIN_BL_DIFF = 50;
        const int MIN_PROP_BL_DIFF = 20;
        public int min_y;
        public bool KO_all = false; //If set to true, all the bad guys will be knocked to the ground.
        public bool KILL_all = false;

        public void Initialise(int min_y)
        {
            KO_all = false;
            this.min_y = min_y;
        }

        public CollisionManager(Ego ego, List<BadGuy> bad_guys, List<Projectile> projectiles, List<Prop> props, SoundBank soundBank, Camera camera, ProjectileManager pm, TextureManager tm, List<Flourish> flourishes, HUD hud, List<Item> items, List<BasicGameObject> all_gameobjects)
        {
            this.ego = ego;
            this.bad_guys = bad_guys;
            this.props = props;
            this.projectiles = projectiles;
            this.soundBank = soundBank;
            this.camera = camera;
            this.pm = pm;
            this.tm = tm;
            this.flourishes = flourishes;
            this.hud = hud;
            this.items = items;
            this.all_gameobjects = all_gameobjects;
        }

        public Rectangle get_random_location(Random r, Rectangle bg_pos)
        {
            //We want to return a random position within the bad guys half of the screen
            //As splitted by the accountant
            int x, y, ego_x;

            ego_x = (int)MathHelper.Clamp(ego.position.Center.X, camera.viewport_rect.Left, camera.viewport_rect.Right);

            if (ego_x > bg_pos.X)
                x = r.Next(camera.viewport_rect.Left, ego_x);
            else
                x = r.Next(ego_x, camera.viewport_rect.Right);

            y = r.Next(min_y, camera.viewport_rect.Bottom);

            return new Rectangle(x, y, 1, 1);
        }

        public Rectangle get_mid_screen_location(Random r)
        {
            int x, y;

            x = r.Next(camera.viewport_rect.Left + 240, camera.viewport_rect.Right - 240);
            y = r.Next(min_y, camera.viewport_rect.Bottom);

            return new Rectangle(x, y, 1, 1);
        }

        //Add a Collision object to Character and Projectile classes.
        public void get_collisions()
        {
            collisions.Clear();

            if (ego.punch_rect != Collision.Empty)
                collisions.Add(ego.punch_rect);

            foreach (BadGuy bg in bad_guys)
            {
                if (bg.punch_rect != null && bg.hp > 0)
                    collisions.Add(bg.punch_rect);
            }

            collisions.AddRange(pm.get_collisions());
        }

        public void manage_badguys()
        {
            if (KO_all)
            {
                KO_all = false;

                foreach (BadGuy bg in bad_guys)
                {
                    if (bg.state != BadGuy.State.dead && bg.state != GameObject.State.vertical_spin && bg.state != GameObject.State.horizontal_spin)
                    {
                        bg.attacked_from_left = bg.facing == GameObject.Direction.right;
                        bg.start_KO_leap();
                    }
                }
            }

            if (KILL_all)
            {
                KILL_all = false;

                foreach (BadGuy bg in bad_guys)
                {
                    if (bg.state != BadGuy.State.death_leap && bg.state != GameObject.State.dead)
                    {
                        bg.attacked_from_left = bg.facing == GameObject.Direction.right;
                        bg.start_death_leap();
                        bg.hp = 0;
                    }
                }
            }
        }

        public static Rectangle scale_rect(Rectangle r, float width_perc, float height_perc)
        {
            int new_width = (int)(r.Width * width_perc);
            int new_height = (int)(r.Height * height_perc);

            r.X += (r.Width - new_width) / 2;
            r.Y += (r.Height - new_height) / 2;
            r.Width = new_width;
            r.Height = new_height;

            return r;
        }

        public bool touching_ego(BadGuy bg)
        {
            return (bg.position.Intersects(scale_rect(ego.position, 0.15f, 1.0f)));
        }

        //Returns the direction the bad guy should face to 
        //face the ego
        public Character.Direction face_ego(int bg_x)
        {
            if (bg_x < ego.posX)
                return GameObject.Direction.right;
            else
                return GameObject.Direction.left;
        }

        public bool position_ok(Character c, Rectangle new_position)
        {
            Ego e = c as Ego;

            //Bad guys cannot clip eachother, but they can walk through the viewport.
            if (e == null)
            {
                BadGuy current_bg = c as BadGuy;
                Rectangle shrunk_ego_pos = scale_rect(ego.position, 0.10f, 1.0f);

                //Check if the bad guys are on top of eachother.
                foreach (BadGuy bg in bad_guys)
                {
                    if (current_bg.correcting_path == false && bg.id != current_bg.id && bg.hp > 0 && baseline_check(bg.baseline, current_bg.baseline) && bg.state != BadGuy.State.KO && same_dest_check(current_bg, bg) && bg.ai_state != BadGuy.AiState.kill_kill_kill && current_bg.ai_state != BadGuy.AiState.kill_kill_kill)
                    {
                        Character.MoveDirection opp_dir = get_opposite_direction(calc_move_dir(current_bg));
                        current_bg.set_destination(opp_dir, min_y);
                    }
                }

                if (shrunk_ego_pos.Intersects(new_position) && c.state == GameObject.State.walking)
                {
                    return false;
                }
            }

            //Ego has to stay within the viewport. HAR HAR
            if (e != null)
            {
                //Don't collide with bad guys.
                foreach (BadGuy bg in bad_guys)
                {
                    if (bg.id != c.id && bg.position.Intersects(new_position) && bg.hp > 0 && baseline_check(bg.baseline, e.new_baseline) && bg.state != GameObject.State.KO)
                    {
                        Rectangle intersect_rect = Rectangle.Intersect(c.new_position, bg.position);
                        if ((intersect_rect.Width * intersect_rect.Height) / (float)(bg.position.Width * bg.position.Height) > bg.grab_amount && ego.state == Ego.State.walking)
                        {
                            do_grab(ego, bg);
                            return false;
                        }
                    }
                }
                
                if (ego.new_baseline < min_y)
                    return false;

                Rectangle shrunk_ego_pos = scale_rect(ego.new_position, 0.4f, 1.0f);

                //If jumping or jump_kicking the Y Position should be jumpY;
                if (ego.state == GameObject.State.jumping || ego.state == GameObject.State.jump_kick1)
                    shrunk_ego_pos.Y = ego.jumpY - shrunk_ego_pos.Height;

                foreach (Prop p in props)
                {
                    if (p.state == GameObject.State.in_tact && prop_baseline_check(p.baseline, ego.new_baseline) && shrunk_ego_pos.Intersects(p.position))
                        return false;
                }
                
                if (camera.viewport_rect.Contains(shrunk_ego_pos) == false)
                {
                    ego.end_animation = true;
                    return false;
                }
            }

            return true;
        }

        public bool same_dest_check(BadGuy bg1, BadGuy bg2)
        {
            Character.MoveDirection bg1_dir = calc_move_dir(bg1);
            Character.MoveDirection bg2_dir = calc_move_dir(bg2);

            if (bg1_dir == bg2_dir)
                return true;
            else
                return false;
        }


        public Character.MoveDirection get_opposite_direction(Character.MoveDirection dir)
        {
            switch (dir)
            {
                case Character.MoveDirection.east:
                    return Character.MoveDirection.west;

                case Character.MoveDirection.west:
                    return Character.MoveDirection.east;

                case Character.MoveDirection.north:
                    return Character.MoveDirection.south;

                case Character.MoveDirection.south:
                    return Character.MoveDirection.north;

                default:
                    return Character.MoveDirection.none;
            }
        }

        public Character.MoveDirection calc_move_dir(BadGuy bg)
        {
            int x_delta, y_delta;
            x_delta = bg.posX - bg.destination.X;
            y_delta = bg.posY - bg.destination.Y;

            if (Math.Abs(x_delta) > Math.Abs(y_delta))
            {
                //BG1 is going left or right
                if (x_delta > 0)
                    return Character.MoveDirection.west;
                else
                    return Character.MoveDirection.east;
            }
            else
            {
                //BG1 is going up or down
                if (y_delta > 0)
                    return Character.MoveDirection.north;
                else
                    return Character.MoveDirection.south;
            }
        }

        public void do_grab(Character grabber, Character grabbed)
        {
            BadGuy bg;

            if (grabbed.sheet == "spritesheets//guido")
            {
                Guido guido = grabbed as Guido;

                soundBank.PlayCue("guido_throw");

                if (guido.facing == ego.facing)
                {
                    if (guido.facing == GameObject.Direction.left)
                        guido.facing = GameObject.Direction.right;
                    else
                        guido.facing = GameObject.Direction.left;
                }

                guido.state = GameObject.State.suplex;
                guido.grabbee = ego;

                ego.state = GameObject.State.being_thrown;
                
                Grab grabber_anim = guido.current_animation as Grab;
                Grab grabbed_anim = ego.current_animation as Grab;

                int dist = convert_to_world_pos(grabbed_anim, ego).X - convert_to_world_pos(grabber_anim, guido).X;

                //Snap ego to the bad guy
                guido.posY = ego.baseline;
                guido.start_suplex();
                guido.posX += dist;
            }
            else if (grabbed.sheet == "spritesheets//coral")
            {
                face_ego(grabbed.posX);
                grabbed.start_attack(GameObject.State.slide_attack);
            }
            else if (grabbed.sheet == "spritesheets//commander")
            {
                face_ego(grabbed.posX);
                grabbed.start_attack(GameObject.State.knife_slash);
            }

            //Ego is grabbing a bad guy
            else if (grabber.id == ego.id && grabbed.grabbable)
            {
                bg = grabbed as BadGuy;
                bg.grab_timer = 180;

                ego.state = GameObject.State.back_grab;
                ego.grabbee = bg;

                if (ego.weapon != null)
                {
                    items.Add(ego.weapon);
                    all_gameobjects.Add(ego.weapon);
                    ego.weapon.start_drop(ego);
                    ego.weapon = null;
                }
                bg.state = GameObject.State.grabbed;

                Grab grabber_anim = ego.current_animation as Grab;
                Grab grabbed_anim = bg.current_animation as Grab;

                int dist = convert_to_world_pos(grabbed_anim, grabbed).X - convert_to_world_pos(grabber_anim, grabber).X;

                //Snap ego to the bad guy
                grabber.posY = grabbed.baseline;
                grabber.posX += dist;
            }

            //Ego is being grabbed by a bad guy
            else
            {
                //Code this later bro.
            }
        }

        public Point convert_to_world_pos(Grab g, Character c)
        {
            Point dist_from_bound;

            //Calculate the distance from the grab point to the bound corner.

            if (g.uses_list)
            {
                dist_from_bound.X = (int)((g.grab_point.X - g.bound_list[0].X) * c.scale);
                dist_from_bound.Y = (int)((g.grab_point.Y - g.bound_list[0].Y) * c.scale);
            }
            else
            {
                dist_from_bound.X = (int)((g.grab_point.X - g.bound.X) * c.scale);
                dist_from_bound.Y = (int)((g.grab_point.Y - g.bound.Y) * c.scale);
            }
            //Add the distance to the current position
            if (c.facing == GameObject.Direction.right)
                return new Point(c.position.X + dist_from_bound.X, c.position.Y + dist_from_bound.Y);
            else
                return new Point(c.position.Right - dist_from_bound.X, c.position.Y + dist_from_bound.Y);
        }


        public void detect_collisions()
        {
            foreach (Collision cl in collisions)
            {
                switch (cl.source)
                {
                    //Collisions that can hurt EVERYONE
                    case Collision.collision_type.all:
                        check_collision(ego, cl);
                        
                        foreach (BadGuy bg in bad_guys)
                            check_collision(bg, cl);
                        break;

                    //Collisions created by a BAD GUY
                    case Collision.collision_type.bad_guy:
                        check_collision(ego, cl);
                        break;

                    //Collisions created by the EGO
                    case Collision.collision_type.ego:
                        foreach (BadGuy bg in bad_guys)
                            check_collision(bg, cl);

                        //Check if it smashed a PROP
                        foreach (Prop p in props)
                            check_collision(p, cl);
                        break;
                }
            }

            //If no sound was played for the 
            if (ego.sound_played == false && ego.punch_rect != null)
            {
                soundBank.PlayCue("missed");
                ego.sound_played = true;
            }
        }

        public void check_collision(Prop p, Collision cl)
        {
            //Should be a lot simpler than the Character version. However I haven't
            //written it yet so really I have no idea.
            if (p.position.Intersects(cl.region) && baseline_check(p.baseline, cl.baseline) && p.state == GameObject.State.in_tact)
            {
                p.start_breaking(cl.facing == GameObject.Direction.left);
                ego.sound_played = true;
                soundBank.PlayCue("glass_break");
                cl.c_source.slow_for = 5;

                if (cl.facing == GameObject.Direction.left) 
                    flourishes.Add(new Flourish("smack", tm, new Rectangle(0, 0, 30, 30), 3, new Vector2(cl.region.Left, cl.region.Top)));
                else
                    flourishes.Add(new Flourish("smack", tm, new Rectangle(0, 0, 30, 30), 3, new Vector2(cl.region.Right, cl.region.Top)));

                //Add new Item at the area where the prop was smashed.
                if (p.containee != Item.Type.none)
                {
                    if (p.containee == Item.Type.baseball_bat)
                    {
                        Weapon w = new Weapon(tm, p.containee, new Point(p.position.Center.X, p.baseline));
                        items.Add(w);
                        all_gameobjects.Add(w);
                    }
                    else
                    {
                        Item it = new Item(tm, p.containee, new Point(p.position.Center.X, p.baseline));
                        items.Add(it);
                        all_gameobjects.Add(it);
                    }
                }
            }
        }

        public void check_collision(Character c, Collision cl)
        {
            Rectangle temp_collision;

            if (c.position.Intersects(cl.region) && baseline_check(c.baseline, cl.baseline))
            {
                temp_collision = Rectangle.Intersect(cl.region, c.position);
                temp_collision = shift_rectangle(temp_collision, c);
                temp_collision = shrink_rectangle(temp_collision, c.scale);
                temp_collision = map_to_collision_sheet(temp_collision, c);

                //if (cl.is_projectile == false)
                HitData this_hit = new HitData(cl.c_source.id, cl.c_source.current_frame, cl.c_source.attack_id, cl.c_source.state);

                if (collision_check(temp_collision, c) && this_hit != c.last_hitter && cl.c_source.id != c.id)
                {
                    c.get_hit(cl.damage, cl.facing != GameObject.Direction.left, cl.does_KO);
                    c.face_collision(cl.facing);
                    soundBank.PlayCue(cl.sfx_name);
                    ego.sound_played = true;
                    cl.c_source.slow_for = 5;
                    c.last_hitter = this_hit;

                    if (cl.projectile != null)
                        cl.projectile.has_hit = true;

                    if (cl.facing == GameObject.Direction.left)
                        flourishes.Add(new Flourish("smack", tm, new Rectangle(0, 0, 30, 30), 3, new Vector2(cl.region.Left, cl.region.Top)));
                    else
                        flourishes.Add(new Flourish("smack", tm, new Rectangle(0, 0, 30, 30), 3, new Vector2(cl.region.Right, cl.region.Top)));

                    BadGuy bg = c as BadGuy;
                    if (bg != null)
                    {
                        hud.set_bad_guy_fields(bg);

                        //We hit a bad guy notify the combo list
                        ego.last_move_hit();
                        bg.change_ai_state(BadGuy.AiState.kill_kill_kill);
                    }
                    else
                    {
                        //We got hit by a bad guy
                        hud.display_bad_guy_data(cl.c_source as BadGuy);
                    }
                }
            }
        }

        public static bool baseline_check(int base1, int base2)
        {
            return Math.Abs(base1 - base2) <= MIN_BL_DIFF;
        }

        public static bool prop_baseline_check(int base1, int base2)
        {
            return Math.Abs(base1 - base2) <= MIN_PROP_BL_DIFF;
        }

        Rectangle crop_rectangle(Rectangle punch_rect, Rectangle bound_rect)
        {
            if (bound_rect.Contains(punch_rect))
            {
                return punch_rect;
            }

            if (bound_rect.Intersects(punch_rect))
            {
                if (punch_rect.Left < bound_rect.Left)
                {
                    punch_rect.Width -= (bound_rect.Left - punch_rect.Left);
                    punch_rect.X = bound_rect.Left;
                }

                if (punch_rect.Right > bound_rect.Right)
                {
                    punch_rect.Width -= (punch_rect.Right - bound_rect.Right);
                }

                if (punch_rect.Top < bound_rect.Top)
                {
                    punch_rect.Y = bound_rect.Top;
                    punch_rect.Height -= bound_rect.Top - punch_rect.Top;
                }

                if (punch_rect.Bottom > bound_rect.Bottom)
                {
                    punch_rect.Height -= (punch_rect.Bottom - bound_rect.Bottom);
                }
            }

            return punch_rect;
        }

        Rectangle shrink_rectangle(Rectangle punch_rect, float amount)
        {
            punch_rect.Width = (int)(punch_rect.Width / amount);
            punch_rect.Height = (int)(punch_rect.Height / amount);

            return punch_rect;
        }

        Rectangle shift_rectangle(Rectangle punch_rect, Character c)
        {
            if (c.facing == Character.Direction.left)
            {
                int half_way = c.position.Left + (c.position.Width / 2);
                int dist_from_edge;

                if (punch_rect.X < half_way)
                {
                    dist_from_edge = punch_rect.Left - c.position.Left;
                    punch_rect.X = c.position.Right - dist_from_edge - punch_rect.Width;
                }
                else
                {
                    dist_from_edge = c.position.Right - punch_rect.Right;
                    punch_rect.X = c.position.Left + dist_from_edge;
                }
            }

            return punch_rect;
        }

        Rectangle map_to_collision_sheet(Rectangle punch_rect, Character c)
        {
            //Calculate how far the punch rect is from bound
            int diffX, diffY;

            diffX = punch_rect.Left - c.position.X;
            diffY = punch_rect.Top - c.position.Y;

            //Reduce the difference by the scale amount
            diffX = (int)(diffX / c.scale);
            diffY = (int)(diffY / c.scale);

            punch_rect.X = c.bound.X + diffX;
            punch_rect.Y = c.bound.Y + diffY;
            
            return punch_rect;
        }

        bool collision_check(Rectangle punch_rect, Character c)
        {
            //Loop through the punch rect looking for a black pixel. If we find one,
            //then a collision happened. Or maybe it just wont work at all for some reason.
            for (int i = punch_rect.Left; i < punch_rect.Right; i++)
            {
                for (int j = punch_rect.Top; j < punch_rect.Bottom; j++)
                {
                    if (get_colour_at_point(c.collision_map, i, j, c.collision_width))
                        return true;
                }
            }

            return false;
        }

        public void calc_coll_point(int x, int y)
        {

        }

        //THIS CODE DOESN'T FUCKING WORK
        //This function checks if pixels in two seperate collision maps overlap. It is used
        //to see if we have walked into the pixel zone of another character.
        //bool collision_check(Rectangle r1, Character c1, Rectangle r2, Character c2)
        //{
        //    int overlapping_pixels = 0;
        //    int pixel_threshold = r1.Width * r1.Height / 8;
        //    //If more than 50% of the pixels are overlapping then it's a collision


        //    //If the rectangles don't have the same dimensions then fuck 'em.
        //    if (r1.Width != r2.Width || r1.Height != r2.Height)
        //        return false;

        //    for (int i = r1.Left; i < r1.Right; i++)
        //    {
        //        for (int j = r1.Top; j < r1.Bottom; j++)
        //        {
        //            bool c1_collided = get_colour_at_point(c1.collision_map, i, j, c1.collision_width);
                    
        //            //If we found a black pixel in the first collision map, check for a corresponding black pixel
        //            //in the second collision map. If we found one then a collision happened. If not, fuck that shit
        //            //continue looping.
        //            if (c1_collided)
        //            {
        //                int x_dist = i - r1.Left;
        //                int y_dist = j - r1.Top;
        //                bool c2_collided = get_colour_at_point(c2.collision_map, x_dist, y_dist, c2.collision_width);

        //                if (c2_collided)
        //                    overlapping_pixels++;

        //                if (overlapping_pixels >= pixel_threshold)
        //                    return true;
        //            }
        //        }
        //    }
        //    return false;
        //}

        public static bool get_colour_at_point(bool[] data, int x, int y, int w)
        {
            if (x < 0 || y < 0 || w <= 0)
                return false;

            int index = x + y * w;
            if (index < 0 || index >= data.Length)
                return false;

            return data[index];
        }

        void fill_bool_map(Color[] col_map, bool[] bool_map)
        {
            

            for (int i = 0; i < col_map.Length; i++)
            {
                if (col_map[i] == Color.Black)
                    bool_map[i] = true;
                else
                    bool_map[i] = false;
            }
        }

        public Item check_for_item(Ego ego)
        {
            foreach (Item i in items)
            {
                if (i.position.Intersects(ego.position) && baseline_check(ego.baseline, i.baseline))
                    return i;
            }

            return null;
        }
    }
}
