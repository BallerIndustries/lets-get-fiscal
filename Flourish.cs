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
    public class Flourish
    {
        public Texture2D texture;
        TextureManager tm;
        public Rectangle bound;
        public Vector2 draw_position;
        public bool visible = true;
        int num_frames;
        int frames;
        readonly int init_x;

        public Flourish(string texture_name, TextureManager tm, Rectangle bound, int num_frames, Vector2 draw_position)
        {
            this.tm = tm;
            this.bound = bound;
            this.draw_position = draw_position;
            this.num_frames = num_frames;

            texture = tm.find_texture(texture_name);
            init_x = bound.X;
        }

        public int current_frame
        {
            get { return (bound.X - init_x) / bound.Width; }
            set { bound.X = init_x + (value * bound.Width); }
        }

        public void Update()
        {
            frames++;

            if (frames > 6)
            {
                frames = 0;
                increment_bound();
            }
        }

        public void increment_bound()
        {
            current_frame++;

            if (current_frame >= num_frames)
            {
                visible = false;
            }
        }
    }
}
