using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lets_Get_Fiscal
{
    public class CircularBuffer
    {
        ComboNode[] previous_moves = new ComboNode[20];
        int index = 0;
        int count = 0;

        public CircularBuffer()
        {
        }

        public void Clear()
        {
            index = 0;
            count = 0;

            for (int i = 0; i < 20; i++)
                previous_moves[i] = null;
        }

        public void add_node(ComboNode cn)
        {
            if (index > previous_moves.Length - 1)
                index = 0;

            previous_moves[index] = cn;

            count++;
            index++;
        }

        public ComboNode get_from_tail(int offset)
        {
            if (offset > previous_moves.Length || count - offset < 0)
                return null;
            else
                return previous_moves[(count - offset) % 20 ];
        }

        public void last_move_hit()
        {
            ComboNode tail = get_from_tail(1);
            if (tail != null)
                tail.did_hit = true;
        }
    }
}
