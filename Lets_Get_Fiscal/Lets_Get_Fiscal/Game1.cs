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
using System.Diagnostics;

namespace Lets_Get_Fiscal
{
    public partial class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        MenuData main_menu_data, in_game_menu_data;
        GameState game_state;

        Microsoft.Xna.Framework.Audio.Cue menuMusic;

        Ego ego;
        List<BadGuy> bad_guys;
        List<WaveElement> waiting_to_spawn;

        
        List<BasicGameObject> all_gameobjects;
        List<Prop> props;
        List<Flourish> flourishes;
        List<Item> items;
        KeyboardState kbState, prevkbState;
        GamePadState gpState, prevgpState;

        AudioEngine audioEngine;
        WaveBank waveBank;
        SoundBank soundBank;

        bool update_frame = true;

        Texture2D whiteDot, arrow, logo;
        SpriteFont HUDFont, MenuFont, VersNumFont, LevelAnnounceFont, DebugFont, GRODGFont;
        GOSign go_sign;

        Singletons singletons;

        TextureManager tm;
        CollisionManager cm;
        ProjectileManager pm;
        Icicles icicles;
        HUD hud;
        Camera camera;
        Random random;
        ulong game_time_from;
        bool pause_everything;

        LevelAnnounce la_one;
        Level tbk_club, newspaper, bifo;
        Act main_club, vip, tbk_office, printing, office, editor_office, bifo_customer, bifo_kitchen, bifo_freezer;
        Fade fade_to_black;

        Rectangle title_safe_rect, ego_pos_rect, comic_safe_rect;
        MoveManager mm;
        PlayerIndex controlling_player;
        Texture2D combo_explain_texture;

        //Texture2D full_menu, trial_menu, full_pause, trial_pause;
       
        
        //Texture2D check_mark;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

            //Set resolution
            graphics.PreferredBackBufferWidth = 960;
            graphics.PreferredBackBufferHeight = 540;
            Window.Title = "Let's Get Fiscal - Baller Industries";

#if XBOX
            Components.Add(new GamerServicesComponent(this));
#endif
        }

        protected override void Initialize()
        {
            base.Initialize();

            go_sign = new GOSign();
            la_one = new LevelAnnounce();
            //la_one = new LevelAnnounce("la_one", 1, spriteBatch, LevelAnnounceFont);
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            audioEngine = new AudioEngine(             "Content\\LGF_Audio.xgs");
            waveBank    = new WaveBank   (audioEngine, "Content\\Wave Bank.xwb");
            soundBank   = new SoundBank  (audioEngine, "Content\\Sound Bank.xsb");
            tm = new TextureManager(Content);

            logo = Content.Load<Texture2D>("logo");

            menu_base = Content.Load<Texture2D>("menu//base");

            menu_buy = Content.Load<Texture2D>("menu//buy");
            menu_controls = Content.Load<Texture2D>("menu//controls");
            menu_exit = Content.Load<Texture2D>("menu//exit");
            menu_new = Content.Load<Texture2D>("menu//new"); 
            menu_resume = Content.Load<Texture2D>("menu//resume");

            buy_check = Content.Load<Texture2D>("menu//buy_check"); 
            controls_check = Content.Load<Texture2D>("menu//control_check"); 
            exit_check = Content.Load<Texture2D>("menu//exit_check");
            new_check = Content.Load<Texture2D>("menu//new_check");
            resume_check = Content.Load<Texture2D>("menu//resume_check");

            stopwatch = new Stopwatch();

            bad_guys = new List<BadGuy>(10);

            waiting_to_spawn = new List<WaveElement>(5);
            all_gameobjects = new List<BasicGameObject>(30);
            flourishes = new List<Flourish>(5);


            menuMusic = soundBank.GetCue("menu_music");
            
            camera = new Camera(new Rectangle(0, 0, 960 * 2, 540), new Rectangle(0, 0, 960, 540));  //Reinit
            pm = new ProjectileManager(camera, all_gameobjects);                                                     
            random = new Random();
            mm = new MoveManager();

            props = new List<Prop>(10);
            game_state = new GameState(GameState.State.Logo);                                           
            
            title_safe_rect = GetTitleSafeArea(0.8f);
            comic_safe_rect = GetTitleSafeArea(0.9f);

            Fonts.HUDFont = HUDFont = Content.Load<SpriteFont>("fonts//QuartzHUD27");
            Fonts.LevelAnnounceFont = LevelAnnounceFont = Content.Load<SpriteFont>("fonts//QuartzHUD90");
            Fonts.SubtitleFont = Content.Load<SpriteFont>("fonts//SubtitleFont");

            MenuFont = Content.Load<SpriteFont>("fonts//MenuFont");
            VersNumFont = Content.Load<SpriteFont>("fonts//VersNumFont");
            DebugFont = Content.Load<SpriteFont>("fonts//StatsFont");
            combo_explain_texture = tm.find_texture("combo_explain");
            Fonts.QuartzMS60 = Content.Load<SpriteFont>("fonts//QuartzMS60");

            whiteDot = Content.Load<Texture2D>("rect");
            Singletons.credit_font = MenuFont;

            singletons = new Singletons(tm, cm, pm, camera, random, soundBank, game_state, mm, audioEngine, new MusicManager(), ego);

            items = new List<Item>(10);

            //Define level data
            define_levels();

            Singletons.ego = ego = new Ego("spritesheets//accountant", 100, "collisions//accountant_m", singletons);  //Reinit
            hud = new HUD(ego.portrait, whiteDot, ego.max_hp, HUDFont, ego.name, Content.Load<Texture2D>("arrow"));
            Singletons.cm = cm = new CollisionManager(ego, bad_guys, null, props, soundBank, camera, pm, tm, flourishes, hud, items, all_gameobjects);

            //Define Ego
            ego.cm = cm;
            ego.facing = Character.Direction.right;         //Reinit
            ego.position = new Rectangle(-200, 500, 0, 0);  //Reinit
            ego.walk_to = new Point(200, 500);              //Reinit
            ego.id = -1;
            ego.LoadContent();

            foreach (BadGuy bg in bad_guys)
                all_gameobjects.Add(bg);

            foreach (Prop p in props)
                all_gameobjects.Add(p);

            foreach (Item i in items)
                all_gameobjects.Add(i);

            all_gameobjects.Add(ego);
            
            main_menu_data = new MenuData();
            main_menu_data.menu_text.Add("New Game");
            main_menu_data.menu_text.Add("Controls");
            main_menu_data.menu_text.Add("Buy Game");
            main_menu_data.menu_text.Add("Exit");

            in_game_menu_data = new MenuData();
            in_game_menu_data.menu_text.Add("Resume");
            in_game_menu_data.menu_text.Add("Controls");
            in_game_menu_data.menu_text.Add("Buy Game");
            in_game_menu_data.menu_text.Add("Quit");

            Load_CutScenes();
            fade_to_black = new Fade(50);

            icicles = new Icicles(singletons);
        }

        //This function is called when we restart the game or
        //start a game from the beginning
        public void initialise_objects()
        {
            Singletons.music_volume = 1.0f;
            Singletons.music_category.SetVolume(Singletons.music_volume);

            la_one.Initialise("la_one", 1, spriteBatch, LevelAnnounceFont);

            //Initialise GameState
            go_sign.Initialise(spriteBatch, tm, soundBank);



            ////////////////////////////////////////////
            // Set the starting level
            ////////////////////////////////////////////
            game_state.Initialise(tbk_club, main_club);
            game_state.current_level_announce = la_one;

            fade_to_black.Initialise();

            //Initialise Camera
            camera.Initialise();

            //Initialise Acts
            main_club.Initialise();
            vip.Initialise();
            tbk_office.Initialise();

            printing.Initialise();
            office.Initialise();
            editor_office.Initialise();

            //Initialise Ego
            ego.Initialise();

            //Initialise Collision Manager
            cm.Initialise(game_state.current_act.min_y);

            hud.initialise();

            //Initialise Lists
            bad_guys.Clear();
            waiting_to_spawn.Clear();
            all_gameobjects.Clear();
            props.Clear();
            items.Clear();
            copy_props();

            all_gameobjects.Add(ego);

            foreach (Prop p in props)
                all_gameobjects.Add(p);

            foreach (Item i in items)
                all_gameobjects.Add(i);

            StageCleared sc = new StageCleared();
            sc.Initialise("stage_clear", 1, spriteBatch, LevelAnnounceFont, DebugFont);
            game_state.current_stage_cleared = sc;
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        
        protected override void Update(GameTime gameTime)
        {
            if (game_state.state == GameState.State.CutScene)
            {
                update_cutscene(gameTime);
            }
            else if (game_state.state == GameState.State.LanguageWarning)
            {
                update_language_warning();
            }
            else if (game_state.state == GameState.State.Logo)
            {
                update_logo();
            }
            else if (game_state.state == GameState.State.ComboExplain)
            {
                update_combo_explain();
            }
            else if (game_state.state == GameState.State.DemoOver)
            {
                update_demo_over();
            }
            else if (game_state.state == GameState.State.Menu)
            {
                if (Singletons.IsTrialMode)
                    update_trial_menu(gameTime);
                else
                    update_full_menu(gameTime);
            }
            else if (game_state.state == GameState.State.InGameMenu)
            {
                if (Singletons.IsTrialMode)
                    update_trial_pause(gameTime);
                else
                    update_full_pause(gameTime);
                //update_in_game_menu(gameTime);
            }
            else if (game_state.state == GameState.State.ActOver)
            {
                update_act_over();
            }
            else if (game_state.state == GameState.State.GamePlay)
            {
                update_gameplay();
            }
            else if (game_state.state == GameState.State.StartPrompt)
            {
                update_start_prompt();
            }
            else if (game_state.state == GameState.State.Controls)
            {
                update_controls();
            }
            else if (game_state.state == GameState.State.LevelAnnounce)
            {
                update_level_announce();
            }


            audioEngine.Update();
            base.Update(gameTime);
        }

        public void update_controls()
        {
            gpState = GamePad.GetState(controlling_player);
            kbState = Keyboard.GetState();

            if (kbState.IsKeyDown(Keys.Escape) && prevkbState.IsKeyUp(Keys.Escape))
            {
                //Go to previous state ACTUALLY
                game_state.state = game_state.prev_state;
            }

            if (gpState.IsButtonDown(Buttons.B) && prevgpState.IsButtonUp(Buttons.B))
            {
                //Go to previous state ACTUALLY
                game_state.state = game_state.prev_state;
            }

            prevgpState = gpState;
            prevkbState = kbState;
        }

        public void draw_controls()
        {
            spriteBatch.Draw(Content.Load<Texture2D>("controls"), Vector2.Zero, Color.White);
        }

        public void update_start_prompt()
        {
#if XBOX
            controlling_player = PlayerIndex.One;

            for (PlayerIndex index = PlayerIndex.One; index <= PlayerIndex.Four; index++)
            {
                if (GamePad.GetState(index).Buttons.Start == ButtonState.Pressed)
                {
                    controlling_player = index;
                    game_state.state = GameState.State.LanguageWarning;
                    break;
                }
            }
#else
            game_state.state = GameState.State.Menu;
#endif
        }

        public void draw_start_prompt()
        {
            //This code only needs to be calculated once.
            Vector2 pos = new Vector2();
            pos.X = (960 - MenuFont.MeasureString("PRESS START").X) / 2;
            pos.Y = (480 - MenuFont.MeasureString("PRESS START").Y) / 2;

            spriteBatch.DrawString(MenuFont, "PRESS START", pos, Color.White);
        }

        public void copy_props()
        {
            foreach (Prop p in game_state.current_act.props)
            {
                props.Add(new Prop(p));
            }
        }

        public void update_gameplay()
        {
            if (Singletons.half_speed)
            {
                update_frame = !update_frame;

                if (!update_frame)
                    return;
            }

            kbState = Keyboard.GetState();
            gpState = GamePad.GetState(controlling_player);

            if (pressed_once(Keys.P))
                pause_everything = !pause_everything;

            if (pause_everything && !pressed_once(Keys.Space))
            {
                prevkbState = kbState;
                prevgpState = gpState;
                return;
            }

            Singletons.music_manager.Update();

            go_sign.Update();

            ego.Update(kbState, prevkbState, gpState, prevgpState, fade_to_black.fade_state == Fade.State.none);

            cm.get_collisions();
            cm.detect_collisions();
            cm.manage_badguys();
            Singletons.update_shake_screen();

            for (int i = 0; i < bad_guys.Count; i++)
            {
                BadGuy bg = bad_guys[i];

                if (bg == null)
                    continue;

                if (ego.state != GameObject.State.jumping && ego.state != GameObject.State.brawl_attack && ego.state != GameObject.State.suplex)
                {
                    ego_pos_rect = ego.position;
                }
                else
                {
                    ego_pos_rect = ego.position;
                    ego_pos_rect.Y = ego.jumpY - ego.posH;
                }

                ego_pos_rect = CollisionManager.scale_rect(ego_pos_rect, 0.15f, 1.0f);
                bg.Update(ego_pos_rect);

                //Remove dead bad guys
                if (bg.hp <= 0 && bg.alpha_val == 0)
                {
                    bad_guys.Remove(bg);
                    all_gameobjects.Remove(bg);
                }

                if ((bg.sheet == "spritesheets//treeboi" || bg.sheet == "spritesheets//kone") && (bg.position.Left > camera.viewport_rect.Right) && bg.state == GameObject.State.running)
                {
                    bad_guys.Remove(bg);
                    all_gameobjects.Remove(bg);
                    //i--;
                }
            }

            for (int i = 0; i < props.Count; i++)
            {
                Prop p = props[i];

                if (p == null)
                    continue;

                p.Update();

                if (p.alpha_val == 0 || p.position.Right < camera.viewport_rect.Left)
                {
                    props.Remove(p);
                    all_gameobjects.Remove(p);
                }
            }

            for (int i = 0; i < flourishes.Count; i++)
            {
                Flourish f = flourishes[i];

                if (f == null)
                    break;

                f.Update();

                if (f.visible == false)
                    flourishes.Remove(f);
            }

            for (int i = 0; i < items.Count; i++)
            {
                Item it = items[i];
                
                if (it == null)
                    break;

                Weapon w = it as Weapon;

                if (w != null)
                    w.Update();


                if (it.visible == false)
                    items.Remove(it);
            }

            //In case the mother fucker of a player moves like a jerk.
            camera.adjust_viewport(ego.position);
            check_spawn_waiters();
            check_for_spawning();

            //This should sort the bad guys by their baseline value.
            pm.move_projectiles();
            all_gameobjects.Sort();

            fade_to_black.Update();

            if (fade_to_black.fade_state == Fade.State.ready)
            {
                change_act();
            }

            if (kbState.IsKeyDown(Keys.D0) && prevkbState.IsKeyUp(Keys.D0))
            {
                initialise_objects();
            }

            if (kbState.IsKeyDown(Keys.K) && prevkbState.IsKeyUp(Keys.K))
            {
                ego.hp = 0;
                ego.start_death_leap();
            }

            if (kbState.IsKeyDown(Keys.L) && prevkbState.IsKeyUp(Keys.L))
            {
                ego.attacked_from_left = (ego.facing == GameObject.Direction.left);
                ego.start_KO_leap();
            }

            if (pressed_once(Keys.Escape) || pressed_once(Buttons.Start)) 
            {
                game_state.state = GameState.State.InGameMenu;
                Singletons.music_category.Pause();
            }

            update_continue_menu();


#if XBOX
            if (!gpState.IsConnected)
            {
                game_state.state = GameState.State.InGameMenu;
                Singletons.music_category.Pause();
            }
#endif

            hud.Update(ego.hp, ego.lives, game_state.score);

            prevkbState = kbState;
            prevgpState = gpState;
        }

        private void update_continue_menu()
        {
            if (!hud.draw_hud)
            {
                if (down_once() || up_once())
                {
                    soundBank.PlayCue("menu_beep");
                    hud.continue_selected = !hud.continue_selected;
                }

                if ((pressed_once(Keys.Enter) || pressed_once(Buttons.A)))
                {
                    if (hud.continue_selected)
                    {
                        hud.draw_hud = true;
                        game_state.score = 0;
                        ego.lives = 3;
                        ego.respawn();
                    }
                    else
                    {
                        hud.draw_hud = true;
                        Singletons.StopMusic();
                        game_state.state = GameState.State.StartPrompt;
                    }
                }
            }
        }

        public void check_spawn_waiters()
        {
            for (int i = 0; i < waiting_to_spawn.Count; i++)
            {
                WaveElement we = waiting_to_spawn[i];

                if (ego.posX > we.ego_x)
                {
                    spawn_bad_guy(we);

                    waiting_to_spawn.Remove(we);
                }
            }
        }

        //This sexy code that once upon a time was the King of Germany
        //is responsible for making bag guys appear onto the screen. How
        //he has fallen since his glory days. Bumsen!
        public void spawn_bad_guy(WaveElement we)
        {
            if (we.type == WaveElement.Type.butabi)
            {
                Butabi bg = new Butabi(we.sheet_name, we.hp, we.collision_name, singletons);
                bg.facing = Character.Direction.left;
                bg.position = new Rectangle(we.spawn_pos.X, we.spawn_pos.Y, 0, 0);
                bg.id = bad_guys.Count;
                bg.LoadContent();

                all_gameobjects.Add(bg);
                bad_guys.Add(bg);
            }
            else if (we.type == WaveElement.Type.fred)
            {
                Fred bg = new Fred(we.sheet_name, we.hp, we.collision_name, singletons, we.init_ai_state, icicles);
                bg.facing = Character.Direction.left;
                bg.position = new Rectangle(we.spawn_pos.X, we.spawn_pos.Y, 0, 0);
                bg.id = bad_guys.Count;
                bg.LoadContent();
                

                all_gameobjects.Add(bg);
                bad_guys.Add(bg);
                icicles.fred = bg;
            }

            else if (we.type == WaveElement.Type.commander )
            {
                Commander bg = new Commander(we.sheet_name, we.hp, we.collision_name, singletons, we.init_ai_state);
                bg.facing = Character.Direction.left;
                bg.position = new Rectangle(we.spawn_pos.X, we.spawn_pos.Y, 0, 0);
                bg.id = bad_guys.Count;
                bg.LoadContent();

                all_gameobjects.Add(bg);
                bad_guys.Add(bg);
            }

            else if (we.type == WaveElement.Type.treeboi || we.type == WaveElement.Type.kone || we.type == WaveElement.Type.running_kone || we.type == WaveElement.Type.running_treeboi)
            {
                TreeboiOrKone bg = new TreeboiOrKone(we.sheet_name, we.hp, we.collision_name, singletons, we.init_ai_state);
                bg.facing = Character.Direction.left;
                bg.position = new Rectangle(we.spawn_pos.X, we.spawn_pos.Y, 0, 0);
                bg.id = bad_guys.Count;
                bg.LoadContent();

                all_gameobjects.Add(bg);
                bad_guys.Add(bg);
            }
            else if (we.type == WaveElement.Type.kiki)
            {
                Kiki bg = new Kiki(we.sheet_name, we.hp, we.collision_name, singletons);
                bg.facing = Character.Direction.left;
                bg.position = new Rectangle(we.spawn_pos.X, we.spawn_pos.Y, 0, 0);
                bg.id = bad_guys.Count;
                bg.LoadContent();

                all_gameobjects.Add(bg);
                bad_guys.Add(bg);
            }

            else if (we.type == WaveElement.Type.fitty_cent)
            {
                Curtis bg = new Curtis(we.sheet_name, we.hp, we.collision_name, singletons);
                bg.facing = Character.Direction.left;
                bg.position = new Rectangle(we.spawn_pos.X, we.spawn_pos.Y, 0, 0);
                bg.id = bad_guys.Count;
                bg.LoadContent();

                all_gameobjects.Add(bg);
                bad_guys.Add(bg);
            }
            else if (we.type == WaveElement.Type.guido_strong)
            {
                Guido bg = new Guido(we.sheet_name, we.hp, we.collision_name, singletons);
                bg.facing = Character.Direction.left;
                bg.position = new Rectangle(we.spawn_pos.X, we.spawn_pos.Y, 0, 0);
                bg.id = bad_guys.Count;
                bg.LoadContent();

                all_gameobjects.Add(bg);
                bad_guys.Add(bg);
            }
            else
            {
                BadGuy bg = new BadGuy(we.sheet_name, we.hp, we.collision_name, singletons, we.init_ai_state, we.scale);
                bg.facing = Character.Direction.left;
                bg.position = new Rectangle(we.spawn_pos.X, we.spawn_pos.Y, 0, 0);
                bg.id = bad_guys.Count;
                bg.LoadContent();
                bg.destination = cm.get_random_location(random, bg.position);

                all_gameobjects.Add(bg);
                bad_guys.Add(bg);
            }   
        }

        public void check_for_spawning()
        {
            bool all_dead = true;

            if (game_state.current_act.act_over)
                return;

            foreach (BadGuy bg in bad_guys)
            {
                if (bg.alpha_val != 0)
                {
                    all_dead = false;
                    break;
                }
            }

            if (waiting_to_spawn.Count > 0)
                all_dead = false;

            if (all_dead)
            {
                //No more waves, change acts
                if (game_state.current_act.current_wave == null)
                {
                    game_state.current_act.act_over = true;
                    Singletons.music_manager.fade_out();

                    if (ego.state == GameObject.State.walking)
                        ego.state = GameObject.State.idle;


                    if (game_state.current_act.position < 2)
                        fade_to_black.fade_state = Fade.State.fade_out;
                    else
                        change_act();
                }
                else
                {
                    camera.limitation = game_state.current_act.current_wave.x_limit;

                    if (game_state.current_act.current_wave.id != 0 && camera.prev_limitation < camera.limitation)
                        go_sign.Start();

                    //Spawn the next wave of bad guys
                    foreach (WaveElement we in game_state.current_act.current_wave.bad_guys)
                    {
                        if (ego.posX > we.ego_x || we.ego_x == 0)
                        {
                            spawn_bad_guy(we);
                        }
                        else
                        {
                            //This guy needs to wait until he is allowed to spawn. BY ORDER OF ANGUS CHENG!!!!!
                            waiting_to_spawn.Add(we);
                        }
                    }

                    game_state.current_act.wave_num++;
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            if (game_state.state == GameState.State.CutScene)
            {
                draw_cutscene();
            }
            else if (game_state.state == GameState.State.Logo)
            {
                draw_logo();
            }
            else if (game_state.state == GameState.State.ComboExplain)
            {
                draw_combo_explain();
            }
            else if (game_state.state == GameState.State.DemoOver)
            {
                draw_demo_over();
            }
            else if (game_state.state == GameState.State.ActOver)
            {
                draw_act_over();
            }
            else if (game_state.state == GameState.State.GamePlay)
            {
                draw_gameplay();
            }
            else if (game_state.state == GameState.State.Menu)
            {
                if (Singletons.IsTrialMode)
                    draw_trial_menu();
                else
                    draw_full_menu();

                    //draw_menu();
            }
            else if (game_state.state == GameState.State.InGameMenu)
            {
                if (Singletons.IsTrialMode)
                    draw_trial_pause();
                else
                    draw_full_pause();

                //draw_in_game_menu();
            }
            else if (game_state.state == GameState.State.StartPrompt)
            {
                draw_start_prompt();
            }
            else if (game_state.state == GameState.State.Controls)
            {
                draw_controls();
            }
            else if (game_state.state == GameState.State.LevelAnnounce)
            {
                draw_level_announce();
            }


            //spriteBatch.DrawString(HUDFont, num_update.ToString(), new Vector2(500, 50), Color.Black);
            //spriteBatch.DrawString(HUDFont, num_draw.ToString(), new Vector2(500, 100), Color.Black);

            //drawRect(title_safe_rect, Color.Red);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void draw_gameplay()
        {
            //Draw backgrounds
            foreach (Background bg in game_state.current_act.backgrounds)
            {
                spriteBatch.Draw(bg.texture, new Rectangle(bg.region.X - camera.viewport_rect.X, Singletons.draw_at, bg.region.Width, 560), new Rectangle(0, 0, 480, 270), Color.White);
            }

            foreach (BasicGameObject bgo in all_gameobjects)
            { 
                if (bgo == null)
                    continue;

                GameObject go = bgo as GameObject;

                if (go != null)
                {
                    camera.Draw(go, spriteBatch);
                    //drawRect(go.position, Color.Black);
                }
                else
                    camera.Draw(bgo, spriteBatch);

                //drawRect(bgo.position, Color.Black);

                //if (go.sheet == "spritesheets//commander")
                //{
                //    spriteBatch.DrawString(MenuFont, go.position.ToString(), new Vector2(100, 100), Color.Black);
                //}
            }

            if (game_state.current_act.act_name == bifo_freezer.act_name)
                icicles.Draw(spriteBatch);

            //pm.Draw(spriteBatch);

            foreach (Flourish f in flourishes)
            {
                if (f == null || f.visible == false)
                    continue;

                camera.Draw(f, spriteBatch);
            }

            foreach (Foreground fg in game_state.current_act.foregrounds)
            {
                spriteBatch.Draw(fg.texture, new Rectangle(fg.region.X - (int)(camera.viewport_rect.X * 1.3f), Singletons.draw_at, fg.region.Width, 560), new Rectangle(0, 0, 480, 270), Color.White);
            }

            go_sign.Draw();
            hud.Draw(spriteBatch);

            //Draw a black rect ontop of everything
            if (fade_to_black.fade_state != Fade.State.none)
            {
                spriteBatch.Draw(whiteDot, new Rectangle(0, 0, 960, 580), Color.Black * fade_to_black.alpha_val);
            }
        }

        public void change_level(Level level, Act act)
        {
            game_state.current_level = level;
            game_state.current_act = act;

            hud.initialise();
            ego.facing = GameObject.Direction.right;
            ego.state = GameObject.State.idle;

            camera.viewport_rect.X = 0;
            cm.min_y = game_state.current_act.min_y;
            
            ego.posX = 100;
            ego.posY = (cm.min_y + 540) / 2;
            ego.weapon = null;

            bad_guys.Clear();
            props.Clear();
            all_gameobjects.Clear();
            items.Clear();

            all_gameobjects.Add(ego);

            foreach (BadGuy bg in bad_guys)
                all_gameobjects.Add(bg);

            copy_props();
            foreach (Prop p in props)
                all_gameobjects.Add(p);

        }

        public void change_act()
        {
            if (ego.state == GameObject.State.walking)
                ego.state = GameObject.State.idle;

            Singletons.music_manager.fade_in();
            game_state.current_act.music.Stop(AudioStopOptions.Immediate);
            //1. Move the ego to the beginning.
            //2. Clear all the bad guys
            //3. Change the current act
            if (game_state.current_act.position < 2)
            {
                game_state.current_act = game_state.current_level.acts[game_state.current_act.position + 1];

                hud.initialise();
                ego.facing = GameObject.Direction.right;
                ego.state = GameObject.State.idle;

                camera.viewport_rect.X = 0;
                cm.min_y = game_state.current_act.min_y;
                fade_to_black.fade_state = Fade.State.fade_in;

                ego.posX = 100;
                ego.posY = (cm.min_y + 540) / 2;
                ego.weapon = null;

                bad_guys.Clear();
                props.Clear();
                all_gameobjects.Clear();
                items.Clear();

                all_gameobjects.Add(ego);

                foreach (BadGuy bg in bad_guys)
                    all_gameobjects.Add(bg);

                copy_props();
                foreach (Prop p in props)
                    all_gameobjects.Add(p);

                //If we are changing to the final act, play a cutscene instead.
                if (game_state.current_act.position == 2)
                {
                    if (game_state.current_level == tbk_club)
                        initialise_cutscene(scene2);
                    else if (game_state.current_level == bifo)
                        initialise_cutscene(scene4);
                    else if (game_state.current_level == newspaper)
                        initialise_cutscene(scene6);
                }
                else
                {
                    Singletons.PlayMusic();
                }
            }
            else
            {
                game_state.state = GameState.State.ActOver;
            }
        }

        private void define_bifo()
        {
            bifo = new Level(Level.LevelName.BIFO);

            bifo_customer = new Act(Act.ActName.customer_area, 368, 5, "main_club_music");
            bifo_kitchen = new Act(Act.ActName.kitchen, 370, 5, "vip_music");
            bifo_freezer = new Act(Act.ActName.freezer, 370, 5, "boss_music");

            bifo.add_act(bifo_customer, 0);
            bifo.add_act(bifo_kitchen, 1);
            bifo.add_act(bifo_freezer, 2);

            bifo_customer.add_background(new Background(new Rectangle(0, 0, 960, 540), "backgrounds//customer_bg_a", tm));
            bifo_customer.add_background(new Background(new Rectangle(0, 0, 960, 540), "backgrounds//customer_bg_b", tm));
            bifo_customer.add_background(new Background(new Rectangle(0, 0, 960, 540), "backgrounds//customer_bg_c", tm));
            bifo_customer.add_background(new Background(new Rectangle(0, 0, 960, 540), "backgrounds//customer_bg_d", tm));
            bifo_customer.add_background(new Background(new Rectangle(0, 0, 960, 540), "backgrounds//customer_bg_e", tm));

            bifo_customer.add_foreground(new Foreground(new Rectangle(0, 0, 960, 540), "foregrounds//customer_fg_a", tm));
            bifo_customer.add_foreground(new Foreground(new Rectangle(0, 0, 960, 540), "foregrounds//customer_fg_b", tm));
            bifo_customer.add_foreground(new Foreground(new Rectangle(0, 0, 960, 540), "foregrounds//customer_fg_c", tm));
            bifo_customer.add_foreground(new Foreground(new Rectangle(0, 0, 960, 540), "foregrounds//customer_fg_d", tm));
            bifo_customer.add_foreground(new Foreground(new Rectangle(0, 0, 960, 540), "foregrounds//customer_fg_e", tm));

            bifo_kitchen.add_background(new Background(new Rectangle(0, 0, 960, 540), "backgrounds//kitchen_1", tm));
            bifo_kitchen.add_background(new Background(new Rectangle(0, 0, 960, 540), "backgrounds//kitchen_2", tm));
            bifo_kitchen.add_background(new Background(new Rectangle(0, 0, 960, 540), "backgrounds//kitchen_3", tm));
            bifo_kitchen.add_background(new Background(new Rectangle(0, 0, 960, 540), "backgrounds//kitchen_4", tm));
            bifo_kitchen.add_background(new Background(new Rectangle(0, 0, 960, 540), "backgrounds//kitchen_5", tm));

            bifo_kitchen.add_foreground(new Foreground(new Rectangle(0, 0, 960, 540), "foregrounds//fg_kitchen_1", tm));
            bifo_kitchen.add_foreground(new Foreground(new Rectangle(0, 0, 960, 540), "foregrounds//fg_kitchen_2", tm));
            bifo_kitchen.add_foreground(new Foreground(new Rectangle(0, 0, 960, 540), "foregrounds//fg_kitchen_3", tm));
            bifo_kitchen.add_foreground(new Foreground(new Rectangle(0, 0, 960, 540), "foregrounds//fg_kitchen_4", tm));
            bifo_kitchen.add_foreground(new Foreground(new Rectangle(0, 0, 960, 540), "foregrounds//fg_kitchen_5", tm));

            bifo_freezer.add_background(new Background(new Rectangle(0, 0, 960, 540), "backgrounds//freezer_1", tm));
            bifo_freezer.add_background(new Background(new Rectangle(0, 0, 960, 540), "backgrounds//freezer_2", tm));

            bifo_freezer.add_foreground(new Foreground(new Rectangle(0, 0, 960, 540), "foregrounds//fg_freezer_1", tm));
            bifo_freezer.add_foreground(new Foreground(new Rectangle(0, 0, 960, 540), "foregrounds//fg_freezer_2", tm));

            ////////////////////////////////////////////////////
            // CUSTOMER AREA
            ////////////////////////////////////////////////////
            Wave wave1 = bifo_customer.waves[0] = new Wave(0, 1600);
            Wave wave2 = bifo_customer.waves[1] = new Wave(0, 3200);
            Wave wave3 = bifo_customer.waves[2] = new Wave(0, 4790);

            //WAVE 1
            wave1.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(-500, 500)));
            wave1.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(-600, 540)));
            wave1.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(-700, 500)));

            wave1.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(1200, 500)));
            wave1.bad_guys.Add(new WaveElement(WaveElement.Type.guido_strong, new Point(1300, 540)));
            wave1.bad_guys.Add(new WaveElement(WaveElement.Type.guido_strong, new Point(1400, 500)));

            //WAVE 2
            wave2.bad_guys.Add(new WaveElement(WaveElement.Type.silvio, new Point(3500, 500), 2400));
            wave2.bad_guys.Add(new WaveElement(WaveElement.Type.coral, new Point(3600, 540), 2400));
            wave2.bad_guys.Add(new WaveElement(WaveElement.Type.nicole, new Point(3700, 500), 2400));

            wave2.bad_guys.Add(new WaveElement(WaveElement.Type.coral, new Point(1300, 500), 2400));
            wave2.bad_guys.Add(new WaveElement(WaveElement.Type.silvio, new Point(1200, 540), 2400));
            wave2.bad_guys.Add(new WaveElement(WaveElement.Type.nicole, new Point(1100, 500), 2400));

            //WAVE 3
            wave3.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(5100, 500), 4000));
            wave3.bad_guys.Add(new WaveElement(WaveElement.Type.fitty_cent, new Point(5200, 540), 4000));
            wave3.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(5300, 500), 4000));

            wave3.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(2900, 500), 4000));
            wave3.bad_guys.Add(new WaveElement(WaveElement.Type.fitty_cent, new Point(2800, 540), 4000));
            wave3.bad_guys.Add(new WaveElement(WaveElement.Type.butabi, new Point(2700, 500), 4000));

            ////////////////////////////////////////////////////
            // KITCHEN AREA
            ////////////////////////////////////////////////////
            wave1 = bifo_kitchen.waves[0] = new Wave(0, 1600);
            wave2 = bifo_kitchen.waves[1] = new Wave(0, 3200);
            wave3 = bifo_kitchen.waves[2] = new Wave(0, 4790);

            //WAVE 1
            wave1.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(-500, 500)));
            wave1.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(-600, 540)));
            wave1.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(-700, 500)));

            wave1.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(1200, 500)));
            wave1.bad_guys.Add(new WaveElement(WaveElement.Type.guido_strong, new Point(1300, 540)));
            wave1.bad_guys.Add(new WaveElement(WaveElement.Type.guido_strong, new Point(1400, 500)));

            //WAVE 2
            wave2.bad_guys.Add(new WaveElement(WaveElement.Type.silvio, new Point(3500, 500), 2400));
            wave2.bad_guys.Add(new WaveElement(WaveElement.Type.coral, new Point(3600, 540), 2400));
            wave2.bad_guys.Add(new WaveElement(WaveElement.Type.nicole, new Point(3700, 500), 2400));

            wave2.bad_guys.Add(new WaveElement(WaveElement.Type.coral, new Point(1300, 500), 2400));
            wave2.bad_guys.Add(new WaveElement(WaveElement.Type.silvio, new Point(1200, 540), 2400));
            wave2.bad_guys.Add(new WaveElement(WaveElement.Type.nicole, new Point(1100, 500), 2400));

            //WAVE 3
            wave3.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(5100, 500), 4000));
            wave3.bad_guys.Add(new WaveElement(WaveElement.Type.fitty_cent, new Point(5200, 540), 4000));
            wave3.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(5300, 500), 4000));

            wave3.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(2900, 500), 4000));
            wave3.bad_guys.Add(new WaveElement(WaveElement.Type.fitty_cent, new Point(2800, 540), 4000));
            wave3.bad_guys.Add(new WaveElement(WaveElement.Type.butabi, new Point(2700, 500), 4000));

            ////////////////////////////////////////////////////
            // FREEZER
            ////////////////////////////////////////////////////
            bifo_freezer.waves[0] = new Wave(0, 1910);
            bifo_freezer.waves[0].bad_guys.Add(new WaveElement(WaveElement.Type.fred, new Point(1200, 300)));
        }

        private void define_tbk_club()
        {
            tbk_club = new Level(Level.LevelName.TBK_Club);

            main_club = new Act(Act.ActName.main_club_area, 412, 6, "main_club_music");
            vip = new Act(Act.ActName.vip_area, 406, 6, "vip_music");
            tbk_office = new Act(Act.ActName.TBK_office, 406, 6, "boss_music");

            tbk_club.add_act(main_club, 0);
            tbk_club.add_act(vip, 1);
            tbk_club.add_act(tbk_office, 2);

            main_club.add_background(new Background(new Rectangle(0, 0, 960, 540), "backgrounds//club1", tm));
            main_club.add_background(new Background(new Rectangle(0, 0, 960, 540), "backgrounds//club2", tm));
            main_club.add_background(new Background(new Rectangle(0, 0, 960, 540), "backgrounds//club3", tm));
            main_club.add_background(new Background(new Rectangle(0, 0, 960, 540), "backgrounds//club4", tm));
            main_club.add_background(new Background(new Rectangle(0, 0, 960, 540), "backgrounds//club5", tm));

            main_club.add_foreground(new Foreground(new Rectangle(0, 0, 960, 540), "foregrounds//main_club_fg_1", tm));
            main_club.add_foreground(new Foreground(new Rectangle(0, 0, 960, 540), "foregrounds//main_club_fg_2", tm));
            main_club.add_foreground(new Foreground(new Rectangle(0, 0, 960, 540), "foregrounds//main_club_fg_3", tm));
            main_club.add_foreground(new Foreground(new Rectangle(0, 0, 960, 540), "foregrounds//main_club_fg_4", tm));
            main_club.add_foreground(new Foreground(new Rectangle(0, 0, 960, 540), "foregrounds//main_club_fg_5", tm));

            vip.add_background(new Background(new Rectangle(0, 0, 960, 540), "backgrounds//vip1", tm));
            vip.add_background(new Background(new Rectangle(0, 0, 960, 540), "backgrounds//vip2", tm));
            vip.add_background(new Background(new Rectangle(0, 0, 960, 540), "backgrounds//vip3", tm));
            vip.add_background(new Background(new Rectangle(0, 0, 960, 540), "backgrounds//vip4", tm));
            vip.add_background(new Background(new Rectangle(0, 0, 960, 540), "backgrounds//vip5", tm));

            vip.add_foreground(new Foreground(new Rectangle(0, 0, 960, 540), "foregrounds//vip_fg_1", tm));
            vip.add_foreground(new Foreground(new Rectangle(0, 0, 960, 540), "foregrounds//vip_fg_2", tm));
            vip.add_foreground(new Foreground(new Rectangle(0, 0, 960, 540), "foregrounds//vip_fg_3", tm));
            vip.add_foreground(new Foreground(new Rectangle(0, 0, 960, 540), "foregrounds//vip_fg_4", tm));
            vip.add_foreground(new Foreground(new Rectangle(0, 0, 960, 540), "foregrounds//vip_fg_5", tm));

            tbk_office.add_background(new Background(new Rectangle(0, 0, 960, 540), "backgrounds//tbk_office1", tm));
            tbk_office.add_background(new Background(new Rectangle(0, 0, 960, 540), "backgrounds//tbk_office2", tm));

            tbk_office.add_foreground(new Foreground(new Rectangle(0, 0, 960, 540), "foregrounds//tbkoffice_fg_1", tm));
            tbk_office.add_foreground(new Foreground(new Rectangle(0, 0, 960, 540), "foregrounds//tbkoffice_fg_2", tm));

            Wave wave1 = main_club.waves[0] = new Wave(0, 1600);
            Wave wave2 = main_club.waves[1] = new Wave(1, 1600);
            Wave wave3 = main_club.waves[2] = new Wave(2, 3200);
            Wave wave4 = main_club.waves[3] = new Wave(3, 4790);
            Wave wave5 = main_club.waves[4] = new Wave(4, 4790);
            
            //Wave 1
            wave1.bad_guys.Add(new WaveElement(WaveElement.Type.running_kone, new Point(400, 500)));
            wave1.bad_guys.Add(new WaveElement(WaveElement.Type.running_treeboi, new Point(500, 500)));

            //Wave 2
            wave2.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(-500, 450), 0));
            wave2.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(-600, 450), 0));
            wave2.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(1500, 450), 0));

            wave2.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(-100, 450), 1000));
            wave2.bad_guys.Add(new WaveElement(WaveElement.Type.guido_strong, new Point(2100, 450), 1000));
            wave2.bad_guys.Add(new WaveElement(WaveElement.Type.guido_strong, new Point(2200, 450), 1000));

            //Wave 3
            wave3.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(4100, 450), 3000));
            wave3.bad_guys.Add(new WaveElement(WaveElement.Type.nicole, new Point(4300, 450), 3000));
            wave3.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(4500, 450), 3000));

            wave3.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(1900, 450), 3000));
            wave3.bad_guys.Add(new WaveElement(WaveElement.Type.guido_strong, new Point(1700, 450), 3000));
            wave3.bad_guys.Add(new WaveElement(WaveElement.Type.guido_strong, new Point(1500, 450), 3000));

            ////Wave 4
            wave4.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(5300, 450), 4200));
            wave4.bad_guys.Add(new WaveElement(WaveElement.Type.silvio, new Point(5500, 450), 4200));
            wave4.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(5700, 450), 4200));
            wave4.bad_guys.Add(new WaveElement(WaveElement.Type.nicole, new Point(3100, 450), 4200));
            wave4.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(2900, 450), 4200));
            wave4.bad_guys.Add(new WaveElement(WaveElement.Type.guido_strong, new Point(2700, 450), 4200));

            ////Wave 5
            wave5.bad_guys.Add(new WaveElement(WaveElement.Type.butabi, new Point(5000, 450)));

            wave1 = vip.waves[0] = new Wave(0, 1200);
            wave2 = vip.waves[1] = new Wave(1, 2400);
            wave3 = vip.waves[2] = new Wave(2, 3600);
            wave4 = vip.waves[3] = new Wave(3, 4790);

            //Wave 1
            wave1.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(-600, 450)));
            wave1.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(-800, 412)));
            wave1.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(1600, 450)));

            wave1.bad_guys.Add(new WaveElement(WaveElement.Type.guido_strong, new Point(2100, 450), 1000));
            wave1.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(2300, 450), 1000));
            wave1.bad_guys.Add(new WaveElement(WaveElement.Type.guido_strong, new Point(-100, 450), 1000));

            //Wave 2
            wave2.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(2900, 450), 1800));
            wave2.bad_guys.Add(new WaveElement(WaveElement.Type.guido_strong, new Point(3100, 450), 1800));
            wave2.bad_guys.Add(new WaveElement(WaveElement.Type.guido_strong, new Point(700, 450), 1800));

            wave2.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(500, 450), 1800));
            wave2.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(3300, 450), 1800));
            wave2.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(300, 450), 1800));

            //Wave 3
            wave3.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(4100, 450), 3000));
            wave3.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(4300, 450), 3000));
            wave3.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(4500, 450), 3000));

            wave3.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(1900, 450), 3000));
            wave3.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(1700, 450), 3000));
            wave3.bad_guys.Add(new WaveElement(WaveElement.Type.guido_strong, new Point(1500, 450), 3000));

            //Wave 4
            wave4.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(5300, 450), 4200));
            wave4.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(5500, 450), 4200));
            wave4.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(5700, 450), 4200));
            wave4.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(3100, 450), 4200));
            wave4.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(2900, 450), 4200));
            wave4.bad_guys.Add(new WaveElement(WaveElement.Type.guido_strong, new Point(2700, 450), 4200));

            //Wave 1
            wave1 = tbk_office.waves[0] = new Wave(0, 1910);
            wave1.bad_guys.Add(new WaveElement(WaveElement.Type.treeboi, new Point(750, 500), 0));
            wave1.bad_guys.Add(new WaveElement(WaveElement.Type.kone, new Point(700, 500), 0));
            
        }

        private void define_newspaper()
        {
            newspaper = new Level(Level.LevelName.Newspaper);

            printing = new Act(Act.ActName.printing_area, 368, 5, "main_club_music");
            office = new Act(Act.ActName.reporters_offices, 358, 5, "vip_music");
            editor_office = new Act(Act.ActName.cheif_editor_office, 314, 3, "boss_music");

            newspaper.add_act(printing, 0);
            newspaper.add_act(office, 1);
            newspaper.add_act(editor_office, 2);

            printing.add_background(new Background(new Rectangle(0, 0, 960, 540), "backgrounds//printing_1", tm));
            printing.add_background(new Background(new Rectangle(0, 0, 960, 540), "backgrounds//printing_2", tm));
            printing.add_background(new Background(new Rectangle(0, 0, 960, 540), "backgrounds//printing_3", tm));
            printing.add_background(new Background(new Rectangle(0, 0, 960, 540), "backgrounds//printing_4", tm));
            printing.add_background(new Background(new Rectangle(0, 0, 960, 540), "backgrounds//printing_5", tm));

            printing.add_foreground(new Foreground(new Rectangle(0, 0, 960, 540), "foregrounds//fg_printing_1", tm));
            printing.add_foreground(new Foreground(new Rectangle(0, 0, 960, 540), "foregrounds//fg_printing_2", tm));
            printing.add_foreground(new Foreground(new Rectangle(0, 0, 960, 540), "foregrounds//fg_printing_3", tm));
            printing.add_foreground(new Foreground(new Rectangle(0, 0, 960, 540), "foregrounds//fg_printing_4", tm));
            printing.add_foreground(new Foreground(new Rectangle(0, 0, 960, 540), "foregrounds//fg_printing_5", tm));

            office.add_background(new Background(new Rectangle(0, 0, 960, 540), "backgrounds//office_1", tm));
            office.add_background(new Background(new Rectangle(0, 0, 960, 540), "backgrounds//office_2", tm));
            office.add_background(new Background(new Rectangle(0, 0, 960, 540), "backgrounds//office_3", tm));
            office.add_background(new Background(new Rectangle(0, 0, 960, 540), "backgrounds//office_4", tm));
            office.add_background(new Background(new Rectangle(0, 0, 960, 540), "backgrounds//office_5", tm));

            office.add_foreground(new Foreground(new Rectangle(0, 0, 960, 540), "foregrounds//fg_office_1", tm));
            office.add_foreground(new Foreground(new Rectangle(0, 0, 960, 540), "foregrounds//fg_office_2", tm));
            office.add_foreground(new Foreground(new Rectangle(0, 0, 960, 540), "foregrounds//fg_office_3", tm));
            office.add_foreground(new Foreground(new Rectangle(0, 0, 960, 540), "foregrounds//fg_office_4", tm));
            office.add_foreground(new Foreground(new Rectangle(0, 0, 960, 540), "foregrounds//fg_office_5", tm));

            editor_office.add_background(new Background(new Rectangle(0, 0, 960, 540), "backgrounds//editor_1", tm));
            editor_office.add_background(new Background(new Rectangle(0, 0, 960, 540), "backgrounds//editor_2", tm));

            ////////////////////////////////////////////
            // PRINTING ACT
            ////////////////////////////////////////////

            Wave wave1 = printing.waves[0] = new Wave(0, 1600);
            Wave wave2 = printing.waves[1] = new Wave(1, 3200);
            Wave wave3 = printing.waves[2] = new Wave(2, 4590);

            //WAVE 1
            wave1.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(-500, 500)));
            wave1.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(-600, 540)));
            wave1.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(-700, 500)));

            wave1.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(1200, 500)));
            wave1.bad_guys.Add(new WaveElement(WaveElement.Type.guido_strong, new Point(1300, 540)));
            wave1.bad_guys.Add(new WaveElement(WaveElement.Type.guido_strong, new Point(1400, 500)));

            //WAVE 2
            wave2.bad_guys.Add(new WaveElement(WaveElement.Type.silvio, new Point(3500, 500), 2400));
            wave2.bad_guys.Add(new WaveElement(WaveElement.Type.coral, new Point(3600, 540), 2400));
            wave2.bad_guys.Add(new WaveElement(WaveElement.Type.nicole, new Point(3700, 500), 2400));

            wave2.bad_guys.Add(new WaveElement(WaveElement.Type.coral, new Point(1300, 500), 2400));
            wave2.bad_guys.Add(new WaveElement(WaveElement.Type.silvio, new Point(1200, 540), 2400));
            wave2.bad_guys.Add(new WaveElement(WaveElement.Type.nicole, new Point(1100, 500), 2400));

            //WAVE 3
            wave3.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(5100, 500), 4000));
            wave3.bad_guys.Add(new WaveElement(WaveElement.Type.fitty_cent, new Point(5200, 540), 4000));
            wave3.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(5300, 500), 4000));

            wave3.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(2900, 500), 4000));
            wave3.bad_guys.Add(new WaveElement(WaveElement.Type.fitty_cent, new Point(2800, 540), 4000));
            wave3.bad_guys.Add(new WaveElement(WaveElement.Type.butabi, new Point(2700, 500), 4000));


            ////////////////////////////////////////////
            // OFFICES  
            ////////////////////////////////////////////
            wave1 = office.waves[0] = new Wave(0, 1600);
            wave2 = office.waves[1] = new Wave(1, 3200);
            wave3 = office.waves[2] = new Wave(2, 4590);

            //WAVE 1
            wave1.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(-500, 500)));
            wave1.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(-600, 540)));
            wave1.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(-700, 500)));

            wave1.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(1200, 500)));
            wave1.bad_guys.Add(new WaveElement(WaveElement.Type.guido_strong, new Point(1300, 540)));
            wave1.bad_guys.Add(new WaveElement(WaveElement.Type.guido_strong, new Point(1400, 500)));

            //WAVE 2
            wave2.bad_guys.Add(new WaveElement(WaveElement.Type.silvio, new Point(3500, 500), 2400));
            wave2.bad_guys.Add(new WaveElement(WaveElement.Type.coral, new Point(3600, 540), 2400));
            wave2.bad_guys.Add(new WaveElement(WaveElement.Type.nicole, new Point(3700, 500), 2400));

            wave2.bad_guys.Add(new WaveElement(WaveElement.Type.coral, new Point(1300, 500), 2400));
            wave2.bad_guys.Add(new WaveElement(WaveElement.Type.silvio, new Point(1200, 540), 2400));
            wave2.bad_guys.Add(new WaveElement(WaveElement.Type.nicole, new Point(1100, 500), 2400));

            //WAVE 3
            wave3.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(5100, 500), 4000));
            wave3.bad_guys.Add(new WaveElement(WaveElement.Type.fitty_cent, new Point(5200, 540), 4000));
            wave3.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(5300, 500), 4000));

            wave3.bad_guys.Add(new WaveElement(WaveElement.Type.ben_seib, new Point(2900, 500), 4000));
            wave3.bad_guys.Add(new WaveElement(WaveElement.Type.fitty_cent, new Point(2800, 540), 4000));
            wave3.bad_guys.Add(new WaveElement(WaveElement.Type.butabi, new Point(2700, 500), 4000));

            //Commander Office  
            editor_office.waves[0] = new Wave(0, 1350);
            editor_office.waves[0].bad_guys.Add(new WaveElement(WaveElement.Type.commander, new Point(900, 500)));
        }

        public void define_levels()
        {
            define_bifo();
            define_tbk_club();
            define_newspaper();
        }
    }
}
