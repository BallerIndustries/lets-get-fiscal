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
    public partial class Game1 : Microsoft.Xna.Framework.Game
    {
        public void update_trial_pause(GameTime gameTime)
        {
            kbState = Keyboard.GetState();
            gpState = GamePad.GetState(controlling_player);

            if (music.IsPaused)
            {
                music.Resume();
            }
            else if (music.IsPlaying == false)
            {
                music = soundBank.GetCue(music.Name);
                music.Play();
            }

            if (down_once())
                in_game_menu_data.selected_index = (int)MathHelper.Clamp(in_game_menu_data.selected_index + 1, 0, 3);

            if (up_once())
                in_game_menu_data.selected_index = (int)MathHelper.Clamp(in_game_menu_data.selected_index - 1, 0, 3);

            if (pressed_once(Keys.Enter) || pressed_once(Buttons.A))
            {
                music.Pause();

                switch (in_game_menu_data.selected_index)
                {
                    //Resume
                    case 0:
                        game_state.state = GameState.State.GamePlay;
                        game_state.current_act.music.Resume();

                        //Singletons.music_category.Resume();
                        break;

                    //New Game
                    //case 1:
                    //    game_state.state = GameState.State.GamePlay;
                    //    initialise_objects();
                    //    break;

                    //Controls
                    case 1:
                        game_state.state = GameState.State.Controls;
                        break;

                    //Buy Game
                    case 2:
                        if (controlling_player.can_buy_game())
                            Guide.ShowMarketplace(controlling_player);
                        break;

                    //Quit
                    case 3:
                        this.Exit();
                        break;
                }
            }

            prevgpState = gpState;
            prevkbState = kbState;
        }

        public void update_full_pause(GameTime gameTime)
        {
            kbState = Keyboard.GetState();
            gpState = GamePad.GetState(controlling_player);

            if (music.IsPaused)
            {
                music.Resume();
            }
            else if (music.IsPlaying == false)
            {
                music = soundBank.GetCue(music.Name);
                music.Play();
            }

            if (down_once())
                in_game_menu_data.selected_index = (int)MathHelper.Clamp(in_game_menu_data.selected_index + 1, 0, 2);

            if (up_once())
                in_game_menu_data.selected_index = (int)MathHelper.Clamp(in_game_menu_data.selected_index - 1, 0, 2);

            if (pressed_once(Keys.Enter) || pressed_once(Buttons.A))
            {
                music.Pause();

                switch (in_game_menu_data.selected_index)
                {
                    //Resume
                    case 0:
                        game_state.state = GameState.State.GamePlay;
                        game_state.current_act.music.Resume();
                    
                        //Singletons.music_category.Resume();
                        break;

                    //New Game
                    //case 1:
                    //    game_state.state = GameState.State.GamePlay;
                    //    initialise_objects();
                    //    break;

                    //Controls
                    case 1:
                        game_state.state = GameState.State.Controls;
                        break;

                    //Buy Game
                    //case 2:
                    //    if (controlling_player.can_buy_game())
                    //        Guide.ShowMarketplace(controlling_player);
                    //    break;

                    //Quit
                    case 2:
                        this.Exit();
                        break;
                }
            }

            prevgpState = gpState;
            prevkbState = kbState;
        }


        public void update_full_menu(GameTime gameTime)
        {
            kbState = Keyboard.GetState();
            gpState = GamePad.GetState(controlling_player);


            if (music.IsPaused)
            {
                music.Resume();
            }
            else if (music.IsPlaying == false)
            {
                music = soundBank.GetCue(music.Name);
                music.Play();
            }

            if (down_once())
                main_menu_data.selected_index = (int)MathHelper.Clamp(main_menu_data.selected_index + 1, 0, 2);

            if (up_once())
                main_menu_data.selected_index = (int)MathHelper.Clamp(main_menu_data.selected_index - 1, 0, 2);//main_menu_data.move_up();

            if (pressed_once(Keys.Enter) || pressed_once(Buttons.A))
            {
                music.Pause();

                switch (main_menu_data.selected_index)
                {
                    //New Game
                    case 0:
                        initialise_cutscene(scene1);
                        break;

                    //Controls
                    case 1:
                        game_state.state = GameState.State.Controls;
                        break;

                    //Buy Game
                    //case 2:
                    //    if (controlling_player.can_buy_game())
                    //        Guide.ShowMarketplace(controlling_player);
                    //    break;

                    //Exit
                    case 2:
                        this.Exit();
                        break;
                }
            }

            prevgpState = gpState;
            prevkbState = kbState;
        }

        public void update_trial_menu(GameTime gameTime)
        {
            kbState = Keyboard.GetState();
            gpState = GamePad.GetState(controlling_player);


            if (music.IsPaused)
            {
                music.Resume();
            }
            else if (music.IsPlaying == false)
            {
                music = soundBank.GetCue(music.Name);
                music.Play();
            }

            if (down_once())
                main_menu_data.selected_index = (int)MathHelper.Clamp(main_menu_data.selected_index + 1, 0, 3);

            if (up_once())
                main_menu_data.selected_index = (int)MathHelper.Clamp(main_menu_data.selected_index - 1, 0, 3);//main_menu_data.move_up();

            if (pressed_once(Keys.Enter) || pressed_once(Buttons.A))
            {
                music.Pause();

                switch (main_menu_data.selected_index)
                {
                    //New Game
                    case 0:
                        initialise_cutscene(scene1);
                        break;

                    //Controls
                    case 1:
                        game_state.state = GameState.State.Controls;
                        break;

                    //Buy Game
                    case 2:
                        if (controlling_player.can_buy_game())
                            Guide.ShowMarketplace(controlling_player);
                        break;

                    //Exit
                    case 3:
                        this.Exit();
                        break;
                }
            }

            prevgpState = gpState;
            prevkbState = kbState;
        }

        /////////////////////////////////////////////////////
        /////////////////////////////////////////////////////
        //      BULLSHIT MENU DRAWING CODE BEGINS HERE
        /////////////////////////////////////////////////////
        /////////////////////////////////////////////////////

        private Vector2 pos1 = new Vector2(180, 150);
        private Vector2 pos2 = new Vector2(180, 203);
        private Vector2 pos3 = new Vector2(180, 256);
        private Vector2 pos4 = new Vector2(180, 309);

        private Vector2 check_pos1 = new Vector2(440, 155);
        private Vector2 check_pos2 = new Vector2(440, 207);
        private Vector2 check_pos3 = new Vector2(440, 259);
        private Vector2 check_pos4 = new Vector2(440, 309);

        public void draw_trial_menu()
        {
            spriteBatch.Draw(menu_base, Vector2.Zero, Color.White);

            spriteBatch.Draw(menu_new, pos1, Color.White);
            spriteBatch.Draw(menu_controls, pos2, Color.White);
            spriteBatch.Draw(menu_buy, pos3, Color.White);
            spriteBatch.Draw(menu_exit, pos4, Color.White);

            switch (main_menu_data.selected_index)
            {
                case 0:
                    spriteBatch.Draw(new_check, check_pos1, Color.White);
                    break;

                case 1:
                    spriteBatch.Draw(controls_check, check_pos2, Color.White);
                    break;

                case 2:
                    spriteBatch.Draw(buy_check, check_pos3, Color.White);
                    break;

                case 3:
                    spriteBatch.Draw(exit_check, check_pos4, Color.White);
                    break;
            }
        }

        public void draw_full_menu()
        {
            spriteBatch.Draw(menu_base, Vector2.Zero, Color.White);

            spriteBatch.Draw(menu_new, pos1, Color.White);
            spriteBatch.Draw(menu_controls, pos2, Color.White);
            spriteBatch.Draw(menu_exit, pos3, Color.White);

            switch (main_menu_data.selected_index)
            {
                case 0:
                    spriteBatch.Draw(new_check, check_pos1, Color.White);
                    break;

                case 1:
                    spriteBatch.Draw(controls_check, check_pos2, Color.White);
                    break;

                case 2:
                    spriteBatch.Draw(exit_check, check_pos3, Color.White);
                    break;

                case 3:
                    //spriteBatch.Draw(new_check, check_pos4, Color.White);
                    break;
            }


//            int y_pos = 100;

//            for (int i = 0; i < main_menu_data.num_options; i++)
//            {
//                string text = main_menu_data.menu_text[i];

//#if XBOX
//                if (text == "Buy Game" && Guide.IsTrialMode == false)
//                    main_menu_data.visible[i] = false;
//#else
//                if (text == "Buy Game")
//                    main_menu_data.visible[i] = false;
//#endif

//                if (main_menu_data.visible[i] == false)
//                    continue;

//                if (i == main_menu_data.selected_index)
//                    spriteBatch.DrawString(MenuFont, main_menu_data.menu_text[i], new Vector2(100, y_pos), Color.Yellow);
//                else
//                    spriteBatch.DrawString(MenuFont, main_menu_data.menu_text[i], new Vector2(100, y_pos), Color.White);

//                y_pos += 100;
//            }

//            string vers_text = "August 25th 2011 Build";
//            int x = 960 - (int)VersNumFont.MeasureString(vers_text).X - 50;
//            int y = 540 - (int)VersNumFont.MeasureString(vers_text).Y - 20; 
//            spriteBatch.DrawString(VersNumFont, vers_text, new Vector2(x, y), Color.White);

//#if WINDOWS
//            spriteBatch.DrawString(VersNumFont, "ballerindustries.blogspot.com", Vector2.Zero, Color.White);
//#endif
        }

        public void draw_trial_pause()
        {
            spriteBatch.Draw(menu_base, Vector2.Zero, Color.White);

            spriteBatch.Draw(menu_resume, pos1, Color.White);
            spriteBatch.Draw(menu_controls, pos2, Color.White);
            spriteBatch.Draw(menu_buy, pos3, Color.White);
            spriteBatch.Draw(menu_exit, pos4, Color.White);

            switch (in_game_menu_data.selected_index)
            {
                case 0:
                    spriteBatch.Draw(resume_check, check_pos1, Color.White);
                    break;

                case 1:
                    spriteBatch.Draw(controls_check, check_pos2, Color.White);
                    break;

                case 2:
                    spriteBatch.Draw(buy_check, check_pos3, Color.White);
                    break;

                case 3:
                    spriteBatch.Draw(exit_check, check_pos4, Color.White);
                    break;

            }
        }

        public void draw_full_pause()
        {
            spriteBatch.Draw(menu_base, Vector2.Zero, Color.White);

            spriteBatch.Draw(menu_resume, pos1, Color.White);
            spriteBatch.Draw(menu_controls, pos2, Color.White);
            spriteBatch.Draw(menu_exit, pos3, Color.White);

            switch (in_game_menu_data.selected_index)
            {
                case 0:
                    spriteBatch.Draw(resume_check, check_pos1, Color.White);
                    break;

                case 1:
                    spriteBatch.Draw(controls_check, check_pos2, Color.White);
                    break;

                case 2:
                    spriteBatch.Draw(exit_check, check_pos3, Color.White);
                    break;

                case 3:
                    spriteBatch.Draw(exit_check, check_pos4, Color.White);
                    break;

            }


//            int y_pos = 100;

//            for (int i = 0; i < in_game_menu_data.num_options; i++)
//            {
//                string text = in_game_menu_data.menu_text[i];

//#if XBOX
//                if (text == "Buy Game" && Guide.IsTrialMode == false)
//                    in_game_menu_data.visible[i] = false;
//#else
//                if (text == "Buy Game")
//                    in_game_menu_data.visible[i] = false;
//#endif

//                if (in_game_menu_data.visible[i] == false)
//                    continue;

//                if (i == in_game_menu_data.selected_index)
//                    spriteBatch.DrawString(MenuFont, in_game_menu_data.menu_text[i], new Vector2(100, y_pos), Color.Yellow);
//                else
//                    spriteBatch.DrawString(MenuFont, in_game_menu_data.menu_text[i], new Vector2(100, y_pos), Color.White);

//                y_pos += 100;
//            }
        }
    }
}
