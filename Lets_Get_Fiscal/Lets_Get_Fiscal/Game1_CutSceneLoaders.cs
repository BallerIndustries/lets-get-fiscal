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
        Stopwatch timer;
        CutScene blake_wilkie, nedroid_chinese, scene1, scene2, scene3, scene4, scene5, scene6, credits;

        public void Load_CutScenes()
        {
            timer = new Stopwatch();

            blake_wilkie = new CutScene(GameState.State.Menu, Load_BlakeWilkie, tm);
            nedroid_chinese = new CutScene(GameState.State.Menu, Load_NedroidChinese, tm);
            scene1 = new CutScene(GameState.State.ComboExplain, Load_Scene1_Rudy, tm);
            scene2 = new CutScene(GameState.State.GamePlay, Load_Scene2, tm);
            scene3 = new CutScene(GameState.State.LevelAnnounce, Load_Scene3, tm);

            //Place holder cutscenes.
            scene4 = new CutScene(GameState.State.GamePlay, Load_Scene4, tm);
            scene5 = new CutScene(GameState.State.LevelAnnounce, Load_Scene5, tm);
            scene6 = new CutScene(GameState.State.GamePlay, Load_Scene6, tm);
            credits = new CutScene(GameState.State.Menu, Load_Credits, tm);
        }

        public void Load_Credits()
        {
            CutScene cs = credits;

            List<string> list = new List<string>(5);

            cs.add_cue(new Sound(0, "credits_song"));

            list.Add("Lead Hot Shot Programmer");
            list.Add("ANGUS CHENG");
            cs.add_cue(new CreditText(0, 5000, list));

            list.Clear();
            list.Add("Written By");
            list.Add("ANGUS CHENG");
            cs.add_cue(new CreditText(5000, 10000, list));

            list.Clear();
            list.Add("Bug Introducer");
            list.Add("WILSON CHEN");
            cs.add_cue(new CreditText(10000, 15000, list));

            list.Clear();
            list.Add("Voice Actors");
            list.Add("ZACK JONES");
            list.Add("JOSH KOEN");
            list.Add("JONATHAN TREE");
            cs.add_cue(new CreditText(15000, 20000, list));

            list.Clear();
            list.Add("Character Spriters");
            list.Add("NADEZHDA SMISHLAJEVA");
            list.Add("ADAM ELBAHTIMY");
            list.Add("CLEST ELNITH");
            list.Add("CHRISTINA NAWROCKI");
            list.Add("PAWEL MISZUK");
            list.Add("MICHAEL LUCIDI");
            cs.add_cue(new CreditText(20000, 25000, list));

            list.Clear();
            list.Add("Background Spriters");
            list.Add("DUDU TORRES");
            list.Add("ADAM ELBAHTIMY");
            list.Add("CLEST ELNITH");
            cs.add_cue(new CreditText(25000, 30000, list));

            list.Clear();
            list.Add("Concept Art");
            list.Add("AARON WONG");
            list.Add("RICHARD WINTERSTETTER");
            list.Add("THARINDU MAPALAGAMA");
            cs.add_cue(new CreditText(30000, 35000, list));

            list.Clear();
            list.Add("Music and SFX");
            list.Add("TREEBOI&KONE");
            cs.add_cue(new CreditText(35000, 40000, list));

            list.Clear();
            list.Add("Comic Strips");
            list.Add("RUDY SUMARSO");
            list.Add("JAY REYES");
            cs.add_cue(new CreditText(40000, 45000, list));

            list.Clear();
            list.Add("Special Thanks");
            list.Add("ANGUS CHENG");
            cs.add_cue(new CreditText(45000, 50000, list));

            list.Clear();
            list.Add("Thanks for Playing");
            list.Add("BALLERINDUSTRIES.COM");
            cs.add_cue(new CreditText(50000, 5000000, list));
        }

        public void Load_Scene2()
        {
            CutScene cs = scene2;

            cs.add_cue(new Sound(0, "cs2"));
            //cs.add_cue(new Sound(0,     "cutscene2_a"));
            //cs.add_cue(new Sound(2000,  "cutscene2_b"));
            //cs.add_cue(new Sound(4400,  "cutscene2_c"));
            //cs.add_cue(new Sound(10100, "cutscene2_d"));
            //cs.add_cue(new Sound(12300, "cutscene2_e"));
            //cs.add_cue(new Sound(13500, "cutscene2_f"));
            //cs.add_cue(new Sound(16200, "cutscene2_g"));
            //cs.add_cue(new Sound(20900, "cutscene2_h"));
            //cs.add_cue(new Sound(23300, "cutscene2_i"));
            ////cs.add_cue(new Sound(26300, "cutscene2_j"));
            //cs.add_cue(new Sound(26300, "cutscene2_j1"));
            //cs.add_cue(new Sound(28300, "cutscene2_j2"));
            
            //cs.add_cue(new Sound(32400, "cutscene2_k"));

            cs.add_cue(new ViewPanel(2000, 4400, new Rectangle(48, 39, 1183, 419)));
            cs.add_cue(new ViewPanel(4400, 10100, new Rectangle(44, 480, 803, 411)));
            cs.add_cue(new ViewPanel(10100, 12300, new Rectangle(860, 480, 371, 407)));
            cs.add_cue(new ViewPanel(12300, 13500, new Rectangle(48, 912, 233, 408)));
            cs.add_cue(new ViewPanel(13500, 16200, new Rectangle(295, 912, 233, 410)));
            cs.add_cue(new ViewPanel(16200, 20900, new Rectangle(543, 915, 195, 407)));
            cs.add_cue(new ViewPanel(20900, 23300, new Rectangle(752, 916, 482, 412)));
            cs.add_cue(new ViewPanel(23300, 26300, new Rectangle(48, 1350, 284, 412)));
            cs.add_cue(new ViewPanel(26300, 29000, new Rectangle(353, 1348, 286, 414)));
            cs.add_cue(new ViewPanel(29000, 32400, new Rectangle(651, 1353, 572, 402)));
            cs.add_cue(new ViewPanel(32400, 36100, new Rectangle(1238, 1351, 583, 411)));

            cs.add_cue(new Comic(0, 0, "comics//cs2", tm));
            //cs.add_fades();
            cs.add_transitions();
            
            cs.comic_num = 0;
            //initialise_objects();
        }

        public void Load_Scene3()
        {
            CutScene cs = scene3;

            cs.add_cue(new Sound(0, "cs3"));

            //cs.add_cue(new Text(0, 57300, "*** PLACE HOLDER AUDIO ***", Fonts.SubtitleFont));

            //Page 1
            cs.add_cue(new ViewPanel(0, 2000, new Rectangle(56, 50, 419, 470)));     // 1
            cs.add_cue(new ViewPanel(2000, 4350, new Rectangle(492, 52, 405, 461)));     // 2
            cs.add_cue(new ViewPanel(4350, 6700, new Rectangle(910, 50, 491, 467)));     // 3
            cs.add_cue(new ViewPanel(6700, 12900, new Rectangle(50, 551, 337, 464)));     // 4
            cs.add_cue(new ViewPanel(12900, 21900, new Rectangle(406, 545, 479, 468)));     // 5
            cs.add_cue(new ViewPanel(21900, 30000, new Rectangle(996, 543, 405, 468)));     // 6
            cs.add_cue(new ViewPanel(30000, 37900, new Rectangle(52, 1039, 839, 469)));     // 7
            cs.add_cue(new ViewPanel(37900, 44000, new Rectangle(906, 1041, 499, 465)));     // 8
            cs.add_cue(new ViewPanel(44000, 45100, new Rectangle(52, 1538, 238, 464)));     // 9
            cs.add_cue(new ViewPanel(45100, 46500, new Rectangle(302, 1533, 245, 475)));     // 10
            cs.add_cue(new ViewPanel(46500, 49850, new Rectangle(562, 1538, 539, 463)));     // 11
            cs.add_cue(new ViewPanel(49850, 53200, new Rectangle(1116, 1536, 287, 466)));     // 12

            cs.add_cue(new Comic(0, 0, "comics//cs3_a", tm));

            //Page 2
            cs.add_cue(new ViewPanel(53200, 55300, new Rectangle(53, 48, 418, 466)));                 // 1
            cs.add_cue(new ViewPanel(55300, 57400, new Rectangle(479, 48, 253, 467)));                 // 2
            cs.add_cue(new ViewPanel(57400, 63200, new Rectangle(739, 48, 239, 472)));                 // 3
            cs.add_cue(new ViewPanel(63200, 67300, new Rectangle(989, 48, 184, 472)));                 // 4
            cs.add_cue(new ViewPanel(67300, 70700, new Rectangle(1181, 48, 222, 466)));                 // 5
            cs.add_cue(new ViewPanel(70700, 73400, new Rectangle(52, 547, 338, 467)));                 // 6
            cs.add_cue(new ViewPanel(73400, 78200, new Rectangle(405, 545, 579, 468)));                 // 7
            cs.add_cue(new ViewPanel(78200, 82100, new Rectangle(998, 546, 405, 467)));                 // 8
            cs.add_cue(new ViewPanel(82100, 85000, new Rectangle(55, 1041, 410, 466)));                 // 9
            cs.add_cue(new ViewPanel(85000, 88000, new Rectangle(481, 1037, 407, 470)));                 // 10
            cs.add_cue(new ViewPanel(88000, 91000, new Rectangle(900, 1037, 502, 467)));                 // 11
            cs.add_cue(new ViewPanel(91000, 94000, new Rectangle(50, 1537, 237, 467)));                 // 12
            cs.add_cue(new ViewPanel(94000, 97000, new Rectangle(304, 1535, 240, 468)));                 // 13
            cs.add_cue(new ViewPanel(97000, 100000, new Rectangle(558, 1538, 545, 466)));                 // 14
            cs.add_cue(new ViewPanel(100000, 104000, new Rectangle(1113, 1537, 291, 468)));                 // 15

            cs.add_cue(new Comic(0, 0, "comics//cs3_b", tm));

            cs.add_fades();
            //cs.add_transitions();
            cs.comic_num = 0;
#if XBOX
            if (Guide.IsTrialMode)
            {
                cs.return_state = GameState.State.DemoOver;
                change_level(bifo, bifo_customer);
                game_state.current_level_announce.Initialise(game_state.current_act.music.Name, 2, spriteBatch, Fonts.LevelAnnounceFont);
                game_state.current_stage_cleared.Initialise("stage_clear", 2, spriteBatch, Fonts.LevelAnnounceFont, DebugFont);
            }
            else
            {
                cs.return_state = GameState.State.LevelAnnounce;
                change_level(bifo, bifo_customer);
                game_state.current_level_announce.Initialise(game_state.current_act.music.Name, 2, spriteBatch, Fonts.LevelAnnounceFont);
                game_state.current_stage_cleared.Initialise("stage_clear", 2, spriteBatch, Fonts.LevelAnnounceFont, DebugFont);
            }
#else
            cs.return_state = GameState.State.LevelAnnounce;
            change_level(bifo, bifo_customer);
            game_state.current_level_announce.Initialise(game_state.current_act.music.Name, 2, spriteBatch, Fonts.LevelAnnounceFont);
            game_state.current_stage_cleared.Initialise("stage_clear", 2, spriteBatch, Fonts.LevelAnnounceFont, DebugFont);
#endif
        }

        public void Load_Scene4()
        {
            CutScene cs = scene4;

            cs.add_cue(new ViewPanel(0, 2000, new Rectangle(29, 6, 437, 486)));                //1
            cs.add_cue(new ViewPanel(2000, 4000, new Rectangle(485, 8, 948, 488)));            //2
            cs.add_cue(new ViewPanel(4000, 6000, new Rectangle(29, 522, 538, 487)));            //3
            cs.add_cue(new ViewPanel(6000, 8000, new Rectangle(580, 522, 386, 488)));           //4

            cs.add_cue(new ViewPanel(8000, 10000, new Rectangle(977, 522, 455, 486)));          //5
            cs.add_cue(new ViewPanel(10000, 12000, new Rectangle(30, 1036, 437, 489)));          //6
            cs.add_cue(new ViewPanel(12000, 14000, new Rectangle(485, 1035, 410, 487)));         //7
            cs.add_cue(new ViewPanel(14000, 16000, new Rectangle(913, 1035, 512, 491)));         //8

            cs.add_cue(new ViewPanel(16000, 18000, new Rectangle(29, 1550, 511, 492)));          //9
            cs.add_cue(new ViewPanel(18000, 20000, new Rectangle(553, 1551, 879, 488)));         //10

            cs.add_cue(new Comic(0, 0, "comics//cs4_a", tm));

            cs.add_transitions();
            //cs.add_cue(new Text(0, 10000, "Scene 4", Fonts.SubtitleFont));
        }

        public void Load_Scene5()
        {
            CutScene cs = scene5;

            cs.add_cue(new ViewPanel(0, 2000, new Rectangle( 9, 6, 435, 481)));  //1
            cs.add_cue(new ViewPanel(2000, 4000, new Rectangle(461, 6, 475, 481))); //2
            cs.add_cue(new ViewPanel(4000, 6000, new Rectangle(955, 9, 449, 475))); //3
            cs.add_cue(new ViewPanel(6000, 8000, new Rectangle(9, 521, 542, 481))); //4
            cs.add_cue(new ViewPanel(8000, 10000, new Rectangle(570, 521, 366, 474))); //5
            cs.add_cue(new ViewPanel(10000, 12000, new Rectangle(952, 521, 448, 471))); //6
            cs.add_cue(new ViewPanel(12000, 14000, new Rectangle(13, 1028, 730, 478))); //7
            cs.add_cue(new ViewPanel(14000, 16000, new Rectangle(760, 1025, 202, 481))); //8
            cs.add_cue(new ViewPanel(16000, 18000, new Rectangle(982, 1028, 425, 482))); //9
            cs.add_cue(new ViewPanel(18000, 20000, new Rectangle(6, 1540, 455, 478))); //10
            cs.add_cue(new ViewPanel(20000, 22000, new Rectangle(477, 1540, 927, 478))); //11

            cs.add_cue(new Comic(0, 0, "comics//cs5a", tm));

            cs.add_cue(new ViewPanel(22000, 24000, new Rectangle(53, 53, 421, 464)));  //1
            cs.add_cue(new ViewPanel(24000, 26000, new Rectangle(491, 49, 454, 462))); //2
            cs.add_cue(new ViewPanel(26000, 28000, new Rectangle(965, 49, 439, 465))); //3
            cs.add_cue(new ViewPanel(28000, 30000, new Rectangle(53, 547, 385, 468))); //4
            cs.add_cue(new ViewPanel(30000, 32000, new Rectangle(451, 544, 355, 475))); //5
            cs.add_cue(new ViewPanel(32000, 34000, new Rectangle(819, 547, 581, 462))); //6
            cs.add_cue(new ViewPanel(34000, 36000, new Rectangle(56, 1042, 272, 461))); //7
            cs.add_cue(new ViewPanel(36000, 38000, new Rectangle(341, 1042, 638, 468))); //8
            cs.add_cue(new ViewPanel(38000, 40000, new Rectangle(995, 1038, 409, 468))); //9
            cs.add_cue(new ViewPanel(40000, 42000, new Rectangle(53, 1540, 441, 464))); //10
            cs.add_cue(new ViewPanel(42000, 44000, new Rectangle( 511, 1536, 889, 472))); //11

            cs.add_cue(new Comic(0, 0, "comics//cs5b", tm));
            
            //Game State Changes
            cs.return_state = GameState.State.LevelAnnounce;

            change_level(newspaper, printing);
            game_state.current_level_announce.Initialise(game_state.current_act.music.Name, 3, spriteBatch, Fonts.LevelAnnounceFont);
            game_state.current_stage_cleared.Initialise("stage_clear", 3, spriteBatch, Fonts.LevelAnnounceFont, DebugFont);
        }

        public void Load_Scene6()
        {
            CutScene cs = scene6;

            cs.add_cue(new ViewPanel(0, 2000, new Rectangle(37, 67, 460, 380)));            //1
            cs.add_cue(new ViewPanel(2000, 4000, new Rectangle(521, 70, 460, 380)));        //2
            cs.add_cue(new ViewPanel(4000, 6000, new Rectangle(38, 467, 460, 380)));        //3
            cs.add_cue(new ViewPanel(6000, 8000, new Rectangle(525, 470, 460, 380)));        //4
            cs.add_cue(new ViewPanel(8000, 10000, new Rectangle(38, 875, 460, 380)));        //5
            cs.add_cue(new ViewPanel(10000, 12000, new Rectangle(519, 870, 460, 380)));        //6
            cs.add_cue(new ViewPanel(12000, 14000, new Rectangle(1058, 56, 460, 380)));        //7
            cs.add_cue(new ViewPanel(14000, 16000, new Rectangle(1548, 58, 460, 380)));        //8
            cs.add_cue(new ViewPanel(16000, 18000, new Rectangle(1058, 460, 460, 380)));        //9
            cs.add_cue(new ViewPanel(18000, 20000, new Rectangle(1548, 462, 460, 380)));        //10
            cs.add_cue(new ViewPanel(20000, 22000, new Rectangle(1060, 870, 460, 380)));        //11

            cs.add_fades();

            cs.add_cue(new Comic(0, 0, "comics//cs6", tm));
        }

        public void Load_Scene1_Rudy()
        {
            CutScene cs = scene1;

            cs.add_cue(new Sound(0, "cs1"));

            //cs.add_cue(new Sound(0, "cs1"));
            //cs.add_cue(new Sound(4200, "beep"));
            //cs.add_cue(new Sound(6600, "growl"));
            //cs.add_cue(new Sound(7700, "clock_hit"));
            //cs.add_cue(new Sound(12400, "ten_years"));
            //cs.add_cue(new Sound(15300, "brush_teeth"));
            //cs.add_cue(new Sound(18000, "traffic_noise_a"));
            //cs.add_cue(new Sound(20000, "traffic_noise_b"));
            //cs.add_cue(new Sound(22000, "traffic_noise_c"));
            //cs.add_cue(new Sound(24000, "traffic_noise_d"));
            //cs.add_cue(new Sound(26300, "walking_noise"));
            //cs.add_cue(new Sound(29800, "stelven_get_in"));
            //cs.add_cue(new Sound(32100, "go_down"));
            //cs.add_cue(new Sound(36700, "laundry_get"));
            //cs.add_cue(new Sound(45400, "help_lad"));
            //cs.add_cue(new Sound(48300, "definition"));
            //cs.add_cue(new Sound(51400, "alright"));
            //cs.add_cue(new Sound(57800, "door_hit"));
            //cs.add_cue(new Sound(58600, "probably_duck"));
            //cs.add_cue(new Sound(62300, "introduction"));

            //cs.add_cue(new Text(0, 69000, "*** PLACE HOLDER AUDIO ***", Fonts.SubtitleFont));


            cs.add_cue(new ViewPanel(0, 2000, new Rectangle(49, 43, 462, 464)));                //1
            cs.add_cue(new ViewPanel(2000, 3700, new Rectangle(512, 42, 432, 461)));            //2
            cs.add_cue(new ViewPanel(3700, 5600, new Rectangle(959, 45, 407, 461)));            //3
            cs.add_cue(new ViewPanel(5600, 6300, new Rectangle(51, 528, 271, 461)));            //4
            cs.add_cue(new ViewPanel(6300, 7800, new Rectangle(331, 528, 254, 452)));           //5
            cs.add_cue(new ViewPanel(7800, 9500, new Rectangle(600, 528, 769, 458)));          //6
            cs.add_cue(new ViewPanel(9500, 11200, new Rectangle(51, 1010, 769, 459)));          //7
            cs.add_cue(new ViewPanel(11200, 13000, new Rectangle(835, 1013, 534, 450)));        //8
            cs.add_cue(new ViewPanel(13000, 17000, new Rectangle(51, 1493, 323, 456)));         //9
            cs.add_cue(new ViewPanel(17000, 19000, new Rectangle(386, 1487, 323, 468)));        //10
            cs.add_cue(new ViewPanel(19000, 23100, new Rectangle(724, 1496, 648, 456)));        //11

            cs.add_cue(new Comic(0, 0, "comics//cs1_a", tm));

            //cs.add_cue(new ViewPanel(23100, 23600, new Rectangle(46, 43, 451, 451)));           //1
            
            //A TRANSITION! OOOOOOOH!
            cs.add_cue(new Transition(23100, 27500, new Rectangle(46 + 2048, 43, 451, 451), new Rectangle(514 + 2048, 43, 415, 454)));

            //cs.add_cue(new ViewPanel(24800, 27500, new Rectangle(514, 43, 415, 454)));          //2
            cs.add_cue(new ViewPanel(27500, 30000, new Rectangle(945, 43, 419, 462)));          //3
            cs.add_cue(new ViewPanel(30000, 31000, new Rectangle(49, 527, 259, 455)));          //4
            cs.add_cue(new ViewPanel(31000, 33000, new Rectangle(328, 521, 252, 458)));         //5

            cs.add_cue(new ViewPanel(33000, 35500, new Rectangle(597, 527, 222, 448)));       //6

            cs.add_cue(new ViewPanel(35500, 40000, new Rectangle(836, 531, 169, 451)));         //7

            cs.add_cue(new ViewPanel(40000, 42300, new Rectangle(1022, 531, 345, 454)));      //8

            cs.add_cue(new ViewPanel(42300, 47400, new Rectangle(46, 1009, 770, 451)));         //9
            cs.add_cue(new ViewPanel(47400, 56500, new Rectangle(833, 1005, 531, 458)));        //10
            cs.add_cue(new ViewPanel(56500, 58800, new Rectangle(49, 1490, 140, 451)));         //11
            cs.add_cue(new ViewPanel(58800, 61650, new Rectangle(199, 1493, 302, 455)));        //12
            cs.add_cue(new ViewPanel(61650, 64500, new Rectangle(514, 1496, 850, 452)));        //13

            cs.add_cue(new Comic(0, 0, "comics//cs1_b", tm));

            cs.add_cue(new ViewPanel(64500, 67700, new Rectangle(39, 43, 186, 458)));           //1
            cs.add_cue(new ViewPanel(67700, 74600, new Rectangle(235, 49, 448, 448)));          //2
            cs.add_cue(new ViewPanel(74600, 79000, new Rectangle(697, 49, 229, 448)));          //3
            cs.add_cue(new ViewPanel(79000, 83200, new Rectangle(945, 46, 415, 451)));          //4
            cs.add_cue(new ViewPanel(83200, 85000, new Rectangle(46, 537, 259, 442)));          //5
            cs.add_cue(new ViewPanel(85000, 88700, new Rectangle(325, 527, 382, 458)));         //6
            //cs.add_cue(new ViewPanel(0, 62300, new Rectangle(730, 534, 634, 451)));         //7
            cs.add_cue(new ViewPanel(88700, 92200, new Rectangle(46, 1009, 916, 454)));         //8
            cs.add_cue(new ViewPanel(92200, 96300, new Rectangle(975, 1015, 389, 452)));        //9    
            cs.add_cue(new ViewPanel(96300, 106000, new Rectangle(43, 1500, 1321, 451)));        //10

            cs.add_cue(new Comic(0, 0, "comics//cs1_c", tm));

            //cs.add_transitions();
            cs.add_fades();
            cs.comic_num = 0;
            initialise_objects();
        }


        public void Load_Scene1()
        {
            CutScene cs = scene1;

            cs.add_cue(new Sound(3000, "alarm_noise"));
            cs.add_cue(new Sound(7800, "alarm_off"));
            cs.add_cue(new Sound(9000, "waking_up"));
            cs.add_cue(new Sound(13000, "ten_years"));

            cs.add_cue(new ViewPanel(0, 3000, new Rectangle(92, 72, 299, 486)));
            cs.add_cue(new ViewPanel(3000, 7800, new Rectangle(419, 67, 338, 494)));
            cs.add_cue(new ViewPanel(7800, 9000, new Rectangle(776, 67, 336, 494)));
            cs.add_cue(new ViewPanel(9000, 13000, new Rectangle(86, 572, 696, 528)));


            cs.add_cue(new ViewPanel(13000, 16000, new Rectangle(796, 586, 313, 517)));


            cs.add_cue(new ViewPanel(16000, 17000, new Rectangle(86, 1114, 344, 517)));


            cs.add_cue(new ViewPanel(17000, 18000, new Rectangle(455, 1117, 344, 520)));
            cs.add_cue(new ViewPanel(18000, 19000, new Rectangle(821, 1123, 305, 525)));

            cs.add_cue(new Comic(0, 12000, "comics//cutscene1_1", tm));

            cs.add_cue(new ViewPanel(19000, 20000, new Rectangle(13, 13, 463, 572)));
            cs.add_cue(new ViewPanel(20000, 21000, new Rectangle(493, 75, 557, 524)));
            cs.add_cue(new ViewPanel(21000, 22000, new Rectangle(0, 607, 423, 502)));
            cs.add_cue(new ViewPanel(22000, 23000, new Rectangle(448, 610, 594, 507)));
            cs.add_cue(new ViewPanel(23000, 24000, new Rectangle(16, 1126, 1024, 504)));

            cs.add_cue(new Comic(0, 12000, "comics//cutscene1_2", tm));

            cs.add_cue(new ViewPanel(24000, 25000, new Rectangle(89, 97, 522, 495)));
            cs.add_cue(new ViewPanel(25000, 26000, new Rectangle(637, 97, 475, 498)));
            cs.add_cue(new ViewPanel(26000, 27000, new Rectangle(75, 625, 603, 498)));
            cs.add_cue(new ViewPanel(27000, 28000, new Rectangle(695, 614, 417, 514)));
            cs.add_cue(new ViewPanel(28000, 29000, new Rectangle(78, 1134, 1081, 534)));

            cs.add_cue(new Comic(0, 12000, "comics//cutscene1_3", tm));

            cs.add_transitions();
            cs.comic_num = 0;

            initialise_objects();
        }

        public void Load_Scene1_Jay()
        {
            CutScene cs = scene1;

            cs.add_cue(new Sound(3000, "alarm_noise"));
            cs.add_cue(new Sound(7800, "alarm_off"));
            cs.add_cue(new Sound(8000, "waking_up"));
            cs.add_cue(new Sound(13000, "ten_years"));
            cs.add_cue(new Sound(16000, "brush_teeth"));

            cs.add_cue(new Sound(18200, "street_noise"));
            cs.add_cue(new Sound(23200, "footsteps"));

            cs.add_cue(new Sound(29000, "stelven_get_in"));
            cs.add_cue(new Sound(31000, "go_down"));

            cs.add_cue(new Sound(40000, "what_is"));
            cs.add_cue(new Sound(42000, "organic"));
            cs.add_cue(new Sound(48000, "announce"));
            cs.add_cue(new Sound(54600, "psychadelic"));

            cs.add_cue(new ViewPanel(0, 3000, new Rectangle(92, 74, 141, 262)));
            cs.add_cue(new ViewPanel(3000, 7800, new Rectangle(234, 73, 142, 258)));
            cs.add_cue(new ViewPanel(7800, 9000, new Rectangle(383, 70, 136, 262)));
            cs.add_cue(new ViewPanel(9000, 13000, new Rectangle(88, 331, 437, 142)));
            cs.add_cue(new ViewPanel(13000, 16000, new Rectangle(94, 465, 207, 253)));
            cs.add_cue(new ViewPanel(16000, 18200, new Rectangle(309, 464, 214, 256)));

            cs.add_cue(new Comic(0, 0, "comics//jay_1", tm));

            cs.add_cue(new ViewPanel(18200, 20200, new Rectangle(89, 80, 135, 321)));
            cs.add_cue(new ViewPanel(20200, 23200, new Rectangle(233, 79, 286, 172)));
            cs.add_cue(new ViewPanel(23200, 26600, new Rectangle(231, 255, 290, 144)));
            cs.add_cue(new ViewPanel(26600, 29000, new Rectangle(86, 408, 120, 323)));
            cs.add_cue(new ViewPanel(29000, 31000, new Rectangle(213, 408, 71, 319)));
            cs.add_cue(new ViewPanel(31000, 38200, new Rectangle(290, 406, 232, 320)));

            cs.add_cue(new Comic(0, 0, "comics//jay_2", tm));

            cs.add_cue(new ViewPanel(38200, 40000, new Rectangle(83, 66, 155, 307)));
            cs.add_cue(new ViewPanel(40000, 46000, new Rectangle(240, 71, 168, 293)));
            cs.add_cue(new ViewPanel(46000, 48000, new Rectangle(413, 69, 105, 296)));
            cs.add_cue(new ViewPanel(48000, 51600, new Rectangle(84, 367, 199, 214)));
            cs.add_cue(new ViewPanel(51600, 54600, new Rectangle(318, 367, 201, 175)));
            cs.add_cue(new ViewPanel(54600, 60000, new Rectangle(146, 551, 372, 161)));

            cs.add_cue(new Comic(0, 0, "comics//jay_3", tm));
            
            cs.add_transitions();
            cs.comic_num = 0;
            initialise_objects();
        }

        public void Load_BlakeWilkie()
        {
            CutScene cs = blake_wilkie;

            cs.add_cue(new Comic(0, 12000, "comics//comic_4", tm));
            cs.add_cue(new ViewPanel(0, 2000, new Rectangle(36, 61, 532, 136)));
            cs.add_cue(new ViewPanel(2000, 4000, new Rectangle(22, 215, 189, 171)));
            cs.add_cue(new ViewPanel(4000, 6000, new Rectangle(222, 204, 179, 180)));
            cs.add_cue(new ViewPanel(6000, 8000, new Rectangle(411, 203, 172, 167)));
            cs.add_cue(new ViewPanel(8000, 10000, new Rectangle(37, 397, 532, 334)));
            cs.add_cue(new ViewPanel(10000, 12000, new Rectangle(87, 737, 490, 133)));

            cs.add_transitions();
        }

        public void Load_NedroidChinese()
        {
            CutScene cs = nedroid_chinese;

            cs.add_cue(new Comic(0, 15000, "comics//nedroid_chinese", tm));
            cs.add_cue(new Sound(0, "bear_1"));
            cs.add_cue(new Sound(2500, "bear_2"));
            cs.add_cue(new Sound(9000, "bear_3"));
            cs.add_cue(new Sound(10600, "bear_4"));

            cs.add_cue(new ViewPanel(0, 2500, new Rectangle(21, 18, 269, 308)));
            cs.add_cue(new ViewPanel(2500, 9000, new Rectangle(304, 19, 268, 304)));
            cs.add_cue(new ViewPanel(9000, 10600, new Rectangle(21, 337, 269, 308)));
            cs.add_cue(new ViewPanel(10600, 15000, new Rectangle(302, 339, 270, 308)));

            cs.add_transitions();
        }

    }
}
