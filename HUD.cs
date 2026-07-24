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
    public class HUD
    {
        Point top_left;
        int still_display;

        Texture2D ego_portrait;
        Texture2D bad_guy_portrait;
        Texture2D whiteDot, arrow;

        SpriteFont HUDFont;

        int ego_hp;
        int ego_max_hp;
        int ego_lives;
        string ego_name;

        int prev_ego_hp_width;
        int new_ego_hp_width;
        int ego_frames_passed;

        int score;

        int bad_guy_hp;
        int bad_guy_max_hp;
        string bad_guy_name;

        //Calculated fields
        float ego_hp_percentage;
        int ego_hp_width;

        int health_width;
        
        float bad_hp_percentage;
        int bad_hp_width;
        int new_bad_hp_width;
        int prev_bad_hp_width;

        int blue_hp_width;
        int new_blue_hp_width;
        int prev_blue_hp_width;

        int bad_guy_frames_passed;

        public bool draw_hud = true;   //If false, draw the continue screen instead.
        public bool continue_selected = true;

        public HUD(Texture2D ego_portrait, Texture2D whiteDot, int ego_max_hp, SpriteFont HUDFont, string ego_name, Texture2D arrow)
        {
            this.ego_portrait = ego_portrait;
            this.ego_max_hp = ego_max_hp;
            this.whiteDot = whiteDot;
            this.HUDFont = HUDFont;
            this.ego_name = ego_name;
            this.arrow = arrow;

            top_left = new Point(95, 53);
        }

        //public void set_bad_guy_fields(int bad_guy_hp, int bad_guy_max_hp, string bad_guy_name, Texture2D bad_guy_portrait)
        //{
        //    prev_bad_hp_width = this.new_bad_hp_width;
        //    this.bad_guy_hp = bad_guy_hp;
        //    this.bad_guy_max_hp = bad_guy_max_hp;
        //    this.bad_guy_name = bad_guy_name;
        //    this.bad_guy_portrait = bad_guy_portrait;

        //    still_display = 180;

        //    health_width = (int)(((float)bad_guy_max_hp / ego_max_hp) * 300);
        //    bad_hp_percentage = (float)bad_guy_hp / bad_guy_max_hp;
        //    new_bad_hp_width = (int)(bad_hp_percentage * health_width);
        //    bad_guy_frames_passed = 0;

        //    //This is a HACK! It should cover the case where the HUD
        //    //element is displayed for the first time.
        //    if (prev_bad_hp_width < new_bad_hp_width)
        //        prev_bad_hp_width = health_width;

        //}

        public void set_bad_guy_fields(BadGuy bg)
        {
            this.bad_guy_hp = bg.hp;
            this.bad_guy_max_hp = bg.max_hp;
            this.bad_guy_name = bg.name;
            this.bad_guy_portrait = bg.portrait;

            still_display = 180;


            if (bad_guy_max_hp > ego_max_hp)
            {
                health_width = 300;
                blue_hp_width = 300;

                float blue_hp_percentage = (float)(bad_guy_hp - ego_max_hp) / (float)(bad_guy_max_hp - ego_max_hp);
                float prev_blue_hp_percentage = (float)(bg.prev_hp - ego_max_hp) / (float)(bad_guy_max_hp - ego_max_hp);
                
                new_blue_hp_width = (int)(blue_hp_percentage * blue_hp_width);
                prev_blue_hp_width = (int)(prev_blue_hp_percentage * blue_hp_width);

                //Yellow HP info
                bad_hp_percentage = MathHelper.Clamp((float)bad_guy_hp / ego_max_hp, 0, 1.0f);
                float prev_hp_percentage = MathHelper.Clamp((float)bg.prev_hp / ego_max_hp, 0, 1.0f);

                new_bad_hp_width = (int)(bad_hp_percentage * health_width);
                prev_bad_hp_width = (int)(prev_hp_percentage * health_width);

                bad_guy_frames_passed = 0;
            }
            else
            {
                health_width = (int)(((float)bad_guy_max_hp / ego_max_hp) * 300);

                bad_hp_percentage = (float)bad_guy_hp / bad_guy_max_hp;
                
                new_bad_hp_width = (int)(bad_hp_percentage * health_width);
                prev_bad_hp_width = (int)(((float)bg.prev_hp / bad_guy_max_hp) * health_width);

                bad_guy_frames_passed = 0;
            }
        }

        public void display_bad_guy_data(BadGuy bg)
        {
            this.bad_guy_hp = bg.hp;
            this.bad_guy_max_hp = bg.max_hp;
            this.bad_guy_name = bg.name;
            this.bad_guy_portrait = bg.portrait;

            still_display = 180;

            if (bad_guy_max_hp > ego_max_hp)
            {
                health_width = 300;
                blue_hp_width = 300;

                float blue_hp_percentage = (float)(bad_guy_hp - ego_max_hp) / (float)(bad_guy_max_hp - ego_max_hp);
                //float prev_blue_hp_percentage = (float)(bg.prev_hp - ego_max_hp) / (float)(bad_guy_max_hp - ego_max_hp);

                new_blue_hp_width = (int)(blue_hp_percentage * blue_hp_width);
                
                //Yellow HP info
                bad_hp_percentage = MathHelper.Clamp((float)bad_guy_hp / ego_max_hp, 0, 1.0f);
                //bad_hp_percentage = (float)(bad_guy_hp - ego_hp) / (float)ego_hp;
                //bad_hp_percentage = MathHelper.Clamp((float)bad_guy_hp / bad_guy_max_hp, 0, 1.0f);
                //float prev_hp_percentage = MathHelper.Clamp((float)bg.prev_hp / bad_guy_max_hp, 0, 1.0f);

                new_bad_hp_width = (int)(bad_hp_percentage * health_width);
                
                bad_guy_frames_passed = 0;

                //Set previous widths
                prev_bad_hp_width = new_bad_hp_width;
                prev_blue_hp_width = new_blue_hp_width;
            }
            else
            {
                health_width = (int)(((float)bad_guy_max_hp / ego_max_hp) * 300);
                bad_hp_percentage = (float)bad_guy_hp / bad_guy_max_hp;
                new_bad_hp_width = (int)(bad_hp_percentage * health_width);

                prev_bad_hp_width = new_bad_hp_width;
                bad_guy_frames_passed = 0;
            }
        }

        public void Update(int ego_hp, int ego_lives, int score)
        {
            //No need to update anything if we ain't even drawing shit you MOTHER FUCKER!!!!!
            if (draw_hud == false)
                return;

            //Store the data passed into us from the client code
            this.ego_lives = ego_lives;
            this.score = score;

            if (still_display > 0)
                still_display--;

            //Now for the calculated fields
            if (this.ego_hp != ego_hp)
            {
                ego_frames_passed = 0;

                this.ego_hp = ego_hp;
                ego_hp_percentage = (float)ego_hp / ego_max_hp;
                prev_ego_hp_width = new_ego_hp_width;
                new_ego_hp_width = (int)(ego_hp_percentage * 300);

                if (prev_ego_hp_width < new_ego_hp_width)
                    prev_ego_hp_width = ego_hp_width;
            }

            //The new constant speed way.
            //bad_hp_width = (int)MathHelper.Lerp

            ego_hp_width = (int)MathHelper.Clamp(prev_ego_hp_width, new_ego_hp_width, ego_hp_width + ((new_ego_hp_width - prev_ego_hp_width) / 10));

            //The old way
            if (bad_guy_frames_passed < 20)
            {
                bad_guy_frames_passed++;
                bad_hp_width = (int)MathHelper.Lerp(prev_bad_hp_width, new_bad_hp_width, ((float)bad_guy_frames_passed / 20.0f));
                blue_hp_width = (int)MathHelper.Lerp(prev_blue_hp_width, new_blue_hp_width, ((float)bad_guy_frames_passed / 20.0f));
            }

            if (ego_frames_passed < 20)
            {
                ego_frames_passed++;
                ego_hp_width = (int)MathHelper.Lerp(prev_ego_hp_width, new_ego_hp_width, ((float)ego_frames_passed / 20.0f));
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (draw_hud)
                draw_HUD(spriteBatch);
            else
                draw_continue(spriteBatch);

            //int x = top_left.X;
            //int y = top_left.Y;

            //draw_filled_rect(spriteBatch, new Rectangle(x + 40, y + 30, 300, 10), Color.Red);
            //draw_filled_rect(spriteBatch, new Rectangle(x + 40, y + 30, (int)ego_hp_width, 10), Color.Yellow);
            //spriteBatch.Draw(ego_portrait, new Rectangle(x, y, 40, 40), Color.White);

            //spriteBatch.DrawString(HUDFont, ego_name, new Vector2(x + 60, y), Color.Green);
            //spriteBatch.DrawString(HUDFont, score.ToString("000000"), new Vector2(x + 240, y), Color.Blue);
            //spriteBatch.DrawString(HUDFont, "x" + ego_lives, new Vector2(x + 370, y + 30), Color.White, 0, Vector2.Zero, 2.0f, SpriteEffects.None, 0);

            //if (still_display > 0)
            //{
            //    spriteBatch.DrawString(HUDFont, bad_guy_name, new Vector2(x + 60, y + 50), Color.Green);

            //    spriteBatch.Draw(bad_guy_portrait, new Rectangle(x, y + 50, 40, 40), Color.White);
            //    draw_filled_rect(spriteBatch, new Rectangle(x + 40, y + 80, health_width, 10), Color.Red);
            //    draw_filled_rect(spriteBatch, new Rectangle(x + 40, y + 80, (int)bad_hp_width, 10), Color.Yellow);
            //    draw_filled_rect(spriteBatch, new Rectangle(x + 40, y + 80, (int)blue_hp_width, 10), Color.Blue);
            //}
        }

        private void draw_continue(SpriteBatch spriteBatch)
        {
            int x = top_left.X;
            int y = top_left.Y;

            //if (continue_selected)
            //{
                spriteBatch.DrawString(HUDFont, "CONTINUE", new Vector2(x + 20, y), Color.LightBlue);
                spriteBatch.DrawString(HUDFont, "NEW GAME", new Vector2(x + 20, y + 50), Color.LightBlue);

            if (continue_selected)
                spriteBatch.Draw(arrow, new Vector2(x, y + 5), Color.LightBlue);
            else
                spriteBatch.Draw(arrow, new Vector2(x, y + 55), Color.LightBlue);
            //}
            //else
            //{
            //    spriteBatch.DrawString(HUDFont, "CONTINUE", new Vector2(x, y), Color.LightBlue);
            //    spriteBatch.DrawString(HUDFont, "NEW GAME", new Vector2(x, y + 50), Color.Yellow);
            //}
        }

        private void draw_HUD(SpriteBatch spriteBatch)
        {
            int x = top_left.X;
            int y = top_left.Y;

            draw_filled_rect(spriteBatch, new Rectangle(x + 40, y + 30, 300, 10), Color.Red);
            draw_filled_rect(spriteBatch, new Rectangle(x + 40, y + 30, (int)ego_hp_width, 10), Color.Yellow);
            spriteBatch.Draw(ego_portrait, new Rectangle(x, y, 40, 40), Color.White);

            spriteBatch.DrawString(HUDFont, ego_name, new Vector2(x + 60, y), Color.LightGreen);
            spriteBatch.DrawString(HUDFont, score.ToString("000000"), new Vector2(x + 240, y), Color.LightBlue);
            spriteBatch.DrawString(Fonts.QuartzMS60, "x" + ego_lives, new Vector2(x + 350, y), Color.White);

            if (still_display > 0)
            {
                spriteBatch.DrawString(HUDFont, bad_guy_name, new Vector2(x + 60, y + 50), Color.LightGreen);

                spriteBatch.Draw(bad_guy_portrait, new Rectangle(x, y + 50, 40, 40), Color.White);
                draw_filled_rect(spriteBatch, new Rectangle(x + 40, y + 80, health_width, 10), Color.Red);
                draw_filled_rect(spriteBatch, new Rectangle(x + 40, y + 80, (int)bad_hp_width, 10), Color.Yellow);
                draw_filled_rect(spriteBatch, new Rectangle(x + 40, y + 80, (int)blue_hp_width, 10), Color.Blue);
            }
        }

        public void initialise()
        {
            still_display = 0;
        }

        public void draw_filled_rect(SpriteBatch spriteBatch, Rectangle rect, Color col)
        {
            spriteBatch.Draw(whiteDot, rect, col);
        }
    }
}


//class Wilson
//{
//    ulong num_gay_lovers;
//    int num_pikachu_cars;
//    sbyte account_balance;
//    List<string> list_of_people_he_hates;
//    int hours_allowed_out_of_the_house;

//    public void assign_vals_a(Wilson w)
//    {
//        num_gay_lovers = w.num_gay_lovers;
//        num_pikachu_cars = w.num_pikachu_cars;
//        account_balance = w.account_balance;
//    }

//    public void assign_vals_b(ulong num_gay_lovers, int num_pikachu_cars, sbyte account_balance)
//    {
//        this.num_gay_lovers = num_gay_lovers;
//        this.num_pikachu_cars = num_pikachu_cars;
//        this.account_balance = account_balance;
//    }
//}