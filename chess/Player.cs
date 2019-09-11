using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGames
{
    
    class Player
    {

        protected ConsoleColor color;
        protected byte team;
        protected String name;
        protected int score;
        protected bool isTurn;//important for win condition

        public Player(String newName)
        {
            name = newName;
            team = 0;
            color = ConsoleColor.Black;
            score = 0;
            isTurn = false;
        }


        public String GetName()
        {
            return name;
        }


    }
}
