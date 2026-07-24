using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lets_Get_Fiscal
{
    class MenuData
    {
        //public int num_options;
        public List<string> menu_text;
        public int selected_index = 0;
        public List<bool> visible;
        
        //public bool buy_game_visible = true;

        public MenuData()
        {
            //this.num_options = num_options;
            menu_text = new List<string>(5);
            visible = new List<bool>(menu_text.Count);

            for (int i = 0; i < 5; i++)
                visible.Add(true);
        }

        public int num_options
        {
            get { return menu_text.Count; }
        }

        public void move_up()
        {
            if (selected_index > 0)
            {
                selected_index--;
            }

            if (visible[selected_index] == false)
                move_up();
        }

        public void move_down()
        {
            if (selected_index < num_options - 1)
                selected_index++;

            if (visible[selected_index] == false)
                move_down();
        }
    }
}
