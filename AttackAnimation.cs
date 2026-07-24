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
    public class AttackAnimation : Animation
    {
        public Rectangle[] attacks;
        public bool[] attack_does_ko;
        public int damage;
        public int min_dist_from_bound = 0;
        public string sfx_name;
        
        public AttackAnimation(Rectangle bound, int numFrames, bool repeats, string name, Character.State move, int damage, string sfx_name, int frames) : base(bound, numFrames, repeats, name, move, frames)
        {
            attacks = new Rectangle[numFrames];
            attack_does_ko = new bool[numFrames];
            this.damage = damage;
            this.sfx_name = sfx_name;
        }

        public AttackAnimation(Rectangle bound, int numFrames, bool repeats, string name, Character.State move, int damage)
            : base(bound, numFrames, repeats, name, move, 6)
        {
            attacks = new Rectangle[numFrames];
            attack_does_ko = new bool[numFrames];
            this.damage = damage;
            this.sfx_name = "psh";
        }

        public AttackAnimation(List<Rectangle> bound_list, bool repeats, Character.State move, int damage, string sfx_name, int frames)
            : base(bound_list, repeats, move, frames)
        {
            attacks = new Rectangle[bound_list.Count];
            attack_does_ko = new bool[bound_list.Count];
            this.damage = damage;
            this.sfx_name = sfx_name;
        }

        public AttackAnimation(List<Rectangle> bound_list, bool repeats, Character.State move, int damage)
            : base(bound_list, repeats, move, 6)
        {
            attacks = new Rectangle[bound_list.Count];
            attack_does_ko = new bool[bound_list.Count];
            this.damage = damage;
            this.sfx_name = "psh";
        }

        public void add_attack(Rectangle rect, int frame_num, bool attack_does_ko)
        {
            if (frame_num >= 0 && frame_num < attacks.Length)
            {
                attacks[frame_num] = rect;
                this.attack_does_ko[frame_num] = attack_does_ko;
            }
        }

        public void add_attack(Rectangle rect, int frame_num)
        {
            if (frame_num >= 0 && frame_num < attacks.Length)
            {
                attacks[frame_num] = rect;
                this.attack_does_ko[frame_num] = false;
            }
        }



    }
}