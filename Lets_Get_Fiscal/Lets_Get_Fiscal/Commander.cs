using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.Diagnostics;

namespace Lets_Get_Fiscal
{
    public class Commander : BadGuy
    {
        private bool bullet_fired, suicide_announced;
        int wait = 100;
        int dead_count;
        int bullets_fired;
        private int hor_spin_count; //Number of times we have spinned across the screen horizontally.
        

        public Commander(string sheet, int hp, string coll_name, Singletons singletons, AiState init_ai_state)
            : base(sheet, hp, coll_name, singletons)
        {
        }

        public override void Update(Rectangle position)
        {
            handle_state();
            do_ai();
        }

        private void handle_state()
        {
            punch_rect = Collision.Empty;

            switch (state)
            {
                case State.death_leap:
                    do_death_leap();
                    break;

                case State.KO_leap:
                    do_KO_leap();
                    break;

                case State.dying:
                    die();
                    break;

                case State.getting_up:
                    do_getting_up();
                    break;

                case State.dead:
                    dead();
                    break;

                case State.KO:
                    do_KO();
                    break;

                case State.idle:
                    idle();
                    break;

                case State.none:
                    idle();
                    break;

                case State.suicide:
                    do_suicide();
                    break;

                case State.charging:
                    punch_rect = do_charging();
                    break;

                case State.shooting:
                    do_shooting();
                    break;

                case State.horizontal_spin:
                    punch_rect = do_horizontal_spin();
                    break;

                case State.vertical_spin:
                    punch_rect = do_vertical_spin();
                    break;

                case State.knife_slash:
                    punch_rect = do_knife_slash();
                    break;

                case State.throwing_grenade:
                    do_grenade_throw();
                    break;
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

        private void do_ai()
        {
            if (state == State.suicide || state == State.dead || state == State.charging || state == State.dying || state == State.KO_leap || state == State.KO || state == State.getting_up)
                return;
            
            else if (hp < 40)
            {
                start_charging();
            }
            else if (ai_state == AiState.vertical_spin)
            {
                do_vertical_spin_ai();
            }
            else if (ai_state == AiState.horizontal_spin)
            {
                do_horizontal_spin_ai();
            }
            else if (ai_state == AiState.shooting)
            {
                do_shooting_ai();
            }
            else if (ai_state == AiState.kill_kill_kill)
            {
                start_idle_ai();
            }
            else
            {
                do_idle_ai();
            }
        }

        private void start_idle_ai()
        {
            if (wait <= 0)
                wait = 120;
            
            state = State.idle;
            ai_state = AiState.idle;
        }

        private void do_idle_ai()
        {
            //Start going into VERTICAL CHARGE MODE
            wait--;
            if (wait == 0)
                start_vertical_spin_ai();

            if (Singletons.ego.idle_count > 1200)
                start_suicide();
        }

        private void start_shooting_ai()
        {
            ai_state = AiState.shooting;
            start_shooting();
            bullets_fired = 0;
            
            posY = Singletons.ego.posY;

            if (posX < camera.viewport_rect.Left)
                posX = camera.viewport_rect.Left;
            else if (position.Right > camera.viewport_rect.Right)
                posX = camera.viewport_rect.Right - posW;

        }

        private void do_shooting_ai()
        {
            if (bullets_fired > 4 && state == State.idle)
                start_idle_ai();

            if (state == State.idle)
                start_shooting();
        }

        private void start_horizontal_spin_ai()
        {
            ai_state = AiState.horizontal_spin;
            state = State.horizontal_spin;

            hor_spin_count = 0;

            if (facing == Direction.left)
            {
                facing = Direction.right;
                posX = camera.viewport_rect.Left - posW;
                posY = cm.min_y;
            }
            else if (facing == Direction.right)
            {
                facing = Direction.left;
                posX = camera.viewport_rect.Left - posW;
                posY = cm.min_y;
            }

        }

        private void do_horizontal_spin_ai()
        {
            if (facing == Direction.right && position.Left > camera.viewport_rect.Right)
            {
                facing = Direction.left;
                posY = 540;
                hor_spin_count++;
            }

            if (facing == Direction.left && position.Right < camera.viewport_rect.Left + 200)
            {
                facing = Direction.right;
                posY = cm.min_y;
                hor_spin_count++;
            }

            if (hor_spin_count == 2)
            {
                start_shooting_ai();
            }

            
        }

        private void start_vertical_spin_ai()
        {
            start_vertical_spin(JumpDirection.downwards);
            ai_state = AiState.vertical_spin;

            //If we are on the right of the screen, go left.
            if (position.Center.X > camera.viewport_rect.Center.X)
                facing = Direction.left;
            else
                facing = Direction.right;
        }

        private void do_vertical_spin_ai()
        {
            if (jump_dir == JumpDirection.downwards && posY > 800)
            {
                jump_dir = JumpDirection.upwards;

                if (facing == Direction.left)
                    posX -= 400;
                else
                    posX += 400;

                //Termination Condition.
                if (posX < camera.viewport_rect.Left && facing == Direction.left)
                {
                    start_horizontal_spin_ai();
                }

                //Termination condition
                if (posX > camera.viewport_rect.Right && facing == Direction.right)
                {
                    start_horizontal_spin_ai();
                }
            }

            else if (jump_dir == JumpDirection.upwards && posY < -300)
            {
                jump_dir = JumpDirection.downwards;

                if (facing == Direction.left)
                    posX -= 200;
                else
                    posX += 200;

                //Termination Condition
                if (posX < camera.viewport_rect.Left && facing == Direction.left)
                {
                    start_horizontal_spin_ai();
                }

                //Termination condition
                if (posX > camera.viewport_rect.Right && facing == Direction.right)
                {
                    start_horizontal_spin_ai();
                }
            }
        }

        private void start_grenade_throw()
        {
            state = State.throwing_grenade;
            bullet_fired = false;
            attack_id++;
        }

        private void do_grenade_throw()
        {
            animate_check();

            if (current_frame == 2 && !bullet_fired)
            {
                bullet_fired = true;

                Rectangle bullet = new Rectangle();

                bullet.Y = position.Center.Y - 75;

                if (facing == Direction.left)
                    bullet.X = position.Left + 25;
                else
                    bullet.X = position.Right - 50;

                Animation a = new Animation(new Rectangle(0, 0, 9, 11), 1, true, "bananas", State.hadoken);
                //QuadraticProjectile p = new QuadraticProjectile(bullet, 24, facing, a, singletons, "spritesheets//grenade", this.baseline, this, this.scale, 30);

                //pm.add_projectile(p);
            }
        }

        private void start_shooting()
        {
            state = State.shooting;
            facing = cm.face_ego(this.posX);

            bullet_fired = false;
            attack_id++;
        }

        private void do_shooting()
        {
            animate_check();

            if (current_frame == 2 && !bullet_fired)
            {
                soundBank.PlayCue("bang");
                bullet_fired = true;
                bullets_fired++;

                Rectangle bullet = new Rectangle();

                bullet.Y = position.Center.Y - 75;

                if (facing == Direction.left)
                    bullet.X = position.Left + 25;
                else
                    bullet.X = position.Right - 50;

                Animation a = new Animation(new Rectangle(0, 0, 6, 2), 1, true, "bananas", State.hadoken);
                Projectile p = new Projectile(bullet, new Vector2(24, 0), facing, a, singletons, "spritesheets//bullet", this.baseline, this, this.scale, 30);

                pm.add_projectile(p);
            }
        }

        private void start_suicide()
        {
            state = State.suicide;
            bullet_fired = false;
            suicide_announced = false;
            wait = 200;
        }

        private void do_suicide()
        {
            //if (current_frame == 2)
            //{
            //    if (!suicide_announced)
            //    {
            //        suicide_announced = true;
            //        soundBank.PlayCue("suicide_warning");
            //    }
            //    wait--;

            //    if (wait <= 0)
            //        animate_check();
            //}
            
            if (current_frame == 3 && !bullet_fired)
            {
                bullet_fired = true;
                soundBank.PlayCue("bang");
            }

            if (current_frame < 5)
                animate_check();
            else
            {
                dead_count++;
                Singletons.cm.hud.display_bad_guy_data(this);
                hp = (int)MathHelper.Clamp(hp - 3, 0, max_hp);

                if (hp == 0)
                {   
                    state = State.dead;
                }
            }
        }

        private void start_getting_up()
        {
            state = State.getting_up;
        }

        private void do_getting_up()
        {
            animate_check();
            punch_rect = getCollision();
        }

        

        private void start_horizontal_spin()
        {
            state = State.horizontal_spin;
            attack_id++;
        }

        private Collision do_horizontal_spin()
        {
            animate_check();

            if (facing == Direction.left)
                posX -= 20;
            else
                posX += 20;

            return getCollision();
        }

        private void start_vertical_spin(JumpDirection dir)
        {
            jump_dir = dir;
            state = State.vertical_spin;
            attack_id++;
        }

        private Collision do_vertical_spin()
        {
            animate_check();

            if (jump_dir == JumpDirection.upwards)
                posY -= 20;
            else
                posY += 20;

            return getCollision();
        }

        private void start_charging()
        {
            state = State.charging;
        }

        private Collision do_charging()
        {
            animate_check();

            if (current_frame > 0)
            {
                Singletons.cm.hud.display_bad_guy_data(this);
                hp = (int)MathHelper.Clamp(hp + 1, 0, max_hp);
            }
            //Repeat charging until health is full.
            if (current_frame == 4 && hp < max_hp)
                current_frame = 1;

            return getCollision();
        }

        //public override void increment_animation_vanilla()
        //{
        //    int lastFramePos = current_animation.init_x + current_animation.bound.Width * (current_animation.numFrames - 1);

        //    if (bound.X < lastFramePos)
        //        boundX += current_animation.bound.Width;
        //    else
        //    {
        //        //Should we return to the idle animation or go to the first frame.
        //        if (state == State.death_leap)
        //            state = State.dead;
        //        else if (state == State.KO_leap || state == State.being_thrown)
        //            state = State.KO;
        //        else if (state == State.getting_up)
        //        {
        //            state = State.idle;
        //        }
        //        else if (current_animation.repeats == false)
        //            state = State.idle;

        //        boundX = current_animation.init_x;
        //    }
        //}
    }
}
