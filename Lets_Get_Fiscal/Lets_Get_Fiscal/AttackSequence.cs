using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lets_Get_Fiscal
{
    //A list of AttackSequences defines how the AI attacks
    //Stelven the accountant.
    public class AttackSequence
    {
        public Character.State state;
        public int amount;

        public AttackSequence(Character.State state, int amount)
        {
            this.state = state;
            this.amount = amount;
        }

        public AttackSequence(Character.State state)
        {
            this.state = state;
            this.amount = 1;
        }

        public AttackSequence(AttackSequence AS)
        {
            this.state = AS.state;
            this.amount = AS.amount;
        }

        public void decrement_amount()
        {
            amount--;
        }
    }
}
