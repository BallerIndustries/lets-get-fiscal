using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Diagnostics;

namespace Lets_Get_Fiscal
{
    public class CrashDebugGame : Game
    {
        private SpriteBatch spriteBatch;
        private SpriteFont font;
        private readonly Exception exception;

        public CrashDebugGame(Exception exception)
        {
            this.exception = exception;
            new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            font = Content.Load<SpriteFont>("DebugFont");
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            spriteBatch.DrawString(
               font,
               "**** CRASH LOG ****",
               new Vector2(10, 10),
               Color.White);

            spriteBatch.DrawString(
               font,
               "Press Back to Exit",
               new Vector2(10, 30),
               Color.White);

            spriteBatch.DrawString(
               font,
               string.Format("Exception: {0}", exception.Message),
               new Vector2(10, 50),
               Color.White);

            spriteBatch.DrawString(
               font, string.Format("Stack Trace:\n{0}", exception.StackTrace),
               new Vector2(10, 70),
               Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}