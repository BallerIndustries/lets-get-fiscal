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
    //This class was created by LORD ANGUS CHENG after he walked from
    //Aberdeen to The Cricket Club. Also known as a stage of the Hong Kong Trail.
    //The purpose of this object is to be a whole bunch of software code. It describes
    //a trash can, or a box, or something that can be smashed open to reveal an item, a weapon
    //or maybe just nothing
    public class Prop : GameObject
    {
        public Item.Type containee; //The thing contained in this container.
        Rectangle in_tact;
        Rectangle fucked;
        int flash_for = 30; // describes how long the container will be in the flashing state.
        bool attacked_from_left;
        
        public new Rectangle bound
        {
            get
            {
                if (state == State.idle) return in_tact;
                else if (visible)
                    return fucked;
                else
                    return new Rectangle(1, 1, 1, 1);
            }
        }

        public enum PropType
        {
            club_table,
            stool,
            vip_table,
            tall_lamp,
            filing_cabinet,
            swivel_chair,
            recycling_bin,
            frozen_crate,
            thawed_crate,
            bifo_bin
        }

        public Prop(Singletons singletons, Point position, PropType type, Item.Type containee, float scale) : base("props", singletons)
        {
            this.sheet = "props";
            this.texture = tm.find_texture(sheet);
            this.scale = scale;
            this.containee = containee;

            set_animation(type);
            posX = position.X;
            posY = position.Y;
        }

        public Prop(Singletons singletons, Point position, PropType type, Item.Type containee)
            : base("props", singletons)
        {
            this.sheet = "props";
            this.texture = tm.find_texture(sheet);
            this.scale = 3f;
            this.containee = containee;

            set_animation(type);
            posX = position.X;
            posY = position.Y;
        }

        //public Prop(string sheet, Singletons singletons, GameObject containee, Rectangle in_tact, Rectangle fucked, Point position, float scale = 3f) : base(sheet, singletons)
        //{
        //    this.containee = containee;
        //    this.texture = tm.find_texture(sheet);
        //    this.in_tact = in_tact;
        //    this.fucked = fucked;
        //    this.scale = scale;

        //    posX = position.X;
        //    posY = position.Y;

        //    move_list = new AnimationCollection();
        //    move_list.addMove(new Animation(in_tact, 1, true, "in_tact", State.in_tact));
        //    move_list.addMove(new Animation(fucked, 1, true, "broken", State.broken));

        //    this.state = State.in_tact;
        //}

        //Copy constructor. Why not do something useful instead of just
        //copying other objects you LAZY copy constructor!
        public Prop(Prop p) : base(p.sheet, p.singletons, p.scale)
        {
            this.containee = p.containee;
            this.texture = p.texture;
            this.in_tact = p.in_tact;
            this.fucked = p.fucked;
            this.scale = p.scale;
            posX = p.posX;
            posY = p.posY;

            this.move_list = p.move_list;
            this.state = p.state;
        }

        //This should be really fucking simple. There are three states
        //1. Unharmed container, a container that is just sitting there doing nothing
        //2. Container that has just been smashed open
        //3. No more container, don't draw ANYTHING
        public void Update()
        {
            if (state == State.broken)
            {
                do_flashing();
                flash_for--;

                if (attacked_from_left)
                    posX -= 2;
                else
                    posX += 2;

                if (flash_for == 0)
                {
                    visible = false;
                    state = State.none;
                }
            }
        }

        public void start_breaking(bool attacked_from_left)
        {
            state = State.broken;
            this.attacked_from_left = attacked_from_left;
        }

        public void do_flashing()
        {
            if (moves >= 3)
            {
                moves = 0;
                visible = !visible;
            }
            else
            {
                moves++;
            }
        }

        public void set_animation(PropType type)
        {
            switch (type)
            {
                case PropType.club_table:
                    in_tact = new Rectangle(11, 62, 62, 77);
                    fucked = new Rectangle(79, 61, 62, 82);
                    break;

                case PropType.stool:
                    in_tact = new Rectangle(149, 86, 30, 54);
                    fucked = new Rectangle(183, 88, 26, 52);
                    break;

                case PropType.vip_table:
                    in_tact = new Rectangle(216, 80, 38, 59);
                    fucked = new Rectangle(260, 76, 26, 56);
                    break;

                case PropType.tall_lamp:
                    in_tact = new Rectangle(291, 49, 25, 96);
                    fucked = new Rectangle(333, 51, 32, 92);
                    break;

                case PropType.filing_cabinet:
                    in_tact = new Rectangle(364, 13, 52, 136);
                    fucked = new Rectangle(425, 15, 52, 133);
                    break;

                case PropType.swivel_chair:
                    in_tact = new Rectangle(18, 160, 41, 67);
                    fucked = new Rectangle(64, 159, 37, 68);
                    break;

                case PropType.recycling_bin:
                    in_tact = new Rectangle(108, 159, 30, 67);
                    fucked = new Rectangle(142, 176, 33, 49);
                    break;

                case PropType.frozen_crate:
                    in_tact = new Rectangle(8, 237, 86, 93);
                    fucked = new Rectangle(103, 237, 85, 98);
                    break;

                case PropType.thawed_crate:
                    in_tact = new Rectangle(191, 236, 88, 98);
                    fucked = new Rectangle(284, 235, 84, 102);
                    break;

                case PropType.bifo_bin:
                    in_tact = new Rectangle(374, 255, 53, 74);
                    fucked = new Rectangle(434, 253, 55, 76);
                    break;
                    

                //case PropType.orange_bin:
                //    in_tact = new Rectangle(11, 19, 33, 65);
                //    fucked = new Rectangle(60, 20, 32, 65);
                //    break;

                //case PropType.crate:
                //    in_tact = new Rectangle(132, 25, 33, 59);
                //    fucked = new Rectangle(183, 22, 32, 63);
                //    break;

                //case PropType.table:
                //    in_tact = new Rectangle(323, 32, 66, 54);
                //    fucked = new Rectangle(406, 31, 59, 56);
                //    break;

                //case PropType.chair:
                //    in_tact = new Rectangle(501, 29, 34, 58);
                //    fucked = new Rectangle(501, 29, 34, 58);
                //    break;
            }

            move_list = new AnimationCollection();
            move_list.addMove(new Animation(in_tact, 1, true, "in_tact", State.in_tact));
            move_list.addMove(new Animation(fucked, 1, true, "broken", State.broken));

            this.state = State.in_tact;
        }
    }
}
