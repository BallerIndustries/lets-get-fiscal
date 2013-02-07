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
    public class Weapon : Item
    {
        public int damage;
        public List<Rectangle> bounds;
        public Point[] points;
        public string sfx_name;
        
        //All to do with when the Weapon is falling to the ground.
        public bool is_falling = false;
        public bool going_up;
        public int current_frame;
        public int baseline;
        public int ceiling;
        private int frames;
        public SpriteEffects se;
        public Weapon(TextureManager tm, Item.Type type, Point position) : base(tm, type, position)
        {
            set_bounds(type);
        }

        private void set_bounds(Item.Type type)
        {
            switch (type)
            {
                case Item.Type.baseball_bat:
                    sfx_name = "lead_pipe_start";
                    bounds = new List<Rectangle>(8);
                    points = new Point[8];

                    bounds.Add(new Rectangle(14, 12, 9, 48));
                    bounds.Add(new Rectangle(29, 22, 31, 36));
                    bounds.Add(new Rectangle(64, 33, 48, 9));
                    bounds.Add(new Rectangle(116, 20, 35, 33));
                    bounds.Add(new Rectangle(162, 9, 10, 46));
                    bounds.Add(new Rectangle(181, 19, 35, 32));
                    bounds.Add(new Rectangle(221, 31, 46, 9));
                    bounds.Add(new Rectangle(271, 22, 33, 35));

                    add_grab_point(new Point(17, 57), 0);
                    add_grab_point(new Point(31, 55), 1);
                    add_grab_point(new Point(67, 36), 2);
                    add_grab_point(new Point(118, 24), 3);
                    add_grab_point(new Point(166, 12), 4);
                    add_grab_point(new Point(212, 22), 5);
                    add_grab_point(new Point(265, 35), 6);
                    add_grab_point(new Point(300, 54), 7);
                    break;

                //case Item.Type.lead_pipe:
                //    sfx_name = "lead_pipe_start";
                //    bounds = new List<Rectangle>(8);
                //    points = new Point[8];

                //    bounds.Add(new Rectangle(6, 7, 12, 49));
                //    bounds.Add(new Rectangle(24, 12, 41, 41));
                //    bounds.Add(new Rectangle(81, 27, 54, 9));
                //    bounds.Add(new Rectangle(144, 12, 40, 43));
                //    bounds.Add(new Rectangle(189, 8, 12, 49));
                //    bounds.Add(new Rectangle(208, 12, 41, 43));
                //    bounds.Add(new Rectangle(258, 27, 54, 10));
                //    bounds.Add(new Rectangle(318, 13, 43, 43));

                //    add_grab_point(new Point(12, 53), 0);
                //    add_grab_point(new Point(27, 50), 1);
                //    add_grab_point(new Point(83, 32), 2);
                //    add_grab_point(new Point(147, 16), 3);
                //    add_grab_point(new Point(194, 10), 4);
                //    add_grab_point(new Point(244, 16), 5);
                //    add_grab_point(new Point(308, 31), 6);
                //    add_grab_point(new Point(357, 51), 7);
                //    break;
            }
        }

        private void add_grab_point(Point p, int frame)
        {
            //Measure the distance between the top left corner of the frame and
            //the grab point.
            Point bound_tl = new Point(bounds[frame].Left, bounds[frame].Top);

            points[frame] = new Point(p.X - bound_tl.X, p.Y - bound_tl.Y);
        }
        
        public void Update()
        {
            if (!is_falling)
                return;

            if (going_up)
                posY -= 5;
            else
                posY += 5;

            posX += 2;

            animate_check();

            if (going_up && posY < ceiling)
                going_up = false;

            if (going_up == false && posY > baseline)
                is_falling = false;
        }

        public void start_drop(Character c)
        {
            ceiling = posY - 10;
            posX = c.posX;
            baseline = c.baseline;
            going_up = true;
            is_falling = true;
            visible = true;
            se = c.se;
        }

        public void animate_check()
        {
            frames++;

            if (frames > 1)
            {
                frames = 0;
                increment_animation();
            }
        }

        public void increment_animation()
        {
            if (current_frame < bounds.Count - 1)
                current_frame++;
            else
                current_frame = 0;
        }
    }
}
