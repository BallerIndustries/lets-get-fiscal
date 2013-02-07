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
    public class Grab : Animation
    {
        public int damage;
        public bool grabbing;
        public Point grab_point;

        public Grab(Rectangle bound, int numFrames, Character.State move, bool grabbing, Point grab_point)
            : base(bound, numFrames, true, "jur", move)
        {
            this.grab_point = grab_point;
            this.grabbing = grabbing;
        }

        public Grab(Rectangle bound, int numFrames, Character.State move, bool grabbing, Point grab_point, int frames)
            : base(bound, numFrames, true, "jur", move, frames)
        {
            this.grab_point = grab_point;
            this.grabbing = grabbing;
        }

        public Grab(List<Rectangle> bounds, Character.State move, bool grabbing, Point grab_point) :
            base(bounds, false, move)
        {
            this.grab_point = grab_point;
            this.grabbing = grabbing;
        }

        public Grab(List<Rectangle> bounds, Character.State move, bool grabbing, Point grab_point, int frames) :
            base(bounds, false, move, frames)
        {
            this.grab_point = grab_point;
            this.grabbing = grabbing;
        }
    }
}