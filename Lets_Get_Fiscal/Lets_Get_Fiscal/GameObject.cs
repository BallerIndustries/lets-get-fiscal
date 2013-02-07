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
    public class GameObject : BasicGameObject
    {
        public enum Direction
        {
            none,
            left,
            right
        }

        public enum State
        {
            none,
            walking,
            punching,
            idle,
            jumping,
            dying,
            jumping_forward,
            jumping_backward,
            kicking,
            right_punch,
            snap_kick,
            uppercut,
            jur_kick,
            grab,
            shoruken,
            hurricane_kick,
            hadoken,
            dead,
            death_leap,
            KO_leap,
            KO,
            getting_up,
            grabbed,
            grabbing,
            dragon_rush,
            jump_kick1,
            jump_kick2,
            jump_kick3,
            rebound,
            walk_left,
            walk_right,
            walk_down,
            walk_up,
            suplex,
            being_thrown,
            force_walk,
            in_tact,
            broken,
            slide_attack,
            lunging_attack,
            brawl_attack,
            circle_kick,
            low_kick,
            mid_kick,
            high_kick,
            jump_kick,
            back_grab,
            front_grab,
            charge,
            leap_attack,
            item_get,
            grab_kick,
            front_grabbed,
            back_grabbed,
            back_attack,
            weapon_attack,
            fitty_brawl,
            grab_start,
            glass_throw,
            whirling_attack,
            teleport_in,
            teleport_out,
            shoot_laser,
            running,
            shooting,
            throwing_grenade,
            charging,
            knife_slash,
            horizontal_spin,
            vertical_spin,
            suicide,
            leap,
            dash,
            sword_projectiles,
            sword_attack,
            sword_down
        }

        public TextureManager tm;
        public SoundBank soundBank;
        public SpriteEffects se;
        public Singletons singletons;
        private Direction _facing = Direction.left;
        private State _state;
        public string sheet;
        private int _speed;
        public int speed
        {
            get { return _speed; }
            set { _speed = value; }
        }
        
        //private Rectangle _position;
        private Animation _current_animation;
        private MoveManager mm;
        public AnimationCollection move_list;
        public float scale;
        public int moves;
        public bool sprite_faces_right = true;
        public float alpha_val = 255;

        private int _current_frame;
        public int current_frame
        {
            get 
            { 
                if (current_animation.uses_list)
                    return _current_frame; 
                else
                    return (bound.X - current_animation.init_x) / bound.Width;  
            }
            set
            {
                if (current_animation.uses_list)
                {
                    _current_frame = value;
                    posW = current_animation.bound_list[value].Width;
                    posH = current_animation.bound_list[value].Height;
                }
                else
                {
                    bound = current_animation.bound;
                    boundX = current_animation.init_x + (value * current_animation.bound.Width);
                }
            }
        }

        private Rectangle _bound;
        public new Rectangle bound
        {
            get
            {
                if (current_animation.uses_list)
                    return current_animation.bound_list[current_frame];
                else
                    return _bound;
            }
            set
            {
                if (current_animation.uses_list == false)
                    _bound = value;
            }
        }

        public int boundX
        {
            get {   return _bound.X; }
            set {   _bound.X = value; }
        }

        public GameObject(string sheet, Singletons singletons, float scale)
            : base(Singletons.tm, sheet)
        {
            this.sheet = sheet;
            this.speed = 10;
            this.tm = Singletons.tm;
            this.current_animation = new Animation(new Rectangle(0, 0, 49, 87), 4, true, "idle", Character.State.idle);
            this.sheet = sheet;
            this.scale = scale;
            this.singletons = singletons;

            mm = Singletons.mm;
            move_list = mm.find_move_set(sheet);
        }

        public GameObject(string sheet, Singletons singletons)
            : base(Singletons.tm, sheet)
        {
            this.sheet = sheet;
            this.speed = 10;
            this.tm = Singletons.tm;
            this.current_animation = new Animation(new Rectangle(0, 0, 49, 87), 4, true, "idle", Character.State.idle);
            this.sheet = sheet;
            this.scale = 2.8f;
            this.singletons = singletons;

            mm = Singletons.mm;
            move_list = mm.find_move_set(sheet);
        }

        public State state
        {
            get
            { return _state; }
            set
            {
                //If we are changing states then re-assign current_animation.
                if (_state != value)
                {
                    _state = value;
                    if (move_list.getMove(value) != null)
                        current_animation = move_list.getMove(value);
                    else
                    {
                        _state = State.idle;
                        current_animation = new Animation(new Rectangle(0, 0, 49, 87), 4, true, "idle", Character.State.idle);
                    }
                }
            }
        }

        public Animation current_animation
        {
            get { return _current_animation; }
            set
            {
                _current_animation = value;

                bound = current_animation.bound;
                moves = 0;

                if (current_animation.uses_list)
                {
                    posW = current_animation.bound_list[current_frame].Width;
                    posH = current_animation.bound_list[current_frame].Height;
                }
                else
                {
                    posW = current_animation.bound.Width;
                    posH = current_animation.bound.Height;
                }
            }
        }

        public new int posX
        {
            set { base.posX  = value; }
            get { return base.posX; }
        }

        public new int posY
        {
            set { base.posY = value; }
            get { return base.posY; }
        }

        public new int posW
        {
            set { base.posW = value;  }
            get { return (int)(base.posW * scale); }
        }

        public new int posH
        {
            set { base.posH = value; }
            get { return (int)(base.posH * scale); }
        }

        public override int baseline
        {
            get
            { return posY; }
        }

        public new Rectangle position
        {
            get
            {
                if (facing == Direction.left)
                    return new Rectangle(posX - posW, posY - posH, posW, posH);
                else
                    return new Rectangle(posX, posY - posH, posW, posH);
            }
            set
            {
                base.position = value;
            }
        }

        public void set_p_facing(Direction facing)
        {
            _facing = facing;
        }

        public virtual Direction facing
        {
            get
            { return _facing; }
            set
            {
                //Sprite effects stuff
                if (sprite_faces_right)
                {
                    if (value == Direction.left)
                        se = SpriteEffects.FlipHorizontally;
                    else
                        se = SpriteEffects.None;
                }
                else
                {
                    if (value == Direction.right)
                        se = SpriteEffects.FlipHorizontally;
                    else
                        se = SpriteEffects.None;
                }

                //Adjust the posX if we have changed facing.

                if (value != _facing)
                {
                    if (value == Direction.left)
                        posX += posW;
                    else
                        posX -= posW;
                }
                _facing = value;
            }
        }

        public Vector2 bottom_left
        {
            get { return new Vector2(posX, posY - posH); }
        }

        public bool animate_check()
        {
            if (moves >= current_animation.frames)
            {
                moves = 0;
                increment_animation();
                return true;
            }
            else
            {
                moves++;
                return false;
            }
        }

        public void increment_animation()
        {
            if (current_animation.uses_list)
                increment_animation_list();
            else
                increment_animation_vanilla();
        }

        public void increment_animation_list()
        {
            int last_frame_num = current_animation.bound_list.Count - 1;

            if (current_frame < last_frame_num)
                current_frame++;
            else
            {
                current_frame = 0;

                //Should we return to the idle animation or go to the first frame.
                if (state == State.death_leap)
                    state = State.dead;
                else if (state == State.KO_leap || state == State.being_thrown)
                    state = State.KO;
                else if (current_animation.repeats == false)
                {
                    state = State.idle;

                    Ego e = this as Ego;
                    if (e != null)
                    {
                        Singletons.camera.snap_to_camera(e);
                    }
                }
            }
        }

        public virtual void increment_animation_vanilla()
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
                else if (current_animation.repeats == false)
                {
                    state = State.idle;

                    Ego e = this as Ego;
                    if (e != null)
                    {
                        Singletons.camera.snap_to_camera(e);
                    }
                }
                boundX = current_animation.init_x;
            }
        }
    }
}
