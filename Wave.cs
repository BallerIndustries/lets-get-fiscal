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
    public class Wave
    {
        public List<WaveElement> bad_guys;
        public int id;
        public int x_limit;

        public Wave(int id, int x_limit, int num)
        {
            bad_guys = new List<WaveElement>(num);
            this.id = id;
            this.x_limit = x_limit;
        }

        public Wave(int id, int x_limit)
        {
            bad_guys = new List<WaveElement>(2);
            this.id = id;
            this.x_limit = x_limit;
        }
    }

    public class WaveElement
    {
        public string sheet_name;
        public string collision_name;
        public int hp;
        public Point spawn_pos;
        public int ego_x; //The BadGuy will not spawn unless the ego has passed this point.
        public BadGuy.AiState init_ai_state;
        public float scale;
        public Type type;

        public enum Type
        {
            guido_strong,
            guido_weak,
            ken_strong,
            ken_weak,
            ben_seib,
            coral,
            nicole,
            fitty_cent,
            butabi,
            silvio,
            customer_girl,
            kiki,
            treeboi,
            kone,
            running_treeboi,
            running_kone,
            commander,
            fred
        }


        public WaveElement(Type type, Point spawn_pos, int ego_x)
        {
            this.spawn_pos = spawn_pos;
            this.ego_x = ego_x;
            this.type = type;
           
            //Fill in remaining fields based on the type
            set_fields(type);
            //init_ai_state = BadGuy.AiState.walk;
        }

        public WaveElement(Type type, Point spawn_pos)
        {
            this.spawn_pos = spawn_pos;
            this.ego_x = 0;
            this.type = type;

            //Fill in remaining fields based on the type
            set_fields(type);
        }

        private void set_fields(Type type)
        {
            switch (type)
            {
                case Type.fred:
                    sheet_name = "spritesheets//fernando";
                    collision_name = "collisions//fernando_m";
                    hp = 200;
                    scale = 2.8f;
                    init_ai_state = BadGuy.AiState.none;
                    break;

                case Type.commander:
                    sheet_name = "spritesheets//commander";
                    collision_name = "collisions//commander_m";
                    hp = 200;
                    scale = 2.8f;
                    init_ai_state = BadGuy.AiState.none;
                    break;

                case Type.running_treeboi:
                    sheet_name = "spritesheets//treeboi";
                    collision_name = "collisions//treeboi_m";
                    hp = 200;
                    scale = 2.8f;
                    init_ai_state = BadGuy.AiState.running;
                    break;

                case Type.running_kone:
                    sheet_name = "spritesheets//kone";
                    collision_name = "collisions//kone_m";
                    hp = 200;
                    scale = 2.8f;
                    init_ai_state = BadGuy.AiState.running;
                    break;

                case Type.treeboi:
                    sheet_name = "spritesheets//treeboi";
                    collision_name = "collisions//treeboi_m";
                    hp = 200;
                    scale = 2.8f;
                    init_ai_state = BadGuy.AiState.none;
                    break;

                case Type.kone:
                    sheet_name = "spritesheets//kone";
                    collision_name = "collisions//kone_m";
                    hp = 200;
                    scale = 2.8f;
                    init_ai_state = BadGuy.AiState.none;
                    break;

                case Type.kiki:
                    sheet_name = "spritesheets//kiki";
                    collision_name = "collisions//kiki_m";
                    hp = 200;
                    scale = 2.8f;
                    init_ai_state = BadGuy.AiState.none;
                    break;

                case Type.customer_girl:
                    sheet_name = "spritesheets//customer_girl";
                    collision_name = "collisions//customer_girl_m";
                    hp = 40;
                    init_ai_state = BadGuy.AiState.none;
                    scale = 2.8f;
                    break;

                case Type.ben_seib:
                    sheet_name = "spritesheets//ben_seib";
                    collision_name = "collisions//ben_seib_m";
                    hp = 40;
                    init_ai_state = BadGuy.AiState.none;
                    scale = 2.8f;
                    break;

                case Type.butabi:
                    sheet_name = "spritesheets//roxbury";
                    collision_name = "collisions//roxbury_m";
                    hp = 100;
                    init_ai_state = BadGuy.AiState.none;
                    scale = 2.8f;
                    break;

                case Type.coral:
                    sheet_name = "spritesheets//coral";
                    collision_name = "collisions//coral_m";
                    hp = 50;
                    init_ai_state = BadGuy.AiState.none;
                    scale = 2.8f;
                    break;

                case Type.fitty_cent:
                    sheet_name = "spritesheets//fitty_cent";
                    collision_name = "collisions//fitty_cent_m";
                    hp = 10;
                    init_ai_state = BadGuy.AiState.none;
                    scale = 2.8f;
                    break;

                case Type.guido_strong:
                    sheet_name = "spritesheets//guido";
                    collision_name = "collisions//guido_m";
                    hp = 100;
                    init_ai_state = BadGuy.AiState.none;
                    scale = 2.8f;
                    break;

                case Type.guido_weak:
                    sheet_name = "spritesheets//guido";
                    collision_name = "collisions//guido_m";
                    hp = 50;
                    init_ai_state = BadGuy.AiState.none;
                    scale = 2.8f;
                    break;

                case Type.ken_strong:
                    sheet_name = "spritesheets//ken";
                    collision_name = "collisions//ken_m";
                    hp = 100;
                    init_ai_state = BadGuy.AiState.none;
                    scale = 2.8f;
                    break;

                case Type.ken_weak:
                    sheet_name = "spritesheets//ken";
                    collision_name = "collisions//ken_m";
                    hp = 50;
                    init_ai_state = BadGuy.AiState.none;
                    scale = 2.8f;
                    break;

                case Type.nicole:
                    sheet_name = "spritesheets//nicole";
                    collision_name = "collisions//nicole_m";
                    hp = 50;
                    init_ai_state = BadGuy.AiState.none;
                    scale = 2.8f;
                    break;

                case Type.silvio:
                    sheet_name = "spritesheets//silvio";
                    collision_name = "collisions//silvio_m";
                    hp = 70;
                    init_ai_state = BadGuy.AiState.none;
                    scale = 2.8f;
                    break;
            }
        }
    }
}
