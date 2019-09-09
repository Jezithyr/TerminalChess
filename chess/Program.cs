using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ticktactoe
{
    class gameboard
    {
        
        int size;
        char[,] grid;

        public gameboard(int inSize)
        {

            if (inSize > 9)
            {
               Console.WriteLine("Board size must be smaller than 10 you numpty");
            }

            size = inSize;
            grid = new Char[size, size];


            for (int sizeX = 0; sizeX < size; sizeX++)
            {
                for (int sizeY = 0; sizeY < size; sizeY++)
                {
                    grid[sizeX, sizeY] = '-';
                }
            }
        }

        public string displayboard()
        {
            string tempString = " ";
            for (int gridsize = 0;gridsize < size; gridsize++)
            {
                tempString = tempString + " " + (char)(gridsize + 65);

            }



            for (int sizeY = 0; sizeY < size; sizeY++)
            {
                tempString = tempString + "\n" + (char)(sizeY + 49);
                for (int sizeX = 0; sizeX < size; sizeX++)
                {



                    tempString = tempString + " " + grid[sizeX,sizeY];
                }
            }

            return tempString;
        }


    }





    class core
    { 
        static void Main(string[] args)
        {
            gameboard testGame = new gameboard();
            Console.WriteLine(testGame.displayboard());
            Console.ReadLine();
        }


    }
}
