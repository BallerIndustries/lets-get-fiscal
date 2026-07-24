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
    public partial class Ego : Character
    {   
        private bool brawled_once = false;
        private bool receieved_damage = false;
        private bool can_move = true;
        public Point walk_to;
        private KeyboardState kbState, prevkbState;
        private GamePadState gpState, prevgpState;
        
        private UInt64 time_ms = 0;
        private CircularBuffer previous_actions = new CircularBuffer();
        public int lives = 3;
        bool[] frame = new bool[5];

        public int idle_count;
        
        
        public Ego(string sheet, int hp, string coll_name, Singletons singletons, float scale) : 
            base(sheet, hp, coll_name, singletons, scale)
        {
            id = -1;
            name = "Stelven";
            portrait = tm.find_texture("portraits//acc_portrait");
            death_noise = "accountant_death";
        }

        public Ego(string sheet, int hp, string coll_name, Singletons singletons) :
            base(sheet, hp, coll_name, singletons, 2.8f)
        {
            id = -1;
            name = "Stelven";
            portrait = tm.find_texture("portraits//acc_portrait");
            death_noise = "accountant_death";
        }

        public void Initialise()
        {
            brawled_once = false;
            time_ms = 0;
            previous_actions.Clear();

            hp = max_hp;
            facing = Character.Direction.right;
            position = new Rectangle(-200, 500, 0, 0);
            state = GameObject.State.force_walk;
            walk_to = new Point(200, 500);
            lives = 3;
            alpha_val = 255;
            weapon = null;
            idle_count = 0;
        }


        //public override Direction facing
        //{
        //    get
        //    { return base.facing; }
        //    set
        //    {
        //        //Sprite effects stuff
        //        if (sprite_faces_right)
        //        {
        //            if (value == Direction.left)
        //                se = SpriteEffects.FlipHorizontally;
        //            else
        //                se = SpriteEffects.None;
        //        }
        //        else
        //        {
        //            if (value == Direction.right)
        //                se = SpriteEffects.FlipHorizontally;
        //            else
        //                se = SpriteEffects.None;
        //        }

        //        //Adjust the posX if we have changed facing.

        //        if (value != base.facing)
        //        {
        //            if (value == Direction.left)
        //                posX += posW / 2;
        //            else
        //                posX -= posW / 2;
        //        }
        //        set_p_facing(value);
        //    }
        //}

        //This ridiculous code that shouldn't be here signifies that the ego's last 
        //punch or whatever successfully fucked up some bad guy.
        public void last_move_hit()
        {
            previous_actions.last_move_hit();
        }

        public void respawn()
        {
            facing = Direction.right;
            posX = camera.left_of_cam(-200);
            hor_jump_dir = HorJumpDirection.none;

            jumpY = (cm.min_y + 540) / 2;
            posY = -200;

            state = State.jumping;
            jump_dir = JumpDirection.downwards;
            jump_speed = 20;

            alpha_val = 255;
            hp = max_hp;
            cm.KO_all = true;
        }

        public new void dead()
        {
            base.dead();

            //Then eat a large bowl of shit and go fuck yourself!
            if (alpha_val == 0)
            {
                lives--;
                if (lives >= 0)
                    respawn();
                else
                {
                    cm.hud.draw_hud = false;

                }
                //cm.hud.

                //singletons.
                //else
                //{
                //    Singletons.music_category.Stop(AudioStopOptions.Immediate);
                //    game_state.state = GameState.State.Menu;
                //    
                //}
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
            soundBank.PlayCue("throw_noise");
        }

        public void do_suplex()
        {
            animate_check();

            do_suplex_0_to_4();

            //Upwards and horizontally. Goes to five when reaching the peak.
            if (current_frame == 4)
            {
                moves = 0;
                posY -= 4;

                if (facing == Direction.left)
                    delta_position.X = -4; //posX -= 4;
                else
                    delta_position.X = 4;//posX += 4;

                if (jumpY - posY > 50)
                    current_frame = 5;
            }

            //Downwards and horizontally. Goes to six when passing jumpY
            if (current_frame == 5)
            {
                moves = 0;
                posY += 4;

                if (facing == Direction.left)
                    delta_position.X = -4; //posX -= 4;
                else
                    delta_position.X = 4; //posX += 4;

                if (posY >= jumpY)
                {
                    posY = jumpY;
                    current_frame = 6;
                }
            }
        }

        void frame_spec_action(int frame_num, int x_diff, int y_diff)
        {
            if (facing == Direction.left)
                x_diff *= -1;

            //If we haven't already done this action
            if (frame[frame_num] == false)
            {
                grabbee.posX += x_diff;
                grabbee.posY += y_diff;
                frame[frame_num] = true;

                if (frame_num == 0 && facing == Direction.right)
                    grabbee.posX = this.posX + 300;
                else if (frame_num == 0 && facing == Direction.left)
                    grabbee.posX = this.posX - 300;

                if (frame_num == 3)
                {
                    grabbee.current_frame = 1;
                    grabbee.jumpY = this.jumpY;
                }
            }
        }

        public void do_suplex_0_to_4()
        {
            switch (current_frame)
            {
                case 0:
                    frame_spec_action(0, 0, 0);
                    break;

                case 1:
                    frame_spec_action(1, -20, -20);
                    break;

                case 2:
                    frame_spec_action(2, -50, -80);
                    break;

                case 3:
                    frame_spec_action(3, -120, 50);
                    break;

                case 4:
                    frame_spec_action(4, -50, 50);
                    break;
            }
        }


        public void start_brawl_attack()
        {
            attack_id++;

            state = State.brawl_attack;
            jumpY = posY;
            brawled_once = false;
            jump_dir = JumpDirection.upwards;
            hor_jump_dir = HorJumpDirection.none;
        }

        public Collision do_brawl_attack()
        {
            animate_check();

            //This code makes frames 0 - 5 repeat once
            if (current_frame == 7 && brawled_once == false)
            {
                brawled_once = true;
                current_frame = 0;
            }

            if (current_frame == 9)
            {
                //This is a dirty way of negating the effects of animate_check() disgusting
                moves = 0;

                if (jumpY - posY > 200)
                    current_frame = 10;

                posY -= 14;
            }

            if (current_frame == 10 || current_frame == 11)
            {
                posY += 14;
            }

            if (current_frame == 11)
            {
                //Dirty trick
                moves = 0;

                //Test if we have hit the ground
                if (posY >= jumpY)
                {
                    //Take some damage
                    //hp -= 8;
                    soundBank.PlayCue("land_noise");

                    posY = jumpY;
                    current_frame = 12;
                }
            }

            return getCollision();
        }


        public void start_lunge_attack()
        {
            attack_id++;
            soundBank.PlayCue("charging_face");

            state = State.lunging_attack;
            end_animation = false;
            leapX = position.Center.X;//leapX = posX;
        }

        public Collision do_lunge_attack()
        {
            animate_check();

            if (Math.Abs(leapX - position.Center.X) < 400 && !end_animation)
            {
                if (current_frame >= 3)
                {
                    if (facing == Direction.right)
                        delta_position.X = 12;
                    else
                        delta_position.X = -12;
                }

                //This effectively repeats the last two frames 
                if (current_frame == 5)
                    current_frame = 3;

                //We want to set the current frame once we pass the threshold.
                if (Math.Abs(leapX - position.Center.X) >= 400)
                {
                    moves = 0;
                    current_frame = 2;
                }
            }
            else
            {
                if (facing == Direction.right)
                    delta_position.X = 4;
                else
                    delta_position.X = -4;

                if (moves == current_animation.frames - 1)
                    state = State.idle;
            }

            return getCollision();
        }

        public Collision do_jump_kick()
        {
            if (state == State.jump_kick1)
            {
                if (current_frame == 0)
                    animate_check();
            }
            else if (state == State.jump_kick2)
            {
                if (current_frame < 2)
                    animate_check();
            }

            return getCollision();
        }

        public void start_weapon_attack()
        {
            state = State.weapon_attack;
            attack_id++;

            State move = look_for_combo(State.punching);
            add_combo(move);

            //previous_actions.Add(new ComboNode(time_ms, move));
            if (move == State.lunging_attack)
                start_lunge_attack();
            else
                soundBank.PlayCue(weapon.sfx_name);
        }

        public void start_circle_kick()
        {
            state = State.circle_kick;
            add_combo(State.circle_kick);
            receieved_damage = false;
        }

        public Collision do_circle_kick()
        {
            animate_check();

            //if (previous_actions.get_from_tail(1).did_hit && !receieved_damage)
            //{
            //    receieved_damage = true;
            //    hp -= 5;
            //}

            return getCollision();
        }

        public new void start_attack(State move)
        {
            move = look_for_combo(move);
            add_combo(move);
            
            //previous_actions.Add(new ComboNode(time_ms, move));
            if (move == State.lunging_attack)
                start_lunge_attack();
            else if (move == State.brawl_attack)
                start_brawl_attack();
            else if (move == State.circle_kick)
                start_circle_kick();
            else
                base.start_attack(move);
        }

        public void add_combo(State move)
        {
            previous_actions.add_node(new ComboNode(time_ms, move));
            //previous_actions.Add(new ComboNode(time_ms, move));
        }

        State look_for_combo(State move)
        {
            ComboNode last_action = previous_actions.get_from_tail(1);
            ComboNode second_last_action = previous_actions.get_from_tail(2);
            ComboNode third_last_action = previous_actions.get_from_tail(3);
            //ComboNode fourth_last_action = previous_actions.get_from_tail(4);

            if (last_action == null || second_last_action == null || move == State.circle_kick)
                return move;

            if (move == State.punching && second_last_action.action == Character.State.walk_left && last_action.action == Character.State.walk_left &&
               (last_action.time - second_last_action.time) < 20 && (time_ms - last_action.time) < 20)
                return State.lunging_attack;

            if (move == State.punching && second_last_action.action == Character.State.walk_right && last_action.action == Character.State.walk_right &&
               (last_action.time - second_last_action.time) < 20 && (time_ms - last_action.time) < 20)
                return State.lunging_attack;


            if (move == State.punching && second_last_action.action == Character.State.walk_left && last_action.action == Character.State.walk_right &&
               (last_action.time - second_last_action.time) < 20 && (time_ms - last_action.time) < 20)
                return State.brawl_attack;

            if (move == State.punching && second_last_action.action == Character.State.walk_right && last_action.action == Character.State.walk_left &&
               (last_action.time - second_last_action.time) < 20 && (time_ms - last_action.time) < 20)
                return State.brawl_attack;

            if (third_last_action != null)
            {

                if (second_last_action.action == State.walk_down && last_action.action == State.walk_left
                    && last_action.time - second_last_action.time < 20 && time_ms - last_action.time < 20)
                    return State.circle_kick;

                if (second_last_action.action == State.walk_down && last_action.action == State.walk_right
                    && last_action.time - second_last_action.time < 20 &&  time_ms - last_action.time < 20)
                    return State.circle_kick;
            }

            if (weapon != null)
                return State.weapon_attack;

            //Look for the right handed punch
            if (last_action.action == Character.State.punching && last_action.did_hit &&
                second_last_action.action == Character.State.punching && second_last_action.did_hit &&
                (time_ms - last_action.time) < 50 && (last_action.time - second_last_action.time) < 50)
                return State.right_punch;

            //Look for the major side kick
            if (last_action.action == Character.State.right_punch && last_action.did_hit && (time_ms - last_action.time) < 50)
                return State.kicking;

            return move;
        }

        public void Update(KeyboardState kbState, KeyboardState prevkbState, GamePadState gpState, GamePadState prevgpState, bool can_move)
        {
            punch_rect = Collision.Empty;

            this.kbState = kbState;
            this.prevkbState = prevkbState;
            this.gpState = gpState;
            this.prevgpState = prevgpState;
            this.can_move = can_move;

            if (slow_for > 0)
            {
                slow_for--;
                return;
            }


            //If state wasn't handled go to handle_input
            bool state_handled = handle_state();
            
            if (state_handled == false)
                handle_input();

            //Check if the move is alright
            if (delta_position != Point.Zero)
            {
                if (cm.position_ok(this, new_xPosition))
                    posX += delta_position.X;
                if (cm.position_ok(this, new_yPosition))
                    posY += delta_position.Y;

                delta_position = Point.Zero;
            }

            

            if (left_once())
                add_combo(State.walk_left);

            if (right_once())
                add_combo(State.walk_right);

            if (down_once())
                add_combo(State.walk_down);

            if (up_once())
                add_combo(State.walk_up);

            //if (kbState.GetPressedKeys().
            
            time_ms++;
        }

        
        private bool handle_state()
        {
            bool state_handled = true;

            if (state == Character.State.jumping)
            {
                calculate_jump();
                animate_jump();

                if (pressed_once(Keys.E) && pressed_once(Buttons.A))
                {
                    attack_id++;
                    state = State.jump_kick1;
                }
            }
            else if (state == State.jump_kick1 || state == State.jump_kick2 || state == State.jump_kick3)
            {
                calculate_jump();
                punch_rect = do_jump_kick();
            }
            else if (state == State.item_get)
                animate_check();

            else if (state == Character.State.death_leap)
                do_death_leap();
            else if (state == Character.State.KO_leap)
                do_KO_leap();
            else if (state == State.weapon_attack)
                punch_rect = do_attack();

            else if (state == State.rebound)
                animate_check();
            else if (state == Character.State.dead)
                dead();
            else if (state == Character.State.KO)
                do_KO();
            else if (state == Character.State.dying)
                die();
            else if (state == State.getting_up)
                animate_check();
            else if (state == State.back_attack)
                punch_rect = do_attack();
            else if (state == State.circle_kick)
                punch_rect = do_circle_kick();
            else if (state == State.lunging_attack)
                punch_rect = do_lunge_attack();
            else if (state == State.brawl_attack)
                punch_rect = do_brawl_attack();

            else if (state == State.back_grab || state == State.front_grab)
            {
                animate_check();

                if ((pressed_once(Keys.W) || pressed_once(Buttons.X)) && hp > 15)
                {
                    if (left() || right())
                        start_brawl_attack();
                    else
                        start_circle_kick();
                }

                else if (pressed_once(Keys.E) || pressed_once(Buttons.A))
                {
                    start_suplex();
                }
            }
            else if (state == State.being_thrown)
                do_being_thrown();

            else if (state == Character.State.punching)
                punch_rect = do_attack();

            else if (state == Character.State.kicking)
                punch_rect = do_attack();

            else if (state == Character.State.right_punch)
                punch_rect = do_attack();

            else if (state == State.suplex)
                do_suplex();

            else if (state == State.force_walk)
                do_force_walk();
            else if (state == State.being_thrown)
                do_being_thrown();



            else if (can_move == false)
                animate_check();
            else
                state_handled = false;

            return state_handled;
        }

        private void handle_input()
        {
            if (both_pressed(Keys.E, Keys.Q) || both_pressed(Buttons.A, Buttons.B))
                start_attack(State.back_attack);

            else if (pressed_once(Keys.Q) || pressed_once(Buttons.B))
            {
                if (right())
                {
                    facing = Direction.right;
                    hor_jump_dir = HorJumpDirection.right;
                }
                else if (left())
                {
                    facing = Direction.left;
                    hor_jump_dir = HorJumpDirection.left;
                }
                else
                    hor_jump_dir = HorJumpDirection.none;

                startJump();
            }
            //else if ((pressed_once(Keys.W) || pressed_once(Buttons.X)) && hp > 15)
            //{
            //    if (left() || right())
            //        start_brawl_attack();
            //    else
            //        start_circle_kick();
            //}
            else if (pressed_once(Keys.E) || pressed_once(Buttons.A))
            {
                Item i = cm.check_for_item(this);

                if (i == null)
                {
                    if (weapon == null)
                        start_attack(Character.State.punching);
                    else
                        start_weapon_attack();
                }
                else
                {
                    state = State.item_get;
                    i.visible = false;
                    process_item(i);
                }
            }
            else if (right() || left() || up() || down() )
            {
                //1. Move if the player wants us to
                if (left())
                    moveLeft();
                else if (right())
                    moveRight();

                if (up())
                    moveUp();
                else if (down())
                    moveDown();

                state = State.walking;
                animate_check();
            }

            else
                idle();

            if (pressed(Keys.E) || pressed(Keys.Q) || pressed(Buttons.A))
                idle_count = 0;
            else
                idle_count++;
        }

        private bool pressed_once(Keys key)
        {
#if WINDOWS
            return kbState.IsKeyDown(key) && prevkbState.IsKeyUp(key);
#else
            return false;
#endif
        }

        private bool pressed_once(Buttons button)
        {
#if XBOX
            return gpState.IsButtonDown(button) && prevgpState.IsButtonUp(button);
#else
            return false;
#endif
        }

        private bool pressed(Keys key)
        {
            return kbState.IsKeyDown(key);
        }

        private bool pressed(Buttons button)
        {
            return gpState.IsButtonDown(button);
        }

        private bool both_pressed(Keys keyA, Keys keyB)
        {
            return kbState.IsKeyDown(keyA) && kbState.IsKeyDown(keyB) && (prevkbState.IsKeyUp(keyA) || prevkbState.IsKeyUp(keyB));
        }

        private bool both_pressed(Buttons buttonA, Buttons buttonB)
        {
            return gpState.IsButtonDown(buttonA) && gpState.IsButtonDown(buttonB) && (prevgpState.IsButtonUp(buttonA) || prevgpState.IsButtonUp(buttonB));
        }

        private bool any_walking()
        {
            
#if WINDOWS
            return kbState.IsKeyDown(Keys.Left) || kbState.IsKeyDown(Keys.Right) || kbState.IsKeyDown(Keys.Up) || kbState.IsKeyDown(Keys.Down);
#else
            return gpState.IsButtonDown(Buttons.DPadLeft) || gpState.IsButtonDown(Buttons.DPadRight) || gpState.IsButtonDown(Buttons.DPadUp) || gpState.IsButtonDown(Buttons.DPadDown)
                || Math.Abs(gpState.ThumbSticks.Left.X) > 0.5f || Math.Abs(gpState.ThumbSticks.Left.Y) > 0.5f;
#endif      
        }

        private bool up()
        {
#if WINDOWS
            return pressed(Keys.Up);
#else
            return pressed(Buttons.DPadUp) || gpState.ThumbSticks.Left.Y > 0.5f;
#endif
        }

        private bool down()
        {
#if WINDOWS
            return pressed(Keys.Down);
#else
            return pressed(Buttons.DPadDown) || gpState.ThumbSticks.Left.Y < -0.5f;
#endif
        }

        private bool left()
        {
#if WINDOWS
            return pressed(Keys.Left);
#else
            return pressed(Buttons.DPadLeft) || gpState.ThumbSticks.Left.X < -0.5f;
#endif
        }

        private bool right()
        {
#if WINDOWS
            return pressed(Keys.Right);
#else
            return pressed(Buttons.DPadRight) || gpState.ThumbSticks.Left.X > 0.5f;
#endif
        }

        private bool left_once()
        {
#if WINDOWS
            return pressed_once(Keys.Left);
#else
            return pressed_once(Buttons.DPadLeft) || gpState.ThumbSticks.Left.X < -0.5f && prevgpState.ThumbSticks.Left.X >= -0.5f;
#endif
        }

        private bool right_once()
        {
#if WINDOWS
            return pressed_once(Keys.Right);
#else
            return pressed_once(Buttons.DPadRight) || gpState.ThumbSticks.Left.X > 0.5f && prevgpState.ThumbSticks.Left.X <= 0.5f;
#endif
        }

        private bool down_once()
        {
#if WINDOWS
            return pressed_once(Keys.Down);
#else
            return pressed_once(Buttons.DPadDown) || gpState.ThumbSticks.Left.Y < -0.5f && prevgpState.ThumbSticks.Left.Y >= -0.5f;
#endif
        }

        private bool up_once()
        {
#if WINDOWS
            return pressed_once(Keys.Up);
#else
            return pressed_once(Buttons.DPadUp) || gpState.ThumbSticks.Left.Y > 0.5f && prevgpState.ThumbSticks.Left.Y <= 0.5f;
#endif
        }



        public override Collision getCollision()
        {
            //The following code is from do_attack(). It does not call animate_check()
            AttackAnimation aa = current_animation as AttackAnimation;
            Rectangle region;

            if (aa == null)
                return Collision.Empty;

            if (aa.attacks[current_frame] != Rectangle.Empty)
            {
                region = getAttackRect(bound, aa.attacks[current_frame]);
                return new Collision(Collision.collision_type.ego, region, aa.damage, baseline, facing, aa.attack_does_ko[current_frame], aa.sfx_name, this, null);
            }
            else
                return Collision.Empty;
        }

        public void do_force_walk()
        {
            //Walk to the walk_to location
            if (posX < walk_to.X)
            {
                posX = (int)MathHelper.Clamp(posX + speed, -10000, walk_to.X);
            }
            else if (posX > walk_to.X)
            {
                posX = (int)MathHelper.Clamp(posX - speed, walk_to.X, 10000);
            }

            if (posY < walk_to.Y)
            {
                posY = (int)MathHelper.Clamp(posY + speed, -10000, walk_to.Y);
            }
            else if (posY > walk_to.Y)
            {
                posY = (int)MathHelper.Clamp(posY - speed, -walk_to.Y, 10000);
            }

            animate_check();

            if (posX == walk_to.X && posY == walk_to.Y)
            {
                state = State.idle;
            }
        }

        public virtual Collision do_being_thrown()
        {
            if (current_frame == 1)
            {
                if (facing == Direction.right)
                    posX -= 12;
                else
                    posX += 12;

                posY += 12;

                if (posY >= jumpY)
                {
                    posY = jumpY;
                    current_frame = 2;
                    soundBank.PlayCue("thump");
                    Singletons.shake_screen = true;
                    jump_dir = JumpDirection.upwards;

                    hp -= 20;

                    if (hp <= 0)
                    {
                        soundBank.PlayCue(death_noise);
                    }
                }
            }

            if (current_frame == 2)
            {
                if (facing == Direction.right)
                    posX -= 6;
                else
                    posX += 6;

                if (jump_dir == JumpDirection.upwards)
                {
                    posY -= 6;

                    if (Math.Abs(jumpY - posY) > 50)
                        jump_dir = JumpDirection.downwards;
                }

                if (jump_dir == JumpDirection.downwards)
                {
                    posY += 6;

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

            return Collision.Empty;
            //return new Collision(Collision.collision_type.ego, this.position, 10, this.baseline, this.facing, true, "pah", this, null);
        }

        void process_item(Item  i)
        {
            switch (i.type)
            {
                case Item.Type.coke:
                    soundBank.PlayCue("hp_up");
                    hp = (int)MathHelper.Clamp(hp + 20, 0, max_hp);
                    break;

                case Item.Type.steak:
                    soundBank.PlayCue("hp_up");
                    hp = max_hp;
                    break;

                case Item.Type.diamonds:
                    soundBank.PlayCue("money");
                    game_state.score += 1000;
                    break;

                case Item.Type.cash:
                    soundBank.PlayCue("money");
                    game_state.score += 500;
                    break;

                case Item.Type.one_up:
                    soundBank.PlayCue("one_up");
                    lives += 1;
                    break;

                case Item.Type.baseball_bat:
                    weapon = i as Weapon;
                    break;

                ////case Item.Type.lead_pipe:
                ////    weapon = i as Weapon;
                ////    break;

            }
        }
    }
}
