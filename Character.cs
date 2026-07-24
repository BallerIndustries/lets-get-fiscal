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
    /*
     * The first mother fucking class for Lets Get Fiscal. Tomorrow
     * I am going hiking with Seedo, we are going to Two Creeks in 
     * Lindfield.
     */

    public class Character : GameObject
    {   
        public enum JumpDirection
        {
            upwards,
            downwards
        }

        public enum HorJumpDirection
        {
            none,
            left,
            right
        }

        public enum MoveDirection
        {
            none,
            north,
            east,
            south,
            west
        }

        public bool end_animation;
        public string coll_name;
        private int _hp;
        public int hp
        {
            get { return _hp; }
            set { prev_hp = hp; _hp = value; }
        }

        public int prev_hp;
        public readonly int max_hp;
        public Collision punch_rect;
        public JumpDirection jump_dir;
        public HorJumpDirection hor_jump_dir;
        public int jumpY, leapX;
        public bool[] collision_map;
        public int collision_width;
        public bool attacked_from_left;
        public CollisionManager cm;
        public ProjectileManager pm;
        public Point delta_position;
        public int KO_holds;
        public bool sound_played = true;
        public bool slide_sound_played;
        public Character grabbee;
        public Camera camera;
        public Texture2D portrait;
        public Texture2D collision;
        public string name;
        public bool has_bounced;
        public GameState game_state;
        public bool grabbable = false;
        public int jump_speed = 10;
        public int slow_for;
        public int frames_passed;
        public uint attack_id;
        public HitData last_hitter;
        public string death_noise;
        public Weapon weapon;

        public Character(string sheet, int hp, string coll_name, Singletons singletons, float scale) :
            base(sheet, singletons, scale)
        {
            this.speed = 6;
            this.current_animation = new Animation(new Rectangle(0, 0, 49, 87), 4, true, "idle", Character.State.idle);
            this.soundBank = Singletons.soundBank;
            this.coll_name = coll_name;
            this.hp = hp;
            this.prev_hp = hp;
            this.max_hp = hp;
            this.cm = Singletons.cm;
            this.pm = Singletons.pm;
            this.game_state = Singletons.game_state;
            this.camera = Singletons.camera;

            collision = tm.find_texture(coll_name);
            collision_map = tm.find_collision(coll_name);
            collision_width = tm.find_collision_width(coll_name);

            //This better not be here for too long you LAZY BASTARD
            //if (portrait_name != string.Empty)
            //    portrait = tm.find_texture(portrait_name);
        }

        public Character(string sheet, int hp, string coll_name, Singletons singletons) :
            base(sheet, singletons, 2.8f)
        {
            this.speed = 6;
            this.current_animation = new Animation(new Rectangle(0, 0, 49, 87), 4, true, "idle", Character.State.idle);
            this.soundBank = Singletons.soundBank;
            this.coll_name = coll_name;
            this.hp = hp;
            this.max_hp = hp;
            this.cm = Singletons.cm;
            this.pm = Singletons.pm;
            this.game_state = Singletons.game_state;
            this.camera = Singletons.camera;

            collision = tm.find_texture(coll_name);
            collision_map = tm.find_collision(coll_name);
            collision_width = tm.find_collision_width(coll_name);

            //This better not be here for too long you LAZY BASTARD
            //if (portrait_name != string.Empty)
            //    portrait = tm.find_texture(portrait_name);
        }

        public new State state
        {
            get { return base.state; }
            set
            {
                if (value != base.state)
                    current_frame = 0;
                
                base.state = value;
            }
        }

        public WeaponData weapon_data
        {
            get
            {
                if (current_animation.weapon_data != null)
                    return current_animation.weapon_data[current_frame];
                else
                    return new WeaponData();
            }
        }

        public override int baseline
        {
            get
            {
                if (state == State.jumping || state == State.jump_kick1 || state == State.jump_kick2 || state == State.jump_kick3 || state == State.leap_attack)
                    return jumpY;
                else
                    return posY;
            }
        }

        public Rectangle new_position
        {
            get { return new Rectangle(position.X + delta_position.X , position.Y + delta_position.Y, position.Width, position.Height); }
        }

        public Rectangle new_xPosition
        {
            get { return new Rectangle(position.X + delta_position.X, position.Y, position.Width, position.Height); }
        }

        public Rectangle new_yPosition
        {
            get { return new Rectangle(position.X, position.Y + delta_position.Y, position.Width, position.Height); }
        }

        public int new_baseline
        {
            get { return baseline + delta_position.Y; }
        }

        public void start_slide_attack()
        {
            attack_id++;
            state = State.slide_attack;
            slide_sound_played = false;
            leapX = position.Center.X;
            //leapX = posX;
        }
        
        public Collision do_slide_attack()
        {
            //If we aren't on the last frame, ie the slide frame
            if (current_frame != 1)
                animate_check();
            //If we are on the slide frame
            else
            {
                if (slide_sound_played == false)
                {
                    soundBank.PlayCue("slide_noise");
                    slide_sound_played = true;
                }

                //Do the slide
                if (Math.Abs(leapX - position.Center.X) < 300)
                {
                    if (facing == Direction.left)
                        posX -= 10;
                    else
                        posX += 10;
                }
                else
                {
                    animate_check();
                }
            }

            return getCollision();
        }

        public void startDying()
        {
            state = State.dying;
        }

        public void idle()
        {
            state = State.idle;
            animate_check();
        }

        public virtual void dead()
        {
            alpha_val = MathHelper.Clamp(alpha_val - 10, 0, 255);
        }

        public void do_KO()
        {
            if (KO_holds > 50)
            {
                attack_id++;
                state = State.getting_up;
            }
            else
                KO_holds++;
        }

        public virtual void start_death_leap()
        {
            end_animation = false;
            state = State.death_leap;
            soundBank.PlayCue(death_noise);
            leapX = position.Center.X;
            //leapX = posX;
            jumpY = posY;
            jump_dir = JumpDirection.upwards;
            has_bounced = false;
        }

        public void do_death_leap()
        {
            if (has_bounced == false)
            {
                if (attacked_from_left)
                    posX += 12;
                else
                    posX -= 12;

                if (jump_dir == JumpDirection.upwards)
                    posY -= 12;
                else
                    posY += 12;

                //Go up or go down or something
                if (jumpY - posY > 200)
                    jump_dir = JumpDirection.downwards;

                //Only animate if we are not on the last frame.
                if (current_frame != (current_animation.numFrames - 1))
                    animate_check();
                else if (Math.Abs(leapX - position.Center.X) > 400)
                {
                    //Now we want the character to bounce once
                    has_bounced = true;
                    jump_dir = JumpDirection.upwards;
                    soundBank.PlayCue("thump");
                }
            }
            else
            {
                if (attacked_from_left)
                    posX += 4;
                else
                    posX -= 4;

                if (jump_dir == JumpDirection.upwards)
                    posY -= 4;
                else
                    posY += 4;

                //Go up or go down or something
                if (jumpY - posY > 20)
                    jump_dir = JumpDirection.downwards;

                //Only animate if we are not on the last frame.
                if (current_frame != (current_animation.numFrames - 1))
                    animate_check();
                else if (Math.Abs(leapX - position.Center.X) > 440)
                {
                    //Now we want the character to bounce once
                    has_bounced = true;
                    state = State.dead;
                    posY = jumpY;
                }
            }
        }

        public void start_KO_leap()
        {
            leapX = position.Center.X;//leapX = posX;
            end_animation = false;

            if (state != State.jumping)
                jumpY = posY;

            state = State.KO_leap;
            KO_holds = 0;
            jump_dir = JumpDirection.upwards;
            has_bounced = false;
        }

        public void do_KO_leap()
        {
            if (attacked_from_left)
                delta_position.X = 12;
            else
                delta_position.X = -12;

            if (jump_dir == JumpDirection.upwards)
                posY -= 12;
            else
                posY += 12;

            //Go up or go down or something
            if (jumpY - posY > 100)
                jump_dir = JumpDirection.downwards;

            //Only animate if we are not on the last frame.
            if (current_frame != (current_animation.numFrames - 1))
                animate_check();
            else if (jump_dir == JumpDirection.downwards && posY >= jumpY)
            {
                posY = jumpY;
                state = State.KO;
            }
        }

        public void face_collision(Direction dir)
        {
            if (dir == Direction.left)
                facing = Direction.right;
            else
                facing = Direction.left;
        }

        public Collision do_being_thrown()
        {
            if (current_frame == 1)
            {
                if (facing == Direction.left)
                    posX -= 6;
                else
                    posX += 6;

                posY += 6;

                if (posY >= jumpY)
                {
                    posY = jumpY;
                    current_frame = 2;
                    soundBank.PlayCue("thump");
                    Singletons.shake_screen = true;
                    jump_dir = JumpDirection.upwards;

                    hp -= 20;
                    cm.hud.set_bad_guy_fields(this as BadGuy);


                    if (hp <= 0)
                    {
                        soundBank.PlayCue(death_noise);
                    }
                }

                
                

            }

            if (current_frame == 2)
            {
                if (facing == Direction.left)
                    posX -= 4;
                else
                    posX += 4;

                if (jump_dir == JumpDirection.upwards)
                {
                    posY -= 4;

                    if (Math.Abs(jumpY - posY) > 50)
                        jump_dir = JumpDirection.downwards;
                }

                if (jump_dir == JumpDirection.downwards)
                {
                    posY += 4;

                    if (posY >= jumpY)
                    {
                        if (hp <= 0)
                            state = State.dead;
                        else
                        {
                            KO_holds = 0;
                            state = State.KO;
                        }
                    }
                }
            }

            //if (current_frame < 2)
                return new Collision(Collision.collision_type.ego, this.position, 10, this.baseline, this.facing, true, "pah", this, null);
            //else
            //    return Collision.Empty;
        }

        public void die()
        {
            animate_check();
        }

        public void startForwardJump()
        {
            state = State.jumping_forward;
            jumpY = posY;
            jump_dir = JumpDirection.upwards;
        }

        public void startBackwardJump()
        {
            state = State.jumping_backward;
            jumpY = posY;
            jump_dir = JumpDirection.upwards;
        }

        public virtual void start_attack(State move)
        {
            attack_id++;
            sound_played = false;
            state = move;
        }

        public Collision do_attack()
        {
            animate_check();
            AttackAnimation aa = current_animation as AttackAnimation;

            return getCollision();
        }

        public void startJump()
        {
            soundBank.PlayCue("jump_noise");

            state = State.jumping;
            jumpY = posY;
            jump_dir = JumpDirection.upwards;
            jump_speed = 10;
        }

        public void calculate_jump()
        {
            if (jumpY - posY > 200)
                jump_dir = JumpDirection.downwards;

            if (jump_dir == JumpDirection.upwards)
            posY -= jump_speed;

            else if (jump_dir == JumpDirection.downwards)
                posY += jump_speed;

            if (hor_jump_dir == HorJumpDirection.left)
                delta_position.X = -speed;

            else if (hor_jump_dir == HorJumpDirection.right)
                delta_position.X = speed;

                //Test if we have hit the ground
                if (posY >= jumpY && jump_dir == JumpDirection.downwards)
                {
                    posY = jumpY;
                    state = State.rebound;

                    soundBank.PlayCue("land_noise");
                }
        }

        public void animate_jump()
        {
            if (current_frame == 0)
                animate_check();

            if (jump_dir == JumpDirection.downwards)
                current_frame = 2;
        }

        public void moveRight()
        {
            if (facing == Direction.left)
                facing = Direction.right;
            else
                delta_position.X = speed;
        }

        public void moveLeft()
        {
            if (facing == Direction.right)
                facing = Direction.left;
            else
                delta_position.X = -speed;
        }

        public void moveUp()
        {
            delta_position.Y = - speed;
        }

        public void moveDown()
        {
            delta_position.Y = speed;
        }

        public void get_hit(int damage, bool from_left, bool does_KO)
        {

            if (this.id == Singletons.ego.id)
            {
                hp -= (damage / 2);
                Singletons.ego.idle_count = 0;
            }
            else
            {
                hp -= damage;
            }
            
            attacked_from_left = from_left;

            //Free the guy you are grabbing
            if (state == State.back_grab)
                grabbee.state = State.idle;

            //If we hit a bad guy then increase the score
            BadGuy bg = this as BadGuy;
            if (bg != null)
                game_state.score += damage * 2;

            if (hp <= 0)
                start_death_leap();
            else if (does_KO || state == State.jumping || state == State.jump_kick1)
                start_KO_leap();
            else
                startDying();
        }

        //bound should be from the Animation class and punchRect defines a
        //rectangle on a massive spritesheet.
        //First frame is frame number 0
        public Rectangle getAttackRect(Rectangle bound, Rectangle punchRect)
        {
            Rectangle retRect = new Rectangle();

            retRect.X = (int)(position.X + ((punchRect.X - bound.X) * scale));
            retRect.Y = (int)(position.Y + ((punchRect.Y - bound.Y) * scale));
            retRect.Width = (int)(punchRect.Width * scale);
            retRect.Height = (int)(punchRect.Height * scale);

            if (facing == Direction.left)
            {
                int dist_from_bound = (bound.X + bound.Width) - (punchRect.X + punchRect.Width);
                dist_from_bound = (int)(scale * dist_from_bound);
                retRect.X = position.X + dist_from_bound;
            }
            return retRect;
        }

        public void LoadContent()
        {
            //Get a pointer to the character's texture.
            //Get a pointer to the character's collision array.
        }

        public virtual Collision getCollision()
        {
            return Collision.Empty;
        }

        //public void manage_character_speed()
        //{
        //    if (slow_for > 0)
        //    {
        //        slow_for--;
        //    }
        //}
    }
}
