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
    public class Foreground
    {
        public Rectangle region;
        public string texture_name;
        public Texture2D texture;
        public TextureManager tm;

        public Foreground(Rectangle region, string texture_name, TextureManager tm)
        {
            this.region = region;
            this.texture_name = texture_name;
            this.tm = tm;

            texture = tm.find_texture(texture_name);
        }
    }
}
