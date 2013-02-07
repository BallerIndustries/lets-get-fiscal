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
    public partial class Game1 : Microsoft.Xna.Framework.Game
    {
        IAsyncResult result;
        int count;
        float alpha_val = 0;

        const int start_fade_out = 120;
        const int complete_fade_out = 180;
        const int leave_state = 210;
        
        public void update_logo()
        {
            kbState = Keyboard.GetState();
            gpState = GamePad.GetState(PlayerIndex.One);

            count++;

            if (count > start_fade_out)
                alpha_val = MathHelper.Lerp(0, 1, (count - start_fade_out) / (float)(complete_fade_out - start_fade_out));

            if (pressed_once(Keys.Enter) || pressed_once(Buttons.A) || count > leave_state)
            {
                count = 0;
                alpha_val = 0;

#if XBOX
                game_state.state = GameState.State.StartPrompt;
#else
                game_state.state = GameState.State.Menu;
#endif

                //Exit();
                //Game1.current_state = Game1.menu_state;
            }

            prevkbState = kbState;
            prevgpState = gpState;
        }

        public void draw_logo()
        {
            spriteBatch.Draw(logo, new Vector2(294, 82), Color.White);
            spriteBatch.Draw(whiteDot, new Rectangle(0, 0, 960, 540), Color.Black * alpha_val);
        }

        public void update_combo_explain()
        {
            gpState = GamePad.GetState(controlling_player);
            kbState = Keyboard.GetState();

            if (pressed_once(Buttons.A) || pressed_once(Keys.Space))
            {
                game_state.state = GameState.State.LevelAnnounce;
            }

            prevgpState = gpState;
            prevkbState = kbState;
        }

        public void draw_combo_explain()
        {
            spriteBatch.Draw(combo_explain_texture, Vector2.Zero, Color.White);
        }

        public void update_language_warning()
        {
#if XBOX
            if (!Guide.IsVisible)
                result = Guide.BeginShowMessageBox("Attention", "This game contains heavy swearing and drug references. Do you wish to continue?", new String[] { "Yes", "No" }, 0, MessageBoxIcon.Warning, this.complete_language_warning, null);
#endif
        }

        void complete_language_warning(IAsyncResult result)
        {
            int? num = Guide.EndShowMessageBox(result);
            if (num == 0)
                game_state.state = GameState.State.Menu;
            if (num == 1)
                this.Exit();
        }

        public void update_demo_over()
        {
            kbState = Keyboard.GetState();
            gpState = GamePad.GetState(controlling_player);

            if (Guide.IsTrialMode == false)
            {
                game_state.state = GameState.State.LevelAnnounce;
            }

            if (pressed_once(Buttons.A) || pressed_once(Keys.Space))
            {
                
                if (controlling_player.can_buy_game())
                {
                    game_state.state = GameState.State.CutScene;
                    Guide.ShowMarketplace(controlling_player);
                    return;
                }
                else
                {
                    game_state.state = GameState.State.Menu;
                    return;
                }
            }

            prevkbState = kbState;
            prevgpState = gpState;
        }

        public void draw_demo_over()
        {
            Vector2 position = new Vector2();
            Vector2 position2 = new Vector2();

            string text = "DEMO OVER";
            string text2 = "BALLERINDUSTRIES.COM";

            position.X = (960 - Singletons.credit_font.MeasureString(text).X) / 2;
            position.Y = (540 - Singletons.credit_font.MeasureString(text).Y) / 2;

            position2.X = (960 - Singletons.credit_font.MeasureString(text2).X) / 2;
            position2.Y = position.Y + Singletons.credit_font.MeasureString(text2).Y;



            spriteBatch.DrawString(Singletons.credit_font, text, position, Color.White);
            spriteBatch.DrawString(Singletons.credit_font, text2, position2, Color.White); 
        }

        public void update_level_announce()
        {
            game_state.current_level_announce.Update();

            if (game_state.current_level_announce.time > 150)
            {
                game_state.state = GameState.State.GamePlay;
                fade_to_black.fade_state = Fade.State.fade_in;
                fade_to_black.alpha_val = 1;
            }
        }

        public void draw_level_announce()
        {
            game_state.current_level_announce.Draw();
        }

        public void update_act_over()
        {
            kbState = Keyboard.GetState();
            gpState = GamePad.GetState(controlling_player);
            
            //Don't update stuff we can't even see. BRO.
            if (fade_to_black.fade_state != Fade.State.darkness)
            {
                ego.Update(kbState, prevkbState, gpState, prevgpState, false);
                hud.Update(ego.hp, ego.lives, game_state.score);
                fade_to_black.Update();
            }

            game_state.current_stage_cleared.Update();

            if (game_state.current_stage_cleared.time > StageCleared.fade_out)
                fade_to_black.fade_state = Fade.State.fade_out;

            //Press enter or A to return to the main menu.
            if (game_state.current_stage_cleared.time > StageCleared.stats_complete)
            {
                if (pressed_once(Buttons.A) || pressed_once(Keys.Enter))
                {
                    if (game_state.current_level == tbk_club)
                        initialise_cutscene(scene3);
                    else if (game_state.current_level == bifo)
                        initialise_cutscene(scene5);
                    else if (game_state.current_level == newspaper)
                        initialise_cutscene(credits);
                }
                   // game_state.state = GameState.State.Menu;
            }


            prevgpState = gpState;
            prevkbState = kbState;

        }

        public void draw_act_over()
        {

            if (fade_to_black.fade_state != Fade.State.darkness)
                draw_gameplay();

            game_state.current_stage_cleared.Draw();

            //string h1 = "ACT OVER";
            //string h2 = "Treeboi & Kone Accounted For";

            //int h1_x, h2_x;

            //h1_x = (960 - (int)MenuFont.MeasureString(h1).X) / 2;
            //h2_x = (960 - (int)MenuFont.MeasureString(h2).X) / 2;

            //spriteBatch.DrawString(MenuFont, h1, new Vector2(h1_x, 80), Color.White);
            //spriteBatch.DrawString(MenuFont, h2, new Vector2(h2_x, 130), Color.White);

            //spriteBatch.Draw(Content.Load<Texture2D>("treeboi"), new Vector2(283, 207), Color.White);
            //spriteBatch.Draw(Content.Load<Texture2D>("kone"), new Vector2(476, 201), Color.White);
        }

        public void StopEffects()
        {
            AudioCategory c = audioEngine.GetCategory("Default");
            c.Stop(AudioStopOptions.AsAuthored);
        }

        public Rectangle transition_rect(Rectangle from, Rectangle to, float percentage)
        {
            //Returns a rectangle that represents the transition from one rectangle to another
            //at the point in a certain percentage.
            Rectangle ret_rect = new Rectangle();

            ret_rect.X = from.X + (int)((to.X - from.X) * percentage);
            ret_rect.Y = from.Y + (int)((to.Y - from.Y) * percentage);
            ret_rect.Width = from.Width + (int)((to.Width - from.Width) * percentage);
            ret_rect.Height = from.Height + (int)((to.Height - from.Height) * percentage);

            return ret_rect;
        }

        public Rectangle normalize_rect(Rectangle draw_region)
        {
            int width_scale = 960 / draw_region.Width;
            int height_scale = 540 / draw_region.Height;

            if (width_scale > height_scale)
            {
                //Width needs to be adjusted
                int enlarged_width = draw_region.Width * height_scale;
                int diff = 960 - enlarged_width;

                //diff is the number of pixels we need to add to the width
                //to make a rectangle that is in the ration of 960x540
                draw_region.X -= diff / 2;
                draw_region.Width += diff;
            }
            else
            {
                //Height needs to be adjusted
                int enlarged_height = draw_region.Height * width_scale;
                int diff = 540 - enlarged_height;

                //diff is the number of pixels we need to add to the height
                //to make a rectangle that is in the ration of 960x540
                draw_region.Y -= diff / 2;
                draw_region.Height += diff;
            }

            return draw_region;
        }

        public Rectangle scale_rect(Rectangle draw_region)
        {
            //We want to scale the rectangle to fit into a 960x540
            //viewport
            float width_scale = 960 / (float)draw_region.Width;
            float height_scale = 540 / (float)draw_region.Height;

            if (width_scale > height_scale)
            {
                //This means we need to increase both dimensions by height scale
                draw_region.Width = (int)((float)draw_region.Width * height_scale);
                draw_region.Height = (int)((float)draw_region.Height * height_scale);
            }
            else
            {
                //This means we need to increase both dimensions by width scale
                draw_region.Width = (int)((float)draw_region.Width * width_scale);
                draw_region.Height = (int)((float)draw_region.Height * width_scale);
            }

            //Alright that's great. Now we need to center the rectangle. Should be easy I think.

            draw_region.X = (960 - draw_region.Width) / 2;
            draw_region.Y = (540 - draw_region.Height) / 2;

            return draw_region;
        }

        public Rectangle fill_80_percent(Rectangle draw_region)
        {
            //Add 10% padding to each dimension

            Rectangle return_rect = new Rectangle();
            Rectangle viewport_rect = new Rectangle(0, 0, 960, 540);

            int width_increase = (int)((float)draw_region.Width * 0.20f);
            int height_increase = (int)((float)draw_region.Height * 0.20f);

            //Increase the width and height by 20%
            return_rect.Width = draw_region.Width + width_increase;
            return_rect.Height = draw_region.Height + height_increase;

            //Increment the X and Y positions by 10%
            return_rect.X = draw_region.X - (width_increase / 2);
            return_rect.Y = draw_region.Y - (height_increase / 2);

            return normalize_rect(return_rect);
        }

        protected Rectangle GetTitleSafeArea(float percent)
        {
            Rectangle retval = new Rectangle(
                graphics.GraphicsDevice.Viewport.X,
                graphics.GraphicsDevice.Viewport.Y,
                graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height);

            float border = (1 - percent) / 2;
            retval.X = (int)(border * retval.Width);
            retval.Y = (int)(border * retval.Height);
            retval.Width = (int)(percent * retval.Width);
            retval.Height = (int)(percent * retval.Height);
            return retval;
        }

        public void drawHorLine(int y)
        {
            spriteBatch.Draw(whiteDot, new Rectangle(0, y, 1000, 1), Color.Red);
        }

        public void drawHorLine(int y, Color color)
        {
            spriteBatch.Draw(whiteDot, new Rectangle(0, y, 1000, 1), color);
        }

        public void drawRect(Rectangle rect, Color col)
        {
            int x, y, w, h;
            x = rect.X - camera.viewport_rect.X;
            y = rect.Y;
            w = rect.Width;
            h = rect.Height;

            spriteBatch.Draw(whiteDot, new Rectangle(x, y, 2, h), col);
            spriteBatch.Draw(whiteDot, new Rectangle(x, y, w, 2), col);
            spriteBatch.Draw(whiteDot, new Rectangle(x + w, y, 2, h), col);
            spriteBatch.Draw(whiteDot, new Rectangle(x, y + h, w, 2), col);
        }

        public void drawFilledRect(Rectangle rect, Color col)
        {
            spriteBatch.Draw(whiteDot, rect, col);
        }

        private void initialise_cutscene(CutScene cs)
        {
            game_state.current_cs = cs;
            stopwatch.Start();
            time = 0;
            game_state.state = GameState.State.CutScene;
        }

        public void draw_collision_section(Character c)
        {
            //1. Get a rectangle fill it with the 
            for (int i = c.bound.Left; i < c.bound.Right; i++)
            {
                for (int j = c.bound.Top; j < c.bound.Bottom; j++)
                {
                    if (CollisionManager.get_colour_at_point(c.collision_map, i, j, c.collision_width))
                    {
                        spriteBatch.Draw(whiteDot, new Rectangle(i + c.posX, j + c.posY, 1, 1), Color.Black);
                    }
                }
            }

            Rectangle r = c.bound;
            CollisionManager.get_colour_at_point(c.collision_map, 0, 0, c.collision_width);
        }

        //public void draw_hud()
        //{
        //    float ego_hp_percentage = (float)ego.hp / ego.max_hp;
        //    float ego_hp_width = ego_hp_percentage * 300;

        //    //Starting position for the HUD
        //    int x = title_safe_rect.X;
        //    int y = title_safe_rect.Y;

        //    drawFilledRect(new Rectangle(x + 40, y + 30, 300, 10), Color.Red);
        //    drawFilledRect(new Rectangle(x + 40, y + 30, (int)ego_hp_width, 10), Color.Yellow);
        //    spriteBatch.Draw(ego.portrait, new Rectangle(x, y, 40, 40), Color.White);

        //    spriteBatch.DrawString(HUDFont, ego.name, new Vector2(x + 60, y + 10), Color.Green);
        //    spriteBatch.DrawString(HUDFont, game_state.score.ToString("000000"), new Vector2(x + 240, y + 10), Color.Blue);
        //    spriteBatch.DrawString(HUDFont, "x" + ego.lives, new Vector2(x + 370, y + 30), Color.White, 0, Vector2.Zero, 2.0f, SpriteEffects.None, 0);

        //    if (cm.still_display > 0)
        //    {
        //        int health_width = (int)(((float)cm.punched_enemy_max_hp / ego.max_hp) * 300);
        //        float bad_hp_percentage = (float)cm.punched_enemy_hp / cm.punched_enemy_max_hp;
        //        float bad_hp_width = bad_hp_percentage * health_width;

        //        spriteBatch.DrawString(HUDFont, cm.punched_enemy_name, new Vector2(x + 60, y + 60), Color.Green);

        //        spriteBatch.Draw(cm.punched_enemy_portrait, new Rectangle(x, y + 50, 40, 40), Color.White);
        //        drawFilledRect(new Rectangle(x + 40, y + 80, health_width, 10), Color.Red);
        //        drawFilledRect(new Rectangle(x + 40, y + 80, (int)bad_hp_width, 10), Color.Yellow);
        //    }
        //}

        //This ugly game hanging function is just here to make sure the boolean map is working properly.
        //public void draw_collision(bool[] bool_map, Character c)
        //{
        //    for (int i = 0; i < c.texture.Width; i++)
        //    {
        //        for (int j = 0; j < c.texture.Height; j++)
        //        {
        //            if (get_colour_at_point(c.collision_map, i, j, c.collision_width))
        //                spriteBatch.Draw(whiteDot, new Rectangle(i, j, 1, 1), Color.Black);
        //        }
        //    }
        //}

    }


}
