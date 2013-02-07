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
    //Contain a list of acts
    public class Level
    {
        public enum LevelName
        {
            TBK_Club,
            BIFO,
            BS_Brothel,
            Newspaper
        }

        const int num_acts = 3;
        public LevelName level_name;
        public Act[] acts = new Act[num_acts];

        public Level(LevelName level_name)
        {
            this.level_name = level_name;
        }

        public void add_act(Act a, int pos)
        {
            acts[pos] = a;
            a.position = pos;
        }
    }

    //Contain a list of backgrounds
    public class Act
    {
        public enum ActName
        {
            main_club_area,
            vip_area,
            TBK_office,
            customer_area,
            kitchen,
            freezer,
            meeting_area,
            conjoined_rooms,
            top_end_room,
            printing_area,
            reporters_offices,
            cheif_editor_office
        }

        public ActName act_name;
        public Microsoft.Xna.Framework.Audio.Cue music;
        public List<Background> backgrounds = new List<Background>(5);
        public List<Foreground> foregrounds = new List<Foreground>(5);
        public List<Prop> props = new List<Prop>(5);
        public bool act_over = false;
        public int min_y;
        public int wave_num = 0;
        public int position;
        public Wave[] waves;

        public Wave current_wave
        {
            get { return waves[wave_num]; }
        }

        public Act(ActName act_name, int min_y, int num_waves, string music_name)
        {
            this.act_name = act_name;
            this.min_y = min_y;
            waves = new Wave[num_waves];

            music = Singletons.soundBank.GetCue(music_name);
        }

        public void Initialise()
        {
            act_over = false;
            wave_num = 0;
        }

        public void add_background(Background bg)
        {
            //Figure out what position this background should be placed at.
            if (backgrounds.Count == 0)
            {
                bg.region.X = 0;
                bg.region.Y = 0;
            }
            else
            {
                //Get the last index
                Background last_bg = backgrounds[backgrounds.Count - 1];

                bg.region.X = last_bg.region.Right - 1;
                bg.region.Y = 0;
            }

            backgrounds.Add(bg);
        }

        public void add_foreground(Foreground fg)
        {
            if (foregrounds.Count == 0)
            {
                fg.region.X = 0;
                fg.region.Y = 0;
            }
            else
            {
                Foreground last_fg = foregrounds[foregrounds.Count - 1];

                fg.region.X = last_fg.region.Right - 1;
                fg.region.Y = 0;
            }

            foregrounds.Add(fg);
        }
    }
}
