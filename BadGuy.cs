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
    public class BadGuy : Character
    {
        public enum AiState
        {
            none,
            idle,
            walk,
            kill_kill_kill,
            retreat,
            waiting,
            charging,
            brawl,
            leap,
            punch,
            suplex,
            whirling,
            throw_glass,
            running,
            shoot_laser,
            teleport_out,
            teleport_in,
            vertical_spin,
            horizontal_spin,
            shooting,
            projectile_fire,
            dash,
            sword_attack,
            sword_down

        }

        public int grab_timer; //When this reaches 0, the bad guy breaks free from Stelven's clutches
        public AiState ai_state = AiState.none;
        public Random random;
        public int idle_frames;
        public bool correcting_path = false;
        public float grab_amount = 0.65f;

        public Direction facinga, facingb, facingc;

        private Rectangle _destination;
        public Rectangle ego_position;
        public Rectangle destination
        {
            get 
            {
                if (ai_state == AiState.idle || ai_state == AiState.none) return position;
                else return _destination;
            }
            set 
            {
                _destination = value; 
            }
        }

        public int destinationX
        {
            get { return _destination.X; }
            set { _destination.X = value; }
        }

        public int destinationY
        {
            get { return _destination.Y; }
            set { _destination.Y = value; }
        }

        public new int speed
        {
            get 
            {
                if (ai_state == AiState.kill_kill_kill)
                    return base.speed;
                else
                    return ((base.speed * 2) / 3);
            }
            set { base.speed = value; }
        }
        
        public readonly List<AttackSequence> attacks = new List<AttackSequence>(5);
        public List<AttackSequence> writable_attacks = new List<AttackSequence>(5);

        public BadGuy(string sheet, int hp, string coll_name, Singletons singletons)
            : base(sheet, hp, coll_name, singletons, 2.8f)
        {
            this.random = Singletons.random;
            this.ai_state = AiState.none;
            move_list = Singletons.mm.find_move_set(sheet);

            assign_values();
        }

        public BadGuy(string sheet, int hp, string coll_name, Singletons singletons, AiState ai_state, float scale)
            : base(sheet, hp, coll_name, singletons, scale)
        {
            this.random = Singletons.random;
            this.ai_state = ai_state;
            move_list = Singletons.mm.find_move_set(sheet);

            assign_values();
        }

        private void assign_values()
        {
            switch (sheet)
            {
                case "spritesheets//kiki":
                    name = "KIKI";
                    death_noise = "butabi_death";
                    portrait = tm.find_texture("portraits//kiki_portrait");

                    attacks.Add(new AttackSequence(State.idle, 20));

                    break;

                case "spritesheets//fitty_cent":
                    name = "CURTIS";
                    death_noise = "fitty_cent_death";
                    portrait = tm.find_texture("portraits//fitty_portrait");

                    attacks.Add(new AttackSequence(State.idle, 20));
                    attacks.Add(new AttackSequence(State.punching));
                    attacks.Add(new AttackSequence(State.idle, 30));
                    attacks.Add(new AttackSequence(State.punching));
                    attacks.Add(new AttackSequence(State.idle, 30));
                    attacks.Add(new AttackSequence(State.right_punch));
                    break;

                case "spritesheets//roxbury":
                    name = "BUTABI";
                    death_noise = "butabi_death";
                    portrait = tm.find_texture("portraits//roxbury_portrait");

                    attacks.Add(new AttackSequence(State.idle, 20));
                    attacks.Add(new AttackSequence(State.snap_kick));
                    break;

                case "spritesheets//ben_seib":
                    name = "BEN";
                    death_noise = "ben_seib_death";
                    portrait = tm.find_texture("portraits//ben_portrait");

                    attacks.Add(new AttackSequence(State.idle, 20));
                    attacks.Add(new AttackSequence(State.punching));
                    attacks.Add(new AttackSequence(State.idle, 30));
                    attacks.Add(new AttackSequence(State.kicking));
                    break;

                case "spritesheets//guido":
                    name = "VINNY";
                    death_noise = "guido_death";
                    portrait = tm.find_texture("portraits//guido_portrait");
                    break;

                case "spritesheets//ken":
                    death_noise = "argh";
                    name = "KEN";
                    portrait = tm.find_texture("portraits//ken_portrait");
                    break;

                case "spritesheets//coral":
                    name = "CORAL";
                    death_noise = "coral_death";
                    portrait = tm.find_texture("portraits//coral_portrait");

                    attacks.Add(new AttackSequence(State.idle, 20));
                    attacks.Add(new AttackSequence(State.punching));
                    break;

                case "spritesheets//silvio":
                    name = "SILVIO";
                    death_noise = "silvio_death";
                    portrait = tm.find_texture("portraits//silvio_portrait");

                    attacks.Add(new AttackSequence(State.idle, 20));
                    attacks.Add(new AttackSequence(State.low_kick));
                    attacks.Add(new AttackSequence(State.idle, 30));
                    attacks.Add(new AttackSequence(State.mid_kick));
                    attacks.Add(new AttackSequence(State.idle, 30));
                    attacks.Add(new AttackSequence(State.jump_kick));
                    break;

                case "spritesheets//customer_girl":
                    name = "KYLIE";
                    death_noise = "silvio_death";
                    portrait = tm.find_texture("portraits//customer_girl_portrait");
                    break;

                case "spritesheets//treeboi":
                    name = "TREEBOI";
                    death_noise = "silvio_death";
                    portrait = tm.find_texture("portraits//tbk_portrait");
                    break;

                case "spritesheets//kone":
                    name = "KONE";
                    death_noise = "silvio_death";
                    portrait = tm.find_texture("portraits//tbk_portrait");
                    break;

                case "spritesheets//commander":
                    name = "COMMANDER";
                    death_noise = "silvio_death";
                    portrait = tm.find_texture("portraits//commander_portrait");
                    break;

                case "spritesheets//fernando":
                    name = "FRED";
                    death_noise = "silvio_death";
                    portrait = tm.find_texture("portraits//fernando_portrait");
                    break;

                case "spritesheets//nicole":
                    name = "NICOLE";
                    death_noise = "nicole_death";
                    portrait = tm.find_texture("portraits//nicole_portrait");

                    attacks.Add(new AttackSequence(State.idle, 20));
                    attacks.Add(new AttackSequence(State.leap_attack));
                    attacks.Add(new AttackSequence(State.idle, 30));
                    attacks.Add(new AttackSequence(State.punching));
                    break;

                default:
                    portrait = tm.find_texture("portraits//ken_portrait");
                    break;
            }

            //Add some default attacks
            if (attacks.Count == 0)
            {
                attacks.Add(new AttackSequence(State.idle, 20));
                attacks.Add(new AttackSequence(State.punching));
                attacks.Add(new AttackSequence(State.idle, 30));
                attacks.Add(new AttackSequence(State.punching));
            }
        }

        public virtual bool handle_state()
        {
            switch (state)
            {
                case State.idle:
                    idle();
                    break;

                case State.dead:
                    dead();
                    break;

                case State.being_thrown:
                    do_being_thrown();
                    break;

                case State.KO:
                    do_KO();
                    break;

                case State.KO_leap:
                    do_KO_leap();
                    break;

                case State.death_leap:
                    do_death_leap();
                    break;

                case State.dying:
                    die();
                    break;

                case State.getting_up:
                    animate_check();
                    break;

                case State.grabbed:
                    do_grabbed();
                    break;

                case State.leap_attack:
                    punch_rect = do_leap_attack();
                    break;

                default:
                    return false;
            }

            return true;
        }

        public virtual void do_grabbed()
        {
            animate_check();
            grab_timer--;

            if (grab_timer <= 0)
            {
                if (Singletons.ego.facing == Direction.left)
                    this.facing = Direction.right;
                else
                    this.facing = Direction.left;

                start_attack(State.punching);

                Singletons.ego.state = State.idle;
            }
        }

        public override void start_attack(State move)
        {
            if (move == State.leap_attack)
                start_leap_attack();
            else if (move == State.jump_kick)
                start_jump_kick();
            else if (move == State.slide_attack)
                start_slide_attack();
            else if (move == State.knife_slash)
                start_knife_slash();
            else
            {
                attack_id++;
                sound_played = false;
                state = move;
            }
        }

        public virtual void Update(Rectangle position)
        {
            ego_position = position;
            punch_rect = Collision.Empty;

            //int spazzing_out = 0;

            //facingc = facingb;
            //facingb = facinga;
            //facinga = facing;

            //if ((facinga == Direction.left && facingb == Direction.right && facingc == Direction.left) ||
            //    (facinga == Direction.right && facingb == Direction.left && facingc == Direction.right))
            //{

            //    spazzing_out = id;
            //}


            if (slow_for > 0)
            {
                slow_for--;
                return;
            }

            if (state == Character.State.death_leap)
                do_death_leap();
            else if (state == State.KO_leap)
                do_KO_leap();
            else if (state == State.KO)
                do_KO();
            else if (state == State.being_thrown)
                punch_rect = do_being_thrown();
            else if (state == State.getting_up)
                animate_check();
            else if (state == State.grabbed)
                do_grabbed();
            else if (state == State.snap_kick)
                punch_rect = do_attack();

            else if (state == Character.State.dead)
                dead();
            else if (state == Character.State.dying)
                die();
            else if (state == State.punching)
                punch_rect = do_attack();
            else if (state == State.kicking)
                punch_rect = do_attack();
            else if (state == State.right_punch || state == State.low_kick || state == State.mid_kick || state == State.high_kick)
                punch_rect = do_attack();
            else if (state == State.slide_attack)
                punch_rect = do_slide_attack();
            else if (state == State.leap_attack)
                punch_rect = do_leap_attack();
            else if (state == State.jump_kick)
                punch_rect = do_jump_kick();
            else
            {
                if (ai_state == AiState.none)
                    assign_new_state();
                else
                    do_move(position);
            }

            //Check if the move is alright
            if (delta_position != Point.Zero)
            {
                if (cm.position_ok(this, new_xPosition))
                    posX += delta_position.X;
                if (cm.position_ok(this, new_yPosition))
                    posY += delta_position.Y;

                delta_position = Point.Zero;
            }
        }

        public void assign_new_state()
        {
            int number = random.Next(100);

            if (this.position.Intersects(camera.viewport_rect) == false)
            {
                ai_state = AiState.walk;
                destination = cm.get_mid_screen_location(random);
            }
            else if (number < 10)
            {
                //This code should happen whenever the state is changed.
                //Maybe add change_ai_state() function for extra niceness
                change_ai_state(AiState.kill_kill_kill);
            }
            else if (number < 50)
            {
                ai_state = AiState.idle;
                idle_frames = 100;
            }
            else
            {
                ai_state = AiState.walk;
                destination = cm.get_random_location(random, this.position);
            }
        }

        public void do_move(Rectangle position)
        {
            switch (ai_state)
            {
                case AiState.idle:
                    if (idle_frames > 0)
                    {
                        idle();
                        idle_frames--;
                    }
                    else
                    {
                        ai_state = AiState.none;
                    }
                    break;

                case AiState.walk:
                    if (walk_to(destination) == false)
                    {
                        ai_state = AiState.none;
                        correcting_path = false;
                    }
                    break;

                case AiState.retreat:
                    if (retreat(destination) == false)
                        ai_state = AiState.none;
                    break;

                case AiState.kill_kill_kill:

                    //Rethink. Check if we are intersecting with the bad guy. If we are
                    //start the attack stuff.
                    if (cm.touching_ego(this))
                    {
                        //Time for some cool bananas thinking. COOL BANANAS
                        //AttackSequence atk_seq = writable_attacks[0];

                        if (writable_attacks[0].amount <= 0 && writable_attacks.Count > 1)
                        {
                            writable_attacks.Remove(writable_attacks[0]);
                            writable_attacks[0] = writable_attacks[0];
                        }

                        if (writable_attacks[0].state == State.idle)
                            idle();
                        else
                        {
                            start_attack(writable_attacks[0].state);
                        }
                            

                        writable_attacks[0].decrement_amount();
                        //writable_attacks[0].amount = writable_attacks[0].amount - 1;

                        if (writable_attacks.Count == 1 && writable_attacks[0].amount <= 0)
                        {
                            ai_state = AiState.retreat;
                            if (facing == Direction.left)
                                destinationX = posX + 200;
                            else
                                destinationX = posX - 200;

                            destinationY = posY;
                        }
                    }
                    else
                    {
                        walk_to(position);
                    }


                    break;

                case AiState.waiting:
                    if (camera.viewport_rect.Intersects(this.position))
                        ai_state = AiState.none;
                    else
                        idle();
                    break;
            }
        }

        public void change_ai_state(AiState state)
        {
            ai_state = state;

            if (state == AiState.kill_kill_kill)
            {
                writable_attacks.Clear();
                foreach (AttackSequence atk_seq in attacks)
                {
                    writable_attacks.Add(new AttackSequence(atk_seq));
                }

                //Face Stelven the accountant
                if (this.state != State.death_leap && this.state != State.KO_leap)
                    facing = cm.face_ego(this.position.X);
            }
        }

        //This function is a lot like walk_to except it doesn't check for collisions with the ego
        public bool retreat(Rectangle dest)
        {
            if (Math.Abs(dest.Center.X - position.Center.X) >= speed)
            {
                if (position.Center.X > dest.Center.X)
                    moveLeft();
                else if (position.Center.X < dest.Center.X)
                    moveRight();
            }

            //We only want to move if we are ten pixels away
            if (Math.Abs(baseline - dest.Bottom) >= speed)
            {
                if (baseline > dest.Bottom)
                    moveUp();
                else
                    moveDown();
            }

            if (delta_position == Point.Zero)
                return false;
            else
            {
                posX += delta_position.X;
                posY += delta_position.Y;

                state = State.walking;
                delta_position = Point.Zero;
                animate_check();
                return true;
            }
        }

        public bool walk_to(Rectangle dest)
        {
            bool x_move_ok = false;
            bool y_move_ok = false;

            if (Math.Abs(dest.Center.X - position.Center.X) >= speed)
            {
                if (position.Center.X > dest.Center.X)
                    moveLeft();
                else if (position.Center.X < dest.Center.X)
                   moveRight();
            }

            //We only want to move if we are ten pixels away
            if (Math.Abs(baseline - dest.Bottom) >= speed)
            {
                if (baseline > dest.Bottom)
                    moveUp();
                else
                    moveDown();
            }

            if (delta_position == Point.Zero)
                return false; 

            x_move_ok = cm.position_ok(this, new_xPosition);
            y_move_ok = cm.position_ok(this, new_yPosition);

            if (x_move_ok)
                posX += delta_position.X;

            if (y_move_ok)
                posY += delta_position.Y;

            //Check if the move is alright
            if (x_move_ok && y_move_ok)
            {
                state = State.walking;
                delta_position = Point.Zero;
                animate_check();
                return true;
            }
            else
            {
                return false;
            }
        }

        public new void LoadContent()
        {
            //This code figures out whether a bad guy is grabbable. It lets the CM
            //know whether we can begin a grab.
            foreach (Animation a in move_list.moves)
            {
                Grab g = a as Grab;

                if (g != null)
                {
                    if (g.grabbing == false)
                    {
                        grabbable = true;
                        break;
                    }
                }
            }
        }

        public override Collision getCollision()
        {
            //The following code is from do_attack(). It does not call animate_check().
            AttackAnimation aa = current_animation as AttackAnimation;
            Rectangle region;

            if (aa == null)
                return Collision.Empty;

            if (aa.attacks[current_frame] != Rectangle.Empty)
            {
                region = getAttackRect(bound, aa.attacks[current_frame]);
                return new Collision(Collision.collision_type.bad_guy, region, aa.damage, baseline, facing, aa.attack_does_ko[current_frame], aa.sfx_name, this, null);
            }
            else
                return Collision.Empty;
        }

        public new void moveRight()
        {
            if (facing == Direction.left)
                facing = Direction.right;
            else
                delta_position.X = speed;
        }

        public new void moveLeft()
        {
            if (facing == Direction.right)
                facing = Direction.left;
            else
                delta_position.X = -speed;
        }

        public new void moveUp()
        {
            delta_position.Y = -speed;
        }

        public new void moveDown()
        {
            delta_position.Y = speed;
        }

        private void start_charge()
        {
            state = State.charge;
        }

        public bool destination_reached()
        {
            //This function checks whether we are a step away from our destination.
            if (position.Intersects(destination))
                return true;

            Rectangle rect = new Rectangle();
            rect.X = position.X - speed;
            rect.Y = position.Y - speed;
            rect.Width = position.Width + (speed * 2);
            rect.Height = position.Height + (speed * 2);

            if (rect.Intersects(destination))
                return true;
            else
                return false;
        }

        public void start_knife_slash()
        {
            state = State.knife_slash;
            jump_dir = JumpDirection.upwards;
            jumpY = posY;
            attack_id++;
        }

        public Collision do_knife_slash()
        {
            animate_check();
            return getCollision();
        }

        private void start_jump_kick()
        {
            leapX = position.Center.X;
            jumpY = posY;
            jump_dir = JumpDirection.upwards;

            state = State.jump_kick;
        }

        private Collision do_jump_kick()
        {
            if (current_frame < 1)
                animate_check();

            if (facing == Direction.left)
                posX -= 10;
            else
                posX += 10;

            if (jump_dir == JumpDirection.upwards)
                posY -= 4;
            else
                posY += 4;

            if (Math.Abs(jumpY - posY) > 30)
                jump_dir = JumpDirection.downwards;


            if (posY > jumpY)
            {
                state = State.idle;
                posY = jumpY;
            }
            

            return getCollision();
        }

        private void start_leap_attack()
        {
            leapX = position.Center.X;
            jumpY = posY;
            state = State.leap_attack;
            jump_dir = JumpDirection.upwards;
        }

        private Collision do_leap_attack()
        {
            if (current_frame < 1)
                animate_check();
            else
            {

                if (facing == Direction.left)
                    posX -= 10;
                else
                    posX += 10;

                if (jump_dir == JumpDirection.upwards)
                    posY -= 10;
                else
                    posY += 10;

                if (Math.Abs(jumpY - posY) > 160)
                    jump_dir = JumpDirection.downwards;

                if (jump_dir == JumpDirection.downwards && posY >= jumpY)
                {
                    state = State.idle;
                    posY = jumpY;
                }
            }

            return getCollision();
        }


        public void set_destination(MoveDirection dir, int min_y)
        {
            destination = new Rectangle(posX, posY, 1, 1);
            change_ai_state(AiState.walk);
            state = State.walking;

            switch (dir)
            {
                case MoveDirection.north:
                    destinationY = (int)MathHelper.Clamp(destinationY - 100, min_y, 540);
                    break;

                case MoveDirection.east:

                    destinationY = (int)MathHelper.Clamp(destinationY - 100, min_y, 540);
                    //destinationX = (int)MathHelper.Clamp(destinationX + 100, camera.viewport_rect.Left, camera.viewport_rect.Right);
                    break;

                case MoveDirection.south:
                    destinationY = (int)MathHelper.Clamp(destinationY + 100, min_y, 540); ;
                    break;

                case MoveDirection.west:

                    destinationY = (int)MathHelper.Clamp(destinationY + 100, min_y, 540);
                    //destinationX = (int)MathHelper.Clamp(destinationX - 100, camera.viewport_rect.Left, camera.viewport_rect.Right); ;
                    break;
            }
        }
    }
}