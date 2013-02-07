using System;

namespace Lets_Get_Fiscal
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
#if XBOX

            try
            {
                using (Game1 game = new Game1())
                    game.Run();
            }
            catch (Exception e)
            {
                using (CrashDebugGame game = new CrashDebugGame(e))
                    game.Run();
            }
#else
            Game1 game = new Game1();
            game.Run();

#endif 

        }
    }
}

