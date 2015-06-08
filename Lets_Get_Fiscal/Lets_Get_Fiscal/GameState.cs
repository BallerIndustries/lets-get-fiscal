using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lets_Get_Fiscal
{
    public class GameState
    {
        public enum State
        {
            Menu,
            InGameMenu,
            GamePlay,
            CutScene,
            ActOver,
            StartPrompt,
            //Controls,
            LevelAnnounce,
            GameOver,
            DemoOver,
            LanguageWarning,
            //ComboExplain,
            Logo
        }

        private CutScene _current_cs;
        public CutScene current_cs
        {
            get { return _current_cs; }
            set 
            {   
                _current_cs = value;
                //Also find the starting comic
                _current_cs.load_action.Invoke();
            }
        }

        public LevelAnnounce current_level_announce;
        public StageCleared current_stage_cleared;

        private State _state;
        public State prev_state;
        public State state
        {
            get { return _state; }
            set { prev_state = _state; _state = value; }
        }
        public Level current_level;
        public Act current_act;
        public int score;

        public GameState(State state)
        {
            this.state = state;
        }

        public void Initialise(Level current_level, Act current_act)
        {
            this.current_level = current_level;
            this.current_act = current_act;
            current_act.act_over = false;
            score = 0;
            current_act.wave_num = 0;
        }

       

    }
}
