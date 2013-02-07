using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lets_Get_Fiscal
{
    public struct HitData
    {
        public int id;
        public int frame_num;
        public uint attack_id;
        public GameObject.State state;

        public HitData(int id, int frame_num, uint attack_id, GameObject.State state)
        {
            this.id = id;
            this.frame_num = frame_num;
            this.attack_id = attack_id;
            this.state = state;
        }

        public static bool operator ==(HitData x, HitData y)
        {
            return x.id == y.id && x.frame_num == y.frame_num && x.state == y.state && x.attack_id == y.attack_id;
        }

        public static bool operator !=(HitData x, HitData y)
        {
            return x.id != y.id || x.frame_num != y.frame_num || x.state != y.state || x.attack_id != y.attack_id;
        }

        public override bool Equals(Object obj)
        {
            return obj is HitData && this == (HitData)obj;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
