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

namespace Lets_Get_Fiscal
{
    class Curtis : BadGuy
    {
        private int brawl_repeats;

        public Curtis(string sheet, int hp, string coll_name, Singletons singletons)
            : base(sheet, hp, coll_name, singletons)
        {
            writable_attacks.Clear();
            foreach (AttackSequence atk_seq in attacks)
            {
                writable_attacks.Add(new AttackSequence(atk_seq));
            }
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

                case State.grabbed:
                    do_grabbed();
                    break;

                case State.punching:
                    punch_rect = do_attack();
                    break;

                case State.right_punch:
                    punch_rect = do_attack();
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

                case State.leap_attack:
                    punch_rect = do_leap_attack();
                    break;

                case State.grab_start:
                    do_grab_start();
                    break;

                case State.fitty_brawl:
                    punch_rect = do_brawl_attack();
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

        private void do_ai()
        {
            if (ai_state == AiState.none || ai_state == AiState.kill_kill_kill)
            {
                int i = Singletons.random.Next(100);

                if (i < 50)
                {
                    ai_state = AiState.walk;
                    destination = cm.get_random_location(Singletons.random, this.position);
                }
                else if (i < 60)
                {
                    ai_state = AiState.idle;
                    state = State.idle;
                    idle_frames = 100;
                }
                else
                {
                    ai_state = AiState.punch;
                    writable_attacks.Clear();
                    foreach (AttackSequence atk_seq in attacks)
                        writable_attacks.Add(new AttackSequence(atk_seq));
                }


                //else if (num == 2)
                //    ai_state = AiState.leap;
                //else if (num == 3)
                //    ai_state = AiState.brawl;

            }
            else if (ai_state == AiState.idle)
            {
                if (idle_frames > 0)
                {
                    //state = State.idle;
                    //idle();
                    idle_frames--;
                }
                else
                {
                    ai_state = AiState.none;
                }
            }


            else if (ai_state == AiState.retreat || ai_state == AiState.walk)
            {
                state = State.walking;

                //If we are within a step away from the destination.
                if (destination_reached())
                    ai_state = AiState.none;
            }

            else if (ai_state == AiState.punch)
            {
                //Go up to Stelven and punch him. SIMPLE.
                if (cm.touching_ego(this))
                {
                    //Time for some cool bananas thinking. COOL BANANAS
                    AttackSequence atk_seq = writable_attacks[0];

                    if (atk_seq.amount <= 0 && writable_attacks.Count > 1)
                    {
                        writable_attacks.Remove(writable_attacks[0]);
                        atk_seq = writable_attacks[0];
                    }

                    if (atk_seq.state == State.idle)
                        state = State.idle;
                    else if (atk_seq.state != State.idle)
                    {
                        start_attack(atk_seq.state);
                    }

                    atk_seq.decrement_amount();

                    if (writable_attacks.Count == 1 && atk_seq.amount < 0)
                    {
                        state = State.walking;
                        ai_state = AiState.retreat;

                        if (facing == Direction.left)
                            destinationX = posX + 200;
                        else
                            destinationX = posX - 200;

                        destinationY = posY - 10;
                    }
                }
                else
                {
                    destination = ego_position;
                    state = State.walking;
                }
            }
            else if (ai_state == AiState.leap)
            {
                //Go NEAR Stelven and leap onto him.
                //if (cm.touching_ego(this))
                
                if (Math.Abs(ego_position.Center.X - position.Center.X) < 150)
                    start_leap_attack();
                else
                {
                    destination = ego_position;
                    state = State.walking;
                }
            }
            else if (ai_state == AiState.brawl)
            {
                if (cm.touching_ego(this))
                    start_grab_start();
                else
                {
                    destination = ego_position;
                    state = State.walking;
                }
            }
        }

        private void start_grab_start()
        {
            state = State.grab_start;
        }

        private void do_grab_start()
        {
            animate_check();

            if (current_frame == 1 && Math.Abs(ego_position.Center.X - this.position.Center.X) < 80)
            {
                start_brawl_attack();
            }

            if (current_frame == 1 && moves == current_animation.frames)
            {
                state = State.idle;
                ai_state = AiState.retreat;
            }
        }

        public void start_brawl_attack()
        {
            state = State.fitty_brawl;
            brawl_repeats = 0;
        }

        public Collision do_brawl_attack()
        {
            if (brawl_repeats < 1 && current_frame == 3)
            {
                brawl_repeats++;
                current_frame = 0;
            }
            animate_check();

            return getCollision();
        }

        private void start_leap_attack()
        {
            state = State.leap_attack;
            jumpY = posY;
            jump_dir = JumpDirection.upwards;
            attack_id++;
        }

        private Collision do_leap_attack()
        {
            if (facing == Direction.right)
                posX += 10;
            else
                posX -= 10;

            if (jump_dir == JumpDirection.upwards)
               posY -= 7;
            else
               posY += 7;

            if (jump_dir == JumpDirection.upwards && Math.Abs(jumpY - posY) > 100)
                jump_dir = JumpDirection.downwards;

            if (jump_dir == JumpDirection.downwards && posY > jumpY)
            {
                posY = jumpY;
                state = State.idle;
                ai_state = AiState.none;
            }

            return getCollision();
        }
    }
}
