using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
//using System.Timers;
using System.Diagnostics;

namespace Lets_Get_Fiscal
{
//#if XBOX

public static class PlayerIndexExtensions
{
    public static bool can_buy_game(this PlayerIndex player)
    {
#if XBOX
        SignedInGamer gamer = Gamer.SignedInGamers[player];

        if (gamer == null)
            return false;

        if (!gamer.IsSignedInToLive)
            return false;

        return gamer.Privileges.AllowPurchaseContent;
#else
        return false;
#endif
    }
}

//#endif
}
