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
    public class GOSign
    {
        SpriteBatch spriteBatch;
        TextureManager tm;
        SoundBank soundBank;
        Texture2D sign_texture;
        Texture2D fight_sign_texture;

        bool visible = false;
        bool playing = false;
        int hold_for, play_count;

        public GOSign()
        {   
        }

        public void Initialise(SpriteBatch spriteBatch, TextureManager tm, SoundBank soundBank)
        {
            this.spriteBatch = spriteBatch;
            this.tm = tm;
            this.soundBank = soundBank;

            sign_texture = tm.find_texture("GO_Sign");
            fight_sign_texture = tm.find_texture("FIGHT_Sign");

            playing = false;
            visible = false;
        }

        public void Start()
        {
            visible = true;
            playing = true;

            soundBank.PlayCue("go_noise");
            hold_for = 20;
            play_count = 5;
        }

        //Called when changing acts and other such NONSENSE!
        public void Stop()
        {
            visible = false;
            playing = false;
        }

        public void Update()
        {
            if (playing)
            {
                hold_for--;

                if (hold_for <= 0)
                {
                    visible = !visible;

                    hold_for = 20;


                    if (visible)
                    {
                        play_count--;

                        if (play_count < 0)
                            playing = false;
                        else
                            soundBank.PlayCue("go_noise");
                    }
                }

                
            }
        }

        public void Draw()
        {
            if (visible && playing)
            {
                if (play_count > 0)
                    spriteBatch.Draw(sign_texture, new Rectangle(710, 83, sign_texture.Width * 2, sign_texture.Height * 2), Color.White);//spriteBatch.Draw(sign_texture, new Vector2(782, 83), Color.White);
                else
                    spriteBatch.Draw(fight_sign_texture, new Rectangle(700, 83, fight_sign_texture.Width * 2, fight_sign_texture.Height * 2), Color.White);//spriteBatch.Draw(sign_texture, new Vector2(782, 83), Color.White);
            }
        }

    }
}
