using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace Lets_Get_Fiscal
{
    public partial class Game1 : Microsoft.Xna.Framework.Game
    {
        //ulong time;

        Stopwatch stopwatch;
        double time, prev_time;

        public void update_cutscene(GameTime gameTime)
        {
            kbState = Keyboard.GetState();
            gpState = GamePad.GetState(controlling_player);

            CutScene cs = game_state.current_cs;
            List<Cue> aso = cs.all_scene_objects;
            List<Cue> cdo = cs.currently_displayed;

            //ulong time = (ulong)gameTime.TotalGameTime.TotalMilliseconds - game_time_from;
            //time += (ulong)gameTime.ElapsedGameTime.TotalMilliseconds;
            time += stopwatch.ElapsedMilliseconds - prev_time;
            prev_time = stopwatch.ElapsedMilliseconds;


            //1. Look for Cues that can be added to currently displayed objects
            for (int i = 0; i < aso.Count; i++)
            {
                Cue cue = aso[i];
                if (time > cue.fire_at)
                {
                    cdo.Add(cue);
                    aso.Remove(cue);
                }
            }

            //2. Remove expired Cues from currently displayed objects
            for (int i = 0; i < cdo.Count; i++)
            {
                Cue cue = cdo[i];
                if (time > cue.remove_at && cue.type != CueType.Sound)
                {
                    cdo.Remove(cue);
                }
            }

            //3. Look for the current comic and update any transitions
            foreach (Cue cue in cdo)
            {
                //if (cue.type == CueType.Comic)
                //{
                //    Comic c = cue as Comic;
                //}

                if (cue.type == CueType.Transition)
                {
                    Transition t = cue as Transition;
                    float percentage = ((float)time - (float)t.fire_at) / ((float)t.remove_at - (float)t.fire_at);
                    t.calc_trans_rect(percentage);
                }
                else if (cue.type == CueType.FadeCue)
                {
                    FadeCue f = cue as FadeCue;
                    f.Update(time);
                    
                    //f.
                }

            }

            //4. Play sounds and then remove them
            for (int i = 0; i < cdo.Count; i++)
            {
                Cue cue = cdo[i];

                if (cue.type == CueType.Sound)
                {
                    Sound s = cue as Sound;
                    soundBank.PlayCue(s.name);
                    cdo.Remove(s);
                }
            }

            //5. Check if we have reached the end of the CutScene
            if (cdo.Count == 0 && aso.Count == 0 || pressed_once(Keys.Enter) || pressed_once(Buttons.B))
            {
                StopEffects();
                
                stopwatch.Reset();
                prev_time = 0;
                game_state.state = cs.return_state;
                cdo.Clear();
                aso.Clear();

                //This only happens when going to the boss fight. So we must play music.
                //This is clearly a disgusting hack
                if (game_state.state == GameState.State.GamePlay)
                {
                    Singletons.music_manager.fade_in();
                    Singletons.PlayMusic();
                }

            }

            //6. Skip to the next panel 
            if (pressed_once(Keys.Space) || pressed_once(Buttons.A))
            {
                StopEffects();

                ViewPanel vp = next_vp();

                if (vp != null)
                {
                    time = vp.fire_at;
                }
                else
                {
                    stopwatch.Reset();
                    prev_time = 0;
                    game_state.state = cs.return_state;
                    cdo.Clear();
                    aso.Clear();

                    //This only happens when going to the boss fight. So we must play music.
                    //This is clearly a disgusting hack
                    if (game_state.state == GameState.State.GamePlay)
                    {
                        Singletons.music_manager.fade_in();
                        Singletons.PlayMusic();
                    }
                }
            }

            prevgpState = gpState;
            prevkbState = kbState;
        }


        public void draw_cutscene()
        {
            game_state.current_cs.Draw(spriteBatch);
        }

        ViewPanel next_vp()
        {
            foreach (Cue cue in game_state.current_cs.all_scene_objects)
            {
                if (cue.type == CueType.ViewPanel)
                    return (ViewPanel)cue;
            }

            return null;
        }
    }
}
