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
    public class TreeboiOrKone : BadGuy
    {
        public enum TBK_AI
        {
            wait,
            teleport1,
            charge1,
            teleport2,
            shoot_lasers
        }

        //public bool deletable;

        bool laser_sent;
        bool animation_end;
        private readonly bool is_kone = false;
        int wait_time = 80;
        private TBK_AI boss_ai;
        private int charge_count, shoot_count;

        public TreeboiOrKone(string sheet, int hp, string coll_name, Singletons singletons, AiState init_ai_state) 
            : base(sheet, hp, coll_name, singletons)
        {
            this.ai_state = init_ai_state;

            if (sheet == "spritesheets//kone")
                is_kone = true;
        }

        public override void Update(Rectangle position)
        {
            handle_state();
            if (ai_state != AiState.running && state != State.death_leap && state != State.dead)
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
                    animate_check();
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

                case State.shoot_laser:
                    do_laser_shoot();
                    break;

                case State.running:
                    do_running();
                    break;

                case State.charge:
                    do_charge();
                    break;

                case State.teleport_out:
                    do_teleport_out();
                    break;

                case State.teleport_in:
                    do_teleport_in();
                    break;

                case State.none:
                    if (ai_state == AiState.running)
                    {
                        Singletons.soundBank.PlayCue("get_out");
                        state = State.running;
                        facing = Direction.right;
                    }
                    else
                    {
                        idle();
                    }
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
            if (boss_ai == TBK_AI.wait)
            {
                wait_time--;

                if (wait_time == 0)
                {
                    boss_ai++;
                    wait_time = 150;
                }
            }
            else if (boss_ai == TBK_AI.teleport1)
            {
                start_teleport_out();
                boss_ai++;
            }
            else if (boss_ai == TBK_AI.charge1)
            {
                if (state == State.idle)
                    start_charge();
            }
            else if (boss_ai == TBK_AI.teleport2)
            {
                if (state == State.idle)
                    start_teleport_in();
            }
            else if (boss_ai == TBK_AI.shoot_lasers)
            {
                if (state == State.idle)
                {
                    start_laser_shoot();
                    shoot_count++;
                }

                if (shoot_count > 3)
                {
                    boss_ai = TBK_AI.wait;
                    state = State.idle;
                    shoot_count = 0;
                }
            }
        }

        private void do_running()
        {
            animate_check();

            if (facing == Direction.right)
                posX += 10;
            else
                posX -= 10;
        }

        private void start_teleport_in()
        {
            state = State.teleport_in;
            //1. KONE spawns on same Y Plane as EGO.
            //2. Treeboi 50 pixels higher or lower.

            posY = Singletons.ego.posY;

            //if (is_kone)
            //{
            //    posY = Singletons.ego.posY;
            //}
            //else if (Singletons.ego.posY + 50 < 540)
            //{
            //    posY = Singletons.ego.posY + 50;
            //}
            //else
            //{
            //    posY = Singletons.ego.posY - 50;
            //}

            //1. X Plane is whichever side of the ego has more space.
            if (Singletons.ego.position.Center.X > camera.viewport_rect.Center.X)
            {
                //Spawn to the left  of the ego
                int dist = Singletons.ego.position.Left - camera.viewport_rect.Left;
                posX = camera.viewport_rect.Left + Singletons.random.Next(dist);
            }
            else
            {
                //Spawn to the right of the ego
                int dist = camera.viewport_rect.Right - Singletons.ego.position.Right;
                posX = Singletons.ego.position.Center.X + Singletons.random.Next(dist);
            }
        }

        private void do_teleport_in()
        {
            animate_check();
            punch_rect = getCollision();

            if (moves + 1 >= current_animation.frames)
            {
                boss_ai++;
                state = State.idle;
            }
        }

        private void start_teleport_out()
        {
            state = State.teleport_out;
        }

        public override void start_death_leap()
        {
            base.start_death_leap();
            cm.KILL_all = true;

            Singletons.half_speed = true;
        }

        //public override void start_death_leap()
        //{
        //    base.start_death_leap();
        //    Singletons.half_speed = true;
        //}

        public override void dead()
        {
            base.dead();
            Singletons.half_speed = false;
        }


        private void do_teleport_out()
        {
            animate_check();

            //If we are on the last frame and about to tick over to IDLE.
            if (current_frame == current_animation.numFrames - 1 && moves + 1 == current_animation.frames)
                posY = -1000;

            punch_rect = getCollision();
        }

        private void start_charge()
        {
            state = State.charge;
            charge_count = 0;

            soundBank.PlayCue("dj_charge");

            if (is_kone)
            {
                facing = Direction.right;
                posX = Singletons.camera.viewport_rect.X;
                posY = cm.min_y;
            }
            else
            {
                facing = Direction.left;
                posX = Singletons.camera.viewport_rect.Right;
                posY = 540;
            }
        }

        private void do_charge()
        {
            animate_check();

            if (facing == Direction.left)
                posX -= 16;
            else
                posX += 16;

            if (facing == Direction.right && position.Left > Singletons.camera.viewport_rect.Right)
            {
                charge_count++;
                facing = Direction.left;
            }

            else if (facing == Direction.left && position.Right < Singletons.camera.viewport_rect.Left)
            {
                charge_count++;
                facing = Direction.right;
            }

            if (charge_count == 2)
            {
                boss_ai++;
                state = State.idle;
            }


           

            punch_rect = getCollision();
        }

        private void start_laser_shoot()
        {
            //facing = Direction.left;

            facing = Singletons.cm.face_ego(this.posX);
            
            state = State.shoot_laser;
            laser_sent = false;
            animation_end = false;
            attack_id++;
        }

        private void do_laser_shoot()
        {
            animate_check();

            if (current_frame > 0)
                animation_end = true;


            if (current_frame == 0 && !laser_sent && animation_end)
            {
                soundBank.PlayCue("pew");
                laser_sent = true;
                Rectangle laser = new Rectangle();

                laser.Y = position.Center.Y - 75;

                if (facing == Direction.left)
                    laser.X = position.Left + 25;
                else
                    laser.X = position.Right - 50;

                //glass.Width = 10;
                //glass.Height = 10;

                Animation a = new Animation(new Rectangle(1351, 3, 49, 13), 4, true, "bananas", State.hadoken);
                Projectile p = new Projectile(laser, new Vector2(24, 0), facing, a, singletons, "spritesheets//kone", this.baseline, this, this.scale, 30);

                pm.add_projectile(p);
            }
        }

        public override void increment_animation_vanilla()
        {
            int lastFramePos = current_animation.init_x + current_animation.bound.Width * (current_animation.numFrames - 1);

            if (bound.X < lastFramePos)
                boundX += current_animation.bound.Width;
            else
            {
                //Should we return to the idle animation or go to the first frame.
                if (state == State.death_leap)
                    state = State.dead;
                else if (state == State.KO_leap || state == State.being_thrown)
                    state = State.KO;
                else if (state == State.getting_up)
                {
                    boss_ai = TBK_AI.teleport1;
                    state = State.idle;
                    //soundBank.PlayCue("turbo_charge");
                    //state = State.charge;
                    //ai_state = AiState.charging;
                }
                else if (current_animation.repeats == false)
                    state = State.idle;

                boundX = current_animation.init_x;
            }
        }

    }
}
