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
    public struct Collision
    {
        public static Collision Empty
        {
            get { return new Collision(collision_type.none, Rectangle.Empty, 0, 0, GameObject.Direction.left, false, String.Empty, null, null); }
        }

        public static bool operator ==(Collision a, Collision b)
        {
            return a.source == b.source && a.facing == b.facing && a.region == b.region 
                && a.damage == b.damage && a.baseline == b.baseline && a.sfx_name == b.sfx_name 
                && a.does_KO == b.does_KO && a.is_projectile == b.is_projectile && a.c_source == b.c_source;
        }

        public static bool operator !=(Collision a, Collision b)
        {
            return a.source != b.source || a.facing != b.facing || a.region != b.region
                && a.damage != b.damage || a.baseline != b.baseline || a.sfx_name != b.sfx_name
                && a.does_KO != b.does_KO || a.is_projectile != b.is_projectile || a.c_source != b.c_source;
        }

        public enum collision_type
        {
            none,
            ego,
            bad_guy,
            all
        }

        public collision_type source;
        public GameObject.Direction facing;
        public Rectangle region;
        public int damage;
        public int baseline;
        public string sfx_name;
        public bool does_KO;
        //public bool sound_played;
        public bool is_projectile;
        public Character c_source;
        public Projectile projectile;

        public Collision(collision_type source, Rectangle region, int damage, int baseline, GameObject.Direction facing, bool does_KO, string sfx_name, Character c_source, bool is_projectile, Projectile projectile)
        {
            this.source = source;
            this.region = region;
            this.damage = damage;
            this.baseline = baseline;
            this.facing = facing;
            this.does_KO = does_KO;
            this.sfx_name = sfx_name;
            this.is_projectile = is_projectile;
            this.c_source = c_source;
            this.projectile = projectile;
        }

        public Collision(collision_type source, Rectangle region, int damage, int baseline, GameObject.Direction facing, bool does_KO, string sfx_name, Character c_source, Projectile projectile)
        {
            this.source = source;
            this.region = region;
            this.damage = damage;
            this.baseline = baseline;
            this.facing = facing;
            this.does_KO = does_KO;
            this.sfx_name = sfx_name;
            this.is_projectile = false;
            this.c_source = c_source;
            this.projectile = projectile;
        }
    }
}
