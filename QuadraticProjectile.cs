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
    //public class QuadraticProjectile : Projectile
    //{
    //    int initial_x;

    //    public QuadraticProjectile(Rectangle position, int speed, Direction facing, Animation animation, Singletons singletons, string texture_name, int baseline, Character source, float scale, int damage)
    //        : base(position, speed, facing, animation, singletons, texture_name, baseline, source, scale, damage)
    //    {
    //        initial_x = position.Center.X;
    //    }

    //    public override void move()
    //    {
    //        if (posY >= baseline)
    //        {
    //            posY = baseline;
    //            return;
    //        }

    //        posY += 4;
    //        posX -= 4;

    //        //animate_check();

    //        //posY += 4;
    //        //int dist = Math.Abs(posY - baseline);
    //        //int dist_squared = dist * dist;
    //        //int smaller = dist_squared / 10000;
            
    //        ////dist = dist / 100;

    //        //if (facing == Character.Direction.left)
    //        //    posX = initial_x + smaller;
    //        //else
    //        //    posX = initial_x - smaller;
    //    }
    //}
}
