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
    public class Projectile : GameObject
    {
        int _baseline;
        public Character source;
        public bool has_hit;
        public int damage;
        Vector2 speed;

        public Projectile(Rectangle position, Vector2 speed, Character.Direction facing, Animation current_animation, Singletons singletons, string texture_name, int baseline, Character source, float scale, int damage) :
            base(texture_name, singletons, scale)
        {
            this.source = source;
            this.position = position;
            this.speed = speed;
            //this.speed = speed;
            this.facing = facing;
            this.current_animation = current_animation;
            this.damage = damage;
            _baseline = baseline;
        }

        public override int baseline
        {
            get { return _baseline; }
        }

        public virtual void move()
        {
            animate_check();

            if (facing == Character.Direction.left)
                posX -= (int)speed.X;
            else
                posX += (int)speed.X;

            posY += (int)speed.Y;
        }
    }
}
