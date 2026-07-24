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
    public class LevelAnnounce
    {
        string audio_name;
        int level_num;

        Vector2 a_end = new Vector2(162, 223);
        Vector2 b_end = new Vector2(443, 223);
        Vector2 c_end = new Vector2(515, 223);

        Vector2 a_pos;
        Vector2 b_pos;
        Vector2 c_pos;

        Vector2 a_start = new Vector2(960, 223);
        Vector2 b_start = new Vector2(443, -200);
        Vector2 c_start = new Vector2(-300, 223);

        public int time;
        SpriteBatch spriteBatch;
        SpriteFont LevelAnnounceFont;

        bool sound_played;

        public enum State
        {
            moving_in,
            holding,
            moving_out
        }

        public LevelAnnounce()
        {
        }

        public void Initialise(string audio_name, int level_num, SpriteBatch spriteBatch, SpriteFont LevelAnnounceFont)
        {
            this.audio_name = audio_name;
            this.level_num = level_num;
            this.LevelAnnounceFont = LevelAnnounceFont;

            this.spriteBatch = spriteBatch;

            time = 0;
            sound_played = false;

            a_pos = a_start;
            b_pos = b_start;
            c_pos = c_start;
        }

        public void Update()
        {
            time++;

            if (!sound_played)
            {
                Singletons.PlayMusic();
                sound_played = true;
            }
            //First 2 seconds scroll in
            //Next 2 seconds hold
            //Next 2 seconds scroll out.
            //Next 2 seconds fade in.

            if (time < 60)
            {
                a_pos.X = MathHelper.SmoothStep(a_start.X, a_end.X, time / 60f);
                b_pos.Y = MathHelper.SmoothStep(b_start.Y, b_end.Y, time / 60f);
                c_pos.X = MathHelper.SmoothStep(c_start.X, c_end.X, time / 60f);
            }
            else if (time >= 120 && time < 150)
            {
                a_pos.X = MathHelper.SmoothStep(a_end.X, a_start.X, (time - 120) / 30.0f);
                b_pos.Y = MathHelper.SmoothStep(b_end.Y, b_start.Y, (time - 120) / 30.0f);
                c_pos.X = MathHelper.SmoothStep(c_end.X, c_start.X, (time - 120) / 30.0f);
            }
        }

        public void Draw()
        {
            spriteBatch.DrawString(LevelAnnounceFont, "LEVEL", a_pos, Color.White);
            spriteBatch.DrawString(LevelAnnounceFont, level_num.ToString(), b_pos, Color.White);
            spriteBatch.DrawString(LevelAnnounceFont, "START", c_pos, Color.White);
        }
    }
}
