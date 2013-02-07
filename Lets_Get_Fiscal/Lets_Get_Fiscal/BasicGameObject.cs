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
    public class BasicGameObject : IComparable
    {
        private Rectangle _position;
        public Rectangle bound;
        public Texture2D texture;
        public int id;
        public bool visible = true;

        public BasicGameObject(TextureManager tm, string sheet)
        {
            this.texture = tm.find_texture(sheet);
        }

        public Rectangle position
        {
            get { return new Rectangle(posX, posY - posH, posW, posH);}
            set { _position = value; }
        }

        public int posX
        {
            set { _position.X = value; }
            get { return _position.X; }
        }

        public int posY
        {
            set { _position.Y = value; }
            get { return _position.Y; }
        }

        public int posW
        {
            set { _position.Width = value; }
            get { return (int)(_position.Width); }
        }

        public int posH
        {
            set { _position.Height = value; }
            get { return (int)(_position.Height); }
        }

        public virtual int baseline
        {
            get { return posY; }
        }

        public int CompareTo(object obj)
        {
            BasicGameObject c = obj as BasicGameObject;

            if (c.baseline == baseline)
            {
                if (c.id < id) return -1;
                else return 1;
            }
            else
            {
                if (c.baseline > baseline) return -1;
                else return 1;
            }
        }
    }
}
