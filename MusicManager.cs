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
    public class MusicManager
    {
        public enum State
        {
            none,
            fading_in,
            fading_out
        }

        public State state;

        public MusicManager()
        {
            state = State.none;
        }

        public void fade_in()
        {
            state = State.fading_in;
        }

        public void fade_out()
        {
            state = State.fading_out;
        }

        public void Update()
        {
            if (state == State.fading_in)
            {
                Singletons.music_volume = MathHelper.Clamp(Singletons.music_volume + 0.03f, 0.0f, 1.0f);
                Singletons.music_category.SetVolume(Singletons.music_volume);
                //0.03f

                if (Singletons.music_volume == 1.0f)
                    state = State.none;

            }
            else if (state == State.fading_out)
            {
                Singletons.music_volume = MathHelper.Clamp(Singletons.music_volume - 0.03f, 0.0f, 1.0f);
                Singletons.music_category.SetVolume(Singletons.music_volume);
            }
            else
            {
                //if (!Singletons.game_state.current_act.music.IsPlaying)
                //    Singletons.game_state.current_act.music.Play();
            }
        }

    }
}
