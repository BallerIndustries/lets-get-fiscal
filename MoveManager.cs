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
    public class MoveManager
    {
        const int SIZE = 20;

        AnimationCollection[] all_animation_sets = new AnimationCollection[SIZE];
        string[] move_set_names = new string[SIZE];
        int index = 0;

        public MoveManager()
        {
            LoadContent();
        }

        public AnimationCollection find_move_set(string name)
        {
            for (int i = 0; i < all_animation_sets.Length; i++)
            {
                if (move_set_names[i] == name)
                    return all_animation_sets[i];
            }

            return null;
        }

        private void LoadContent()
        {
            //Load_YSignal();
            //Load_Bison();
            //Load_ChunLi();
            //Load_Electra();
            //Load_Axel();
            //Load_Ryu();
            //Load_Ken();
            
            Load_Ben_Seib();
            Load_Guido();
            Load_FittyCent();
            Load_Coral();
            Load_Silvio();
            Load_Roxbury();
            Load_Nicole();
            Load_Accountant();
            Load_Customer_Girl();
            Load_Kiki();
            Load_Treeboi();
            Load_Kone();
            Load_Commander();
            Load_Fernando();
        }

        void add_move_set(string name, AnimationCollection move_set)
        {
            all_animation_sets[index] = move_set;
            move_set_names[index] = name;
            index++;
        }

        void Load_Fernando()
        {
            AnimationCollection move_list = new AnimationCollection();

            move_list.addMove(new Animation(new Rectangle(0, 1560, 120, 120), 4, true, "idle", GameObject.State.idle));
            move_list.addMove(new Animation(new Rectangle(0, 120, 120, 120), 5, false, "leap", GameObject.State.leap));
            move_list.addMove(new Animation(new Rectangle(0, 240, 120, 120), 1, false, "dash", GameObject.State.dash));

            move_list.addMove(new Animation(new Rectangle(0, 360, 120, 120), 8, false, "sword_projectiles", GameObject.State.sword_projectiles));
            //move_list.addMove(new Animation(new Rectangle(0, 600, 120, 120), 5, false, "sword_attack", GameObject.State.sword_attack));
            move_list.addMove(new Animation(new Rectangle(0, 480, 120, 120), 3, false, "teleport", GameObject.State.teleport_out));
            move_list.addMove(new Animation(new Rectangle(0, 720, 120, 120), 3, false, "sword_down", GameObject.State.sword_down));

            AttackAnimation sword_attack = new AttackAnimation(new Rectangle(0, 600, 120, 120), 5, false, "sword_attack", GameObject.State.sword_attack, 20, "pah", 3);

            sword_attack.add_attack(new Rectangle(321, 639, 33, 42), 2);
            sword_attack.add_attack(new Rectangle(446, 659, 33, 41), 3);

            move_list.addMove(sword_attack);

            //Just here to remove the glitching animation.
            move_list.addMove(new Animation(new Rectangle(0, 1080, 120, 120), 2, true, "dying", GameObject.State.dying));
            move_list.addMove(new Animation(new Rectangle(0, 1200, 120, 120), 2, true, "death_leap", GameObject.State.death_leap));
            move_list.addMove(new Animation(new Rectangle(0, 1200, 120, 120), 2, true, "KO_leap", GameObject.State.KO_leap));
            move_list.addMove(new Animation(new Rectangle(0, 1440, 120, 120), 1, true, "dead", GameObject.State.dead));
            move_list.addMove(new Animation(new Rectangle(0, 1440, 120, 120), 1, true, "KO", GameObject.State.KO));
            move_list.addMove(new Animation(new Rectangle(0, 1320, 120, 120), 2, true, "getting_up", GameObject.State.getting_up));

            add_move_set("spritesheets//fernando", move_list);
        }

        void Load_Commander()
        {
            AnimationCollection move_list = new AnimationCollection();

            move_list.addMove(new Animation(new Rectangle(0, 0, 70, 100), 4, true, "idle", GameObject.State.idle));
            move_list.addMove(new Animation(new Rectangle(300, 0, 70, 100), 4, true, "walking", GameObject.State.walking));
            move_list.addMove(new Animation(new Rectangle(600, 0, 70, 100), 5, false, "shooting", GameObject.State.shooting));
            move_list.addMove(new Animation(new Rectangle(970, 0, 70, 100), 4, false, "throwing_grenade", GameObject.State.throwing_grenade));
            move_list.addMove(new Animation(new Rectangle(0, 120, 100, 100), 2, false, "death_leap", GameObject.State.death_leap));
            move_list.addMove(new Animation(new Rectangle(0, 120, 100, 100), 2, false, "ko_leap", GameObject.State.KO_leap));

            move_list.addMove(new Animation(new Rectangle(1270, 0, 70, 100), 2, false, "dying", GameObject.State.dying));
            
            move_list.addMove(new Animation(new Rectangle(220, 120, 100, 100), 1, true, "KO", GameObject.State.KO));
            //move_list.addMove(new Animation(new Rectangle(660, 120, 100, 100), 5, false, "charging", GameObject.State.charging));
            move_list.addMove(new Animation(new Rectangle(520, 240, 100, 100), 6, false, "suicide", GameObject.State.suicide));

            move_list.addMove(new Animation(new Rectangle(1020, 240, 100, 100), 1, true, "dead", GameObject.State.dead));

            AttackAnimation getting_up =    new AttackAnimation(new Rectangle(340, 120, 100, 100), 3, false, "getting_up", GameObject.State.getting_up, 20);
            AttackAnimation knife_slash =   new AttackAnimation(new Rectangle(0, 240, 100, 100), 5, false, "knife_slash", GameObject.State.knife_slash, 10, "pah", 4);
            AttackAnimation hor_spin = new AttackAnimation(new Rectangle(1140, 240, 100, 100), 3, true, "vertical_spin", GameObject.State.vertical_spin, 25);
            AttackAnimation vert_spin = new AttackAnimation(new Rectangle(1460, 240, 100, 100), 3, true, "horizontal_spin", GameObject.State.horizontal_spin, 25);
            AttackAnimation charging = new AttackAnimation(new Rectangle(660, 120, 100, 100), 5, false, "charging", GameObject.State.charging, 25);


            getting_up.add_attack(new Rectangle(519, 156, 19, 43), 1, true);

            //knife_slash.add_attack(new Rectangle(159, 263, 11, 29), 1, true);
            //knife_slash.add_attack(new Rectangle(220, 262, 39, 13), 2, true);
            //knife_slash.add_attack(new Rectangle(370, 262, 14, 30), 3, true);

            knife_slash.add_attack(new Rectangle(100, 240, 100, 100), 1, true);
            knife_slash.add_attack(new Rectangle(200, 240, 100, 100), 2, true);
            knife_slash.add_attack(new Rectangle(300, 240, 100, 100), 3, true);

            hor_spin.add_attack(new Rectangle(1172, 243, 38, 92), 0, true);
            hor_spin.add_attack(new Rectangle(1273, 243, 39, 93), 1, true);
            hor_spin.add_attack(new Rectangle(1373, 244, 42, 91), 2, true);

            vert_spin.add_attack(new Rectangle(1464, 269, 93, 41), 0, true);
            vert_spin.add_attack(new Rectangle(1563, 271, 93, 42), 1, true);
            vert_spin.add_attack(new Rectangle(1666, 273, 91, 41), 2, true);

            charging.add_attack(new Rectangle(760, 120, 100, 100), 1, true);
            charging.add_attack(new Rectangle(860, 120, 100, 100), 2, true);
            charging.add_attack(new Rectangle(960, 120, 100, 100), 3, true);

            move_list.addMove(getting_up);
            move_list.addMove(knife_slash);
            move_list.addMove(hor_spin);
            move_list.addMove(vert_spin);
            move_list.addMove(charging);
            
            add_move_set("spritesheets//commander", move_list);
        }

        void Load_Kone()
        {
            AnimationCollection move_list = new AnimationCollection();

            move_list.addMove(new Animation(new Rectangle(0, 0, 70, 90), 4, true, "idle", GameObject.State.idle));
            move_list.addMove(new Animation(new Rectangle(900, 0, 70, 90), 5, false, "shoot_laser", GameObject.State.shoot_laser));

            move_list.addMove(new Animation(new Rectangle(0, 110, 90, 90), 4, true, "running", GameObject.State.running));

            AttackAnimation charge = new AttackAnimation(new Rectangle(380, 110, 90, 90), 5, true, "charge", GameObject.State.charge, 30);
            //AttackAnimation teleport_out = new AttackAnimation(new Rectangle(300, 0, 70, 90), 4, false, "teleport_out", GameObject.State.teleport_out, 15);
            AttackAnimation teleport_in = new AttackAnimation(new Rectangle(600, 0, 70, 90), 4, false, "teleport_in", GameObject.State.teleport_in, 15);
            move_list.addMove(new Animation(new Rectangle(300, 0, 70, 90), 4, false, "teleport_in", GameObject.State.teleport_out));

            move_list.addMove(new Animation(new Rectangle(1640, 0, 70, 90), 2, false, "dying", GameObject.State.dying));

            move_list.addMove(new Animation(new Rectangle(850, 110, 90, 90), 2, false, "ko_leap", GameObject.State.KO_leap));
            move_list.addMove(new Animation(new Rectangle(850, 110, 90, 90), 2, false, "death_leap", GameObject.State.death_leap));

            move_list.addMove(new Animation(new Rectangle(1050, 110, 90, 90), 1, false, "dead", GameObject.State.dead));
            move_list.addMove(new Animation(new Rectangle(1050, 110, 90, 90), 1, false, "KO", GameObject.State.KO));

            move_list.addMove(new Animation(new Rectangle(1160, 110, 90, 90), 2, false, "getting_up", GameObject.State.getting_up));

            charge.add_attack(new Rectangle(451, 139, 13, 41), 0, true);
            charge.add_attack(new Rectangle(541, 139, 14, 42), 1, true);
            charge.add_attack(new Rectangle(629, 135, 15, 41), 2, true);
            charge.add_attack(new Rectangle(701, 141, 18, 21), 3, true);
            charge.add_attack(new Rectangle(806, 136, 19, 38), 4, true);

            //teleport_out.add_attack(new Rectangle(333, 6, 23, 81), 0, true);
            //teleport_out.add_attack(new Rectangle(390, 8, 27, 79), 1, true);
            //teleport_out.add_attack(new Rectangle(460, 5, 26, 81), 2, true);

            teleport_in.add_attack(new Rectangle(693, 7, 22, 78), 1, true);
            teleport_in.add_attack(new Rectangle(762, 10, 29, 73), 2, true);
            teleport_in.add_attack(new Rectangle(831, 11, 26, 69), 3, true);

            move_list.addMove(charge);
            move_list.addMove(teleport_in);
            //move_list.addMove(teleport_out);

            add_move_set("spritesheets//kone", move_list);
        }

        void Load_Treeboi()
        {
            AnimationCollection move_list = new AnimationCollection();

            move_list.addMove(new Animation(new Rectangle(0, 0, 70, 90), 4, true, "idle", GameObject.State.idle));
            move_list.addMove(new Animation(new Rectangle(900, 0, 70, 90), 5, false, "shoot_laser", GameObject.State.shoot_laser));

            move_list.addMove(new Animation(new Rectangle(0, 110, 90, 90), 4, true, "running", GameObject.State.running));

            AttackAnimation charge = new AttackAnimation(new Rectangle(380, 110, 90, 90), 5, true, "charge", GameObject.State.charge, 30);
            //AttackAnimation teleport_out = new AttackAnimation(new Rectangle(300, 0, 70, 90), 4, false, "teleport_out", GameObject.State.teleport_out, 15);
            AttackAnimation teleport_in = new AttackAnimation(new Rectangle(600, 0, 70, 90), 4, false, "teleport_in", GameObject.State.teleport_in, 15);
            move_list.addMove(new Animation(new Rectangle(300, 0, 70, 90), 4, false, "teleport_in", GameObject.State.teleport_out));

            move_list.addMove(new Animation(new Rectangle(1640, 0, 70, 90), 2, false, "dying", GameObject.State.dying));

            move_list.addMove(new Animation(new Rectangle(850, 110, 90, 90), 2, false, "ko_leap", GameObject.State.KO_leap));
            move_list.addMove(new Animation(new Rectangle(850, 110, 90, 90), 2, false, "death_leap", GameObject.State.death_leap));

            move_list.addMove(new Animation(new Rectangle(1050, 110, 90, 90), 1, false, "dead", GameObject.State.dead));
            move_list.addMove(new Animation(new Rectangle(1050, 110, 90, 90), 1, false, "KO", GameObject.State.KO));

            move_list.addMove(new Animation(new Rectangle(1160, 110, 90, 90), 2, false, "getting_up", GameObject.State.getting_up));

            charge.add_attack(new Rectangle(451, 139, 13, 41), 0, true);
            charge.add_attack(new Rectangle(541, 139, 14, 42), 1, true);
            charge.add_attack(new Rectangle(629, 135, 15, 41), 2, true);
            charge.add_attack(new Rectangle(701, 141, 18, 21), 3, true);
            charge.add_attack(new Rectangle(806, 136, 19, 38), 4, true);

            //teleport_out.add_attack(new Rectangle(333, 6, 23, 81), 0, true);
            //teleport_out.add_attack(new Rectangle(390, 8, 27, 79), 1, true);
            //teleport_out.add_attack(new Rectangle(460, 5, 26, 81), 2, true);

            teleport_in.add_attack(new Rectangle(693, 7, 22, 78), 1, true);
            teleport_in.add_attack(new Rectangle(762, 10, 29, 73), 2, true);
            teleport_in.add_attack(new Rectangle(831, 11, 26, 69), 3, true);

            move_list.addMove(charge);
            move_list.addMove(teleport_in);
            //move_list.addMove(teleport_out);

            add_move_set("spritesheets//treeboi", move_list);
        }

        void Load_Ken()
        {
            AnimationCollection move_list = new AnimationCollection();

            //Define non attacking animations
            move_list.addMove(new Animation(new Rectangle(273, 8, 54, 84), 5, true, "walking", Character.State.walking));
            move_list.addMove(new Animation(new Rectangle(12, 9, 52, 85), 3, true, "idle", Character.State.idle));
            move_list.addMove(new Animation(new Rectangle(249, 1328, 50, 83), 2, false, "dying", Character.State.dying));


            move_list.addMove(new Animation(new Rectangle(265, 1500, 83, 37), 1, true, "dead", Character.State.dead));
            move_list.addMove(new Animation(new Rectangle(265, 1500, 83, 37), 1, true, "KO", Character.State.KO));
            move_list.addMove(new Animation(new Rectangle(0, 1475, 80, 76), 3, false, "death_leap", Character.State.death_leap));
            move_list.addMove(new Animation(new Rectangle(0, 1475, 80, 76), 3, false, "KO_leap", Character.State.KO_leap));

            move_list.addMove(new Grab(new Rectangle(364, 1328, 48, 82), 1, Character.State.grabbed, false, new Point(370, 1336)));
            List<Rectangle> bound_list = new List<Rectangle>(3);
            bound_list.Add(new Rectangle(128, 1334, 49, 75));
            bound_list.Add(new Rectangle(160, 1475, 80, 76));
            bound_list.Add(new Rectangle(265, 1500, 83, 37));

            move_list.addMove(new Animation(bound_list, false, Character.State.being_thrown));
            move_list.addMove(new Animation(new Rectangle(365, 1422, 66, 106), 4, false, "getting_up", Character.State.getting_up));
            

            //Instantiate Attack Animations
            AttackAnimation punch = new AttackAnimation(new Rectangle(162, 285, 66, 85), 2, false, "punching", Character.State.punching, 10);
            AttackAnimation kick = new AttackAnimation(new Rectangle(19, 107, 69, 88), 3, false, "kicking", Character.State.kicking, 20);

            //Add attack boxes to the attack animations
            punch.add_attack(new Rectangle(278, 301, 14, 8), 1);
            kick.add_attack(new Rectangle(217, 110, 8, 8), 2);

            //Add attack animations to the move list. Bayby.
            move_list.addMove(punch);
            move_list.addMove(kick);

            add_move_set("spritesheets//ken", move_list);
        }

        void Load_Ben_Seib()
        {
            AnimationCollection move_list = new AnimationCollection();

            //Define non attacking animations
            move_list.addMove(new Animation(new Rectangle(11, 0, 54, 80), 4, true, "walking", Character.State.walking));
            move_list.addMove(new Animation(new Rectangle(259, 0, 51, 78), 3, true, "idle", Character.State.idle));
            move_list.addMove(new Animation(new Rectangle(276, 167, 69, 75), 2, false, "dying", Character.State.dying));


            move_list.addMove(new Animation(new Rectangle(196, 201, 76, 46), 1, true, "dead", Character.State.dead));
            move_list.addMove(new Animation(new Rectangle(196, 201, 76, 46), 1, true, "KO", Character.State.KO));
            move_list.addMove(new Animation(new Rectangle(31, 163, 72, 83), 2, false, "death_leap", Character.State.death_leap));
            move_list.addMove(new Animation(new Rectangle(31, 163, 72, 83), 2, false, "KO_leap", Character.State.KO_leap));

            //TODO: GET THE ARTIST TO DO THE GETTING UP STUFF
            move_list.addMove(new Animation(new Rectangle(4, 261, 71, 72), 2, false, "getting_up", Character.State.getting_up));

            //Instantiate Attack Animations
            AttackAnimation punch = new AttackAnimation(new Rectangle(155, 85, 60, 74), 2, false, "punching", Character.State.punching, 10);
            AttackAnimation kick = new AttackAnimation(new Rectangle(16, 87, 62, 72), 2, false, "kicking", Character.State.kicking, 20);

            move_list.addMove(new Grab(new Rectangle(345, 167, 69, 75), 1, Character.State.grabbed, false, new Point(389, 188)));
            
            List<Rectangle> bound_list = new List<Rectangle>(3);
            bound_list.Add(new Rectangle(276, 167, 69, 75));
            bound_list.Add(new Rectangle(103, 163, 72, 83));
            bound_list.Add(new Rectangle(196, 201, 76, 46));
            move_list.addMove(new Animation(bound_list, false, GameObject.State.being_thrown));

            //Add attack boxes to the attack animations
            punch.add_attack(new Rectangle(255, 103, 19, 7), 1);
            kick.add_attack(new Rectangle(115, 127, 24, 14), 1);

            //Add attack animations to the move list. Bayby.
            move_list.addMove(punch);
            move_list.addMove(kick);

            add_move_set("spritesheets//ben_seib", move_list);
        }

        void Load_Geek()
        {
            AnimationCollection move_list = new AnimationCollection();

            move_list.addMove(new Animation(new Rectangle(14, 58, 75, 86), 2, true, "idle", Character.State.idle));
            move_list.addMove(new Animation(new Rectangle(16, 157, 70, 82), 3, true, "walking", Character.State.walking));

            add_move_set("spritesheets//geek", move_list);
        }

        void Load_YSignal()
        {
            AnimationCollection move_list = new AnimationCollection();

            //Define non attacking animations
            move_list.addMove(new Animation(new Rectangle(10, 10, 46, 90), 3, true, "walking", Character.State.walking));
            move_list.addMove(new Animation(new Rectangle(10, 10, 46, 90), 1, true, "idle", Character.State.idle));
            move_list.addMove(new Animation(new Rectangle(250, 23, 74, 73), 1, false, "dying", Character.State.dying));


            move_list.addMove(new Animation(new Rectangle(582, 171, 99, 28), 1, true, "dead", Character.State.dead));
            move_list.addMove(new Animation(new Rectangle(582, 171, 99, 28), 1, true, "KO", Character.State.KO));
            move_list.addMove(new Animation(new Rectangle(250, 23, 74, 73), 3, false, "death_leap", Character.State.death_leap));
            move_list.addMove(new Animation(new Rectangle(250, 23, 74, 73), 3, false, "KO_leap", Character.State.KO_leap));
            move_list.addMove(new Animation(new Rectangle(574, 35, 68, 64), 2, false, "getting_up", Character.State.getting_up));

            //Instantiate Attack Animations
            AttackAnimation punch = new AttackAnimation(new Rectangle(236, 112, 69, 81), 3, false, "punching", Character.State.punching, 10);

            //Add attack boxes to the attack animations
            punch.add_attack(new Rectangle(416, 131, 26, 7), 2);

            //Add attack animations to the move list. Bayby.
            move_list.addMove(punch);

            add_move_set("spritesheets//ysignal", move_list);
        }

        void Load_Bison()
        {
            AnimationCollection move_list = new AnimationCollection();

            move_list.addMove(new Animation(new Rectangle(33, 16, 73, 90), 3, true, "idle", Character.State.idle));
            move_list.addMove(new Animation(new Rectangle(185, 819, 67, 104), 2, false, "dying", Character.State.dying));
            move_list.addMove(new Animation(new Rectangle(618, 851, 98, 40), 1, true, "dead", Character.State.dead));
            move_list.addMove(new Animation(new Rectangle(184, 820, 82, 101), 5, false, "death_leap", Character.State.death_leap));

            move_list.addMove(new Animation(new Rectangle(284, 5, 68, 94), 3, true, "walking", Character.State.walking));

            add_move_set("spritesheets//bison", move_list);
        }

        void Load_ChunLi()
        {
            AnimationCollection move_list = new AnimationCollection();

            move_list.addMove(new Animation(new Rectangle(9, 13, 59, 87), 3, true, "idle", Character.State.idle));
            move_list.addMove(new Animation(new Rectangle(26, 801, 62, 88), 2, false, "dying", Character.State.dying));
            move_list.addMove(new Animation(new Rectangle(311, 793, 80, 100), 3, false, "death_leap", Character.State.death_leap));
            move_list.addMove(new Animation(new Rectangle(561, 850, 86, 37), 1, true, "dead", Character.State.dead));

            move_list.addMove(new Animation(new Rectangle(212, 12, 67, 85), 4, true, "walking", Character.State.walking));

            add_move_set("spritesheets//chunli", move_list);
        }

        void Load_Electra()
        {
            AnimationCollection move_list = new AnimationCollection();

            move_list.addMove(new Animation(new Rectangle(10, 12, 54, 88), 4, true, "walking", Character.State.walking));
            move_list.addMove(new Animation(new Rectangle(10, 12, 54, 88), 1, true, "idle", Character.State.idle));
            move_list.addMove(new Animation(new Rectangle(240, 17, 66, 83), 2, false, "dying", Character.State.dying));


            move_list.addMove(new Animation(new Rectangle(511, 70, 99, 30), 1, true, "dead", Character.State.dead));
            move_list.addMove(new Animation(new Rectangle(389, 18, 59, 81), 2, false, "death_leap", Character.State.death_leap));

            //Instantiate Attack Animations
            AttackAnimation punch = new AttackAnimation(new Rectangle(200, 226, 124, 115), 6, false, "punching", Character.State.punching, 10);

            //Add attack boxes to the attack animations
            punch.add_attack(new Rectangle(715, 297, 56, 4), 4);

            //Add attack animations to the move list. Bayby.
            move_list.addMove(punch);

            add_move_set("spritesheets//electra", move_list);
        }

        void Load_Guido()
        {
            AnimationCollection move_list = new AnimationCollection();

            move_list.addMove(new Animation(new Rectangle(0, 0, 72, 102), 4, true, "walking", Character.State.walking));
            move_list.addMove(new Animation(new Rectangle(308, 0, 72, 102), 3, true, "idle", Character.State.idle));
            move_list.addMove(new Animation(new Rectangle(544, 0, 72, 102), 2, false, "dying", Character.State.dying));

            move_list.addMove(new Animation(new Rectangle(0, 122, 130, 102), 2, false, "KO_Leap", Character.State.KO_leap));
            move_list.addMove(new Animation(new Rectangle(0, 122, 130, 102), 2, false, "death_leap", Character.State.death_leap));

            move_list.addMove(new Animation(new Rectangle(260, 122, 130, 102), 1, true, "KO", Character.State.KO));
            move_list.addMove(new Animation(new Rectangle(260, 122, 130, 102), 1, true, "dead", Character.State.dead));

            move_list.addMove(new Animation(new Rectangle(410, 122, 130, 102), 2, false, "getting_up", Character.State.getting_up));
            //move_list.addMove(new Animation(new Rectangle(700, 122, 130, 102), 2, false, "throw", GameObject.State.suplex));

            List<Rectangle> suplex_bounds = new List<Rectangle>(3);
            suplex_bounds.Add(new Rectangle(540, 122, 130, 102));
            suplex_bounds.Add(new Rectangle(700, 122, 130, 102));
            suplex_bounds.Add(new Rectangle(830, 122, 130, 102));

            move_list.addMove(new Grab(suplex_bounds, GameObject.State.suplex, true, new Point(595, 160), 12));
            //move_list.addMove(new Animation(suplex_bounds, false, GameObject.State.suplex));

            AttackAnimation karate_chop = new AttackAnimation(new Rectangle(980, 122, 130, 102), 4, false, "right_punch", Character.State.punching, 20, "psh", 3);
            karate_chop.add_attack(new Rectangle(1196, 141, 24, 8), 1);
            karate_chop.add_attack(new Rectangle(1322, 163, 37, 10), 2);

            move_list.addMove(karate_chop);

            add_move_set("spritesheets//guido", move_list);
        }

        void Load_FittyCent()
        {
            AnimationCollection move_list = new AnimationCollection();

            move_list.addMove(new Animation(new Rectangle(0, 0, 60, 85), 4, true, "walking", Character.State.walking));
            move_list.addMove(new Animation(new Rectangle(260, 0, 60, 85), 3, true, "idle", Character.State.idle));
            move_list.addMove(new Animation(new Rectangle(460, 0, 60, 85), 2, false, "dying", Character.State.dying));

            List<Rectangle> bound_list = new List<Rectangle>(3);
            bound_list.Add(new Rectangle(680, 0, 60, 85));
            bound_list.Add(new Rectangle(740, 0, 60, 85));
            bound_list.Add(new Rectangle(680, 0, 60, 85));

            AttackAnimation left_punch = new AttackAnimation(new Rectangle(600, 0, 60, 85), 1, false, "punching", Character.State.punching, 10);
            AttackAnimation punch = new AttackAnimation(bound_list, false, Character.State.right_punch, 20);

            AttackAnimation brawl_attack = new AttackAnimation(new Rectangle(630, 105, 90, 85), 4, false, "brawl_attack", GameObject.State.fitty_brawl, 20, "psh", 5);
            AttackAnimation leap_attack = new AttackAnimation(new Rectangle(515, 110, 91, 80), 1, false, "leap_attack", GameObject.State.leap_attack, 20);
            
            brawl_attack.add_attack(new Rectangle(775, 131, 19, 8), 1);
            brawl_attack.add_attack(new Rectangle(950, 135, 23, 7), 3);

            move_list.addMove(brawl_attack);

            leap_attack.add_attack(new Rectangle(582, 147, 17, 13), 0);


            left_punch.add_attack(new Rectangle(638, 25, 22, 7), 0);
            punch.add_attack(new Rectangle(781, 24, 19, 7), 1, true);

            move_list.addMove(new Animation(new Rectangle(0, 110, 91, 85), 2, true, "ko_leap", Character.State.KO_leap));
            move_list.addMove(new Animation(new Rectangle(0, 110, 91, 85), 2, true, "death_leap", Character.State.death_leap));

            move_list.addMove(new Animation(new Rectangle(202, 110, 91, 80), 1, true, "KO", Character.State.KO));
            move_list.addMove(new Animation(new Rectangle(202, 110, 91, 80), 1, false, "dead", Character.State.dead));

            move_list.addMove(new Animation(new Rectangle(313, 110, 91, 80), 2, false, "getting_up", Character.State.getting_up));

            move_list.addMove(new Animation(new Rectangle(1010, 108, 91, 82), 2, false, "grabbing", GameObject.State.grab_start));

            move_list.addMove(new Grab(new Rectangle(820, 0, 91, 80), 1, GameObject.State.grabbed, false, new Point(883, 21)));

            List<Rectangle> being_thrown = new List<Rectangle>(3);

            being_thrown.Add(new Rectangle(520, 0, 60, 85));
            being_thrown.Add(new Rectangle(91, 110, 91, 85));
            being_thrown.Add(new Rectangle(202, 110, 91, 80));

            move_list.addMove(new Animation(being_thrown, false, GameObject.State.being_thrown));

            move_list.addMove(left_punch);
            move_list.addMove(punch);
            move_list.addMove(leap_attack);

            add_move_set("spritesheets//fitty_cent", move_list);
        }

        void Load_Coral()
        {
            AnimationCollection move_list = new AnimationCollection();

            move_list.addMove(new Animation(new Rectangle(0, 0, 57, 92), 3, true, "idle", Character.State.idle));
            move_list.addMove(new Animation(new Rectangle(184, 0, 57, 92), 3, true, "walking", Character.State.walking));
            move_list.addMove(new Animation(new Rectangle(0, 102, 50, 84), 2, false, "dying", Character.State.dying));

            AttackAnimation flame_kick = new AttackAnimation(new Rectangle(0, 263, 73, 85), 5, false, "punching", Character.State.punching, 10, "psh", 4);
            AttackAnimation slide_kick = new AttackAnimation(new Rectangle(0, 370, 100, 85), 2, false, "slide_kick", Character.State.slide_attack, 20);

            flame_kick.add_attack(new Rectangle(78, 313, 21, 15), 1);
            flame_kick.add_attack(new Rectangle(151, 286, 15, 23), 2);
            flame_kick.add_attack(new Rectangle(264, 273, 15, 22), 3);
            flame_kick.add_attack(new Rectangle(348, 287, 15, 21), 4);

            slide_kick.add_attack(new Rectangle(160, 436, 38, 12), 1, true);

            move_list.addMove(flame_kick);
            move_list.addMove(slide_kick);

            move_list.addMove(new Animation(new Rectangle(119, 111, 61, 77), 2, true, "ko_leap", Character.State.KO_leap));
            move_list.addMove(new Animation(new Rectangle(119, 111, 61, 77), 2, true, "death_leap", Character.State.death_leap));

            move_list.addMove(new Animation(new Rectangle(258, 138, 71, 39), 1, true, "KO", Character.State.KO));
            move_list.addMove(new Animation(new Rectangle(258, 138, 71, 39), 1, false, "dead", Character.State.dead));

            move_list.addMove(new Animation(new Rectangle(0, 191, 73, 58), 2, false, "getting_up", Character.State.getting_up));

            add_move_set("spritesheets//coral", move_list);
        }

        void Load_Silvio()
        {
            AnimationCollection move_list = new AnimationCollection();

            move_list.addMove(new Animation(new Rectangle(6, 2, 56, 83), 3, true, "idle", Character.State.idle));
            move_list.addMove(new Animation(new Rectangle(10, 88, 56, 83), 4, true, "walking", Character.State.walking));
            move_list.addMove(new Animation(new Rectangle(6, 342, 55, 81), 2, false, "dying", Character.State.dying));

            AttackAnimation low_kick = new AttackAnimation(new Rectangle(11, 178, 66, 82), 2, false, "low_kick", Character.State.low_kick, 10);
            AttackAnimation mid_kick = new AttackAnimation(new Rectangle(159, 178, 81, 82), 1, false, "mid_kick", Character.State.mid_kick, 10);
            AttackAnimation high_kick = new AttackAnimation(new Rectangle(239, 178, 80, 82), 1, false, "high_kick", Character.State.high_kick, 10);
            AttackAnimation jump_kick = new AttackAnimation(new Rectangle(11, 266, 62, 73), 2, false, "jump_kick", Character.State.jump_kick, 10);

            low_kick.add_attack(new Rectangle(128, 237, 11, 14), 1);
            mid_kick.add_attack(new Rectangle(220, 214, 16, 11), 0);
            high_kick.add_attack(new Rectangle(292, 206, 20, 7), 0);
            jump_kick.add_attack(new Rectangle(114, 299, 16, 9), 1, true);

            move_list.addMove(low_kick);
            move_list.addMove(mid_kick);
            move_list.addMove(high_kick);
            move_list.addMove(jump_kick);

            move_list.addMove(new Animation(new Rectangle(155, 342, 60, 81), 2, true, "ko_leap", Character.State.KO_leap));
            move_list.addMove(new Animation(new Rectangle(155, 342, 60, 81), 2, true, "death_leap", Character.State.death_leap));

            move_list.addMove(new Animation(new Rectangle(279, 395, 59, 28), 1, true, "KO", Character.State.KO));
            move_list.addMove(new Animation(new Rectangle(279, 395, 59, 28), 1, false, "dead", Character.State.dead));

            move_list.addMove(new Animation(new Rectangle(7, 426, 53, 59), 2, false, "getting_up", Character.State.getting_up));

            add_move_set("spritesheets//silvio", move_list);
        }

        void Load_Roxbury()
        {
            AnimationCollection move_list = new AnimationCollection();

            move_list.addMove(new Animation(new Rectangle(300, 0, 70, 90), 4, true, "idle", Character.State.idle));
            move_list.addMove(new Animation(new Rectangle(0, 0, 70, 90), 4, true, "walking", Character.State.walking));
            move_list.addMove(new Animation(new Rectangle(600, 0, 70, 90), 2, false, "dying", Character.State.dying));

            AttackAnimation snap_kick = new AttackAnimation(new Rectangle(0, 220, 90, 110), 2, false, "snap_kick", Character.State.snap_kick, 10);
            AttackAnimation charge = new AttackAnimation(new Rectangle(510, 110, 90, 90), 5, true, "charge", Character.State.charge, 20);

            snap_kick.add_attack(new Rectangle(148, 250, 12, 35), 1);

            move_list.addMove(snap_kick);


            move_list.addMove(new Animation(new Rectangle(0, 110, 90, 90), 2, true, "ko_leap", Character.State.KO_leap));
            move_list.addMove(new Animation(new Rectangle(0, 110, 90, 90), 2, true, "death_leap", Character.State.death_leap));

            move_list.addMove(new Animation(new Rectangle(200, 110, 90, 90), 1, true, "KO", Character.State.KO));
            move_list.addMove(new Animation(new Rectangle(200, 110, 90, 90), 1, false, "dead", Character.State.dead));

            move_list.addMove(new Animation(new Rectangle(310, 110, 90, 90), 2, false, "getting_up", Character.State.getting_up));

            move_list.addMove(new Grab(new Rectangle(200, 220, 90, 90), 1, GameObject.State.grabbed, false, new Point(247, 254)));


            charge.add_attack(new Rectangle(577, 173, 7, 12), 0, true);
            charge.add_attack(new Rectangle(668, 172, 7, 14), 1, true);
            charge.add_attack(new Rectangle(758, 170, 7, 15), 2, true);
            charge.add_attack(new Rectangle(847, 173, 9, 13), 3, true);
            charge.add_attack(new Rectangle(936, 172, 9, 12), 4, true);


            move_list.addMove(charge);

            List<Rectangle> bound_list = new List<Rectangle>(3);
            bound_list.Add(new Rectangle(670, 0, 70, 90));
            bound_list.Add(new Rectangle(90, 110, 90, 90));
            bound_list.Add(new Rectangle(200, 110, 90, 90));
            move_list.addMove(new Animation(bound_list, false, GameObject.State.being_thrown));

            add_move_set("spritesheets//roxbury", move_list);
        }

        void Load_Nicole()
        {
            AnimationCollection move_list = new AnimationCollection();

            move_list.addMove(new Animation(new Rectangle(6, 4, 61, 82), 3, true, "idle", Character.State.idle));
            move_list.addMove(new Animation(new Rectangle(13, 89, 61, 83), 3, true, "walking", Character.State.walking));
            move_list.addMove(new Animation(new Rectangle(8, 175, 48, 82), 2, false, "dying", Character.State.dying));

            AttackAnimation slap = new AttackAnimation(new Rectangle(3, 414, 61, 83), 3, false, "punching", Character.State.punching, 10);
            AttackAnimation leap_attack = new AttackAnimation(new Rectangle(9, 327, 93, 81), 2, true, "leap_attack", Character.State.leap_attack, 20);

            slap.add_attack(new Rectangle(105, 428, 19, 10), 1);
            leap_attack.add_attack(new Rectangle(158, 380, 36, 14), 1);

            move_list.addMove(slap);
            move_list.addMove(leap_attack);


            move_list.addMove(new Animation(new Rectangle(152, 175, 59, 82), 2, true, "ko_leap", Character.State.KO_leap));
            move_list.addMove(new Animation(new Rectangle(152, 175, 59, 82), 2, true, "death_leap", Character.State.death_leap));

            move_list.addMove(new Animation(new Rectangle(8, 284, 65, 36), 1, true, "KO", Character.State.KO));
            move_list.addMove(new Animation(new Rectangle(8, 284, 65, 36), 1, false, "dead", Character.State.dead));

            move_list.addMove(new Animation(new Rectangle(78, 265, 73, 55), 2, false, "getting_up", Character.State.getting_up));

            add_move_set("spritesheets//nicole", move_list);
        }

        void Load_Axel()
        {
            AnimationCollection move_list = new AnimationCollection();

            move_list.addMove(new Animation(new Rectangle(200, 21, 67, 91), 6, true, "walking", Character.State.walking));
            move_list.addMove(new Animation(new Rectangle(200, 21, 67, 91), 6, true, "walking", Character.State.force_walk));

            move_list.addMove(new Animation(new Rectangle(11, 31, 62, 81), 3, true, "idle", Character.State.idle));
            move_list.addMove(new Animation(new Rectangle(9, 917, 89, 61), 3, false, "getting_up", Character.State.getting_up));
            move_list.addMove(new Animation(new Rectangle(9, 917, 89, 61), 1, true, "dead", Character.State.dead));
            move_list.addMove(new Animation(new Rectangle(10, 280, 57, 93), 3, false, "jumping", Character.State.jumping));
            move_list.addMove(new Animation(new Rectangle(10, 280, 57, 93), 1, false, "rebound", Character.State.rebound));
            move_list.addMove(new Animation(new Rectangle(18, 1005, 55, 83), 1, false, "dying", Character.State.dying));
            move_list.addMove(new Animation(new Rectangle(101, 1032, 81, 72), 2, false, "death_leap", Character.State.death_leap));

            move_list.addMove(new Animation(new Rectangle(0, 439, 73, 69), 4, false, "suplex", Character.State.suplex));

            //move_list.addMove(new Animation(new Rectangle(487, 878, 52, 91), 1, true, "grabbing", State.grabbing));
            move_list.addMove(new Grab(new Rectangle(487, 878, 52, 91), 1, Character.State.grabbing, true, new Point(533, 909)));

            AttackAnimation punch = new AttackAnimation(new Rectangle(12, 764, 69, 86), 1, false, "punching", Character.State.punching, 10);
            AttackAnimation right_punch = new AttackAnimation(new Rectangle(94, 763, 69, 87), 2, false, "right_punch", Character.State.right_punch, 15);
            AttackAnimation kick = new AttackAnimation(new Rectangle(8, 654, 93, 87), 4, false, "kick", Character.State.kicking, 20);
            AttackAnimation dragon_rush = new AttackAnimation(new Rectangle(4, 124, 81, 117), 7, false, "dragon_rush", Character.State.dragon_rush, 20);

            AttackAnimation jump_kick1 = new AttackAnimation(new Rectangle(203, 288, 88, 82), 2, false, "jump_kick1", Character.State.jump_kick1, 20);
            AttackAnimation jump_kick2 = new AttackAnimation(new Rectangle(393, 276, 58, 93), 3, false, "jump_kick2", Character.State.jump_kick2, 20);
            AttackAnimation jump_kick3 = new AttackAnimation(new Rectangle(591, 298, 54, 72), 1, false, "jump_kick3", Character.State.jump_kick3, 20);

            punch.add_attack(new Rectangle(52, 780, 25, 8), 0);
            right_punch.add_attack(new Rectangle(202, 780, 27, 8), 1);
            kick.add_attack(new Rectangle(245, 689, 35, 11), 2);
            kick.add_attack(new Rectangle(347, 677, 29, 8), 3, true);

            dragon_rush.add_attack(new Rectangle(233, 184, 11, 22), 2);
            dragon_rush.add_attack(new Rectangle(309, 163, 15, 24), 3);
            dragon_rush.add_attack(new Rectangle(368, 153, 10, 20), 4, true);

            jump_kick1.add_attack(new Rectangle(345, 346, 31, 14), 1, true);
            jump_kick2.add_attack(new Rectangle(482, 314, 17, 12), 1, true);
            jump_kick2.add_attack(new Rectangle(540, 300, 11, 7), 2, true);
            jump_kick3.add_attack(new Rectangle(609, 346, 31, 11), 0, true);

            move_list.addMove(punch);
            move_list.addMove(right_punch);
            move_list.addMove(kick);
            move_list.addMove(dragon_rush);
            move_list.addMove(jump_kick1);
            move_list.addMove(jump_kick2);
            move_list.addMove(jump_kick3);

            add_move_set("spritesheets//axel", move_list);
        }

        void Load_Ryu()
        {
            AnimationCollection move_list = new AnimationCollection();

            //Ryu
            move_list.addMove(new Animation(new Rectangle(0, 90, 49, 88), 5, true, "walking", Character.State.walking));
            move_list.addMove(new Animation(new Rectangle(0, 0, 49, 87), 4, true, "idle", Character.State.idle));

            move_list.addMove(new Animation(new Rectangle(0, 264, 50, 93), 7, false, "jump", Character.State.jumping));
            move_list.addMove(new Animation(new Rectangle(0, 264, 50, 93), 7, false, "jumping_forward", Character.State.jumping_forward));
            move_list.addMove(new Animation(new Rectangle(0, 264, 50, 93), 7, false, "jumping_backward", Character.State.jumping_backward));
            move_list.addMove(new Animation(new Rectangle(157, 636, 65, 93), 6, false, "grab", Character.State.grab));
            move_list.addMove(new Animation(new Rectangle(0, 1835, 57, 88), 2, false, "dying", Character.State.dying));

            move_list.addMove(new Animation(new Rectangle(295, 1922, 77, 74), 3, false, "death_leap", Character.State.death_leap));
            move_list.addMove(new Animation(new Rectangle(529, 1960, 78, 36), 1, true, "dead", Character.State.dead));
            move_list.addMove(new Animation(new Rectangle(0, 1047, 57, 84), 1, true, "grabbing", Character.State.grabbing));

            //Instantiate Attack Animations
            AttackAnimation punch = new AttackAnimation(new Rectangle(0, 453, 75, 90), 2, false, "punching", Character.State.punching, 10);
            AttackAnimation right_punch = new AttackAnimation(new Rectangle(222, 453, 76, 92), 3, false, "right_punch", Character.State.right_punch, 15);
            AttackAnimation kick = new AttackAnimation(new Rectangle(74, 545, 74, 92), 2, false, "kick", Character.State.kicking, 20);
            AttackAnimation uppercut = new AttackAnimation(new Rectangle(0, 729, 67, 108), 5, false, "uppercut", Character.State.uppercut, 15);
            AttackAnimation jur_kick = new AttackAnimation(new Rectangle(0, 842, 78, 93), 5, false, "jur_kick", Character.State.jur_kick, 15);
            AttackAnimation snap_kick = new AttackAnimation(new Rectangle(0, 932, 86, 116), 6, false, "snap_kick", Character.State.snap_kick, 20);
            AttackAnimation shoruken = new AttackAnimation(new Rectangle(0, 1527, 54, 115), 7, false, "shoruken", Character.State.shoruken, 20);
            AttackAnimation hurricane_kick = new AttackAnimation(new Rectangle(0, 1639, 60, 109), 13, false, "hurricane_kick", Character.State.hurricane_kick, 15);
            AttackAnimation hadoken = new AttackAnimation(new Rectangle(0, 1747, 78, 86), 4, false, "hadoken", Character.State.hadoken, 5);

            //Add attack boxes to the attack animations
            right_punch.add_attack(new Rectangle(346, 469, 27, 9), 1);
            punch.add_attack(new Rectangle(108, 472, 28, 12), 1);
            kick.add_attack(new Rectangle(195, 545, 19, 17), 1);
            uppercut.add_attack(new Rectangle(183, 731, 11, 25), 2);
            jur_kick.add_attack(new Rectangle(212, 886, 24, 24), 2);
            snap_kick.add_attack(new Rectangle(217, 935, 12, 20), 2);
            snap_kick.add_attack(new Rectangle(334, 958, 9, 16), 3);
            shoruken.add_attack(new Rectangle(97, 1557, 8, 22), 1);
            shoruken.add_attack(new Rectangle(145, 1527, 11, 23), 2);
            shoruken.add_attack(new Rectangle(192, 1530, 12, 23), 3);
            hurricane_kick.add_attack(new Rectangle(243, 1685, 29, 12), 4);
            hurricane_kick.add_attack(new Rectangle(391, 1679, 27, 15), 6);

            //Add attack animations to the move list. Bayby.
            move_list.addMove(punch);
            move_list.addMove(kick);
            move_list.addMove(right_punch);
            move_list.addMove(jur_kick);
            move_list.addMove(snap_kick);
            move_list.addMove(uppercut);
            move_list.addMove(shoruken);
            move_list.addMove(hurricane_kick);
            move_list.addMove(hadoken);

            add_move_set("spritesheets//ryu", move_list);
        }

        void Load_Kiki()
        {
            AnimationCollection move_list = new AnimationCollection();

            move_list.addMove(new Animation(new Rectangle(6, 10, 65, 86), 3, true, "walking", GameObject.State.walking));
            move_list.addMove(new Animation(new Rectangle(212, 10, 65, 86), 3, true, "idle", GameObject.State.idle));
            move_list.addMove(new Animation(new Rectangle(412, 10, 65, 86), 3, false, "dying", GameObject.State.dying));
            move_list.addMove(new Animation(new Rectangle(6, 114, 72, 86), 4, false, "glass_throw", GameObject.State.glass_throw));

            move_list.addMove(new Animation(new Rectangle(392, 114, 84, 86), 2, false, "death_leap", GameObject.State.death_leap));
            move_list.addMove(new Animation(new Rectangle(392, 114, 84, 86), 2, false, "KO_Leap", GameObject.State.KO_leap));

            move_list.addMove(new Animation(new Rectangle(567, 167, 100, 42), 1, true, "dead", GameObject.State.dead));
            move_list.addMove(new Animation(new Rectangle(567, 167, 100, 42), 1, true, "KO", GameObject.State.KO));

            move_list.addMove(new Animation(new Rectangle(689, 114, 82, 86), 2, false, "getting_up", GameObject.State.getting_up));

            AttackAnimation whirling_attack = new AttackAnimation(new Rectangle(0, 205, 105, 107), 4, true, "whirling_attack", GameObject.State.whirling_attack, 15);

            whirling_attack.add_attack(new Rectangle(85, 217, 18, 68), 0);
            whirling_attack.add_attack(new Rectangle(186, 231, 20, 70), 1, true);
            whirling_attack.add_attack(new Rectangle(287, 239, 19, 70), 2);
            whirling_attack.add_attack(new Rectangle(384, 232, 22, 66), 3, true);

            move_list.addMove(whirling_attack);

            add_move_set("spritesheets//kiki", move_list);
        }


        void Load_Customer_Girl()
        {
            AnimationCollection move_list = new AnimationCollection();

            move_list.addMove(new Animation(new Rectangle(0, 524, 82, 29), 1, false, "KO", GameObject.State.KO));
            move_list.addMove(new Animation(new Rectangle(0, 524, 82, 29), 1, false, "Dead", GameObject.State.dead));

            move_list.addMove(new Animation(new Rectangle(6, 575, 72, 73), 2, false, "getting_up", GameObject.State.getting_up));

            move_list.addMove(new Animation(new Rectangle(2, 45, 40, 82), 4, true, "idle", GameObject.State.idle));
            move_list.addMove(new Animation(new Rectangle(2, 143, 40, 82), 4, false, "walking", GameObject.State.walking));

            AttackAnimation punch = new AttackAnimation(new Rectangle(4, 236, 63, 87), 3, false, "punch", GameObject.State.punching, 10);
            punch.add_attack(new Rectangle(112, 259, 13, 6), 1);
            move_list.addMove(punch);

            move_list.addMove(new Animation(new Rectangle(3, 437, 74, 62), 2, false, "ko_leap", GameObject.State.KO_leap));
            move_list.addMove(new Animation(new Rectangle(3, 437, 74, 62), 2, false, "ko_leap", GameObject.State.death_leap));

            move_list.addMove(new Animation(new Rectangle(3, 341, 45, 83), 2, false, "getting_hit", GameObject.State.dying));

            add_move_set("spritesheets//customer_girl", move_list);
        }

        void Load_Accountant()
        {
            AnimationCollection move_list = new AnimationCollection();

            Animation walking = new Animation(new Rectangle(0, 0, 70, 90), 4, true, "walking", Character.State.walking);
            //walking.add_weapon_data(new WeaponData(new Point(31, 52), 2), 0);
            //walking.add_weapon_data(new WeaponData(new Point(108, 50), 1), 1);
            //walking.add_weapon_data(new WeaponData(new Point(169, 52), 2), 2);
            //walking.add_weapon_data(new WeaponData(new Point(233, 54), 2), 3);
            
            move_list.addMove(walking);

            Animation idle = new Animation(new Rectangle(300, 0, 70, 90), 4, true, "idle", Character.State.idle);
            //idle.add_weapon_data(new WeaponData(new Point(321, 54), 2), 0);
            //idle.add_weapon_data(new WeaponData(new Point(392, 52), 2), 1);
            //idle.add_weapon_data(new WeaponData(new Point(465, 51), 2), 2);
            //idle.add_weapon_data(new WeaponData(new Point(533, 52), 2), 3);

            move_list.addMove(idle);

            Animation jump = new Animation(new Rectangle(1170, 0, 70, 90), 3, false, "jumping", Character.State.jumping);
            //jump.add_weapon_data(new WeaponData(new Point(1206, 61), 3), 0);
            //jump.add_weapon_data(new WeaponData(new Point(1279, 35), 2), 1);
            //jump.add_weapon_data(new WeaponData(new Point(1349, 24), 1), 2);

            move_list.addMove(jump);

            move_list.addMove(new Animation(new Rectangle(0, 0, 70, 90), 4, true, "force_walk", Character.State.force_walk));
            move_list.addMove(new Animation(new Rectangle(1170, 0, 70, 90), 1, false, "rebound", Character.State.rebound));

            move_list.addMove(new Animation(new Rectangle(0, 110, 90, 90), 2, false, "death_leap", Character.State.death_leap));
            move_list.addMove(new Animation(new Rectangle(0, 110, 90, 90), 2, false, "ko_keap", Character.State.KO_leap));

            move_list.addMove(new Animation(new Rectangle(200, 110, 90, 90), 1, true, "dead", Character.State.dead));
            move_list.addMove(new Animation(new Rectangle(200, 110, 90, 90), 1, true, "ko", Character.State.KO));

            move_list.addMove(new Animation(new Rectangle(310, 110, 90, 90), 2, false, "getting_up", Character.State.getting_up));
            move_list.addMove(new Animation(new Rectangle(600, 0, 70, 90), 2, false, "dying", Character.State.dying));

            move_list.addMove(new Animation(new Rectangle(400, 110, 90, 90), 1, false, "item_get", GameObject.State.item_get));
            move_list.addMove(new Animation(new Rectangle(0, 350, 90, 90), 2, false, "grab_kick", GameObject.State.grab_kick));
            move_list.addMove(new Animation(new Rectangle(490, 350, 90, 90), 1, true, "back_grabbed", GameObject.State.back_grabbed));
            move_list.addMove(new Animation(new Rectangle(400, 350, 90, 90), 1, true, "front_grabbed", GameObject.State.front_grabbed));


            List<Rectangle> bound_list = new List<Rectangle>(7);

            //First five frames from suplex
            bound_list.Add(new Rectangle(510, 110, 90, 90));
            bound_list.Add(new Rectangle(600, 110, 90, 90));
            bound_list.Add(new Rectangle(690, 110, 90, 90));
            bound_list.Add(new Rectangle(780, 110, 90, 90));
            bound_list.Add(new Rectangle(870, 110, 90, 90));
            //Last two frames from brawl_attack;
            bound_list.Add(new Rectangle(1730, 220, 90, 110));
            bound_list.Add(new Rectangle(1820, 220, 90, 110));

            move_list.addMove(new Animation(bound_list, false, Character.State.suplex));

            List<Rectangle> right_bunch_bl = new List<Rectangle>(3);
            right_bunch_bl.Add(new Rectangle(850, 0, 70, 90));
            right_bunch_bl.Add(new Rectangle(920, 0, 70, 90));
            right_bunch_bl.Add(new Rectangle(850, 0, 70, 90));

            List<Rectangle> thrown_bounds = new List<Rectangle>(3);
            thrown_bounds.Add(new Rectangle(400, 350, 90, 90));
            thrown_bounds.Add(new Rectangle(0, 110, 90, 90));
            thrown_bounds.Add(new Rectangle(90, 110, 90, 90));

            //move_list.addMove(new Animation(thrown_bounds, false, GameObject.State.being_thrown));
            move_list.addMove(new Grab(thrown_bounds, GameObject.State.being_thrown, false, new Point(477, 381)));

            AttackAnimation right_punch = new AttackAnimation(right_bunch_bl, false, Character.State.right_punch, 10, "pah", 4);

            AttackAnimation punch           = new AttackAnimation(new Rectangle(760, 0, 70, 90), 1, false, "punching", Character.State.punching, 10, "psh", 4);
            AttackAnimation kick            = new AttackAnimation(new Rectangle(980, 110, 90, 90), 4, false, "kicking", Character.State.kicking, 20, "pah", 4);
            AttackAnimation lunging_attack  = new AttackAnimation(new Rectangle(1360, 110, 90, 90), 6, false, "lunging_attack", Character.State.lunging_attack, 20, "pah", 4);
            AttackAnimation circle_kick     = new AttackAnimation(new Rectangle(0, 220, 90, 110), 8, false, "circle_kick", Character.State.circle_kick, 10, "pah", 2);
            AttackAnimation brawl_attack    = new AttackAnimation(new Rectangle(740, 220, 90, 110), 13, false, "brawl_attack", Character.State.brawl_attack, 10, "pah", 3);
            AttackAnimation jump_kick       = new AttackAnimation(new Rectangle(200, 350, 90, 90), 2, false, "jump_kick", GameObject.State.jump_kick1, 10);
            AttackAnimation back_attack     = new AttackAnimation(new Rectangle(600, 350, 90, 90), 4, false, "back_attack", GameObject.State.back_attack, 15);
            AttackAnimation weapon_attack   = new AttackAnimation(new Rectangle(0, 460, 90, 110), 4, false, "weapon_attack", GameObject.State.weapon_attack, 25, "pipe_hit", 4);

            Grab front_grab = new Grab(new Rectangle(1010, 0, 70, 90), 1, Character.State.front_grab, true, new Point(1068, 30));
            Grab back_grab = new Grab(new Rectangle(1080, 0, 70, 90), 1, Character.State.back_grab, true, new Point(1140, 23));

            punch.add_attack(new Rectangle(808, 25, 20, 8), 0);

            right_punch.add_attack(new Rectangle(970, 28, 19, 8), 1);

            kick.add_attack(new Rectangle(1221, 148, 23, 13), 2);
            kick.add_attack(new Rectangle(1309, 141, 25, 12), 3, true);

            lunging_attack.add_attack(new Rectangle(1695, 135, 24, 38), 3, true);
            lunging_attack.add_attack(new Rectangle(1784, 135, 24, 38), 4, true);

            circle_kick.add_attack(new Rectangle(147, 283, 23, 12), 1, true);
            circle_kick.add_attack(new Rectangle(237, 256, 29, 18), 2, true);
            circle_kick.add_attack(new Rectangle(311, 243, 28, 15), 3, true);
            circle_kick.add_attack(new Rectangle(399, 242, 27, 14), 4, true);
            circle_kick.add_attack(new Rectangle(476, 229, 36, 13), 5, true);
            circle_kick.add_attack(new Rectangle(546, 241, 16, 49), 6, true);

            brawl_attack.add_attack(new Rectangle(796, 269, 21, 8), 0);
            brawl_attack.add_attack(new Rectangle(985, 261, 16, 9), 2);
            brawl_attack.add_attack(new Rectangle(1161, 265, 19, 10), 4);
            brawl_attack.add_attack(new Rectangle(1524, 238, 12, 27), 8, true);

            jump_kick.add_attack(new Rectangle(349, 395, 24, 13), 1, true);

            back_attack.add_attack(new Rectangle(693, 426, 23, 11), 1);
            back_attack.add_attack(new Rectangle(804, 419, 19, 10), 2);

            weapon_attack.add_attack(new Rectangle(160, 520, 55, 8), 1, true);
            weapon_attack.add_weapon_data(new WeaponData(new Point(16, 484), 7), 0);
            weapon_attack.add_weapon_data(new WeaponData(new Point(161, 525), 2), 1);
            weapon_attack.add_weapon_data(new WeaponData(new Point(236, 539), 3), 2);
            weapon_attack.add_weapon_data(new WeaponData(new Point(285, 540), 5), 3);

            move_list.addMove(punch);
            move_list.addMove(right_punch);
            move_list.addMove(kick);
            move_list.addMove(lunging_attack);
            move_list.addMove(circle_kick);
            move_list.addMove(brawl_attack);

            move_list.addMove(back_grab);
            move_list.addMove(front_grab);
            move_list.addMove(jump_kick);
            move_list.addMove(back_attack);
            move_list.addMove(weapon_attack);

            add_move_set("spritesheets//accountant", move_list);
        }

    }
}
