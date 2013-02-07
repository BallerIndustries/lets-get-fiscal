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
    public class Guido : BadGuy
    {
        bool[] frame = new bool[5];
        
        public Guido(string sheet, int hp, string coll_name, Singletons singletons) : 
            base(sheet, hp, coll_name, singletons)
        {
            writable_attacks.Clear();
            foreach (AttackSequence atk_seq in attacks)
            {
                writable_attacks.Add(new AttackSequence(atk_seq));
            }

            grab_amount = 0.75f;
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
                case State.walking:
                    if (ai_state == AiState.retreat)
                        retreat(destination);
                    else
                    {
                        if (walk_to(destination) == false)
                            ai_state = AiState.none;
                    }
                    break;

                case State.idle:
                    idle();
                    break;

                case State.dying:
                    die();
                    break;

                case State.KO_leap:
                    do_KO_leap();
                    break;

                case State.death_leap:
                    do_death_leap();
                    break;

                case State.KO:
                    do_KO();
                    break;

                case State.dead:
                    dead();
                    break;

                case State.getting_up:
                    animate_check();
                    break;

                case State.suplex:
                    do_suplex();
                    break;

                case State.punching:
                    punch_rect = do_attack();
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
                else if (i < 70)
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
                do_attack_sequence();
            }
        }

        private void do_attack_sequence()
        {
            //Go up to Stelven and punch him. SIMPLE.
            if (Math.Abs(ego_position.Center.X - position.Center.X) < 200)
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

                if (writable_attacks.Count == 1 && atk_seq.amount <= 0)
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

        public void start_suplex()
        {
            state = State.suplex;
            grabbee.state = State.being_thrown;
            leapX = position.Center.X; //leapX = posX;
            jumpY = posY;
            jump_dir = JumpDirection.upwards;

            if (facing == Direction.left)
                grabbee.facing = Direction.right;
            else
                grabbee.facing = Direction.left;

            frame[0] = frame[1] = frame[2] = frame[3] = frame[4] = false;
            //soundBank.PlayCue("throw_noise");
        }

        private void do_suplex()
        {
            animate_check();

            if (current_frame == 1 && frame[1] == false)
            {
                frame[1] = true;

                if (facing == Direction.right)
                    grabbee.posX -= 50;
                else
                    grabbee.posX += 50;

                grabbee.posY -= 50;
            }

            if (current_frame == 2 && frame[2] == false)
            {
                frame[2] = true;
                grabbee.current_frame = 1;
                grabbee.jumpY = this.baseline;
                //grabbee.state = State.being_thrown;
            }
        }

        //public override void set_destination(MoveDirection dir, int min_y)
        //{

        //}
    }
}
