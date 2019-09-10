using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGames
{
    struct GridPos
    {
        byte x;
        byte y;
        public GridPos(byte a, byte b)
        {
            x = a;
            y = b;
        }

        public byte[] GetPos()
        {
            return (new byte[] { x, y });
        }

        public byte GetX()
        {
            return x;
        }
        public byte GetY()
        {
            return y;
        }

    }

    struct GamePiece
    {
        char icon;
        ConsoleColor iconColor;

        public GamePiece(char startIcon)
        {
            icon = startIcon;
            iconColor = ConsoleColor.Gray;
        }


        public GamePiece(char startIcon, ConsoleColor startColor)
        {
            icon = startIcon;
            iconColor = startColor;
        }

        public char GetIcon()
        {
            return icon;
        }

        public ConsoleColor GetColor()
        {
            return iconColor;
        }

        //equality operators
        public static bool operator ==(GamePiece peice1, GamePiece peice2)
        {
            return ((peice1.GetColor() == peice2.GetColor()) && (peice1.GetIcon() == peice2.GetIcon()));
        }
        public static bool operator !=(GamePiece peice1, GamePiece peice2)
        {
            return !((peice1.GetColor() == peice2.GetColor()) && (peice1.GetIcon() == peice2.GetIcon()));
        }
        public override bool Equals(object other)
        {
            if (!(other is GamePiece))
                return false;
            GamePiece otherPiece = (GamePiece)other; //cast the object to a gamepiece struct

            if ((icon == otherPiece.GetIcon()) && (iconColor == otherPiece.GetColor()))
            {
                return true;
            }
            return false;
        }
    }

    class Gameboard
    {
        protected byte size = 5;
        protected GamePiece[][] grid;



        public Gameboard(byte inSize)
        {
            if (inSize > 9)//board size cannot be greater than 9 because of the ASCII shortcut for printing the board to the user
            {
                Console.WriteLine("Board size must be smaller than 10, you numpty!");
            }

            size = inSize;
            grid = new GamePiece[size][];
            for (byte index = 0; index < grid.Length;index++)
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



        public int[] ParseGridPos(String inputString)
        {
            int[] returnArray = new int[2];
            if (inputString.Length > 2)
            {
                return returnArray; //return an empty array if the input is less than 2 chars
            }
            char char1 = inputString[0];
            char char2 = inputString[1];

            //checks to see if the first character is a valid letter
            if ((97 > Convert.ToInt32(char1)) && (Convert.ToInt32(char1) > 122))
            {
                return returnArray; //if the first character is not a valid letter
            }

            if ((49 > Convert.ToInt32(char2)) && (Convert.ToInt32(char2) > 57))
            {
                return returnArray; //if the second character is not a valid number
            }

            if ((Convert.ToInt32(char1) - 97) < size)


        }




        //gets the board as a string NOTE: DOES NOT include color!
        public string GetBoardAsString()
        {
            string tempString = " ";
            for (int gridsize = 0;gridsize < size; gridsize++)
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
            //headers 
            String tempString = " " ;

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

                    Console.ForegroundColor = temp.GetColor();
                    Console.Write(temp.GetIcon()+" ", Console.ForegroundColor);
                }
                Console.Write("\n");
            }
            Console.ForegroundColor = ConsoleColor.White;
        }



            public GamePiece GetPiece(GridPos pos)
        {
            return grid[pos.GetX()][ pos.GetY()];
        }

        //searches the grid for the given character and returns the position if found. 
        //Also returns true or false if the search was successful
        public List<object> FindPiece(GamePiece target)
        {
            List<object> temp = new List<object> {false};
            for (byte yIndex= 0; yIndex < size; yIndex++)
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
            GamePiece oldPiece = grid[pos.GetX()][pos.GetY()];
            grid[pos.GetX()][pos.GetY()] = newchar;

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


    class Chessboard : Gameboard
    {
        public Chessboard() : base(8)
        {
            Setup();
        }
        protected void Setup()
        {
            Char[] PieceIcons = { 'R', 'N', 'B', 'K', 'Q', 'P'};
            ConsoleColor[] PlayerColors = { ConsoleColor.DarkRed, ConsoleColor.Blue };
            GamePiece BlankPiece = new GamePiece('-');


            GamePiece[] PieceList = new GamePiece[12];
            for (int colorIndex = 0; colorIndex < 2; colorIndex++)
            {
                for (int pieceIndex = 0; pieceIndex < 6; pieceIndex++)
                {
                    PieceList[pieceIndex + (colorIndex * 6)] = new GamePiece(PieceIcons[pieceIndex], PlayerColors[colorIndex]);
                }
            }
            for (byte index = 0; index < 5; index++)
            {
                SetColumn(index, new GamePiece[] { PieceList[0+index], PieceList[5], BlankPiece, BlankPiece, BlankPiece, BlankPiece, PieceList[11], PieceList[6+index] });
            }
            for (int index = 0; index < 3; index++)
            {
                SetColumn(index+5, new GamePiece[] { PieceList[2 +(-index)], PieceList[5], BlankPiece, BlankPiece, BlankPiece, BlankPiece, PieceList[11], PieceList[8 + (-index)] });
            }
        }
    }

    class TicTackToe : Gameboard
    {
        public TicTackToe() : base(3)
        {
           

        }

    }


    class core
    { 

        static void Main(string[] args)
        {
            Chessboard testGame = new Chessboard();
            testGame.DrawBoard();

            Console.ReadLine();
        }


    }
}
