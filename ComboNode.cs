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
    public class ComboNode// : IComparable
    {
        public Character.State action;
        public UInt64 time;
        public bool did_hit = false;

        public ComboNode(UInt64 time, Character.State action)
        {
            this.time = time;
            this.action = action;
        }

        public int CompareTo(object obj)
        {
            ComboNode cn = (ComboNode)obj;

            if (cn.action == action)
                return 0;
            else
                return -1;
        }

        //public override bool Equals(Object obj)
        //{
        //    //Check for null and compare run-time types.
        //    if (obj == null || GetType() != obj.GetType()) return false;
        //    ComboNode cn = (ComboNode)obj;
        //    return (action == cn.action);
        //}
    }
}
