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
        private bool pressed_once(Keys key)
        {
#if WINDOWS
            return kbState.IsKeyDown(key) && prevkbState.IsKeyUp(key);
#else
            return false;
#endif
        }

        private bool pressed_once(Buttons button)
        {
#if XBOX
            return gpState.IsButtonDown(button) && prevgpState.IsButtonUp(button);
#else
            return false;
#endif
        }

        private bool pressed(Keys key)
        {
            return kbState.IsKeyDown(key);
        }

        private bool pressed(Buttons button)
        {
            return gpState.IsButtonDown(button);
        }

        private bool both_pressed(Keys keyA, Keys keyB)
        {
            return kbState.IsKeyDown(keyA) && kbState.IsKeyDown(keyB) && (prevkbState.IsKeyUp(keyA) || prevkbState.IsKeyUp(keyB));
        }

        private bool both_pressed(Buttons buttonA, Buttons buttonB)
        {
            return gpState.IsButtonDown(buttonA) && gpState.IsButtonDown(buttonB) && (prevgpState.IsButtonUp(buttonA) || prevgpState.IsButtonUp(buttonB));
        }

        private bool any_walking()
        {

#if WINDOWS
            return kbState.IsKeyDown(Keys.Left) || kbState.IsKeyDown(Keys.Right) || kbState.IsKeyDown(Keys.Up) || kbState.IsKeyDown(Keys.Down);
#else
            return gpState.IsButtonDown(Buttons.DPadLeft) || gpState.IsButtonDown(Buttons.DPadRight) || gpState.IsButtonDown(Buttons.DPadUp) || gpState.IsButtonDown(Buttons.DPadDown)
                || Math.Abs(gpState.ThumbSticks.Left.X) > 0.5f || Math.Abs(gpState.ThumbSticks.Left.Y) > 0.5f;
#endif
        }

        private bool up()
        {
#if WINDOWS
            return pressed(Keys.Up);
#else
            return pressed(Buttons.DPadUp) || gpState.ThumbSticks.Left.Y > 0.5f;
#endif
        }

        private bool down()
        {
#if WINDOWS
            return pressed(Keys.Down);
#else
            return pressed(Buttons.DPadDown) || gpState.ThumbSticks.Left.Y < -0.5f;
#endif
        }

        private bool left()
        {
#if WINDOWS
            return pressed(Keys.Left);
#else
            return pressed(Buttons.DPadLeft) || gpState.ThumbSticks.Left.X < -0.5f;
#endif
        }

        private bool right()
        {
#if WINDOWS
            return pressed(Keys.Right);
#else
            return pressed(Buttons.DPadRight) || gpState.ThumbSticks.Left.X > 0.5f;
#endif
        }

        private bool left_once()
        {
#if WINDOWS
            return pressed_once(Keys.Left);
#else
            return pressed_once(Buttons.DPadLeft) || gpState.ThumbSticks.Left.X < -0.5f && prevgpState.ThumbSticks.Left.X >= -0.5f;
#endif
        }

        private bool right_once()
        {
#if WINDOWS
            return pressed_once(Keys.Right);
#else
            return pressed_once(Buttons.DPadRight) || gpState.ThumbSticks.Left.X > 0.5f && prevgpState.ThumbSticks.Left.X <= 0.5f;
#endif
        }

        private bool up_once()
        {
#if WINDOWS
            return pressed_once(Keys.Up);
#else
            return pressed_once(Buttons.DPadUp) || gpState.ThumbSticks.Left.Y > 0.5f && prevgpState.ThumbSticks.Left.Y <= 0.5f;
#endif
        }

        private bool down_once()
        {
#if WINDOWS
            return pressed_once(Keys.Down);
#else
            return pressed_once(Buttons.DPadDown) || gpState.ThumbSticks.Left.Y < -0.5f && prevgpState.ThumbSticks.Left.Y >= -0.5f;
#endif
        }

    }
}
