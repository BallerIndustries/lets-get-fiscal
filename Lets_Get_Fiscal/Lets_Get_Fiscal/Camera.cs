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
    public class Camera
    {
        public Rectangle level_rect;
        public Rectangle viewport_rect;

        private int _limitation;

        public int prev_limitation;
        public int limitation
        {
            get { return _limitation; }
            set 
            { 
                prev_limitation = _limitation; 
                _limitation = value; 
            }
        }
        private const int camera_speed = 8;

        public Camera(Rectangle level_rect, Rectangle viewport_rect)
        {
            this.level_rect = level_rect;
            this.viewport_rect = viewport_rect;
        }

        public void Initialise()
        {
            level_rect = new Rectangle(0, 0, 1920, 540);
            viewport_rect.X = 0;
            limitation = 0;
        }

        //This function checks if the position passed in is greater than the halfway
        //point. If it is we adjust the motherfucking camera. So that that asshole 
        //of a player is in the center of the screen.
        public void adjust_viewport(Rectangle position)
        {
            if (position.X > viewport_rect.Center.X && viewport_rect.X < limitation - viewport_rect.Width)
            {
                int dist_from_center = position.X - viewport_rect.Center.X;

                if (dist_from_center < camera_speed)
                    viewport_rect.X = (int)MathHelper.Clamp((float)viewport_rect.X + dist_from_center, 0, limitation);
                else
                    viewport_rect.X = (int)MathHelper.Clamp((float)viewport_rect.X + camera_speed, 0, limitation);
            }
        }

        public void Draw(GameObject go, SpriteBatch spriteBatch)
        {
            Rectangle r = new Rectangle();

            r = go.position;
            r.X -= viewport_rect.X;

            //Only bother drawing stuff that is on the screen. MOTHER FUCKER.
            if (viewport_rect.Intersects(go.position) && go.visible)
            {
                spriteBatch.Draw(go.texture, r, go.bound, Color.White * (go.alpha_val / 255f), 0, Vector2.Zero, go.se, 1.0f);
                //spriteBatch.Draw(go.texture, r, go.bound, Color.Red * (128 / 255f), 0, Vector2.Zero, go.se, 0);
                

                //Character c = go as Character;

                ////Draw the characters weapon
                //if (c != null)
                //{   
                //    if (c.weapon != null && c.weapon_data != WeaponData.Empty)
                //    {
                //        r.Width = (int)(c.weapon.bounds[c.weapon_data.frame].Width * 2.8f);
                //        r.Height = (int)(c.weapon.bounds[c.weapon_data.frame].Height * 2.8f);
                        
                //        if (c.facing == GameObject.Direction.right)
                //            r.X = c.posX + (int)(c.weapon_data.position.X * c.scale) - (int)(c.weapon.points[c.weapon_data.frame].X * 2.8f);
                //        else
                //            r.X = c.posX - c.weapon.posW - (int)(c.weapon_data.position.X * c.scale) + (int)(c.weapon.points[c.weapon_data.frame].X * 2.8f);

                //        r.Y = c.posY - (int)(c.weapon_data.position.Y * c.scale) - (int)(c.weapon.points[c.weapon_data.frame].Y * 2.8f);
                //        r.X -= viewport_rect.X;

                //        c.weapon.posX = r.X;
                //        c.weapon.posY = r.Y;

                //        if (c.weapon_data.frame != 2)
                //            spriteBatch.Draw(c.weapon.texture, r, c.weapon.bounds[c.weapon_data.frame], Color.White, 0, Vector2.Zero, c.se, 0);
                //        else
                //            spriteBatch.Draw(c.weapon.texture, r, c.weapon.bounds[c.weapon_data.frame], Color.White, 0, Vector2.Zero, c.se, 0);
                //    }
                //}
            }
        }


        public void Draw(BasicGameObject bgo, SpriteBatch spriteBatch)
        {
            Rectangle r = new Rectangle();

            r = bgo.position;
            r.X -= viewport_rect.X;

            //Weapon w = bgo as Weapon;


            //Only bother drawing stuff that is on the screen. MOTHER FUCKER.
            if (viewport_rect.Intersects(bgo.position) && bgo.visible)
            {
                //if (w == null)
                    spriteBatch.Draw(bgo.texture, r, bgo.bound, Color.White);
                //else if (w.is_falling)
                //{
                //    Rectangle bound = w.bounds[w.current_frame];

                //    r.Width = (int)(bound.Width * 2.8f);
                //    r.Height = (int)(bound.Height * 2.8f);
                //    r.X -= r.Width / 2;
                //    r.Height -= r.Height / 2;
                //    spriteBatch.Draw(w.texture, r, bound, Color.White, 0, Vector2.Zero, w.se, 0);
                //}
                //else
                //    spriteBatch.Draw(bgo.texture, r, bgo.bound, Color.White);
            }
        }

        public void Draw(Flourish f, SpriteBatch spriteBatch)
        {
            Rectangle r = new Rectangle();
            r.X = (int)f.draw_position.X - viewport_rect.X;
            r.Y = (int)f.draw_position.Y;
            r.Width = f.bound.Width;
            r.Height = f.bound.Height;

            spriteBatch.Draw(f.texture, r, f.bound, Color.White);
        }

        public void Draw(Item i, SpriteBatch spriteBatch)
        {
            Rectangle r = new Rectangle();

            r.X = i.position.X - viewport_rect.X;
            r.Y = i.position.Y;
            r.Width = i.position.Width;
            r.Height = i.position.Height;

            spriteBatch.Draw(i.texture, r, i.bound, Color.White);
        }

        public void Draw_Collision(Character c, SpriteBatch spriteBatch)
        {
            Rectangle r = c.position;
            r.X -= viewport_rect.X;

            if (viewport_rect.Intersects(c.position))
            {
                spriteBatch.Draw(c.collision, r, c.bound, Color.White, 0, Vector2.Zero, c.se, 0);
            }
        }

        //Runaway from me baby, this function is only for debugging BABY.
        //public void Draw_Collision(Character c, SpriteBatch spriteBatch, Texture2D sprite)
        //{
        //    Rectangle r = c.position;
        //    r.X -= viewport_rect.X;
        //    spriteBatch.Draw(sprite, r, c.current_animation.bound, Color.White, 0, Vector2.Zero, c.se, 0); 
        //}

        public int left_of_cam(int offset)
        {
            return viewport_rect.Left - offset;
        }

        public int right_of_cam(int offset)
        {
            return viewport_rect.Right + offset;
        }

        public void snap_to_camera(Ego e)
        {
            //This code is called when the ego is returning to the idle state.
            //If he is NOT in the viewport then snap him into the viewport.
            Rectangle shrunk = CollisionManager.scale_rect(e.position, 0.4f, 1.0f);
            int dist;

            if (shrunk.Left < viewport_rect.Left)
            {
                dist = viewport_rect.Left - shrunk.Left;
                e.posX += dist;
            }
            else if (shrunk.Right > viewport_rect.Right)
            {
                dist = shrunk.Right - viewport_rect.Right;
                e.posX -= dist;
            }

            if (shrunk.Bottom < Singletons.cm.min_y)
            {
                dist = Singletons.cm.min_y - shrunk.Bottom;
                e.posY += dist;
            }
            else if (shrunk.Bottom > viewport_rect.Bottom)
            {
                dist = shrunk.Bottom - viewport_rect.Bottom;
                e.posY -= dist;
            }

            //THAT OUGHTTA DO IT!
        }
    }
}
