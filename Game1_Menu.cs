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
    public partial class Game1 : Microsoft.Xna.Framework.Game
    {
        Texture2D menu_base;
        Texture2D menu_buy, menu_controls, menu_exit, menu_new, menu_resume;
        Texture2D buy_check, controls_check, exit_check, new_check, resume_check;

        public void update_trial_pause(GameTime gameTime)
        {
            kbState = Keyboard.GetState();
            gpState = GamePad.GetState(controlling_player);

            //if (menuMusic.IsPaused)
            //{
            //    menuMusic.Resume();
            //}
            //else if (menuMusic.IsPlaying == false)
            //{
            //    menuMusic = soundBank.GetCue(menuMusic.Name);
            //    menuMusic.Play();
            //}

            if (down_once())
                in_game_menu_data.selected_index = (int)MathHelper.Clamp(in_game_menu_data.selected_index + 1, 0, 3);

            if (up_once())
                in_game_menu_data.selected_index = (int)MathHelper.Clamp(in_game_menu_data.selected_index - 1, 0, 3);

            if (pressed_once(Keys.Enter) || pressed_once(Buttons.A))
            {
                //menuMusic.Pause();

                switch (in_game_menu_data.selected_index)
                {
                    //Resume
                    case 0:
                        game_state.state = GameState.State.GamePlay;
                        game_state.current_act.music.Resume();

                        //Singletons.music_category.Resume();
                        break;

                    //Controls
                    //case 1:
                    //    game_state.state = GameState.State.Controls;
                    //    break;

                    //Buy Game
                    case 2:
#if XBOX
                        if (controlling_player.can_buy_game())
                            Guide.ShowMarketplace(controlling_player);
#endif
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

            if (down_once())
                in_game_menu_data.selected_index = (int)MathHelper.Clamp(in_game_menu_data.selected_index + 1, 0, 1);

            if (up_once())
                in_game_menu_data.selected_index = (int)MathHelper.Clamp(in_game_menu_data.selected_index - 1, 0, 1);

            if (pressed_once(Keys.Escape))
            {
                game_state.state = GameState.State.GamePlay;
                game_state.current_act.music.Resume();
            }

            if (pressed_once(Keys.Enter) || pressed_once(Buttons.A))
            {
                switch (in_game_menu_data.selected_index)
                {
                    //Resume
                    case 0:
                        game_state.state = GameState.State.GamePlay;
                        game_state.current_act.music.Resume();
                        break;

                    //Quit
                    case 1:
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

            if (down_once())
                main_menu_data.selected_index = (int)MathHelper.Clamp(main_menu_data.selected_index + 1, 0, 1);

            if (up_once())
                main_menu_data.selected_index = (int)MathHelper.Clamp(main_menu_data.selected_index - 1, 0, 1);//main_menu_data.move_up();

            if (pressed_once(Keys.Enter) || pressed_once(Buttons.A))
            {
                //menuMusic.Pause();

                switch (main_menu_data.selected_index)
                {
                    //New Game
                    case 0:
                        initialise_cutscene(scene1);
                        break;

                    //Exit
                    case 1:
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

            if (down_once())
                main_menu_data.selected_index = (int)MathHelper.Clamp(main_menu_data.selected_index + 1, 0, 3);

            if (up_once())
                main_menu_data.selected_index = (int)MathHelper.Clamp(main_menu_data.selected_index - 1, 0, 3);//main_menu_data.move_up();

            if (pressed_once(Keys.Enter) || pressed_once(Buttons.A))
            {
                //menuMusic.Pause();

                switch (main_menu_data.selected_index)
                {
                    //New Game
                    case 0:
                        initialise_cutscene(scene1);
                        break;

                    //Buy Game
                    case 2:
#if XBOX
                        if (controlling_player.can_buy_game())
                            Guide.ShowMarketplace(controlling_player);
#endif
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
            spriteBatch.Draw(menu_exit, pos2, Color.White);

            switch (main_menu_data.selected_index)
            {
                case 0:
                    spriteBatch.Draw(new_check, check_pos1, Color.White);
                    break;

                case 1:
                    spriteBatch.Draw(exit_check, check_pos2, Color.White);
                    break;
            }
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
            spriteBatch.Draw(menu_exit, pos2, Color.White);

            switch (in_game_menu_data.selected_index)
            {
                case 0:
                    spriteBatch.Draw(resume_check, check_pos1, Color.White);
                    break;

                case 1:
                    spriteBatch.Draw(exit_check, check_pos2, Color.White);
                    break;
            }
        }
    }
}
