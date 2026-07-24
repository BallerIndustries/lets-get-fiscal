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
    public struct WeaponData
    {
        public Point position;
        public int frame;

        public static WeaponData Empty
        {
            get { return new WeaponData(); }
        }

        public static bool operator ==(WeaponData a, WeaponData b)
        {
            return a.position == b.position && a.frame == b.frame;
        }

        public static bool operator !=(WeaponData a, WeaponData b)
        {
            return a.position != b.position || a.frame != b.frame;
        }

        public WeaponData(Point position, int frame)
        {
            this.position = position;
            this.frame = frame;
        }
    }

    public class Animation
    {
        public readonly Rectangle bound;
        public List<Rectangle> bound_list;
        //public int current_frame = 0;
        public readonly bool uses_list;
        public readonly int frames;
        private readonly int _numFrames;
        private readonly bool _repeats;
        public readonly string name;
        public readonly Character.State move;
        public readonly int init_x;
        public WeaponData[] weapon_data;

        public Animation()
            : this(new Rectangle(0, 0, 0, 0), 0, false, "", Character.State.idle)
        {
        }

        public Animation(Rectangle bound, int numFrames, bool repeats, string name, Character.State move, int frames)
        {
            this.bound = bound;
            this._numFrames = numFrames;
            this._repeats = repeats;
            this.name = name;
            this.move = move;
            this.frames = frames;

            init_x = bound.X;
            uses_list = false;
        }

        public Animation(Rectangle bound, int numFrames, bool repeats, string name, Character.State move)
        {
            this.bound = bound;
            this._numFrames = numFrames;
            this._repeats = repeats;
            this.name = name;
            this.move = move;
            this.frames = 6;

            init_x = bound.X;
            uses_list = false;
        }

        public Animation(List<Rectangle> bound_list, bool repeats, Character.State move, int frames)
        {
            this.bound_list = bound_list;
            this._repeats = repeats;
            this.move = move;
            this.frames = frames;

            uses_list = true;
        }

        public Animation(List<Rectangle> bound_list, bool repeats, Character.State move)
        {
            this.bound_list = bound_list;
            this._repeats = repeats;
            this.move = move;
            this.frames = 6;

            uses_list = true;
        }

        public void add_weapon_data(WeaponData wd, int frame)
        {
            if (weapon_data == null)
                weapon_data = new WeaponData[numFrames];

            //Measure distance from bottom left of the current frame.
            Point bottom_left;

            if (uses_list)
                bottom_left = new Point(bound_list[frame].Left, bound_list[frame].Bottom);
            else
                bottom_left = new Point(init_x + (bound.Width * frame), bound.Bottom);

            weapon_data[frame] = new WeaponData(new Point(wd.position.X - bottom_left.X, bottom_left.Y - wd.position.Y), wd.frame);
        }

        public void add_bound(Rectangle r)
        {
            bound_list.Add(r);
        }

        public int numFrames
        {
            get { return _numFrames; }
        }

        public bool repeats
        {
            get { return _repeats; }
        }

        //public Rectangle bound
        //{
        //    get
        //    {
        //        if (uses_list)
        //            return bound_list[current_frame];
        //        else
        //            return _bound;
        //    }
        //    set
        //    {
        //        if (uses_list == false)
        //            _bound = value;
        //    }
        //}

        //public int boundX
        //{
        //    set { _bound.X = value; }
        //    get { return _bound.X; }
        //}
    }
}


