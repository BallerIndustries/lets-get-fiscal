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
    //This class is responsible for all the projectiles that are on the screen.
    //It moves them along each frame, removes them from the list if they are off
    //the screen and maybe some other stuff I'll think of later.
    public class ProjectileManager
    {
        public List<Projectile> projectile_list = new List<Projectile>(50);
        public List<BasicGameObject> all_gameobjects = new List<BasicGameObject>(30);
        Camera camera;

        public ProjectileManager(Camera camera, List<BasicGameObject> all_gameobjects)
        {
            this.camera = camera;
            this.all_gameobjects = all_gameobjects;
        }

        public void move_projectiles()
        {
            for (int i = 0; i < projectile_list.Count; i++)
            {
                Projectile p = projectile_list[i];
                p.move();

                if (camera.viewport_rect.Intersects(p.position) == false || p.has_hit)
                {
                    projectile_list.Remove(p);
                    all_gameobjects.Remove(p);
                }
            }
        }

        public void add_projectile(Projectile p)
        {
            projectile_list.Add(p);
            all_gameobjects.Add(p);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Projectile p in projectile_list)
            {
                camera.Draw(p, spriteBatch);
            }
        }

        public List<Collision> get_collisions()
        {
            List<Collision> list = new List<Collision>(projectile_list.Count);

            foreach (Projectile p in projectile_list)
            {
                //Convert the projectile into a Collision
                list.Add(new Collision(Collision.collision_type.bad_guy, p.position, p.damage, p.baseline, p.facing, false, "psh" , p.source, true, p));
            }

            return list;
        }



    }
}
