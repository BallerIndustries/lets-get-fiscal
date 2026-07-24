using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace Lets_Get_Fiscal
{
    public struct AnnounceText
    {
        public Vector2 start;
        public Vector2 pos;
        public Vector2 end;
        public string text;

        public AnnounceText(Vector2 start, Vector2 pos, Vector2 end, string text)
        {
            this.start = start;
            this.pos = pos;
            this.end = end;
            this.text = text;
        }

        public void smooth_step(float percentage)
        {
            pos = Vector2.SmoothStep(start, end, percentage);
        }

        public void allign_end_pos(SpriteFont font, int x_allign)
        {
            this.end.X = x_allign - font.MeasureString(text).X;
        }

        public static void set_start_x_left(ref AnnounceText one, ref AnnounceText two, ref AnnounceText three, SpriteFont font)
        {
            one.pos.X = one.start.X = one.end.X - 1000;
            two.pos.X = two.start.X = two.end.X - 1000;
            three.pos.X = three.start.X = three.end.X - 1000;

            //int num = (int)font.MeasureString(three.text).X;
            //three.start.X = -num;

            //num += (int)font.MeasureString(two.text).X;
            //two.start.X = -num;

            //num += (int)font.MeasureString(one.text).X;
            //three.start.X = -num;
        }

        public static void set_start_x_right(ref AnnounceText one, ref AnnounceText two, ref AnnounceText three, SpriteFont font)
        {
            one.pos.X = one.start.X = one.end.X + 1000;
            two.pos.X = two.start.X = two.end.X + 1000;
            three.pos.X = three.start.X = three.end.X + 1000;

            //int num = 960;
            //one.start.X = num;

            //num += (int)font.MeasureString(two.text).X;
            //two.start.X = num;

            //num += (int)font.MeasureString(three.text).X;
            //three.start.X = num;
        }

        public static void set_end_x(ref AnnounceText one, ref AnnounceText two, ref AnnounceText three, int x, SpriteFont font)
        {
            one.allign_end_pos(font, x);
            two.allign_end_pos(font, x);
            three.allign_end_pos(font, x);
        }

        public static void set_start_y(ref AnnounceText one, ref AnnounceText two, ref AnnounceText three, int y)
        {
            one.start.Y = two.start.Y = three.start.Y = y;
            one.end.Y = two.end.Y = three.end.Y = y;
            one.pos.Y = two.pos.Y = three.pos.Y = y;

            one.start.Y = two.start.Y = three.start.Y = y;
            one.end.Y = two.end.Y = three.end.Y = y;
            one.pos.Y = two.pos.Y = three.pos.Y = y;
        } 
    }

    public class StageCleared
    {
        string audio_name;
        int level_num;

        //const int jur = 50;
        public  const float slide_in_complete = 20;
        public  const float fade_out = 20;
        public  const float slide_up = 60;
        public  const float slide_up_complete = 80;
        public const float stats_complete = 100;

        AnnounceText d1, d2, d3, e1, e2, e3, f1, f2, f3;

        Vector2 a_end = new Vector2(142, 223);
        Vector2 b_end = new Vector2(420, 223);
        Vector2 c_end = new Vector2(495, 223);
        //Vector2 d_end = new Vector2();
        //Vector2 e_end = new Vector2();
        //Vector2 f_end = new Vector2();

        Vector2 a_pos;
        Vector2 b_pos;
        Vector2 c_pos;
        //Vector2 d_pos;
        //Vector2 e_pos;
        //Vector2 f_pos;

        Vector2 a_start = new Vector2(960, 223);
        Vector2 b_start = new Vector2(393, -200);
        Vector2 c_start = new Vector2(-300, 223);
        //Vector2 d_start = new Vector2();
        //Vector2 e_start = new Vector2();
        //Vector2 f_start = new Vector2();



        public int time;
        SpriteBatch spriteBatch;
        SpriteFont LevelAnnounceFont, StatsFont;
        string d_text, e_text, f_text;

        bool sound_played, added_score;
        int score;

        public enum State
        {
            moving_in,
            holding,
            moving_out
        }

        public StageCleared()
        {
        }

        public void Initialise(string audio_name, int level_num, SpriteBatch spriteBatch, SpriteFont LevelAnnounceFont, SpriteFont StatsFont)
        {
            this.audio_name = audio_name;
            this.level_num = level_num;
            this.LevelAnnounceFont = LevelAnnounceFont;
            this.StatsFont = StatsFont;

            this.spriteBatch = spriteBatch;

            time = 0;
            sound_played = false;
            added_score = false;

            a_pos = a_start;
            b_pos = b_start;
            c_pos = c_start;

            float dick_penis = c_end.X + LevelAnnounceFont.MeasureString("CLEARED").X;

            int life_bonus = Singletons.ego.hp * 100;
            int ballertude = Singletons.random.Next(100) * 100;
            int credit_rating = Singletons.random.Next(100) * 100;

            score = life_bonus + ballertude + credit_rating;

            d1 = new AnnounceText(Vector2.Zero, Vector2.Zero, Vector2.Zero, "LIFE BONUS");
            d2 = new AnnounceText(Vector2.Zero, Vector2.Zero, Vector2.Zero, life_bonus.ToString());
            d3 = new AnnounceText(Vector2.Zero, Vector2.Zero, Vector2.Zero, "PTS");

            e1 = new AnnounceText(Vector2.Zero, Vector2.Zero, Vector2.Zero, "BALLERTUDE");
            e2 = new AnnounceText(Vector2.Zero, Vector2.Zero, Vector2.Zero, ballertude.ToString());
            e3 = new AnnounceText(Vector2.Zero, Vector2.Zero, Vector2.Zero, "PTS");

            f1 = new AnnounceText(Vector2.Zero, Vector2.Zero, Vector2.Zero, "CREDIT RATING");
            f2 = new AnnounceText(Vector2.Zero, Vector2.Zero, Vector2.Zero, credit_rating.ToString());
            f3 = new AnnounceText(Vector2.Zero, Vector2.Zero, Vector2.Zero, "PTS");

            AnnounceText.set_start_y(ref d1, ref d2, ref d3, 200);
            AnnounceText.set_start_y(ref e1, ref e2, ref e3, 250);
            AnnounceText.set_start_y(ref f1, ref f2, ref f3, 300);

            AnnounceText.set_end_x(ref d1, ref e1, ref f1, 480, StatsFont);
            AnnounceText.set_end_x(ref d2, ref e2, ref f2, 600, StatsFont);
            AnnounceText.set_end_x(ref d3, ref e3, ref f3, 700, StatsFont);

            AnnounceText.set_start_x_left(ref d1, ref d2, ref d3, StatsFont);
            AnnounceText.set_start_x_right(ref e1, ref e2, ref e3, StatsFont);
            AnnounceText.set_start_x_left(ref f1, ref f2, ref f3, StatsFont);

            //d_text = "life bonus " + life_bonus.ToString() + " pts";
            //e_text = "ballertude " + ballertude.ToString() + " pts";
            //f_text = "credit rating " + credit_rating.ToString() + " pts";

            //d_start.X = d_pos.X = -StatsFont.MeasureString(d_text).X;
            //e_start.X = e_pos.X = 960;
            //f_start.X = f_pos.X = -StatsFont.MeasureString(f_text).X;

            //d_end.Y = d_start.Y = d_pos.Y = 200;
            //e_end.Y = e_start.Y = e_pos.Y = 250;
            //f_end.Y = f_start.Y = f_pos.Y = 300;

            //d_end.X = (960 - StatsFont.MeasureString(d_text).X) / 2;
            //e_end.X = (960 - StatsFont.MeasureString(e_text).X) / 2;
            //f_end.X = (960 - StatsFont.MeasureString(f_text).X) / 2;
        }

        public void add_score()
        {
            Singletons.game_state.score += score;
            added_score = true;
        }

        public void Update()
        {
            time++;

            if (!sound_played)
            {
                Singletons.soundBank.PlayCue(audio_name);
                //Singletons.PlayMusic();
                sound_played = true;
            }

            if (!added_score)
                add_score();

            //First 2 seconds scroll in
            //Next 2 seconds hold
            //Next 2 seconds scroll out.
            //Next 2 seconds fade in.

            if (time < slide_in_complete)
            {
                a_pos = Vector2.SmoothStep(a_start, a_end, time / slide_in_complete);
                b_pos = Vector2.SmoothStep(b_start, b_end, time / slide_in_complete);
                c_pos = Vector2.SmoothStep(c_start, c_end, time / slide_in_complete);
                //a_pos.X = MathHelper.SmoothStep(a_start.X, a_end.X, time / slide_in_complete);
                //b_pos.Y = MathHelper.SmoothStep(b_start.Y, b_end.Y, time / slide_in_complete);
                //c_pos.X = MathHelper.SmoothStep(c_start.X, c_end.X, time / slide_in_complete);
            }
            else if (time >= slide_up && time <= slide_up_complete)
            {
                float percentage = ((float)time - slide_up) / (float)(slide_up_complete - slide_up);

                a_pos.Y = MathHelper.SmoothStep(a_end.Y, 70, percentage);
                b_pos.Y = MathHelper.SmoothStep(b_end.Y, 70, percentage);
                c_pos.Y = MathHelper.SmoothStep(c_end.Y, 70, percentage);
            }
            else if (time >= slide_up_complete && time <= stats_complete)
            {
                float percentage = ((float)time - slide_up_complete) / (float)(stats_complete - slide_up_complete);

                //d_pos = Vector2.SmoothStep(d_start, d_end, percentage);
                //e_pos = Vector2.SmoothStep(e_start, e_end, percentage);
                //f_pos = Vector2.SmoothStep(f_start, f_end, percentage);

                d1.smooth_step(percentage);
                d2.smooth_step(percentage);
                d3.smooth_step(percentage);

                e1.smooth_step(percentage);
                e2.smooth_step(percentage);
                e3.smooth_step(percentage);

                f1.smooth_step(percentage);
                f2.smooth_step(percentage);
                f3.smooth_step(percentage);
            }


        }

        public void Draw()
        {
            spriteBatch.DrawString(LevelAnnounceFont, "LEVEL", a_pos, Color.White);
            spriteBatch.DrawString(LevelAnnounceFont, level_num.ToString(), b_pos, Color.White);
            spriteBatch.DrawString(LevelAnnounceFont, "CLEARED", c_pos, Color.White);

            //spriteBatch.DrawString(StatsFont, d_text, d_pos, Color.White);
            //spriteBatch.DrawString(StatsFont, e_text, e_pos, Color.White);
            //spriteBatch.DrawString(StatsFont, f_text, f_pos, Color.White);

            spriteBatch.DrawString(StatsFont, d1.text, d1.pos, Color.White);
            spriteBatch.DrawString(StatsFont, d2.text, d2.pos, Color.White);
            spriteBatch.DrawString(StatsFont, d3.text, d3.pos, Color.White);

            spriteBatch.DrawString(StatsFont, e1.text, e1.pos, Color.White);
            spriteBatch.DrawString(StatsFont, e2.text, e2.pos, Color.White);
            spriteBatch.DrawString(StatsFont, e3.text, e3.pos, Color.White);

            spriteBatch.DrawString(StatsFont, f1.text, f1.pos, Color.White);
            spriteBatch.DrawString(StatsFont, f2.text, f2.pos, Color.White);
            spriteBatch.DrawString(StatsFont, f3.text, f3.pos, Color.White);
        }
    }
}
