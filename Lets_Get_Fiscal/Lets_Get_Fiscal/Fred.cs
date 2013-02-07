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
    public class Fred : BadGuy
    {
        private int ai_wait_amount = 100;
        private int mechanic_wait_amount;
        private int action_wait;
        private bool a_sent, b_sent, c_sent;
        private Icicles icicles;

        public Fred(string sheet, int hp, string coll_name, Singletons singletons, AiState init_ai_state, Icicles icicles)
            : base(sheet, hp, coll_name, singletons)
        {
            this.icicles = icicles;
        }

        public override void Update(Rectangle position)
        {
            icicles.Update();
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

                case State.sword_attack:
                    punch_rect = do_sword_attack();
                    break;

                case State.none:
                    idle();
                    break;

                case State.leap:
                    do_podium_jump();
                    break;

                case State.teleport_out:
                    do_teleport_out();
                    break;

                case State.sword_projectiles:
                    do_projectile_fire();
                    break;

                case State.sword_down:
                    do_sword_down();
                    break;

                case State.dash:
                    do_dash();
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
            //if (state == State.sword_projectiles)
            //    return;

            //ai_wait_amount--;

            //if (ai_wait_amount <= 0 && icicles.can_sword_down)
            //{
            //    start_sword_down();
            //    //start_podium_jump();
            //    //start_dash(Direction.left);
            //}

            if (state == State.death_leap || state == State.dead)
                return;

            switch (ai_state)
            {

                case AiState.none:
                    do_initial_ai();
                    break;

                case AiState.projectile_fire:
                    do_projectile_fire_ai();
                    break;

                case AiState.teleport_out:
                    do_teleport_out_ai();
                    break;

                case AiState.sword_attack:
                    do_sword_attack_ai();
                    break;

                case AiState.dash:
                    do_dash_ai();
                    break;

                case AiState.sword_down:
                    do_sword_down_ai();
                    break;

                case AiState.idle:
                    do_idle_ai();
                    break;

                case AiState.kill_kill_kill:
                    //if (state == State.idle)
                    //{
                    //    start_teleport_out();
                    //}
                
                    start_idle_ai();
                    break;
            }
        }

        private void start_idle_ai()
        {
            if (ai_wait_amount <= 0)
                ai_wait_amount = 120;
            state = State.idle;
            ai_state = AiState.idle;
        }

        private void do_idle_ai()
        {
            ai_wait_amount--;

            if (ai_wait_amount == 0)
            {
                start_teleport_out_ai();
            }
        }

        private void start_sword_down_ai()
        {
            ai_state = AiState.sword_down;

            start_sword_down();
            alpha_val = 255;
            facing = cm.face_ego(this.posX);
            
            posY = (int)MathHelper.Clamp(Singletons.ego.posY, cm.min_y, 540);
            posX = Singletons.ego.posX + Singletons.ego.posW + (int)(posW * 0.6f);
        }

        private void do_sword_down_ai()
        {
            if (state == State.idle)
                start_idle_ai();
        }

        private void start_projectile_fire_ai()
        {
            ai_state = AiState.projectile_fire;
            start_projectile_fire();
        }

        private void do_projectile_fire_ai()
        {
            if (state == State.idle)
            {
                start_idle_ai();
            }
        }

        private void start_dash_ai()
        {
            ai_state = AiState.dash;
            start_dash(Direction.right);
        }

        private void do_dash_ai()
        {
            if (state == State.idle)
            {
                start_projectile_fire_ai();
            }
        }


        private void start_sword_attack_ai()
        {

            start_sword_attack();

            ai_state = AiState.sword_attack;
            alpha_val = 255;
            facing = cm.face_ego(this.posX);

            


            //posY = Singletons.ego.posY;
            posY = (int)MathHelper.Clamp(Singletons.ego.posY, cm.min_y, 540);
            posX = Singletons.ego.posX + Singletons.ego.posW + (int)(posW * 0.6f);
        }

        private void do_sword_attack_ai()
        {
            if (state == State.idle)
            {
                start_dash_ai();
            }
        }


        private void start_teleport_out_ai()
        {
            start_teleport_out();
            ai_state = AiState.teleport_out;
            ai_wait_amount = 30;
        }

        private void do_teleport_out_ai()
        {
            if (state == State.idle)
            {
                ai_wait_amount--;

                if (ai_wait_amount == 0)
                {
                    //Randomly choose between sword down and sword attack   
                    int num = Singletons.random.Next(1000);

                    if (num < 500)
                        start_sword_attack_ai();
                    else
                        start_sword_down_ai();


                }
            }
        }

        private void do_initial_ai()
        {
            ai_wait_amount--;

            if (ai_wait_amount == 0)
            {
                soundBank.PlayCue("forza");
                start_podium_jump();
            }
            if (ai_wait_amount < 0 && state == State.idle)
            {
                start_projectile_fire();
                ai_state = AiState.projectile_fire;
            }
        }

        #region MECHANICS

        private void start_dash(Direction dir)
        {
            soundBank.PlayCue("dash");
            state = State.dash;
            facing = dir;
            leapX = position.Center.X;
        }

        private void do_dash()
        {
            if (facing == Direction.left)
                posX -= 14;
            else
                posX += 14;

            if (Singletons.ego.baseline >= posY)
            {
                posY = (int)MathHelper.Clamp(posY + 5, cm.min_y, Singletons.ego.baseline);
            }
            else
            {
                posY = (int)MathHelper.Clamp(posY - 5, cm.min_y, Singletons.ego.baseline);
            }

            if (facing == Direction.left && position.Center.X < camera.viewport_rect.Left)
                state = State.idle;
            else if (facing == Direction.right && position.Center.X > camera.viewport_rect.Right)
                state = State.idle;

            //if (Math.Abs(leapX - position.Center.X) > 300)
            //   state = State.idle;
        }

        private void start_sword_down()
        {
            soundBank.PlayCue("ground_smash");
            attack_id++;
            state = State.sword_down;
            action_wait = 30;
        }

        private void do_sword_down()
        {
            if (current_frame < 2)
                animate_check();
            else
            {
                Singletons.shake_screen = true;
                icicles.start_breaking();
                action_wait--;

                if (action_wait < 0)
                    animate_check();
            }
        }

        private void start_sword_attack()
        {
            soundBank.PlayCue("hiya");
            state = State.sword_attack;
        }

        private Collision do_sword_attack()
        {
            animate_check();

            return getCollision();
        }

        private void start_teleport_out()
        {
            state = State.teleport_out;
            mechanic_wait_amount = 10;
            //soundBank.PlayCue("fernando_teleport");
        }

        private void do_teleport_out()
        {
            if (current_frame < 2)
                animate_check();

            if (current_frame == 2)
            {
                mechanic_wait_amount--;

                if (mechanic_wait_amount == 0)
                {
                    alpha_val = MathHelper.Clamp(alpha_val - 90, 20, 255);
                    mechanic_wait_amount = 5;

                    if (alpha_val == 20)
                    {
                        posY = -500;
                        animate_check();
                    }
                }
            }
        }

        private void start_projectile_fire()
        {
            state = State.sword_projectiles;
            a_sent = b_sent = c_sent = false;

            facing = cm.face_ego(this.posX);
        }

        private void do_projectile_fire()
        {
            mechanic_wait_amount--;

            if (mechanic_wait_amount <= 0)
                animate_check();

            if (current_frame == 1 && !a_sent)
            {
                a_sent = true;
                create_icicle();
                mechanic_wait_amount = 15;
                soundBank.PlayCue("fer");
            }

            if (current_frame == 3 && !b_sent)
            {
                b_sent = true;
                create_icicle();
                mechanic_wait_amount = 15;
                soundBank.PlayCue("nan");
            }

            if (current_frame == 5 && !c_sent)
            {
                c_sent = true;
                create_icicle();
                //wait_amount = 30;
                soundBank.PlayCue("do");
            }
        }

        private void create_icicle()
        {
            //soundBank.PlayCue("bang");

            Rectangle icicle = new Rectangle();

            icicle.Y = position.Center.Y;

            if (facing == Direction.left)
                icicle.X = position.Left + 25;
            else
                icicle.X = position.Right - 50;

            Animation a = new Animation(new Rectangle(158, 891, 49, 26), 1, true, "bananas", State.hadoken);
            Projectile p = new Projectile(icicle, new Vector2(24, 0), facing, a, singletons, "spritesheets//fernando", this.baseline, this, 2.0f, 30);

            pm.add_projectile(p);
        }

        private void start_podium_jump()
        {
            jumpY = posY;
            state = State.leap;
            jump_dir = JumpDirection.upwards;
        }

        private void do_podium_jump()
        {
            //Handle Y movement
            if (jump_dir == JumpDirection.upwards)
                posY -= 12;
            else if (posY < Singletons.ego.posY)
                posY += 12;

            //Handle X movement
            if (posY < Singletons.ego.posY)
                posX -= 12;

            //Change jump direction
            if (Math.Abs(jumpY - posY) > 100 && jump_dir == JumpDirection.upwards)
                jump_dir = JumpDirection.downwards;
                
            //Control animation
            if (posY < Singletons.ego.posY && current_frame < 2)
                animate_check();

            if (posY >= Singletons.ego.posY)
                animate_check();

        }
        #endregion
    }
}
