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
    public class Fade
    {
        public enum State
        {
            none,
            fade_out,
            darkness,
            ready,      //This means it is ready to fade in
            fade_in
        }

        //How long before this fade out will happen
        public int delay;
        public float alpha_val = 0;
        int darkness_hold;
        public State fade_state = State.none;

        public Fade(int delay)
        {
            this.delay = delay;
        }

        public void Initialise()
        {
            alpha_val = 0;
            darkness_hold = 0;
            fade_state = State.none;
        }

        void increment_alpha()
        {
            alpha_val = MathHelper.Clamp(alpha_val + 0.03f, 0, 1);

            if (alpha_val == 1.0f)
            {
                fade_state = State.darkness;
                darkness_hold = 50;
            }
        }

        void decrement_alpha()
        {
            alpha_val = MathHelper.Clamp(alpha_val - 0.03f, 0, 1);

            if (alpha_val == 0.0f)
                fade_state = State.none;
        }

        public void Update()
        {
            if (fade_state == State.fade_in)
                decrement_alpha();
            else if (fade_state == State.fade_out)
                increment_alpha();
            else if (fade_state == State.darkness)
            {
                darkness_hold--;

                if (darkness_hold <= 0)
                    fade_state = State.ready;
            }
        }
    }
}
