using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Chess
{
    public class Piece
    {

        //Pseudo-legal moves
        public static List<int> AvailableMovesNoFilter(char pieceType, int pieceIndex)
        {
            List<int> MoveList = new List<int>();

            int PieceRank = ChessBoard.getRank(pieceIndex);
            int PieceFile = ChessBoard.getFile(pieceIndex);

            //Offsets
            int N = 8;
            int S = -8;
            int E = 1;
            int W = -1;

            int NW = 7;
            int NE = 9;
            int SW = -9;
            int SE = -7;
            /////////


            Char colour = ' ';
            if (Char.IsLower(pieceType))
            {
                colour = 'b';
            }
            else if (char.IsUpper(pieceType))
            {
                colour = 'w';
            }
            char oppositeColour = getOppositeColor(pieceType);

            //PIECE MOVES
            if (colour == ChessBoard.playerTurn)
            {
                switch (Char.ToLower(pieceType))
                {
                    case 'p':
                        #region PAWN MOVEMENT
                        
                        //Forward once
                        if (colour == 'w' && PieceRank != 8 && ChessBoard.getPieceColor(pieceIndex + N) == '0')
                        {
                            MoveList.Add(pieceIndex + N);
                        }
                        else if (colour == 'b' && PieceRank != 1 && ChessBoard.getPieceColor(pieceIndex + S) == '0')
                        {
                            MoveList.Add(pieceIndex + S);
                        }


                        //Forward twice at start
                        if (colour == 'w' && PieceRank == 2 && ChessBoard.getPieceColor(pieceIndex + N) == '0' && ChessBoard.getPieceColor(pieceIndex + 2 * N) == '0')
                        {
                            MoveList.Add(pieceIndex + 2 * N);
                        }
                        else if (colour == 'b' && PieceRank == 7 && ChessBoard.getPieceColor(pieceIndex + S) == '0' && ChessBoard.getPieceColor(pieceIndex + 2 * S) == '0')
                        {
                            MoveList.Add(pieceIndex + 2 * S);
                        }
                        

                        //Diagonal Capture
                        if (colour == 'w' && PieceRank != 8)
                        {
                            if (PieceFile != 1 && ChessBoard.getPieceColor(pieceIndex + NW) == 'b')
                            {
                                MoveList.Add(pieceIndex + NW);
                            }

                            if (PieceFile != 8 && ChessBoard.getPieceColor(pieceIndex + NE) == 'b')
                            {
                                MoveList.Add(pieceIndex + NE);
                            }

                        }
                        else if (colour == 'b' && PieceRank != 1)
                        {
                            if (PieceFile != 1 && ChessBoard.getPieceColor(pieceIndex + SW) == 'w')
                            {
                                MoveList.Add(pieceIndex + SW);
                            }

                            if (PieceFile != 8 && ChessBoard.getPieceColor(pieceIndex + SE) == 'w')
                            {
                                MoveList.Add(pieceIndex + SE);
                            }

                        }

                        //En Passant Capture
                        if (ChessBoard.EnPassantAvailable)
                        {
                            if (Math.Abs(ChessBoard.getFile(ChessBoard.pawnToEnPassant) - PieceFile) == 1)
                            {
                                if (colour == 'w' && PieceRank == 5)
                                {
                                    MoveList.Add(ChessBoard.pawnToEnPassant + N);
                                }
                                else if (colour == 'b' && PieceRank == 4)
                                {
                                    MoveList.Add(ChessBoard.pawnToEnPassant + S);
                                }

                            }

                        }

                        #endregion
                        break;

                    case 'r':
                        #region ROOK MOVEMENT

                        //RIGHT
                        for (int i = PieceFile + 1; i <= 8; i++)
                        {
                            if (ChessBoard.getPieceColor(pieceIndex + E * (i - PieceFile)) == colour)
                            {
                                break;
                            }
                            else if (ChessBoard.getPieceColor(pieceIndex + E * (i - PieceFile)) == oppositeColour)
                            {
                                MoveList.Add(pieceIndex + E * (i - PieceFile));
                                break;
                            }
                            MoveList.Add(pieceIndex + E * (i - PieceFile));
                        }

                        //LEFT
                        for (int i = PieceFile - 1; i >= 1; i--)
                        {
                            if (ChessBoard.getPieceColor(pieceIndex + W * (PieceFile - i)) == colour)
                            {
                                break;
                            }
                            else if (ChessBoard.getPieceColor(pieceIndex + W * (PieceFile - i)) == oppositeColour)
                            {
                                MoveList.Add(pieceIndex + W * (PieceFile - i));
                                break;
                            }
                            MoveList.Add(pieceIndex + W * (PieceFile - i));

                        }

                        //UP
                        for (int i = PieceRank + 1; i <= 8; i++)
                        {
                            if (ChessBoard.getPieceColor(pieceIndex + N * (i - PieceRank)) == colour)
                            {
                                break;
                            }
                            else if (ChessBoard.getPieceColor(pieceIndex + N * (i - PieceRank)) == oppositeColour)
                            {
                                MoveList.Add(pieceIndex + N * (i - PieceRank));
                                break;
                            }
                            MoveList.Add(pieceIndex + N * (i - PieceRank));

                        }

                        //DOWN
                        for (int i = PieceRank - 1; i >= 1; i--)
                        {
                            if (ChessBoard.getPieceColor(pieceIndex - (S * (i - PieceRank))) == colour)
                            {
                                break;
                            }
                            else if (ChessBoard.getPieceColor(pieceIndex - (S * (i - PieceRank))) == oppositeColour)
                            {
                                MoveList.Add(pieceIndex - (S * (i - PieceRank)));
                                break;
                            }
                            MoveList.Add(pieceIndex - (S * (i - PieceRank)));

                        }

                        #endregion
                        break;

                    case 'n':
                        #region KNIGHT MOVEMENT

                        //Knight offsets
                        int NW_Up = 2 * N + W;
                        int NW_Dn = 2 * W + N;

                        int NE_Up = 2 * N + E;
                        int NE_Dn = 2 * E + N;

                        int SW_Up = 2 * W + S;
                        int SW_Dn = 2 * S + W;

                        int SE_Up = 2 * E + S;
                        int SE_Dn = 2 * S + E;
                        //////////////


                        //Up-Left High
                        if (PieceFile > 1 && PieceRank < 7 && ChessBoard.getPieceColor(pieceIndex + NW_Up) != colour)
                        {
                            MoveList.Add(pieceIndex + NW_Up);
                        }
                        //Up-Left Low
                        if (PieceFile > 2 && PieceRank < 8 && ChessBoard.getPieceColor(pieceIndex + NW_Dn) != colour)
                        {
                            MoveList.Add(pieceIndex + NW_Dn);
                        }


                        //Up-Right High
                        if (PieceFile < 8 && PieceRank < 7 && ChessBoard.getPieceColor(pieceIndex + NE_Up) != colour)
                        {
                            MoveList.Add(pieceIndex + NE_Up);
                        }
                        //Up-Right Low
                        if (PieceFile < 7 && PieceRank < 8 && ChessBoard.getPieceColor(pieceIndex + NE_Dn) != colour)
                        {
                            MoveList.Add(pieceIndex + NE_Dn);
                        }


                        //Down-Left High
                        if (PieceFile > 2 && PieceRank > 1 && ChessBoard.getPieceColor(pieceIndex + SW_Up) != colour)
                        {
                            MoveList.Add(pieceIndex + SW_Up);
                        }
                        //Down-Left Low
                        if (PieceFile > 1 && PieceRank > 2 && ChessBoard.getPieceColor(pieceIndex + SW_Dn) != colour)
                        {
                            MoveList.Add(pieceIndex + SW_Dn);
                        }


                        //Down-Right High
                        if (PieceFile < 7 && PieceRank > 1 && ChessBoard.getPieceColor(pieceIndex + SE_Up) != colour)
                        {
                            MoveList.Add(pieceIndex + SE_Up);
                        }
                        //Down-Right Low
                        if (PieceFile < 8 && PieceRank > 2 && ChessBoard.getPieceColor(pieceIndex + SE_Dn) != colour)
                        {
                            MoveList.Add(pieceIndex + SE_Dn);
                        }


                        #endregion
                        break;

                    case 'b':
                        #region BISHOP MOVEMENT

                        //UP-RIGHT
                        for (int i = Math.Max(PieceFile, PieceRank) + 1; i <= 8; i++)
                        {
                            if (pieceIndex + NE * (i - Math.Max(PieceFile, PieceRank)) != pieceIndex)
                            {
                                if (ChessBoard.getPieceColor(pieceIndex + NE * (i - Math.Max(PieceFile, PieceRank))) == colour)
                                {
                                    break;
                                }
                                else if (ChessBoard.getPieceColor(pieceIndex + NE * (i - Math.Max(PieceFile, PieceRank))) == oppositeColour)
                                {
                                    MoveList.Add(pieceIndex + NE * (i - Math.Max(PieceFile, PieceRank)));
                                    break;
                                }
                                MoveList.Add(pieceIndex + NE * (i - Math.Max(PieceFile, PieceRank)));
                            }

                        }

                        //UP-LEFT
                        for (int i = Math.Max(8 - PieceFile, PieceRank) + 1; i <= 8; i++)
                        {
                            if (pieceIndex + NW * (i - Math.Max(9 - PieceFile, PieceRank)) != pieceIndex)
                            {
                                if (ChessBoard.getPieceColor(pieceIndex + NW * (i - Math.Max(9 - PieceFile, PieceRank))) == colour)
                                {
                                    break;
                                }
                                else if (ChessBoard.getPieceColor(pieceIndex + NW * (i - Math.Max(9 - PieceFile, PieceRank))) == oppositeColour)
                                {
                                    MoveList.Add(pieceIndex + NW * (i - Math.Max(9 - PieceFile, PieceRank)));
                                    break;
                                }
                                MoveList.Add(pieceIndex + NW * (i - Math.Max(9 - PieceFile, PieceRank)));
                            }

                        }


                        //DOWN-RIGHT
                        for (int i = Math.Max(PieceFile, 8 - PieceRank) + 1; i <= 8; i++)
                        {
                            if (pieceIndex + SE * (i - Math.Max(PieceFile, 9 - PieceRank)) != pieceIndex)
                            {
                                if (ChessBoard.getPieceColor(pieceIndex + SE * (i - Math.Max(PieceFile, 9 - PieceRank))) == colour)
                                {
                                    break;
                                }
                                else if (ChessBoard.getPieceColor(pieceIndex + SE * (i - Math.Max(PieceFile, 9 - PieceRank))) == oppositeColour)
                                {
                                    MoveList.Add(pieceIndex + SE * (i - Math.Max(PieceFile, 9 - PieceRank)));
                                    break;
                                }
                                MoveList.Add(pieceIndex + SE * (i - Math.Max(PieceFile, 9 - PieceRank)));
                            }

                        }


                        //DOWN-LEFT
                        for (int i = Math.Max(8 - PieceFile, 8 - PieceRank) + 1; i <= 8; i++)
                        {
                            if (pieceIndex + SW * (i - Math.Max(9 - PieceFile, 9 - PieceRank)) != pieceIndex)
                            {
                                if (ChessBoard.getPieceColor(pieceIndex + SW * (i - Math.Max(9 - PieceFile, 9 - PieceRank))) == colour)
                                {
                                    break;
                                }
                                else if (ChessBoard.getPieceColor(pieceIndex + SW * (i - Math.Max(9 - PieceFile, 9 - PieceRank))) == oppositeColour)
                                {
                                    MoveList.Add(pieceIndex + SW * (i - Math.Max(9 - PieceFile, 9 - PieceRank)));
                                    break;
                                }
                                MoveList.Add(pieceIndex + SW * (i - Math.Max(9 - PieceFile, 9 - PieceRank)));
                            }

                        }

                        #endregion
                        break;

                    case 'q':
                        #region QUEEN MOVEMENT

                        //RIGHT
                        for (int i = PieceFile + 1; i <= 8; i++)
                        {
                            if (ChessBoard.getPieceColor(pieceIndex + E * (i - PieceFile)) == colour)
                            {
                                break;
                            }
                            else if (ChessBoard.getPieceColor(pieceIndex + E * (i - PieceFile)) == oppositeColour)
                            {
                                MoveList.Add(pieceIndex + E * (i - PieceFile));
                                break;
                            }
                            MoveList.Add(pieceIndex + E * (i - PieceFile));
                        }

                        //LEFT
                        for (int i = PieceFile - 1; i >= 1; i--)
                        {
                            if (ChessBoard.getPieceColor(pieceIndex + W * (PieceFile - i)) == colour)
                            {
                                break;
                            }
                            else if (ChessBoard.getPieceColor(pieceIndex + W * (PieceFile - i)) == oppositeColour)
                            {
                                MoveList.Add(pieceIndex + W * (PieceFile - i));
                                break;
                            }
                            MoveList.Add(pieceIndex + W * (PieceFile - i));

                        }

                        //UP
                        for (int i = PieceRank + 1; i <= 8; i++)
                        {
                            if (ChessBoard.getPieceColor(pieceIndex + N * (i - PieceRank)) == colour)
                            {
                                break;
                            }
                            else if (ChessBoard.getPieceColor(pieceIndex + N * (i - PieceRank)) == oppositeColour)
                            {
                                MoveList.Add(pieceIndex + N * (i - PieceRank));
                                break;
                            }
                            MoveList.Add(pieceIndex + N * (i - PieceRank));

                        }

                        //DOWN
                        for (int i = PieceRank - 1; i >= 1; i--)
                        {
                            if (ChessBoard.getPieceColor(pieceIndex - (S * (i - PieceRank))) == colour)
                            {
                                break;
                            }
                            else if (ChessBoard.getPieceColor(pieceIndex - (S * (i - PieceRank))) == oppositeColour)
                            {
                                MoveList.Add(pieceIndex - (S * (i - PieceRank)));
                                break;
                            }
                            MoveList.Add(pieceIndex - (S * (i - PieceRank)));

                        }

                        //UP-RIGHT
                        for (int i = Math.Max(PieceFile, PieceRank) + 1; i <= 8; i++)
                        {
                            if (pieceIndex + NE * (i - Math.Max(PieceFile, PieceRank)) != pieceIndex)
                            {
                                if (ChessBoard.getPieceColor(pieceIndex + NE * (i - Math.Max(PieceFile, PieceRank))) == colour)
                                {
                                    break;
                                }
                                else if (ChessBoard.getPieceColor(pieceIndex + NE * (i - Math.Max(PieceFile, PieceRank))) == oppositeColour)
                                {
                                    MoveList.Add(pieceIndex + NE * (i - Math.Max(PieceFile, PieceRank)));
                                    break;
                                }
                                MoveList.Add(pieceIndex + NE * (i - Math.Max(PieceFile, PieceRank)));
                            }

                        }

                        //UP-LEFT
                        for (int i = Math.Max(8 - PieceFile, PieceRank) + 1; i <= 8; i++)
                        {
                            if (pieceIndex + NW * (i - Math.Max(9 - PieceFile, PieceRank)) != pieceIndex)
                            {
                                if (ChessBoard.getPieceColor(pieceIndex + NW * (i - Math.Max(9 - PieceFile, PieceRank))) == colour)
                                {
                                    break;
                                }
                                else if (ChessBoard.getPieceColor(pieceIndex + NW * (i - Math.Max(9 - PieceFile, PieceRank))) == oppositeColour)
                                {
                                    MoveList.Add(pieceIndex + NW * (i - Math.Max(9 - PieceFile, PieceRank)));
                                    break;
                                }
                                MoveList.Add(pieceIndex + NW * (i - Math.Max(9 - PieceFile, PieceRank)));
                            }

                        }


                        //DOWN-RIGHT
                        for (int i = Math.Max(PieceFile, 8 - PieceRank) + 1; i <= 8; i++)
                        {
                            if (pieceIndex + SE * (i - Math.Max(PieceFile, 9 - PieceRank)) != pieceIndex)
                            {
                                if (ChessBoard.getPieceColor(pieceIndex + SE * (i - Math.Max(PieceFile, 9 - PieceRank))) == colour)
                                {
                                    break;
                                }
                                else if (ChessBoard.getPieceColor(pieceIndex + SE * (i - Math.Max(PieceFile, 9 - PieceRank))) == oppositeColour)
                                {
                                    MoveList.Add(pieceIndex + SE * (i - Math.Max(PieceFile, 9 - PieceRank)));
                                    break;
                                }
                                MoveList.Add(pieceIndex + SE * (i - Math.Max(PieceFile, 9 - PieceRank)));
                            }

                        }


                        //DOWN-LEFT
                        for (int i = Math.Max(8 - PieceFile, 8 - PieceRank) + 1; i <= 8; i++)
                        {
                            if (pieceIndex + SW * (i - Math.Max(9 - PieceFile, 9 - PieceRank)) != pieceIndex)
                            {
                                if (ChessBoard.getPieceColor(pieceIndex + SW * (i - Math.Max(9 - PieceFile, 9 - PieceRank))) == colour)
                                {
                                    break;
                                }
                                else if (ChessBoard.getPieceColor(pieceIndex + SW * (i - Math.Max(9 - PieceFile, 9 - PieceRank))) == oppositeColour)
                                {
                                    MoveList.Add(pieceIndex + SW * (i - Math.Max(9 - PieceFile, 9 - PieceRank)));
                                    break;
                                }
                                MoveList.Add(pieceIndex + SW * (i - Math.Max(9 - PieceFile, 9 - PieceRank)));
                            }

                        }

                        #endregion
                        break;

                    case 'k':
                        #region KING MOVEMENT

                        //Down
                        if (PieceRank != 1 && ChessBoard.getPieceColor(pieceIndex + S) != colour)
                        {
                            MoveList.Add(pieceIndex + S);
                        }

                        //Up
                        if (PieceRank != 8 && ChessBoard.getPieceColor(pieceIndex + N) != colour)
                        {
                            MoveList.Add(pieceIndex + N);
                        }

                        //Left
                        if (PieceFile != 1 && ChessBoard.getPieceColor(pieceIndex + W) != colour)
                        {
                            MoveList.Add(pieceIndex + W);
                        }

                        //Right
                        if (PieceFile != 8 && ChessBoard.getPieceColor(pieceIndex + E) != colour)
                        {
                            MoveList.Add(pieceIndex + E);
                        }

                        //Down-Right
                        if (PieceFile != 8 && PieceRank != 1 && ChessBoard.getPieceColor(pieceIndex + SE) != colour)
                        {
                            MoveList.Add(pieceIndex + SE);
                        }

                        //Up-Right
                        if (PieceFile != 8 && PieceRank != 8 && ChessBoard.getPieceColor(pieceIndex + NE) != colour)
                        {
                            MoveList.Add(pieceIndex + NE);
                        }

                        //Up-Left
                        if (PieceFile != 1 && PieceRank != 8 && ChessBoard.getPieceColor(pieceIndex + NW) != colour)
                        {
                            MoveList.Add(pieceIndex + NW);
                        }

                        //Down-Left
                        if (PieceFile != 1 && PieceRank != 1 && ChessBoard.getPieceColor(pieceIndex + SW) != colour)
                        {
                            MoveList.Add(pieceIndex + SW);
                        }

                        
                        //CASTLING
                        switch (colour)
                        {
                            case 'w':
                                if (!ChessBoard.hasKingMoved_white)
                                {
                                    if (!ChessBoard.hasRookMoved_kingside_white)
                                    {
                                        if (ChessBoard.chessBoard[7, 5] == '0' && ChessBoard.chessBoard[7, 6] == '0' && ChessBoard.chessBoard[7, 7] == 'R')
                                        {
                                            ChessBoard.hasKingMoved_white = true;
                                            if (!getAttackedSquares(ChessBoard.getOppositeColor(colour)).Contains(pieceIndex) &&
                                                !getAttackedSquares(ChessBoard.getOppositeColor(colour)).Contains(pieceIndex + E))
                                            {
                                                MoveList.Add(6);
                                            }
                                            ChessBoard.hasKingMoved_white = false;

                                        }
                                    }
                                    if (!ChessBoard.hasRookMoved_queenside_white)
                                    {
                                        if (ChessBoard.chessBoard[7, 1] == '0' && ChessBoard.chessBoard[7, 2] == '0' && ChessBoard.chessBoard[7, 3] == '0' && ChessBoard.chessBoard[7, 0] == 'R')
                                        {
                                            ChessBoard.hasKingMoved_white = true;
                                            if (!getAttackedSquares(ChessBoard.getOppositeColor(colour)).Contains(pieceIndex) &&
                                                !getAttackedSquares(ChessBoard.getOppositeColor(colour)).Contains(pieceIndex + W))
                                            {
                                                MoveList.Add(2);
                                            }
                                            ChessBoard.hasKingMoved_white = false;

                                        }
                                    }
                                }
                                break;

                            case 'b':
                                if (!ChessBoard.hasKingMoved_black)
                                {
                                    if (!ChessBoard.hasRookMoved_kingside_black)
                                    {
                                        if (ChessBoard.chessBoard[0, 5] == '0' && ChessBoard.chessBoard[0, 6] == '0' && ChessBoard.chessBoard[0, 7] == 'r')
                                        {
                                            ChessBoard.hasKingMoved_black = true;
                                            if (!getAttackedSquares(ChessBoard.getOppositeColor(colour)).Contains(pieceIndex) &&
                                                !getAttackedSquares(ChessBoard.getOppositeColor(colour)).Contains(pieceIndex + E))
                                            {
                                                MoveList.Add(62);
                                            }
                                            ChessBoard.hasKingMoved_black = false;

                                        }
                                    }
                                    if (!ChessBoard.hasRookMoved_queenside_black)
                                    {
                                        if (ChessBoard.chessBoard[0, 1] == '0' && ChessBoard.chessBoard[0, 2] == '0' && ChessBoard.chessBoard[0, 3] == '0' && ChessBoard.chessBoard[0, 0] == 'r')
                                        {
                                            ChessBoard.hasKingMoved_black = true;
                                            if (!getAttackedSquares(ChessBoard.getOppositeColor(colour)).Contains(pieceIndex) &&
                                                !getAttackedSquares(ChessBoard.getOppositeColor(colour)).Contains(pieceIndex + W))
                                            {
                                                MoveList.Add(58);
                                            }
                                            ChessBoard.hasKingMoved_black = false;

                                        }
                                    }
                                }
                                break;
                        }
                        
                        
                        #endregion
                        break;

                }
            }

            return MoveList;
        }


        public static List<int> AvailableMoves(char pieceType, int pieceIndex)
        {
            //VALIDATING MOVES FOR KING ATTACKS
            List<int> MoveList = AvailableMovesNoFilter(pieceType, pieceIndex);
            List<int> filteredMoveList = new List<int>();


            Char color = ' ';
            if (Char.IsLower(pieceType))
            {
                color = 'b';
            }
            else if (char.IsUpper(pieceType))
            {
                color = 'w';
            }
            char oppositeColour = getOppositeColor(pieceType);



            char[,] originalChessBoard = ChessBoard.chessBoard.Clone() as char[,];


            for (int i = 0; i < MoveList.Count; i++)
            {
                ChessBoard.validatingMoves = true;
                ChessBoard.updateChessBoard(pieceIndex, MoveList[i]);

                List<int> opponentResponses = getAttackedSquares(oppositeColour);
                if (!opponentResponses.Contains(ChessBoard.getKingLocation(color)))
                {
                    filteredMoveList.Add(MoveList[i]);
                }

                ChessBoard.chessBoard = originalChessBoard.Clone() as char[,];
            }


            ChessBoard.validatingMoves = false;
            return filteredMoveList;
        }


        //Used for quiescence search
        public static List<int> AvailableCaptureMoves(char pieceType, int pieceIndex)
        {
            List<int> MoveList = AvailableMovesNoFilter(pieceType, pieceIndex);
            List<int> CaptureMoveList = new List<int>();
            List<int> filteredMoveList = new List<int>();


            Char color = ' ';
            if (Char.IsLower(pieceType))
            {
                color = 'b';
            }
            else if (char.IsUpper(pieceType))
            {
                color = 'w';
            }
            char oppositeColour = getOppositeColor(pieceType);



            char[,] originalChessBoard = ChessBoard.chessBoard.Clone() as char[,];

            //Remove non capture moves
            for (int i = 0; i < MoveList.Count; i++)
            {
                if (ChessBoard.getPieceAtIndex(i) != '0')
                {
                    CaptureMoveList.Add(MoveList[i]);
                }
            }

            //Filter legal moves
            for (int i = 0; i < CaptureMoveList.Count; i++)
            {
                ChessBoard.validatingMoves = true;
                ChessBoard.updateChessBoard(pieceIndex, CaptureMoveList[i]);

                List<int> opponentResponses = getAttackedSquares(oppositeColour);
                if (!opponentResponses.Contains(ChessBoard.getKingLocation(color)))
                {
                    filteredMoveList.Add(CaptureMoveList[i]);
                }

                ChessBoard.chessBoard = originalChessBoard.Clone() as char[,];
            }

            ChessBoard.validatingMoves = false;
            return filteredMoveList;
        }

        public static List<int> getAttackedSquares(char color)
        {
            char OriginalPlayerTurn = ChessBoard.playerTurn;
            ChessBoard.playerTurn = color;

            //GET LIST OF AVAILABLE PIECE INDEXES
            List<int> AvailablePieces = new List<int>();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (ChessBoard.chessBoard[i, j] != '0')
                    {
                        switch (color){
                            case 'w':
                                if (Char.IsUpper(ChessBoard.chessBoard[i, j]))
                                {
                                    AvailablePieces.Add(ChessBoard.getSquareIndex(i, j));
                                }
                                break;
                            case 'b':
                                if (Char.IsLower(ChessBoard.chessBoard[i, j]))
                                {
                                    AvailablePieces.Add(ChessBoard.getSquareIndex(i, j));
                                }
                                break;
                        }

                    }

                }
            }

            //GET LIST OF ALL AVAILABLE MOVES
            List<int> AllAvailableMoves = new List<int>();

            for (int i = 0; i < AvailablePieces.Count; i++)
            {
                AllAvailableMoves.AddRange(AvailableMovesNoFilter(ChessBoard.getPieceAtIndex(AvailablePieces[i]), AvailablePieces[i]));

                //Add pawn attacks too
                if (ChessBoard.getPieceAtIndex(AvailablePieces[i]) == 'P')
                {
                    if (ChessBoard.getFile(AvailablePieces[i]) != 1){
                        if (ChessBoard.chessBoard[8 - ChessBoard.getRank(AvailablePieces[i] + 7), ChessBoard.getFile(AvailablePieces[i] + 7) - 1] == '0')
                        {
                            AllAvailableMoves.Add(AvailablePieces[i] + 7);
                        }
                    }

                    if (ChessBoard.getFile(AvailablePieces[i]) != 8)
                    {
                        if (ChessBoard.chessBoard[8 - ChessBoard.getRank(AvailablePieces[i] + 9), ChessBoard.getFile(AvailablePieces[i] + 9) - 1] == '0')
                        {
                            AllAvailableMoves.Add(AvailablePieces[i] + 9);
                        }
                    }

                }
                else if (ChessBoard.getPieceAtIndex(AvailablePieces[i]) == 'p')
                {
                    if (ChessBoard.getFile(AvailablePieces[i]) != 1)
                    {
                        if (ChessBoard.chessBoard[8 - ChessBoard.getRank(AvailablePieces[i] - 9), ChessBoard.getFile(AvailablePieces[i] - 9) - 1] == '0')
                        {
                            AllAvailableMoves.Add(AvailablePieces[i] - 9);
                        }
                    }

                    if (ChessBoard.getFile(AvailablePieces[i]) != 8)
                    {
                        if (ChessBoard.chessBoard[8 - ChessBoard.getRank(AvailablePieces[i] - 7), ChessBoard.getFile(AvailablePieces[i] - 7) - 1] == '0')
                        {
                            AllAvailableMoves.Add(AvailablePieces[i] - 7);
                        }
                    }

                }

            }


            ChessBoard.playerTurn = OriginalPlayerTurn;
            return AllAvailableMoves;

        }

        public static char getOppositeColor(char pieceType)
        {
            Char color = ' ';
            if (Char.IsLower(pieceType))
            {
                color = 'b';
            }
            else if (char.IsUpper(pieceType))
            {
                color = 'w';
            }

            if (color == 'w')
            {
                return 'b';
            }
            else if (color == 'b')
            {
                return 'w';
            }
            return '0';

        }

    }
}
