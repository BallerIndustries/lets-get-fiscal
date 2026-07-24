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
    public class Item : BasicGameObject
    {
        public enum Type
        {
            none,
            one_up,
            coke,
            steak,
            cash,
            diamonds,
            baseball_bat
        }

        public Type type;
        
        public Item(TextureManager tm, Type type, Point position) : base(tm, "props")
        {
            this.posX = position.X;
            this.posY = position.Y;
            this.type = type;

            set_bound(type);

            if (this as Weapon != null)
                texture = tm.find_texture("weapons");
        }

        public new Rectangle position
        {
            get { return new Rectangle(posX, posY - posH, bound.Width * 3, bound.Height * 3); }
        }

        void set_bound(Type type)
        {
            switch (type)
            {
                case Type.one_up:
                    bound.X = 36;
                    bound.Y = 345;
                    bound.Width = 27;
                    bound.Height = 20;

                    posW = 27;
                    posH = 20;
                    break;

                case Type.coke:
                    bound.X = 71;
                    bound.Y = 348;
                    bound.Width = 11;
                    bound.Height = 19;

                    posW = 11;
                    posH = 19;
                    break;

                case Type.steak:
                    bound.X = 86;
                    bound.Y = 348;
                    bound.Width = 45;
                    bound.Height = 26;

                    posW = 45;
                    posH = 26;
                    break;

                case Type.cash:
                    bound.X = 142;
                    bound.Y = 349;
                    bound.Width = 33;
                    bound.Height = 21;

                    posW = 33;
                    posH = 21;
                    break;

                case Type.diamonds:
                    bound.X = 180;
                    bound.Y = 347;
                    bound.Width = 36;
                    bound.Height = 25;

                    posW = 36;
                    posH = 25;
                    break;

                case Type.baseball_bat:
                    bound.X = 64;
                    bound.Y = 33;
                    bound.Width = 48;
                    bound.Height = 9;

                    posW = 48;
                    posH = 9;
                    break;
            }

            posW = (int)(posW * 2.5f);
            posH = (int)(posH * 2.5f);
        }

    }
}
