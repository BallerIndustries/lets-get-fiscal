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
    public class Singletons
    {
        public static TextureManager tm;
        public static CollisionManager cm;
        public static ProjectileManager pm;
        public static Camera camera;
        public static Random random;
        public static SoundBank soundBank;
        public static GameState game_state;
        public static MoveManager mm;
        public static AudioCategory music_category;
        public static MusicManager music_manager;
        public static float music_volume = 1.0f;
        public static bool half_speed = false;
        public static Ego ego;

        public static SpriteFont credit_font;

        public static bool shake_screen = false;
        public static int draw_at = -10;

        private static int shake_count;
        public static Texture2D subtitle_overlay;


        public Singletons(TextureManager tm, CollisionManager cm, ProjectileManager pm, Camera camera, Random random, SoundBank soundBank, GameState game_state, MoveManager mm, AudioEngine audioEngine, MusicManager music_manager, Ego ego)
        {
            Singletons.ego = ego;
            Singletons.music_manager = music_manager;
            Singletons.tm = tm;
            Singletons.cm = cm;
            Singletons.pm = pm;
            Singletons.camera = camera;
            Singletons.random = random;
            Singletons.soundBank = soundBank;
            Singletons.game_state = game_state;
            Singletons.mm = mm;

            Singletons.music_category = audioEngine.GetCategory("Music");
            Singletons.music_category.SetVolume(music_volume);

            Singletons.subtitle_overlay = tm.find_texture("subtitle_overlay");
        }

        public static void PlayMusic()
        {
            Singletons.game_state.current_act.music = Singletons.soundBank.GetCue(Singletons.game_state.current_act.music.Name);
            Singletons.game_state.current_act.music.Play();
            //Singletons.music_manager.fade_in();
        }

        public static void StopMusic()
        {
            Singletons.music_category.Stop(AudioStopOptions.AsAuthored);
        }

        public static void PauseMusic()
        {
            Singletons.game_state.current_act.music.Pause();
        }

        public static void ResumeMusic()
        {
            Singletons.game_state.current_act.music.Resume();
        }

        public static void update_shake_screen()
        {
            if (shake_screen)
            {
                shake_count++;

                if (draw_at == -10)
                    draw_at = 0;
                else
                    draw_at = -10;

                if (shake_count > 15)
                {
                    draw_at = -10;
                    shake_count = 0;
                    shake_screen = false;
                }
            }
        }




    }
}
