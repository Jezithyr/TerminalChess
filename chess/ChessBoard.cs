using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGames
{

    class Chessboard : Gameboard
    {
        public Chessboard() : base(8)
        {
            Setup();
        }
        protected void Setup()
        {
            Char[] PieceIcons = { 'R', 'N', 'B', 'K', 'Q', 'P' };
            GamePiece BlankPiece = new GamePiece('-');

            GamePiece[] PieceList = new GamePiece[12];
            for (byte colorIndex = 0; colorIndex < 2; colorIndex++)
            {
                for (int pieceIndex = 0; pieceIndex < 6; pieceIndex++)
                {
                    PieceList[pieceIndex + (colorIndex * 6)] = new GamePiece(PieceIcons[pieceIndex], (byte)(colorIndex + 1));
                }
            }
            for (byte index = 0; index < 5; index++)
            {
                SetColumn(index, new GamePiece[] { PieceList[0 + index], PieceList[5], BlankPiece, BlankPiece, BlankPiece, BlankPiece, PieceList[11], PieceList[6 + index] });
            }
            for (int index = 0; index < 3; index++)
            {
                SetColumn(index + 5, new GamePiece[] { PieceList[2 + (-index)], PieceList[5], BlankPiece, BlankPiece, BlankPiece, BlankPiece, PieceList[11], PieceList[8 + (-index)] });
            }
        }




        protected bool[] HorizontalMovementCheck(GridPos curPos, byte pieceTeam)
        {
            //temporary array used for processing positions
            bool[] validRowSpotsTemp = { false, false, false, false, false, false, false, false };

            //go through the column and check to see if there is a free space
            for (byte RowIndex = 0; RowIndex < 8; RowIndex++)
            {
                if (GetPiece(RowIndex, curPos.y).GetTeam() == 0)
                {
                    validRowSpotsTemp[RowIndex] = true;
                }
            }

            return CheckValidLine(validRowSpotsTemp, curPos, pieceTeam,false);
        }


        protected bool[] DiagonalMovementCheck(GridPos curPos, byte pieceTeam,bool isGoingUp)
        {
            //temporary array used for processing positions
            bool[] validSpotsTemp = { false, false, false, false, false, false, false, false };

            byte curX = curPos.x;
            byte curY = curPos.y;
            bool swapcheck = false;//swaps between the direction of the check
            bool checking = true;

            while (checking)
            {
                if (curX < 0 || curY < 0)
                {
                    checking = false;//break out of the loop if either index is out of bounds
                }

                if (curX >= 7 || curY >= 7)//if either of the indices are out of bounds swap the direction of the check
                {
                    Console.WriteLine("SWAPPING");
                    swapcheck = true;
                    curX = curPos.x;
                    curY = curPos.y;
                }
                else
                {
                   if (GetPiece(curX, curY).GetTeam() == 0)
                   {
                      validSpotsTemp[curX] = true;
                   }
                }
                Console.WriteLine(swapcheck);
                Console.WriteLine(curX);

                if (swapcheck)
                {
                    curX++;
                    curY++;
                }
                else
                {

                    curX= (byte)(curX - 1);
                    curY= (byte)(curY - 1);
                }
            }



            return validSpotsTemp;//test return


            return CheckValidLine(validSpotsTemp, curPos, pieceTeam, false,true);
        }



        protected bool[] VerticalMovementCheck(GridPos curPos, byte pieceTeam)
        {
            //temporary array used for processing positions
            bool[] validColSpotsTemp = { false, false, false, false, false, false, false, false };

            //go through the column and check to see if there is a free space
            for (byte colIndex = 0; colIndex < 8; colIndex++)
            {
                if (GetPiece(curPos.x, colIndex).GetTeam() == 0)
                {
                    validColSpotsTemp[colIndex] = true;
                }
            }

            return CheckValidLine(validColSpotsTemp, curPos, pieceTeam,true); ;
        }



        //wrapper for check valid line
        protected bool[] CheckValidLine(bool[] validLineSpotsTemp, GridPos curPos, byte pieceTeam, bool isVertical)
        {
            return CheckValidLine(validLineSpotsTemp, curPos, pieceTeam, isVertical, false);
        }



        protected bool[] CheckValidLine(bool[] validLineSpotsTemp,GridPos curPos, byte pieceTeam,bool isVertical,bool isDiagonal)
        {
            //temp holder array for to return the valid positions
            bool[] validLineSpots = { false, false, false, false, false, false, false, false };


            int startIndex = curPos.y;
            if (!isVertical) { startIndex = curPos.x;};

            int lowCheck = startIndex - 1;//start at the spot to the left of the current piece
            int highCheck = startIndex + 1;//start at the spot to the right of the current piece

            //flags for determining when movement is no longer possible
            bool lowCheckfin = false;
            bool highCheckfin = false;


            foreach (bool temp in validLineSpotsTemp)//run over every element in the passed array parameter
            {
                if ((lowCheck) >= 0) //lower bounds check
                {
                    if (validLineSpotsTemp[lowCheck] && !lowCheckfin)
                    {
                        validLineSpots[lowCheck] = true;
                    }
                    else
                    {
                        lowCheckfin = true;
                    }
                }
                if ((highCheck) < 8) //upper bounds check
                {
                    if ((validLineSpotsTemp[highCheck]) && !highCheckfin)
                    {
                        validLineSpots[highCheck] = true;
                    }
                    else
                    {
                        highCheckfin = true;
                    }
                }

                //add/remove from the current position to shift the check. Stops adding/subtracting when an edge is reached
                if (!lowCheckfin) { lowCheck = lowCheck - 1; }
                if (!highCheckfin) { highCheck = highCheck + 1; }
            }



            byte pos1 = curPos.x;
            byte pos2 = (Byte)highCheck;
            if (!isVertical)
            {
                pos1 = (Byte)highCheck;
                pos2 = curPos.y;
            }

            if (highCheck < 8)//bounding check
            {
                //gets the team of the current position and checks if it is not the current player
                if (GetPiece(pos1,pos2).GetTeam() != 0 && GetPiece(pos1, pos2).GetTeam() != pieceTeam)
                {

                    validLineSpots[highCheck] = true;
                }





            }

            if (lowCheck >= 0)//bounding check
            {
                //gets the team of the current position and checks if it is not the current player
                if (GetPiece(curPos.x, (Byte)lowCheck).GetTeam() != 0 && GetPiece(curPos.x, (Byte)lowCheck).GetTeam() != pieceTeam)
                {
                    //add the current space to the possible move list (for attack)
                    validLineSpots[lowCheck] = true;
                }
            }

            return validLineSpots;
        }

        public List<GridPos> GetPossibleMoves(GridPos curPos)
        {
            GamePiece piece = GetPiece(curPos.x, curPos.y);
            List<GridPos> validGrids = new List<GridPos> { };
            byte pieceTeam = piece.GetTeam();


            switch (piece.GetIcon())
            {
                case 'R': //Rook
                    {
                        bool[] validHorSpots = HorizontalMovementCheck(curPos, pieceTeam);
                        bool[] validVertSpots = VerticalMovementCheck(curPos, pieceTeam);
                        for (int i = 0; i < validHorSpots.Length; i++)
                        {
                            if (validHorSpots[i])
                            {
                                validGrids.Add(new GridPos(i, curPos.y));
                                Console.WriteLine("Valid HorizontalMove: " + i + ":" + curPos.y);
                            }
                        }
                        for (int i = 0; i < validVertSpots.Length; i++)
                        {
                            if (validVertSpots[i])
                            {
                                validGrids.Add(new GridPos(curPos.x, i));
                                Console.WriteLine("Valid VerticalMove: " + i + ":" + curPos.y);
                            }
                        }

                        break;
                    }
                case 'N'://Knight
                    {


                        break;
                    }
                case 'B'://Bishop
                    {
                        bool[] validUpSpots = DiagonalMovementCheck(curPos, pieceTeam,true);
                        bool[] validDownSpots = DiagonalMovementCheck(curPos, pieceTeam,false);

                        for (int i = 0; i < validUpSpots.Length; i++)
                        {
                            if (validUpSpots[i])
                            {
                                validGrids.Add(new GridPos(i, curPos.y));
                                Console.WriteLine("Valid DiagonalMove: " + i + ":" + curPos.y);
                            }
                        }
                        for (int i = 0; i < validDownSpots.Length; i++)
                        {
                            if (validDownSpots[i])
                            {
                                validGrids.Add(new GridPos(curPos.x, i));
                                Console.WriteLine("Valid DiagonalMove: " + i + ":" + curPos.y);
                            }
                        }





                        break;
                    }
                case 'Q'://Queen
                    {


                        break;
                    }
                case 'K'://King
                    {


                        break;
                    }
                case 'P'://Pawn
                    {


                        break;
                    }
            }


            return validGrids;
        }




    }

}
