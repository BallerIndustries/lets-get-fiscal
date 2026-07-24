using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace Lets_Get_Fiscal
{
    public class Kiki : BadGuy
    {
        private int whirl_x;
        private bool glass_thrown;

        public Kiki(string sheet, int hp, string coll_name, Singletons singletons)
            : base(sheet, hp, coll_name, singletons)
        {
        }

        public override void Update(Rectangle position)
        {
            ego_position.X = position.X;
            ego_position.Y = position.Bottom - 1;
            ego_position.Width = position.Width;
            ego_position.Height = 1;

            handle_state();

            if (state == State.idle || state == State.walking)
                do_ai();
        }

        private void handle_state()
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

                case State.whirling_attack:
                    punch_rect = do_whirling_attack();
                    break;

                case State.glass_throw:
                    do_glass_throw();
                    break;

                case State.walking:

                    if (ai_state == AiState.retreat)
                        retreat(destination);
                    else
                    {
                        if (walk_to(destination) == false)
                            ai_state = AiState.none;
                    }
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

        private void start_whirling_attack()
        {
            whirl_x = this.position.Center.X;
            state = State.whirling_attack;
        }

        private Collision do_whirling_attack()
        {
            animate_check();

            if (facing == Direction.left)
                posX -= 15;
            else
                posX += 15;

            if (Math.Abs(position.Center.X - whirl_x) > 800)
            {
                ai_state = AiState.none;
                state = State.idle;
            }
            return getCollision();
        }

        private void start_glass_throw()
        {
            state = State.glass_throw;
            glass_thrown = false;
        }

        private void do_glass_throw()
        {
            animate_check();

            if (current_frame == 3 && glass_thrown == false)
            {
                Rectangle glass = new Rectangle();
                glass_thrown = true;

                glass.Y = position.Center.Y;

                if (facing == Direction.left)
                    glass.X = position.Left;
                else
                    glass.X = position.Right;
                
                //glass.Width = 10;
                //glass.Height = 10;

                Animation a = new Animation(new Rectangle(0, 333, 50, 50), 4, true, "bananas", State.hadoken);
                Projectile p = new Projectile(glass, new Vector2(12, 0), facing, a, singletons, "spritesheets//kiki", baseline, this, 2.0f, 30);

                pm.add_projectile(p);
                
            }

            if (current_frame == 3 && moves >= current_animation.frames - 1)
            {
                state = State.idle;
                ai_state = AiState.none;
            }
        }

        private void do_ai()
        {
            if (ai_state == AiState.none || ai_state == AiState.kill_kill_kill)
            {
                int i = Singletons.random.Next(100);

                if (i < 20)
                {
                    ai_state = AiState.walk;
                    destination = cm.get_random_location(Singletons.random, this.position);
                }
                else if (i < 30)
                {
                    ai_state = AiState.idle;
                    state = State.idle;
                    idle_frames = 100;
                }
                else if (i < 65)
                {
                    ai_state = AiState.whirling;
                }
                else if (i < 100)
                {
                    ai_state = AiState.throw_glass;
                }
            }
            
            else if (ai_state == AiState.idle)
            {
                if (idle_frames > 0)
                {
                    idle_frames--;
                }
                else
                {
                    ai_state = AiState.none;
                }
            }

            else if (ai_state == AiState.retreat  || ai_state == AiState.walk)
            {
                state = State.walking;

                if (destination_reached())
                    ai_state = AiState.none;
            }

            else if (ai_state == AiState.whirling)
            {
                int distance = Math.Abs(ego_position.Center.X - position.Center.X);

                if (distance > 200 && distance < 500)
                {
                    facing = cm.face_ego(this.position.X);
                    start_whirling_attack();
                }
            }
                    

            else if (ai_state == AiState.throw_glass)
            {
                int distance = Math.Abs(ego_position.Center.X - position.Center.X);

                if (distance > 200 && distance < 500)
                {
                    facing = cm.face_ego(this.position.X);
                    start_glass_throw();
                }
            }
        }
    }
}
