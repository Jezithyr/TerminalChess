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
            //this generates the possible spots available spots on the horizontal axis of the piece
            bool[] validRowSpots =  { false, false, false, false, false, false, false, false };
            bool[] validRowSpotsTemp = { false, false, false, false, false, false, false, false };

            


            for (byte colIndex = 0; colIndex < 8; colIndex++)
            {
                if (GetPiece(colIndex, curPos.y).GetTeam() == 0)
                {
                    validRowSpotsTemp[colIndex] = true;
                }
            }
            int lowCheck = 0;
            int highCheck = 0;
            bool lowCheckfin = false;
            bool highCheckfin = false;
            foreach (bool temp in validRowSpotsTemp)
            {
                if ((curPos.x + lowCheck) >0)
                {
                    Console.WriteLine((curPos.x + lowCheck));
                    if (validRowSpotsTemp[(curPos.x + lowCheck)] && !lowCheckfin)
                    {
                        validRowSpots[curPos.x + lowCheck] = true;
                    }else
                    {
                        lowCheckfin = true;
                    }
                }
                if ((highCheck + curPos.x) < 8)
                {
                    if ((validRowSpotsTemp[(curPos.x + highCheck)])&& !highCheckfin)
                    {
                        validRowSpots[curPos.x + highCheck] = true;
                    }else
                    {
                        highCheckfin = true;
                    }
                }

                lowCheck = lowCheck - 1;
                highCheck = highCheck + 1;
            }
            return validRowSpots;
        }

        protected bool[] VerticalMovementCheck(GridPos curPos, byte pieceTeam)
        {
            //this generates the possible spots available spots on the horizontal axis of the piece
            bool[] validColSpots = { false, false, false, false, false, false, false, false };
            bool[] validColSpotsTemp = { false, false, false, false, false, false, false, false };
            for (byte colIndex = 0; colIndex < 8; colIndex++)
            {
                if (GetPiece(curPos.x, colIndex).GetTeam() == 0)
                {
                    validColSpotsTemp[colIndex] = true;
                }
            }




            int lowCheck = 0;
            int highCheck = 0;
            bool lowCheckfin = false;
            bool highCheckfin = false;
            foreach (bool temp in validColSpotsTemp)
            {
                if ((curPos.y + lowCheck ) >= 0)
                {
                    if (validColSpotsTemp[curPos.y + lowCheck] && !lowCheckfin)
                    {
                        validColSpots[curPos.y + lowCheck] = true;
                    }
                    else
                    {
                        lowCheckfin = true;
                    }
                }
                if ((highCheck + curPos.y) < 8)
                {
                    if ((validColSpotsTemp[curPos.y + highCheck]) && !highCheckfin)
                    {
                        validColSpots[curPos.y + highCheck] = true;
                    }
                    else
                    {
                        highCheckfin = true;
                    }
                }

                lowCheck = lowCheck - 1;
                highCheck = highCheck + 1;
            }

                return validColSpots;
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
         
                            }
                        }
                        for (int i = 0; i < validVertSpots.Length; i++)
                        {
                            if (validVertSpots[i])
                            {
                                validGrids.Add(new GridPos(curPos.x, i));
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
