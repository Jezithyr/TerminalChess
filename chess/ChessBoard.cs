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

            bool invertKingQueen = false;
            GamePiece[] PieceList = new GamePiece[12];
            for (byte colorIndex = 0; colorIndex < 2; colorIndex++)
            {
                for (int pieceIndex = 0; pieceIndex < 6; pieceIndex++)
                {
                    PieceList[pieceIndex + (colorIndex * 6)] = new GamePiece(PieceIcons[pieceIndex], (byte)(colorIndex + 1));
                }
                invertKingQueen = !invertKingQueen;
                char temp = PieceIcons[3];
                PieceIcons[3] = 'Q';
                PieceIcons[4] = temp;
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


        private List<GridPos> DiagonalsCheck(GridPos curPos, byte pieceTeam)
        {
            List<GridPos> tempGrids = new List<GridPos> { };
            byte curX = curPos.x;
            byte curY = curPos.y;
            byte startX = curX;
            byte startY = curY;

            for (int count = 0;count < 8;count++) //up and to the right check
            {
                curX++;
                curY++;
                if (curX < 0 || curX > 7 ||curY < 0 ||curY > 7){break;}; //break out of loop if position is out of bounds
                byte targetTeam = GetPiece(curX, curY).GetTeam();
                if (targetTeam == 0 || targetTeam != pieceTeam)
                {
                    tempGrids.Add(new GridPos(curX, curY));
                    if (targetTeam != 0)
                    {
                        break; //break out of the for loop if a target is found
                    }
                }
                else
                {
                    break;
                }
                
            }
            curX = startX;
            curY = startY;

            for (int count = 0; count < 8; count++) //down and to the right check
            {
                curX++;
                curY--;
                if (curX < 0 || curX > 7 || curY < 0 || curY > 7) { break; }; //break out of loop if position is out of bounds
                byte targetTeam = GetPiece(curX, curY).GetTeam();
                if (targetTeam == 0 || targetTeam != pieceTeam)
                {
                    tempGrids.Add(new GridPos(curX, curY));
                    if (targetTeam != 0)
                    {
                        break; //break out of the for loop if a target is found
                    }
                }
                else
                {
                    break;
                }

            }
            curX = startX;
            curY = startY;

            for (int count = 0; count < 8; count++) //down and to the left check
            {
                curX--;
                curY--;
                if (curX < 0 || curX > 7 || curY < 0 || curY > 7) { break; }; //break out of loop if position is out of bounds
                byte targetTeam = GetPiece(curX, curY).GetTeam();
                if (targetTeam == 0 || targetTeam != pieceTeam)
                {
                    tempGrids.Add(new GridPos(curX, curY));
                    if (targetTeam != 0)
                    {
                        break; //break out of the for loop if a target is found
                    }
                }
                else
                {
                    break;
                }

            }
            curX = startX;
            curY = startY;

            for (int count = 0; count < 8; count++) //up and to the left check
            {
                curX--;
                curY++;
                if (curX < 0 || curX > 7 || curY < 0 || curY > 7) { break; }; //break out of loop if position is out of bounds
                byte targetTeam = GetPiece(curX, curY).GetTeam();
                if (targetTeam == 0 || targetTeam != pieceTeam)
                {
                    tempGrids.Add(new GridPos(curX, curY));
                    if (targetTeam != 0)
                    {
                        break; //break out of the for loop if a target is found
                    }
                }
                else
                {
                    break;
                }

            }
            curX = startX;
            curY = startY;

            return tempGrids;
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



        protected bool[] CheckValidLine(bool[] validLineSpotsTemp,GridPos curPos, byte pieceTeam,bool isVertical)
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


        public List<GridPos> GetPossibleMoves(int[] curPos,Player player)
        {
            return GetPossibleMoves(new GridPos(curPos[0], curPos[1]), player);
        }

        public List<GridPos> GetPossibleMoves(GridPos curPos,Player player)
        {
            GamePiece piece = GetPiece(curPos.x, curPos.y);
            List<GridPos> validGrids = new List<GridPos> { };
            byte pieceTeam = piece.GetTeam();


            if (pieceTeam != player.GetTeam())//return no moves available for an enemy players pieces
            {
                return validGrids;
            }

            switch (piece.GetIcon())
            {
                
                case 'N'://Knight
                    {
                        int[][] movePat = new int[][] //possible moves a knight can make
                        {
                            new int[] {-2,1},
                            new int[] {-1,2},
                            new int[] {1,2},
                            new int[] {2,1},
                            new int[] {-2,-1},
                            new int[] {-1,-2},
                            new int[] {1,-2},
                            new int[] {2,-1}
                        };

                        int movePosX, movePosY;
                        byte targetTeam;

                        for (int i = 0;i < 8; i++)
                        {
                            
                            movePosX = curPos.x + movePat[i][0];
                            movePosY = curPos.y + movePat[i][1];

                            if ((movePosX >= 0)&& (movePosX < 8) && (movePosY >= 0) && (movePosY < 8)) //bounds checks
                            {
                                targetTeam = GetPiece((byte)movePosX, (byte)movePosY).GetTeam();
                                if (targetTeam == 0 || pieceTeam != targetTeam)
                                {
                                    validGrids.Add(new GridPos(movePosX, movePosY));
                                }
                                
                            }
                        }


                        break;
                    }
               
               
                    //TODO: add check/checkmate detection/move prevention

                case 'K'://King
                    {
                        int[][] movePat = new int[][] //possible moves a knight can make
                        {
                            new int[] {1,0},
                            new int[] {1,1},
                            new int[] {0,1},
                            new int[] {-1,1},
                            new int[] {-1,0},
                            new int[] {-1,-1},
                            new int[] {0,-1},
                            new int[] {1,-1}
                        };

                        int movePosX, movePosY;
                        byte targetTeam;

                        for (int i = 0; i < 8; i++)
                        {

                            movePosX = curPos.x + movePat[i][0];
                            movePosY = curPos.y + movePat[i][1];

                            if ((movePosX >= 0) && (movePosX < 8) && (movePosY >= 0) && (movePosY < 8)) //bounds checks
                            {
                                targetTeam = GetPiece((byte)movePosX, (byte)movePosY).GetTeam();
                                if (targetTeam == 0 || pieceTeam != targetTeam)
                                {
                                    validGrids.Add(new GridPos(movePosX, movePosY));
                                }

                            }
                        }

                        break;
                    }
                case 'P'://Pawn
                    {
                        byte frontOwner;

                        //======= Combat pattern ========
                        int[][] attackPats = new int[][] //possible attacks a pawn can make
                        {
                            new int[] {1,1},
                            new int[] {-1,1}
                        };

                        int dir = 1;
                        if (pieceTeam == 2)
                        {
                            dir = -1;
                        }

                        // ======== Beginning Jump =======
                        if (curPos.y == 1 || curPos.y == 6)
                        {
                            frontOwner = GetPiece(curPos.x, (byte)(curPos.y + (dir * 2))).GetTeam();
                            if (frontOwner == 0)
                            {
                                validGrids.Add(new GridPos(curPos.x, (byte)(curPos.y + (dir * 2))));
                            }
                        }

                        //TODO Implement en passant! https://en.wikipedia.org/wiki/En_passant

                        //====== Main Movement Logic =========

                        if (curPos.y + 1 < 8)
                        {
                            frontOwner = GetPiece(curPos.x, (byte)(curPos.y + dir)).GetTeam();
                            if (frontOwner == 0)
                            {
                                validGrids.Add(new GridPos(curPos.x, (byte)(curPos.y + dir)));
                            }

                            foreach (int[] attack in attackPats)
                            {
                                if (((byte)(curPos.x + attack[0])>= 0)&& ((byte)(curPos.y + (attack[1] * dir)) >= 0))//bounding check
                                {
                                    if (((byte)(curPos.x + attack[0]) < 8) && ((byte)(curPos.y + (attack[1] * dir)) < 8))//bounding check
                                    {
                                        byte targetTeam = GetPiece((byte)(curPos.x + attack[0]), (byte)(curPos.y + (attack[1] * dir))).GetTeam();
                                        if (targetTeam != 0 && targetTeam != pieceTeam)
                                        {
                                            validGrids.Add(new GridPos((byte)(curPos.x + attack[0]), (byte)(curPos.y + (attack[1] * dir))));
                                        }
                                    }  
                                }
                            }

                        }
                        break;
                    }

                case 'B'://Bishop
                    {

                        validGrids.AddRange(DiagonalsCheck(curPos, pieceTeam));
                        break;
                    }

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

                case 'Q'://Queen
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
                        validGrids.AddRange(DiagonalsCheck(curPos, pieceTeam));
                        break;
                    }
            }


            return validGrids;
        }

        public override void Move(GridPos piecePos1, GridPos piecePos2,Player player)
        {
            GamePiece gamePiece = GetPiece(piecePos1);
            GamePiece target = GetPiece(piecePos2);


            grid[piecePos2.x][piecePos2.y] = GetPiece(piecePos1);//create new duplicate piece over target

            grid[piecePos1.x][piecePos1.y] = new GamePiece('-');//delete the old piece

            CleanHighlights();//remove the movement highlights

            if (target.GetIcon() == 'K')//WIN CONDITION CHECK!
            {
                gameover = true;
                winner = player;
            }





            //base.move(piece1, piece2);
        }



    }

}
