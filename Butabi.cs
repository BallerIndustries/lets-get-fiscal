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
    public class Butabi : BadGuy
    {
        private int charge_count;
        private int idle_frames;
        private int wait_frames;

        public Butabi(string sheet, int hp, string coll_name, Singletons singletons)
            : base(sheet, hp, coll_name, singletons)
        {
        }

        public override void start_death_leap()
        {
            base.start_death_leap();

            //Singletons.half_speed = true;
        }

        public override void dead()
        {
            base.dead();

            //Singletons.half_speed = false;
        }

        public override void do_grabbed()
        {
            animate_check();
            grab_timer--;

            if (grab_timer <= 0)
            {
                if (Singletons.ego.facing == Direction.left)
                    this.facing = Direction.right;
                else
                    this.facing = Direction.left;

                start_attack(State.snap_kick);

                Singletons.ego.state = State.idle;
            }
        }




        private void do_ai()
        {
            //1. Run across the screen three times.
            //2. Stand in the middle of the screen.
            //3. Laugh AKA Idle for a little while. 
            //4. Run off the screen.

            if (ai_state == AiState.none)
            {
                soundBank.PlayCue("turbo_charge");
                ai_state = AiState.charging;
                state = State.charge;
                charge_count = 0;
            }

            if (ai_state == AiState.waiting)
            {
                wait_frames--;

                if (wait_frames <= 0)
                {
                    soundBank.PlayCue("turbo_charge");
                    ai_state = AiState.charging;
                    state = State.charge;
                    charge_count = 0;
                }
            }


            if (ai_state == AiState.kill_kill_kill)
            {
                ai_state = AiState.waiting;
                
                if (wait_frames == 0)
                    wait_frames = 60;
            }

            if (ai_state == AiState.idle)
            {
                idle_frames--;
                //idle();

                if (idle_frames < 0)
                {
                    soundBank.PlayCue("turbo_charge");
                    state = State.charge;
                    ai_state = AiState.charging;
                }
            }

            if (ai_state == AiState.charging)
            {
                //punch_rect = do_charge();
                if (state != State.charge)
                    state = State.charge;

                if (charge_count < 3)
                {
                    if (facing == Direction.left && position.Right < camera.viewport_rect.Left - 200)
                    {
                        charge_count++;
                        facing = Direction.right;
                        posY = cm.get_random_location(Singletons.random, position).Y;
                    }

                    else if (facing == Direction.right && position.X > camera.viewport_rect.Right + 200)
                    {
                        charge_count++;
                        facing = Direction.left;
                        posY = cm.get_random_location(Singletons.random, position).Y;
                    }
                }
                else
                {
                    if (facing == Direction.left && position.X < camera.viewport_rect.Center.X)
                    {
                        charge_count = 0;
                        ai_state = AiState.idle;
                        state = State.idle;
                        idle_frames = 100;
                    }
                    else if (facing == Direction.right && position.X > camera.viewport_rect.Center.X)
                    {
                        charge_count = 0;
                        ai_state = AiState.idle;
                        state = State.idle;
                        idle_frames = 100;
                    }
                }
            }
        }

        private new void handle_state()
        {
            punch_rect = Collision.Empty;

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

                case State.snap_kick:
                    punch_rect = do_attack();
                    break;

                case State.grabbed:
                    do_grabbed();
                    break;

                case State.charge:
                    punch_rect = do_charge();
                    break;

                default:
                    idle();
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

        public override void Update(Rectangle position)
        {
            handle_state();
            
            if (state == State.charge || state == State.idle)
                do_ai();
        }

        private Collision do_charge()
        {
            animate_check();

            if (facing == Direction.left)
                posX -= 12;
            else
                posX += 12;

            return getCollision();
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
                    soundBank.PlayCue("turbo_charge");
                    state = State.charge;
                    ai_state = AiState.charging;
                }
                else if (current_animation.repeats == false)
                    state = State.idle;

                boundX = current_animation.init_x;
            }
        }
    }
}
