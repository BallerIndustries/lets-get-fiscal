using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lets_Get_Fiscal
{
    public class AnimationCollection
    {
        public List<Animation> moves = new List<Animation>(10);
        public List<AttackAnimation> attacks = new List<AttackAnimation>(10);

        public AnimationCollection()
        {
        }

        public void addMove(Animation a)
        {
            moves.Add(a);
            AttackAnimation aa = a as AttackAnimation;

            if (aa != null)
            {
                attacks.Add(aa);
            }
        }

        public Animation getMove(String name)
        {
            foreach (Animation an in moves)
            {
                if (an.name == name)
                    return an;
            }

            return null;
        }

        public Animation getMove(Character.State move)
        {
            foreach (Animation an in moves)
            {
                if (an.move == move)
                    return an;
            }

            return null;
        }


    }
}
