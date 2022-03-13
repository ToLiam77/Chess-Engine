using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace Chess
{
    internal class MoveGeneration
    {
        public static bool playingVsAI = true;

        public static int moveIndex_From;
        public static int moveIndex_To;

        //moveIndex_From;
        //moveIndex_To;

        public static void generateRandomMove()
        {
            //GET LIST OF AVAILABLE PIECE INDEXES
            List<int> AvailablePieces = new List<int>();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (ChessBoard.chessBoard[i, j] != '0')
                    {
                        switch (ChessBoard.playerTurn)
                        {
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


            //PICK RANDOM PIECE AND MAKE RANDOM MOVE
            var random = new Random();
            int pieceToMove = AvailablePieces[random.Next(AvailablePieces.Count)];

            List<int> AvailableMoves = Piece.AvailableMoves(ChessBoard.getPieceAtIndex(pieceToMove), pieceToMove);
            if (AvailableMoves.Count == 0)
            {
                generateRandomMove();
                return;
            }

            int targetIndex = AvailableMoves[random.Next(AvailableMoves.Count)];

            //Pawn promotion case
            switch (ChessBoard.getPieceAtIndex(pieceToMove))
            {
                case 'p':
                    if (ChessBoard.getRank(targetIndex) == 1)
                    {
                        char[] promotionOptions = { 'q', 'r', 'b', 'n' };
                        ChessBoard.promotionPiece = promotionOptions[random.Next(4)];
                    }
                    break;

                case 'P':
                    if (ChessBoard.getRank(targetIndex) == 8)
                    {
                        char[] promotionOptions = { 'Q', 'R', 'B', 'N' };
                        ChessBoard.promotionPiece = promotionOptions[random.Next(4)];
                    }
                    break;
            }


            moveIndex_From = pieceToMove;
            moveIndex_To = targetIndex;
            ChessBoard.updateChessBoard(pieceToMove, targetIndex);
        }



        public static void generateMove()
        {
            //Show loading icon on mouse when waiting for move
            Cursor.Current = Cursors.WaitCursor;

            //Measure time taken to generate move
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();


            //Debug.WriteLine(Search(3, float.NegativeInfinity, float.PositiveInfinity, true));


            int depthSearch = 3;
            Debug.WriteLine(Search2(depthSearch, -1000, 1000));


            if (ChessBoard.playerTurn == 'b')
            {
                //Debug.WriteLine(Search2(depthSearch, float.NegativeInfinity, float.PositiveInfinity));
            }
            else
            {
                //Debug.WriteLine(-Search2(depthSearch, float.NegativeInfinity, float.PositiveInfinity));
            }


            




            stopWatch.Stop();
            Debug.WriteLine("Time taken: " + stopWatch.ElapsedMilliseconds + " millisecons");


            //ChessBoard.updateChessBoard(moveIndex_From, moveIndex_To);

        }




        public static void SearchNode()
        {
            //GET LIST OF AVAILABLE PIECE INDEXES
            List<int> AvailablePieces = getAvailablePieces(ChessBoard.playerTurn);
            //Get Number of possible Moves
            List<List<int>> AvailableMoves = getPieceMoves(AvailablePieces);

            float bestNode = 0;

            for (int i = 0; i < AvailablePieces.Count; i++)
            {
                for (int j = 0; j < AvailableMoves[i].Count; j++)
                {

                }

            }

        }






        public static float Search2(int depth, float alpha, float beta)
        {
            if (depth == 0)
            {
                return BoardEvaluation.getBoardEvaluation();
            }


            //GET LIST OF AVAILABLE PIECE INDEXES
            List<int> AvailablePieces = getAvailablePieces(ChessBoard.playerTurn);
            //Get Number of possible Moves
            List<List<int>> AvailableMoves = getPieceMoves(AvailablePieces);



            int pieceToMove = 0;
            int targetIndex = 0;
            //float bestEvaluation = 0;


            for (int i = 0; i < AvailablePieces.Count; i++)
            {
                for (int j = 0; j < AvailableMoves[i].Count; j++)
                {
                    #region Pawn promotion case
                    //Pawn promotion case
                    if (ChessBoard.getPieceAtIndex(AvailablePieces[i]) == 'P' && ChessBoard.getRank(AvailableMoves[i][j]) == 8)
                    {
                        char[] promoteOptions = { 'Q', 'R', 'B', 'N' };
                        char bestPromotion = ' ';
                        for (int k = 0; k < 4; k++)
                        {
                            #region Save State
                            //Save Casling values
                            bool[] castlingRights = new bool[]
                            {
                                ChessBoard.hasKingMoved_white,
                                ChessBoard.hasKingMoved_black,
                                ChessBoard.hasRookMoved_queenside_white,
                                ChessBoard.hasRookMoved_kingside_white,
                                ChessBoard.hasRookMoved_queenside_black,
                                ChessBoard.hasRookMoved_kingside_black
                            };

                            //Save En Passant Values
                            bool EnPassantAvailable = ChessBoard.EnPassantAvailable;
                            int pawnToEnPassant = ChessBoard.pawnToEnPassant;


                            char[,] originalChessBoard = ChessBoard.chessBoard.Clone() as char[,];
                            #endregion

                            ChessBoard.promotionPiece = promoteOptions[k];
                            ChessBoard.validatingMoves = true;
                            ChessBoard.updateChessBoard(AvailablePieces[i], AvailableMoves[i][j]);
                            ChessBoard.validatingMoves = false;

                            float evaluation = -Search2(depth - 1, -beta, -alpha);

                            #region Restore State
                            ChessBoard.chessBoard = originalChessBoard.Clone() as char[,];


                            //Reset Casling values
                            ChessBoard.hasKingMoved_white = castlingRights[0];
                            ChessBoard.hasKingMoved_black = castlingRights[1];
                            ChessBoard.hasRookMoved_queenside_white = castlingRights[2];
                            ChessBoard.hasRookMoved_kingside_white = castlingRights[3];
                            ChessBoard.hasRookMoved_queenside_black = castlingRights[4];
                            ChessBoard.hasRookMoved_kingside_black = castlingRights[5];

                            //Reset En Passant Values
                            ChessBoard.EnPassantAvailable = EnPassantAvailable;
                            ChessBoard.pawnToEnPassant = pawnToEnPassant;
                            #endregion

                            if (evaluation >= beta)
                            {
                                //Prune branch
                                return beta;
                            }
                            alpha = Math.Max(alpha, evaluation);
                            bestPromotion = promoteOptions[k];

                        }
                        ChessBoard.promotionPiece = bestPromotion;
                    }
                    else if (ChessBoard.getPieceAtIndex(AvailablePieces[i]) == 'p' && ChessBoard.getRank(AvailableMoves[i][j]) == 1)
                    {
                        char[] promoteOptions = { 'q', 'r', 'b', 'n' };
                        char bestPromotion = ' ';
                        for (int k = 0; k < 4; k++)
                        {
                            #region Save State
                            //Save Casling values
                            bool[] castlingRights = new bool[]
                            {
                                ChessBoard.hasKingMoved_white,
                                ChessBoard.hasKingMoved_black,
                                ChessBoard.hasRookMoved_queenside_white,
                                ChessBoard.hasRookMoved_kingside_white,
                                ChessBoard.hasRookMoved_queenside_black,
                                ChessBoard.hasRookMoved_kingside_black
                            };

                            //Save En Passant Values
                            bool EnPassantAvailable = ChessBoard.EnPassantAvailable;
                            int pawnToEnPassant = ChessBoard.pawnToEnPassant;


                            char[,] originalChessBoard = ChessBoard.chessBoard.Clone() as char[,];
                            #endregion

                            ChessBoard.promotionPiece = promoteOptions[k];
                            ChessBoard.validatingMoves = true;
                            ChessBoard.updateChessBoard(AvailablePieces[i], AvailableMoves[i][j]);
                            ChessBoard.validatingMoves = false;

                            float evaluation = -Search2(depth - 1, -beta, -alpha);

                            #region Restore State
                            ChessBoard.chessBoard = originalChessBoard.Clone() as char[,];


                            //Reset Casling values
                            ChessBoard.hasKingMoved_white = castlingRights[0];
                            ChessBoard.hasKingMoved_black = castlingRights[1];
                            ChessBoard.hasRookMoved_queenside_white = castlingRights[2];
                            ChessBoard.hasRookMoved_kingside_white = castlingRights[3];
                            ChessBoard.hasRookMoved_queenside_black = castlingRights[4];
                            ChessBoard.hasRookMoved_kingside_black = castlingRights[5];

                            //Reset En Passant Values
                            ChessBoard.EnPassantAvailable = EnPassantAvailable;
                            ChessBoard.pawnToEnPassant = pawnToEnPassant;
                            #endregion

                            if (evaluation >= beta)
                            {
                                //Prune branch
                                return beta;
                            }
                            alpha = Math.Max(alpha, evaluation);
                            bestPromotion = promoteOptions[k];

                        }
                        ChessBoard.promotionPiece = bestPromotion;
                    }
                    #endregion
                    else
                    {
                        #region Save State
                        //Save Casling values
                        bool[] castlingRights = new bool[]
                        {
                        ChessBoard.hasKingMoved_white,
                        ChessBoard.hasKingMoved_black,
                        ChessBoard.hasRookMoved_queenside_white,
                        ChessBoard.hasRookMoved_kingside_white,
                        ChessBoard.hasRookMoved_queenside_black,
                        ChessBoard.hasRookMoved_kingside_black
                        };

                        //Save En Passant Values
                        bool EnPassantAvailable = ChessBoard.EnPassantAvailable;
                        int pawnToEnPassant = ChessBoard.pawnToEnPassant;


                        char[,] originalChessBoard = ChessBoard.chessBoard.Clone() as char[,];
                        #endregion
                        ChessBoard.validatingMoves = true;
                        ChessBoard.updateChessBoard(AvailablePieces[i], AvailableMoves[i][j]);
                        ChessBoard.validatingMoves = false;

                        float evaluation = -Search2(depth - 1, -beta, -alpha);

                        #region Restore State
                        ChessBoard.chessBoard = originalChessBoard.Clone() as char[,];


                        //Reset Casling values
                        ChessBoard.hasKingMoved_white = castlingRights[0];
                        ChessBoard.hasKingMoved_black = castlingRights[1];
                        ChessBoard.hasRookMoved_queenside_white = castlingRights[2];
                        ChessBoard.hasRookMoved_kingside_white = castlingRights[3];
                        ChessBoard.hasRookMoved_queenside_black = castlingRights[4];
                        ChessBoard.hasRookMoved_kingside_black = castlingRights[5];

                        //Reset En Passant Values
                        ChessBoard.EnPassantAvailable = EnPassantAvailable;
                        ChessBoard.pawnToEnPassant = pawnToEnPassant;
                        #endregion

                        if (evaluation >= beta)
                        {
                            //Prune branch
                            return beta;
                        }
                        alpha = Math.Max(alpha, evaluation);


                        //pieceToMove = AvailablePieces[i];
                        //targetIndex = AvailableMoves[i][j];
                        //moveIndex_From = pieceToMove;
                        //moveIndex_To = targetIndex;

                    }

                }
            }

            //Debug.WriteLine(pieceToMove + ",  " + targetIndex);
            //Debug.WriteLine(",  " + alpha);

            //moveIndex_From = pieceToMove;
            //moveIndex_To = targetIndex;

            return alpha;

        }






        //Minimax search algorithm
        public static float Search(int depth, float alpha, float beta, bool maximizingPlayer)
        {
            if (depth == 0)
            {
                return BoardEvaluation.getBoardEvaluation();
            }


            //GET LIST OF AVAILABLE PIECE INDEXES
            List<int> AvailablePieces = getAvailablePieces(ChessBoard.playerTurn);
            //Get Number of possible Moves
            List<List<int>> AvailableMoves = getPieceMoves(AvailablePieces);



            int pieceToMove = ' ';
            int targetIndex = 0;
            //float bestEvaluation = 0;

            //White moves
            if (maximizingPlayer)
            {
                float maxEvaluation = float.NegativeInfinity;

                for (int i = 0; i < AvailablePieces.Count; i++)
                {
                    for (int j = 0; j < AvailableMoves[i].Count; j++)
                    {
                        #region Pawn promotion case
                        //Pawn promotion case
                        if (ChessBoard.getPieceAtIndex(AvailablePieces[i]) == 'P' && ChessBoard.getRank(AvailableMoves[i][j]) == 8)
                        {
                            char[] promoteOptions = { 'Q', 'R', 'B', 'N' };
                            char bestPromotion = ' ';
                            for (int k = 0; k < 4; k++)
                            {
                                #region Save State
                                //Save Casling values
                                bool[] castlingRights = new bool[]
                                {
                                ChessBoard.hasKingMoved_white,
                                ChessBoard.hasKingMoved_black,
                                ChessBoard.hasRookMoved_queenside_white,
                                ChessBoard.hasRookMoved_kingside_white,
                                ChessBoard.hasRookMoved_queenside_black,
                                ChessBoard.hasRookMoved_kingside_black
                                };

                                //Save En Passant Values
                                bool EnPassantAvailable = ChessBoard.EnPassantAvailable;
                                int pawnToEnPassant = ChessBoard.pawnToEnPassant;


                                char[,] originalChessBoard = ChessBoard.chessBoard.Clone() as char[,];
                                #endregion

                                ChessBoard.promotionPiece = promoteOptions[k];
                                ChessBoard.validatingMoves = true;
                                ChessBoard.updateChessBoard(AvailablePieces[i], AvailableMoves[i][j]);
                                ChessBoard.validatingMoves = false;

                                float evaluation = Search(depth - 1, alpha, beta, false);

                                if (evaluation > maxEvaluation)
                                {
                                    bestPromotion = promoteOptions[k];
                                    maxEvaluation = evaluation;
                                    pieceToMove = AvailablePieces[i];
                                    targetIndex = AvailableMoves[i][j];
                                }

                                #region Restore State
                                ChessBoard.chessBoard = originalChessBoard.Clone() as char[,];


                                //Reset Casling values
                                ChessBoard.hasKingMoved_white = castlingRights[0];
                                ChessBoard.hasKingMoved_black = castlingRights[1];
                                ChessBoard.hasRookMoved_queenside_white = castlingRights[2];
                                ChessBoard.hasRookMoved_kingside_white = castlingRights[3];
                                ChessBoard.hasRookMoved_queenside_black = castlingRights[4];
                                ChessBoard.hasRookMoved_kingside_black = castlingRights[5];

                                //Reset En Passant Values
                                ChessBoard.EnPassantAvailable = EnPassantAvailable;
                                ChessBoard.pawnToEnPassant = pawnToEnPassant;
                                #endregion
                                //ChessBoard.changePlayerTurn();
                            }
                            ChessBoard.promotionPiece = bestPromotion;
                        }
                        else if (ChessBoard.getPieceAtIndex(AvailablePieces[i]) == 'p' && ChessBoard.getRank(AvailableMoves[i][j]) == 1)
                        {
                            char[] promoteOptions = { 'q', 'r', 'b', 'n' };
                            char bestPromotion = ' ';
                            for (int k = 0; k < 4; k++)
                            {
                                #region Save State
                                //Save Casling values
                                bool[] castlingRights = new bool[]
                                {
                                ChessBoard.hasKingMoved_white,
                                ChessBoard.hasKingMoved_black,
                                ChessBoard.hasRookMoved_queenside_white,
                                ChessBoard.hasRookMoved_kingside_white,
                                ChessBoard.hasRookMoved_queenside_black,
                                ChessBoard.hasRookMoved_kingside_black
                                };

                                //Save En Passant Values
                                bool EnPassantAvailable = ChessBoard.EnPassantAvailable;
                                int pawnToEnPassant = ChessBoard.pawnToEnPassant;


                                char[,] originalChessBoard = ChessBoard.chessBoard.Clone() as char[,];
                                #endregion

                                ChessBoard.promotionPiece = promoteOptions[k];
                                ChessBoard.validatingMoves = true;
                                ChessBoard.updateChessBoard(AvailablePieces[i], AvailableMoves[i][j]);
                                ChessBoard.validatingMoves = false;

                                float evaluation = Search(depth - 1, alpha, beta, false);


                                if (evaluation > maxEvaluation)
                                {
                                    bestPromotion = promoteOptions[k];
                                    maxEvaluation = evaluation;
                                    pieceToMove = AvailablePieces[i];
                                    targetIndex = AvailableMoves[i][j];
                                }

                                #region Restore State
                                ChessBoard.chessBoard = originalChessBoard.Clone() as char[,];


                                //Reset Casling values
                                ChessBoard.hasKingMoved_white = castlingRights[0];
                                ChessBoard.hasKingMoved_black = castlingRights[1];
                                ChessBoard.hasRookMoved_queenside_white = castlingRights[2];
                                ChessBoard.hasRookMoved_kingside_white = castlingRights[3];
                                ChessBoard.hasRookMoved_queenside_black = castlingRights[4];
                                ChessBoard.hasRookMoved_kingside_black = castlingRights[5];

                                //Reset En Passant Values
                                ChessBoard.EnPassantAvailable = EnPassantAvailable;
                                ChessBoard.pawnToEnPassant = pawnToEnPassant;
                                #endregion
                                //ChessBoard.changePlayerTurn();
                            }
                            ChessBoard.promotionPiece = bestPromotion;
                        }
                        #endregion
                        else
                        {
                            #region Save State
                            //Save Casling values
                            bool[] castlingRights = new bool[]
                            {
                        ChessBoard.hasKingMoved_white,
                        ChessBoard.hasKingMoved_black,
                        ChessBoard.hasRookMoved_queenside_white,
                        ChessBoard.hasRookMoved_kingside_white,
                        ChessBoard.hasRookMoved_queenside_black,
                        ChessBoard.hasRookMoved_kingside_black
                            };

                            //Save En Passant Values
                            bool EnPassantAvailable = ChessBoard.EnPassantAvailable;
                            int pawnToEnPassant = ChessBoard.pawnToEnPassant;


                            char[,] originalChessBoard = ChessBoard.chessBoard.Clone() as char[,];
                            #endregion
                            ChessBoard.validatingMoves = true;
                            ChessBoard.updateChessBoard(AvailablePieces[i], AvailableMoves[i][j]);
                            ChessBoard.validatingMoves = false;

                            float evaluation = Search(depth - 1, alpha, beta, false);
                            maxEvaluation = Math.Max(maxEvaluation, evaluation);
                            
                            #region Restore State
                            ChessBoard.chessBoard = originalChessBoard.Clone() as char[,];


                            //Reset Casling values
                            ChessBoard.hasKingMoved_white = castlingRights[0];
                            ChessBoard.hasKingMoved_black = castlingRights[1];
                            ChessBoard.hasRookMoved_queenside_white = castlingRights[2];
                            ChessBoard.hasRookMoved_kingside_white = castlingRights[3];
                            ChessBoard.hasRookMoved_queenside_black = castlingRights[4];
                            ChessBoard.hasRookMoved_kingside_black = castlingRights[5];

                            //Reset En Passant Values
                            ChessBoard.EnPassantAvailable = EnPassantAvailable;
                            ChessBoard.pawnToEnPassant = pawnToEnPassant;
                            #endregion

                            //alpha = Math.Max(alpha, evaluation);
                            if (evaluation > alpha)
                            {
                                alpha = evaluation;
                                //moveIndex_From = AvailablePieces[i];
                                //moveIndex_To = AvailableMoves[i][j];
                            }


                            if (beta <= alpha)
                            {
                                //Prune branch
                                break;
                            }

                        }

                    }
                }
                return maxEvaluation;
            }
            //Black moves
            else
            {
                float minEvaluation = float.PositiveInfinity;

                for (int i = 0; i < AvailablePieces.Count; i++)
                {
                    for (int j = 0; j < AvailableMoves[i].Count; j++)
                    {
                        #region Pawn promotion case
                        //Pawn promotion case
                        if (ChessBoard.getPieceAtIndex(AvailablePieces[i]) == 'P' && ChessBoard.getRank(AvailableMoves[i][j]) == 8)
                        {
                            char[] promoteOptions = { 'Q', 'R', 'B', 'N' };
                            char bestPromotion = ' ';
                            for (int k = 0; k < 4; k++)
                            {
                                #region Save State
                                //Save Casling values
                                bool[] castlingRights = new bool[]
                                {
                                ChessBoard.hasKingMoved_white,
                                ChessBoard.hasKingMoved_black,
                                ChessBoard.hasRookMoved_queenside_white,
                                ChessBoard.hasRookMoved_kingside_white,
                                ChessBoard.hasRookMoved_queenside_black,
                                ChessBoard.hasRookMoved_kingside_black
                                };

                                //Save En Passant Values
                                bool EnPassantAvailable = ChessBoard.EnPassantAvailable;
                                int pawnToEnPassant = ChessBoard.pawnToEnPassant;


                                char[,] originalChessBoard = ChessBoard.chessBoard.Clone() as char[,];
                                #endregion

                                ChessBoard.promotionPiece = promoteOptions[k];
                                ChessBoard.validatingMoves = true;
                                ChessBoard.updateChessBoard(AvailablePieces[i], AvailableMoves[i][j]);
                                ChessBoard.validatingMoves = false;

                                float evaluation = Search(depth - 1, alpha, beta, true);

                                if (evaluation < minEvaluation)
                                {
                                    bestPromotion = promoteOptions[k];
                                    minEvaluation = evaluation;
                                    pieceToMove = AvailablePieces[i];
                                    targetIndex = AvailableMoves[i][j];
                                }

                                #region Restore State
                                ChessBoard.chessBoard = originalChessBoard.Clone() as char[,];


                                //Reset Casling values
                                ChessBoard.hasKingMoved_white = castlingRights[0];
                                ChessBoard.hasKingMoved_black = castlingRights[1];
                                ChessBoard.hasRookMoved_queenside_white = castlingRights[2];
                                ChessBoard.hasRookMoved_kingside_white = castlingRights[3];
                                ChessBoard.hasRookMoved_queenside_black = castlingRights[4];
                                ChessBoard.hasRookMoved_kingside_black = castlingRights[5];

                                //Reset En Passant Values
                                ChessBoard.EnPassantAvailable = EnPassantAvailable;
                                ChessBoard.pawnToEnPassant = pawnToEnPassant;
                                #endregion
                                //ChessBoard.changePlayerTurn();
                            }
                            ChessBoard.promotionPiece = bestPromotion;
                        }
                        else if (ChessBoard.getPieceAtIndex(AvailablePieces[i]) == 'p' && ChessBoard.getRank(AvailableMoves[i][j]) == 1)
                        {
                            char[] promoteOptions = { 'q', 'r', 'b', 'n' };
                            char bestPromotion = ' ';
                            for (int k = 0; k < 4; k++)
                            {
                                #region Save State
                                //Save Casling values
                                bool[] castlingRights = new bool[]
                                {
                                ChessBoard.hasKingMoved_white,
                                ChessBoard.hasKingMoved_black,
                                ChessBoard.hasRookMoved_queenside_white,
                                ChessBoard.hasRookMoved_kingside_white,
                                ChessBoard.hasRookMoved_queenside_black,
                                ChessBoard.hasRookMoved_kingside_black
                                };

                                //Save En Passant Values
                                bool EnPassantAvailable = ChessBoard.EnPassantAvailable;
                                int pawnToEnPassant = ChessBoard.pawnToEnPassant;


                                char[,] originalChessBoard = ChessBoard.chessBoard.Clone() as char[,];
                                #endregion

                                ChessBoard.promotionPiece = promoteOptions[k];
                                ChessBoard.validatingMoves = true;
                                ChessBoard.updateChessBoard(AvailablePieces[i], AvailableMoves[i][j]);
                                ChessBoard.validatingMoves = false;

                                float evaluation = Search(depth - 1, alpha, beta, true);


                                if (evaluation < minEvaluation)
                                {
                                    bestPromotion = promoteOptions[k];
                                    minEvaluation = evaluation;
                                    pieceToMove = AvailablePieces[i];
                                    targetIndex = AvailableMoves[i][j];
                                }

                                #region Restore State
                                ChessBoard.chessBoard = originalChessBoard.Clone() as char[,];


                                //Reset Casling values
                                ChessBoard.hasKingMoved_white = castlingRights[0];
                                ChessBoard.hasKingMoved_black = castlingRights[1];
                                ChessBoard.hasRookMoved_queenside_white = castlingRights[2];
                                ChessBoard.hasRookMoved_kingside_white = castlingRights[3];
                                ChessBoard.hasRookMoved_queenside_black = castlingRights[4];
                                ChessBoard.hasRookMoved_kingside_black = castlingRights[5];

                                //Reset En Passant Values
                                ChessBoard.EnPassantAvailable = EnPassantAvailable;
                                ChessBoard.pawnToEnPassant = pawnToEnPassant;
                                #endregion
                                //ChessBoard.changePlayerTurn();
                            }
                            ChessBoard.promotionPiece = bestPromotion;
                        }
                        #endregion
                        else
                        {
                            #region Save State
                            //Save Casling values
                            bool[] castlingRights = new bool[]
                            {
                        ChessBoard.hasKingMoved_white,
                        ChessBoard.hasKingMoved_black,
                        ChessBoard.hasRookMoved_queenside_white,
                        ChessBoard.hasRookMoved_kingside_white,
                        ChessBoard.hasRookMoved_queenside_black,
                        ChessBoard.hasRookMoved_kingside_black
                            };

                            //Save En Passant Values
                            bool EnPassantAvailable = ChessBoard.EnPassantAvailable;
                            int pawnToEnPassant = ChessBoard.pawnToEnPassant;


                            char[,] originalChessBoard = ChessBoard.chessBoard.Clone() as char[,];
                            #endregion
                            ChessBoard.validatingMoves = true;
                            ChessBoard.updateChessBoard(AvailablePieces[i], AvailableMoves[i][j]);
                            ChessBoard.validatingMoves = false;

                            float evaluation = Search(depth - 1, alpha, beta, true);
                            minEvaluation = Math.Min(minEvaluation, evaluation);

                            #region Restore State
                            ChessBoard.chessBoard = originalChessBoard.Clone() as char[,];


                            //Reset Casling values
                            ChessBoard.hasKingMoved_white = castlingRights[0];
                            ChessBoard.hasKingMoved_black = castlingRights[1];
                            ChessBoard.hasRookMoved_queenside_white = castlingRights[2];
                            ChessBoard.hasRookMoved_kingside_white = castlingRights[3];
                            ChessBoard.hasRookMoved_queenside_black = castlingRights[4];
                            ChessBoard.hasRookMoved_kingside_black = castlingRights[5];

                            //Reset En Passant Values
                            ChessBoard.EnPassantAvailable = EnPassantAvailable;
                            ChessBoard.pawnToEnPassant = pawnToEnPassant;
                            #endregion

                            //beta = Math.Min(beta, evaluation);
                            if (evaluation < alpha)
                            {
                                alpha = evaluation;
                                //moveIndex_From = AvailablePieces[i];
                                //moveIndex_To = AvailableMoves[i][j];
                            }

                            if (beta <= alpha)
                            {
                                //Prune branch
                                break;
                            }
                        }

                    }
                    
                }
                return minEvaluation;

            }



            

            //Debug.WriteLine(bestEvaluation);

            moveIndex_From = pieceToMove;
            moveIndex_To = targetIndex;
            //Debug.WriteLine(pieceToMove + " to " + targetIndex);

            //Debug.WriteLine("REACHED??????????????????????????");
            return 0;// bestEvaluation;




        }



        public static List<int> getAvailablePieces(char color)
        {
            List<int> AvailablePieces = new List<int>();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (ChessBoard.chessBoard[i, j] != '0')
                    {
                        switch (color)
                        {
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

            return AvailablePieces;
        }


        public static List<List<int>> getPieceMoves(List<int> AvailablePieces)
        {
            List<List<int>> AllAvailableMoves = new List<List<int>>();

            for (int i = 0; i < AvailablePieces.Count; i++)
            {
                AllAvailableMoves.Add(Piece.AvailableMoves(ChessBoard.getPieceAtIndex(AvailablePieces[i]), AvailablePieces[i]));
            }

            return AllAvailableMoves;

        }

    }
}
