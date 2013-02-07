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
    public partial class Ego : Character
    {
        //private int num_h_kicks;
        //private bool sent_hadoken;
        //private int hadoken_holds;
        //public const int hurricane_kicks = 3;

        //public void do_hadoken()
        //{
        //    if (current_frame == 3)
        //    {
        //        hadoken_holds++;

        //        if (hadoken_holds > 20)
        //            animate_check();
        //    }
        //    else
        //    {
        //        animate_check();
        //    }

        //    if (current_frame == 3 && sent_hadoken == false)
        //    {
        //        Rectangle fire_ball = getAttackRect(bound, new Rectangle(300, 1875, 12, 25));
        //        fire_ball.Width = 37;
        //        fire_ball.Height = 29;
        //        fire_ball.Y -= position.Height;
        //        sent_hadoken = true;

        //        pm.add_projectile(new Projectile(fire_ball, 12, facing, new Animation(new Rectangle(316, 1764, 37, 29), 3, true, "bananas", State.hadoken), singletons, "ryu", baseline));
        //    }
        //}

        //public Collision do_shoruken()
        //{
        //    if (current_frame < 3)
        //        animate_check();

        //    if (jumpY - posY > 225)
        //        jump_dir = JumpDirection.downwards;

        //    if (jump_dir == JumpDirection.upwards && current_frame > 2)
        //    {
        //        posY -= 15;
        //        if (facing == Direction.right)
        //            posX += 5;
        //        else
        //            posX -= 5;
        //    }

        //    if (jump_dir == JumpDirection.downwards)
        //    {
        //        animate_check();
        //        posY += 15;
        //    }
        //    //Test if we have hit the ground
        //    if (posY >= jumpY && jump_dir == JumpDirection.downwards)
        //        posY = jumpY;

        //    return getCollision();
        //}

        //public void start_dragon_rush()
        //{
        //    state = State.dragon_rush;
        //    add_combo(State.dragon_rush);
        //}

        //public Collision do_dragon_rush()
        //{
        //    if (facing == Direction.right)
        //        delta_position.X = 5;
        //    else
        //        delta_position.X = -5;

        //    animate_check();
        //    AttackAnimation aa = current_animation as AttackAnimation;

        //    return getCollision();
        //}

        //public void start_hurricane_kick()
        //{
        //    state = State.hurricane_kick;
        //    num_h_kicks = 0;
        //}

        //public Collision do_hurricane_kick()
        //{
        //    animate_check();

        //    //If we haven't done the amount of h_kicks we are supposed to do.
        //    if (num_h_kicks < hurricane_kicks && current_frame > 6)
        //    {
        //        current_frame = 4;
        //        num_h_kicks++;
        //    }

        //    if (current_frame <= 8)
        //    {
        //        if (facing == Direction.right)
        //            delta_position.X = speed / 2;
        //        else
        //            delta_position.X = speed / -2;
        //    }

        //    return getCollision();
        //}

        //public void start_hadoken()
        //{
        //    state = State.hadoken;

        //    sent_hadoken = false;
        //    hadoken_holds = 0;
        //}

        //public void start_shoruken()
        //{
        //    state = State.shoruken;
        //    jumpY = posY;
        //    jump_dir = JumpDirection.upwards;
        //}
    }
}
