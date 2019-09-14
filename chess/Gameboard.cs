using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGames
{
    class Gameboard
    {
        protected byte size = 5;
        protected GamePiece[][] grid;
        protected bool gameover = false;
        protected Player winner = null;

        public bool IsGameover()
        {
            return gameover;
        }
        public Player GetWinner()
        {
            return winner;
        }

        public Gameboard(byte inSize)
        {
            if (inSize > 9)//board size cannot be greater than 9 because of the ASCII shortcut for printing the board to the user
            {
                Console.WriteLine("Board size must be smaller than 10, you numpty!");
            }

            size = inSize;
            grid = new GamePiece[size][];
            for (byte index = 0; index < grid.Length; index++)
            {
                grid[index] = new GamePiece[size];
            }


            for (int sizeX = 0; sizeX < size; sizeX++)
            {
                for (int sizeY = 0; sizeY < size; sizeY++)
                {
                    grid[sizeX][sizeY] = new GamePiece('-');
                }
            }
        }

        public void CleanHighlights()
        {
            foreach (GamePiece[] row in grid)
            {
                foreach (GamePiece piece in row)
                {
                    piece.SetHighlight(ConsoleColor.Black);
                }

            }
        }

        public void HighLightGrids(GridPos[] gridPositions,ConsoleColor highlight)
        {
            foreach (GridPos curGrid in gridPositions)
            {
                (grid[curGrid.x][curGrid.y]).SetHighlight(highlight);
            }

        }


        public int[] ParseGridPos(String inputString)
        {
            int[] returnArray = new int[2] { -1, -1 };

            if (inputString == null || inputString.Length != 2)//return an empty array if the input is not 2 chars
            {
                return returnArray;
            }

            inputString = inputString.ToLower();
            
          
            char char1 = inputString[0];
            char char2 = inputString[1];

            //checks to see if the first character is a valid letter
            if ((97 > Convert.ToInt32(char1)) && (Convert.ToInt32(char1) > 122))
            {
                returnArray[0] = -2;
                return returnArray; //if the first character is not a valid letter
            }

            if ((48 > Convert.ToInt32(char2)) && (Convert.ToInt32(char2) > 57))
            {
                returnArray[1] = -2;
                return returnArray; //if the second character is not a valid number
            }

            int colIndex = 0;
            int rowIndex = 0;
            colIndex = Convert.ToInt32(char1) - 97;
            rowIndex = Convert.ToInt32(char2) - 49;

            if (colIndex >= size || rowIndex >= size)
            {
                return returnArray;
            }
            else
            {
                return (new int[2] { colIndex, rowIndex });
            }
        }

        public virtual void Move(GridPos piece1, GridPos piece2, Player player) { }


        //gets the board as a string NOTE: DOES NOT include color!
        public string GetBoardAsString()
        {
            string tempString = " ";
            for (int gridsize = 0; gridsize < size; gridsize++)
            {
                tempString = tempString + " " + (char)(gridsize + 65);

            }

            for (byte sizeY = 0; sizeY < size; sizeY++)
            {
                tempString = tempString + "\n" + (char)(sizeY + 49);
                for (byte sizeX = 0; sizeX < size; sizeX++)
                {
                    tempString = tempString + " " + grid[sizeX][sizeY].GetIcon();
                }
            }
            return tempString;
        }


        public void DrawBoard()
        {
            DrawBoard(new GridPos[] { });
        }

        public void DrawBoard(GridPos[] highlightPositions)
        {
            //headers 
            String tempString = " ";

            //generate the top header
            for (int gridsize = 0; gridsize < size; gridsize++)
            {
                tempString = tempString + " " + (char)(gridsize + 65);
            }
            tempString = tempString + "  ";
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(tempString, Console.BackgroundColor, Console.ForegroundColor);

            for (byte row = 0; row < size; row++)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write(row + 1 + " ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                for (byte col = 0; col < size; col++)
                {
                    GamePiece temp = grid[col][row];

                    Console.BackgroundColor = temp.GetHighlight();
                    Console.ForegroundColor = temp.GetColor();
                    Console.Write(temp.GetIcon() + " ", Console.ForegroundColor, Console.BackgroundColor);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                Console.Write("\n");
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        public GamePiece GetPiece(byte x,byte y)
        {
            return GetPiece(new GridPos(x,y));
        }

        public GamePiece GetPiece(GridPos pos)
        {
          return grid[pos.x][pos.y];
        }

        //searches the grid for the given character and returns the position if found. 
        //Also returns true or false if the search was successful
        public List<object> FindPiece(GamePiece target)
        {
            List<object> temp = new List<object> { false };
            for (byte yIndex = 0; yIndex < size; yIndex++)
            {
                for (byte xIndex = 0; xIndex < size; xIndex++)
                {
                    if (grid[xIndex][yIndex] == target)
                    {
                        temp.Add(new GridPos(xIndex, yIndex));
                        temp[0] = true;
                    }
                }
            }
            return temp;
        }


        //sets the character at the defined position and returns the previous character.
        public GamePiece SetPiece(GridPos pos, GamePiece newchar)
        {
            GamePiece oldPiece = grid[pos.x][pos.y];
            grid[pos.x][pos.y] = newchar;

            return oldPiece;
        }

        protected void SetColumn(byte rowIndex, GamePiece[] newRow)
        {
            if (grid[0].Length != newRow.Length)
            {
                Console.WriteLine("Error: cannot set a smaller new column!");
                return;
            }
            grid[rowIndex] = newRow;
        }

        protected void SetColumn(int rowIndex, GamePiece[] newRow)
        {
            byte temp = (byte)rowIndex;
            SetColumn(temp, newRow);
        }


    }

}
