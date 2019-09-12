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
        private byte team;
        protected String name;
        private int score;

        public Player(String newName)
        {
            name = newName;
            team = 0;
            color = ConsoleColor.Black;
            score = 0;
        }

        public Player(String newName,int teamId)
        {
            name = newName;
            team = (byte)teamId;
            score = 0;
            switch (teamId)
            {
                case 0:
                    {
                        color = ConsoleColor.Gray;
                        break;
                    }
                case 1:
                    {
                        color = ConsoleColor.Red;
                        break;
                    }
                case 2:
                    {
                        color = ConsoleColor.Blue;
                        break;
                    }
                default:
                    {
                        color = ConsoleColor.Black;
                        break;
                    }
            }

        }

        public ConsoleColor GetColor()
        {
            return color;
        }
        public String GetName()
        {
            return name;
        }

        public byte GetTeam()
        {
            return team;
        }

        public void SetTeam(byte newTeam)
        {
            team = newTeam;
        }

        public void SetTeam(int newTeam)
        {
            team = (byte)newTeam;
        }

    }
}
