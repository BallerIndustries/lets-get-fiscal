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
    //This badass motherfucker is responsible for managing all
    //the textures and collision maps. Totally choice.
    public class TextureManager
    {
        const int max_elements = 120;
        int tex_count, col_count = 0;

        ContentManager Content;
        Texture2D[] texture_list = new Texture2D[max_elements];

        //These three lists are all related. Sounds like we need an
        //encapsulating object. BUT FUCK THAT HAHAHAHAHAH
        bool[][] collision_list = new bool[max_elements][];
        string[] collision_name = new string[max_elements];
        int[] collision_width = new int[max_elements];

        public TextureManager(ContentManager Content)
        {
            this.Content = Content;
        }

        public Texture2D find_texture(string name)
        {
            foreach (Texture2D texture in texture_list)
            {
                if (texture == null)
                    continue;

                if (name == texture.Name)
                    return texture;
            }

            //Couldn't find the texture, try adding it.
            return add_texture(name);
        }

        public bool[] find_collision(string name)
        {
            for (int i = 0; i < collision_list.Length; i++)
            {
                if (name == collision_name[i])
                    return collision_list[i];
            }

            return add_collision(name);
        }

        //public Color[] find_color(string name)
        //{
        //    for (int i = 0; i < col_list.Length; i++)
        //    {
        //        if (name == collision_name[i])
        //            return col_list[i];
        //    }

        //    return null;
        //}

        public int find_collision_width(string name)
        {
            for (int i = 0; i < collision_width.Length; i++)
            {
                if (name == collision_name[i])
                    return collision_width[i];
            }

            return -1;
        }

        Texture2D add_texture(string name)
        {
            texture_list[tex_count] = Content.Load<Texture2D>(name);
            texture_list[tex_count].Name = name;
            tex_count++;

            return texture_list[tex_count - 1];
        }

        bool[] add_collision(string name)
        {
            Texture2D texture = Content.Load<Texture2D>(name);
            Color[] col_map          = new Color[texture.Width * texture.Height];
            collision_list[col_count] = new bool[texture.Width * texture.Height];

            texture.GetData(col_map);
            fill_bool_map(col_map, collision_list[col_count]);
            collision_name[col_count] = name;
            collision_width[col_count] = texture.Width;

            col_count++;
            texture = null;
            col_map = null;

            return collision_list[col_count - 1];
        }

        void fill_bool_map(Color[] col_map, bool[] bool_map)
        {
            for (int i = 0; i < col_map.Length; i++)
            {
                if (col_map[i] == Color.Black)
                    bool_map[i] = true;
                else
                    bool_map[i] = false;
            }
        }
    }
}
