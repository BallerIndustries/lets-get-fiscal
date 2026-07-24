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
    public class Background
    {
        public Rectangle region;
        public string texture_name;
        public Texture2D texture;
        public TextureManager tm;

        public Background(Rectangle region, string texture_name, TextureManager tm)
        {
            this.region = region;
            this.texture_name = texture_name;
            this.tm = tm;

            texture = tm.find_texture(texture_name);
        }
    }
}
