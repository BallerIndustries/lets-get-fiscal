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
    public class Icicles : BasicGameObject
    {
        private enum State
        {
            formed,
            breaking,
            //broken
        }

        public bool can_sword_down
        {
            get { return state == State.formed && alpha_val == 1.0f; }
        }

        private Singletons singletons;
        private bool projectiles_sent;
        private State state;
        private float alpha_val = 1.0f;
        private float scale = 2.0f;
        private List<Point> positions;
        private int frames;
        public Fred fred;

        private readonly Rectangle formed;
        private readonly Rectangle breaking;
        //private readonly Rectangle broken;
       
        public Icicles(Singletons singletons)
            : base(Singletons.tm, "spritesheets//fernando")
        {
            formed      = new Rectangle(0, 960, 120, 65);
            breaking    = new Rectangle(120, 960, 120, 65);
            //broken      = new Rectangle(480, 960, 120, 65);

            this.bound = formed;
            this.singletons = singletons;

            positions = new List<Point>(4);
            positions.Add(new Point(100, 0));
            positions.Add(new Point(300, 0));
            positions.Add(new Point(500, 0));
            positions.Add(new Point(700, 0));
            positions.Add(new Point(900, 0));
        }

        public void Update()
        {
            switch (state)
            {
                case State.breaking:
                    do_breaking();
                    break;

                case State.formed:
                    do_forming();
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Point pos in positions)
            {
                Rectangle r = new Rectangle();
                r.X = pos.X;
                r.Y = pos.Y;
                r.Width = (int)(bound.Width * scale);
                r.Height = (int)(bound.Height * scale);

                r.X -= Singletons.camera.viewport_rect.X;

                spriteBatch.Draw(this.texture, r, bound, Color.White * alpha_val); 
            }
        }

        public void start_breaking()
        {
            state = State.breaking;
            bound = breaking;
            projectiles_sent = false;
        }

        private void do_breaking()
        {
            frames++;

            //Animate
            if (frames > 4 && bound.X < 480)
            {
                frames = 0;
                bound.X += 120;
            }

            if (bound.X == 480 && !projectiles_sent)
            {
                projectiles_sent = true;
                foreach (Point pos in positions)
                    create_projectiles(pos);
            }

            if (bound.X == 480)
            {
                alpha_val = MathHelper.Clamp(alpha_val - 0.01f, 0.0f, 1.0f);

                if (alpha_val == 0.0f)
                    start_forming();
            }
        }

        public void start_forming()
        {
            bound = formed;
            state = State.formed;

            int initial_x = Singletons.camera.viewport_rect.X + Singletons.random.Next(200);

            //foreach (Point point in positions)
            
            for (int i = 0; i < positions.Count; i++)
            {
                positions[i] = new Point(initial_x, 0);
                //positions[i].X = initial_x;
                initial_x += 200;
            }
        }

        public void do_forming()
        {
            alpha_val = MathHelper.Clamp(alpha_val + 0.01f, 0.0f, 1.0f);
        }

        private void create_projectiles(Point position)
        {
            Rectangle icicle = new Rectangle();

            icicle.Y = 120;
            icicle.X = position.X + 95;

            Animation a = new Animation(new Rectangle(646, 971, 26, 55), 1, true, "bananas", GameObject.State.hadoken);
            Projectile p = new Projectile(icicle, new Vector2(0, 24), GameObject.Direction.right, a, singletons, "spritesheets//fernando", Singletons.ego.baseline, fred, 2.0f, 30);

            Singletons.pm.add_projectile(p);
        }

        //public void jur()
        //{
        //    Rectangle r = new Rectangle();

        //    r = go.position;
        //    r.X -= viewport_rect.X;

        //    //Only bother drawing stuff that is on the screen. MOTHER FUCKER.
        //    if (viewport_rect.Intersects(go.position) && go.visible)
        //    {
        //        spriteBatch.Draw(go.texture, r, go.bound, Color.White * (go.alpha_val / 255f), 0, Vector2.Zero, go.se, 1.0f);
        //    }
        //}

    }
}
