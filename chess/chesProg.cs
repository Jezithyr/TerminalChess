using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ConsoleGames
{
    struct GridPos
    {
        public byte x;
        public byte y;
        public GridPos(byte a, byte b)
        {
            x = a;
            y = b;
        }

        public GridPos(int a, int b)
        {
            x =(byte) a;
            y =(byte) b;
        }

        public byte[] GetPos()
        {
            return (new byte[] { x, y });
        }

        public static bool operator ==(GridPos grid1, GridPos grid2)
        {
            return ((grid1.x == grid2.x) && (grid1.y == grid2.y));
        }
        public static bool operator !=(GridPos grid1, GridPos grid2)
        {
            return ((grid1.x != grid2.x) && (grid1.y != grid2.y));
        }
        public override bool Equals(object other)
        {
            if (!(other is GridPos))
                return false;
            GridPos otherGrid = (GridPos)other; //cast the object to a gamepiece struct

            if ((x == otherGrid.x) && (y == otherGrid.y))
            {
                return true;
            }
            return false;
        }


    }

    struct GamePiece
    {
        char icon;
        ConsoleColor iconColor;
        byte team;
        ConsoleColor highlight;

        public GamePiece(char startIcon)
        {
            icon = startIcon;
            iconColor = ConsoleColor.Gray;
            highlight = ConsoleColor.Black;
            team = 0;
        }

        public GamePiece(char startIcon, byte teamID)
        {
            icon = startIcon;
            team = teamID;
            highlight = ConsoleColor.Black;
            switch (teamID)
            {
                case 0:
                    {
                        iconColor = ConsoleColor.Gray;
                        break;
                    }
                case 1:
                    {
                        iconColor = ConsoleColor.Red;
                        break;
                    }
                case 2:
                    {
                        iconColor = ConsoleColor.Blue;
                        break;
                    }
                default:
                    {
                        iconColor = ConsoleColor.Gray;
                        break;
                    }
            }
        }

        public byte GetTeam()
        {
            return team;
        }

        public ConsoleColor GetHighlight()
        {
            return highlight;
        }

        public void SetHighlight(ConsoleColor newColor)
        {
            highlight = newColor;
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

    class Core
    {
        protected static Gameboard Game;
        static Player player1;
        static Player player2;
        static Player startPlayer;
        static bool isStartPlayerTurn;

        static List<object> PromptForMove(Player activePlayer)
        {
            List<object> temp = new List<object> { false };
            Console.WriteLine("\n");
            Console.WriteLine(activePlayer.GetName() + "'s Turn. Pick a piece by typing it's position or surrender by typing SURRENDER");
            String userInput = Console.ReadLine();

            //surrender flag goes here

            int[] SelectionPos = Game.ParseGridPos(userInput);

            if ((SelectionPos[0] < 0)|| (SelectionPos[1] < 0))
            {
                Console.WriteLine("Invalid input, Please try again.");
            }
            else
            {

                GridPos selectedPos = new GridPos(SelectionPos[0], SelectionPos[1]);
                temp.Add(selectedPos);
                temp.Add(Game.GetPiece(selectedPos));
                temp[0] = true;
            }
            return temp;
        }

        public bool CheckInput(String userInput,String[] options)
        {
            if (userInput.Length == 0)
            {
                return false;
            }

            foreach (String option in options)
            {
               if (userInput.ToLower().Equals(option))
                {
                    return true;
                }
            }
            return false;
        }


        static protected Player Intro()
        {
            Console.WriteLine("\n\nWelcome to CHESS! Press any key to start...");
            //Console.ReadLine();
            Console.Clear();
            Console.Write("\n\nChess is a TWO player game, teams are symbolized with the colors ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Red");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" and ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Blue\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("The board is divided into an 8 by 8 grid labeled with letters and numbers.\n");
            Console.WriteLine("To move a piece, simply type in the letter and number when prompted.");
            Console.Write("Once you have selected a piece, you can make a move onto any ");
            Console.BackgroundColor = ConsoleColor.Green;
            Console.Write("Green Highlighted");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine(" Tile.");
            Console.WriteLine("\nLet's begin! Player One,\n Please Enter your name:");
            //String player1Name = Console.ReadLine();

            String player1Name = "test1";

            Console.WriteLine("\n Player One is: " + player1Name);

            player1 = new Player(player1Name,1);

            //Thread.Sleep(1000);
            Console.Clear();

            Console.WriteLine("Player Two,\n Please Enter your name:");
            //String player2Name = Console.ReadLine();

            String player2Name = "test1";

            Console.WriteLine("\n Player Two is: " + player2Name);

            player2 = new Player(player2Name, 2);

            //Thread.Sleep(1000);
            Console.Clear();

            Player startPlayer = null;
            String userInput;

            userInput = "1";

            bool invalidInput = true;
            int userSelection = 0;
            while (invalidInput)
            {
                Console.WriteLine("Who would like to start? 1 for Player One, 2 for Player Two, and 3 for random:");
                //userInput = Console.ReadLine();
                invalidInput = !(Int32.TryParse(userInput, out userSelection));
                if (userSelection > 3 || userSelection < 1) { invalidInput = true; };
                if (invalidInput)
                {
                    Console.WriteLine("Invalid input please try again!");
                    //Thread.Sleep(1000);
                    Console.Clear();
                }
            }
            switch (userSelection)
            {
                case 1:
                    {
                        startPlayer = player1;
                        player1.SetTeam(1);
                        player2.SetTeam(2);
                        break;
                    }
                case 2:
                    {
                        startPlayer = player2;
                        player2.SetTeam(1);
                        player1.SetTeam(2);

                        break;
                    }
                case 3:
                    {
                        Random random = new Random();
                        int randomSel = (random.Next())%2;
                        switch (randomSel)
                        {
                            case 0:
                                {
                                    startPlayer = player1;
                                    player1.SetTeam(1);
                                    player2.SetTeam(2);
                                    break;
                                }
                            case 1:
                                {
                                    startPlayer = player2;
                                    player2.SetTeam(1);
                                    player1.SetTeam(2);
                                    break;
                                }
                        }
                        break;
                    }
            }
            return startPlayer;
        }

        static private void StartTurn(Player player)
        {
            bool validMove = false;
            String userInput;
            int[] gridCords;
            int[] targetCords;
            GridPos targetGrid = new GridPos(99,99);
            while (!validMove)
            {
               // Console.Clear();
                Game.DrawBoard();
                Console.Write("\n It's ");
                Console.ForegroundColor = player.GetColor();
                Console.Write(player.GetName() + "'s ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Turn!\n Select a piece to move:");
                userInput = Console.ReadLine();
                bool moveSelection = true;
                gridCords = Game.ParseGridPos(userInput);

                Console.WriteLine(gridCords[0] + ";" + gridCords[1]);

                if (gridCords[0] > 0 || gridCords[1] > 0)//nullcheck the entered coordinates
                {
                    List<GridPos> validMoves = ((Chessboard)Game).GetPossibleMoves(gridCords,player);
                    if (validMoves.Count == 0)
                    {
                        Console.WriteLine("There are no possible moves for that piece try another one.");
                    }else
                    {
                        moveSelection = true;
                        while (moveSelection)
                        {
                            Console.Clear();
                            Game.HighLightGrids(validMoves.ToArray(), ConsoleColor.Green);
                            Game.DrawBoard();
                            Game.HighLightGrids(validMoves.ToArray(), ConsoleColor.Black);
                            Console.WriteLine("Make your move. Possible moves are highlighted in Green.");
                            userInput = Console.ReadLine();
                            targetCords = Game.ParseGridPos(userInput);
                            targetGrid = new GridPos(targetCords[0], targetCords[1]);
                            validMoves.ForEach(x=>
                            {
                                if (x == targetGrid)
                                {
                                    moveSelection = false;
                                }
                            });
                        }
                        Game.Move(new GridPos((byte)gridCords[0], (byte)gridCords[1]), targetGrid, player);
                        Console.Clear();




                    }

                }else
                {
                    Console.WriteLine("Invalid Input, please try again!");
                }


            }





        }












        static void Main(string[] args)
        {

            startPlayer = Intro();
            Player followPlayer = player2;
            if (startPlayer == player2) { followPlayer = player1; };

            Game = new Chessboard();
            isStartPlayerTurn = true;
            bool GameRunning = true;
            while (GameRunning)
            {
                if (isStartPlayerTurn)
                {
                    StartTurn(startPlayer);
                } else
                {
                    StartTurn(followPlayer);

                }
                isStartPlayerTurn = !isStartPlayerTurn;
            }
        }
    }
}
