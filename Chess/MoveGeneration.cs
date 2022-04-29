using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace Chess
{
    internal class MoveGeneration
    {
        public static bool playingVsAI = false;

        public static int moveIndex_From;
        public static int moveIndex_To;

        public static int SearchDepth = 3;

        //Random move generation used for testing
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

            //Pawn promotion case (pick random promotion)
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

            //Count number of pieces for dynamic depth
            List<int> AvailablePiecesWhite = getAvailablePieces('w');
            List<int> AvailablePiecesBlack = getAvailablePieces('b');
            int pieceCount = AvailablePiecesWhite.Count + AvailablePiecesBlack.Count;
            if (pieceCount < 10) SearchDepth = 5;

            //Find and execute move
            SelectMove(SearchDepth);


            stopWatch.Stop();
            Debug.WriteLine("Move Selected Was " + moveIndex_From + " to " + moveIndex_To);
            Debug.WriteLine("Time taken: " + stopWatch.ElapsedMilliseconds + " millisecons");


            ChessBoard.updateChessBoard(moveIndex_From, moveIndex_To);

        }


        public static void SelectMove(int depth)
        {
            char playerTurn = ChessBoard.playerTurn;


            //GET LIST OF AVAILABLE PIECE INDEXES
            List<int> AvailablePieces = getAvailablePieces(ChessBoard.playerTurn);
            //Get Number of possible Moves
            List<List<int>> AvailableMoves = getPieceMoves(AvailablePieces);


            //Select most favourable move
            float bestScore = float.PositiveInfinity;

            for (int i = 0; i < AvailablePieces.Count; i++)
            {
                for (int j = 0; j < AvailableMoves[i].Count; j++)
                {
                    Debug.WriteLine("Analyzing move " + AvailablePieces[i] + " to " + AvailableMoves[i][j]);


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

                            ChessBoard.changePlayerTurn();
                            ChessBoard.validatingMoves = false;

                            float score = Search(depth - 1, float.NegativeInfinity, float.PositiveInfinity, true);
                            Debug.WriteLine("Score promotion " + promoteOptions[k] + ": " + score);

                            if (score < bestScore)
                            {
                                bestPromotion = promoteOptions[k];
                                bestScore = score;
                                moveIndex_From = AvailablePieces[i];
                                moveIndex_To = AvailableMoves[i][j];
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
                            ChessBoard.changePlayerTurn();
                            ChessBoard.validatingMoves = false;

                            float score = Search(depth - 1, float.NegativeInfinity, float.PositiveInfinity, true);
                            Debug.WriteLine("Score promotion " + promoteOptions[k] + ": " + score);

                            if (score < bestScore)
                            {
                                bestPromotion = promoteOptions[k];
                                bestScore = score;
                                moveIndex_From = AvailablePieces[i];
                                moveIndex_To = AvailableMoves[i][j];
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
                        ChessBoard.changePlayerTurn();
                        ChessBoard.validatingMoves = false;

                        float score = Search(depth - 1, float.NegativeInfinity, float.PositiveInfinity, true);
                        
                        Debug.WriteLine("Score: " + score);

                        if (score < bestScore)
                        {
                            bestScore = score;
                            moveIndex_From = AvailablePieces[i];
                            moveIndex_To = AvailableMoves[i][j];
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

                    }

                    Debug.WriteLine("------------------------");

                }
            }


            ChessBoard.playerTurn = playerTurn;

        }


        //Quiescence search
        public static float SearchCaptureMoves(float alpha, float beta)
        {
            float evaluation = BoardEvaluation.getBoardEvaluation();
            if (evaluation >= beta)
            {
                return beta;
            }
            alpha = Math.Min(alpha,evaluation);


            //GET LIST OF AVAILABLE PIECE INDEXES
            List<int> AvailablePieces = getAvailablePieces(ChessBoard.playerTurn);
            //Get Number of possible capture Moves
            List<List<int>> AvailableCaptureMoves = getPieceCaptureMoves(AvailablePieces);




            for (int i = 0; i < AvailablePieces.Count; i++)
            {
                for (int j = 0; j < AvailableCaptureMoves[i].Count; j++)
                {
                    #region Pawn promotion case
                    //Pawn promotion case
                    if (ChessBoard.getPieceAtIndex(AvailablePieces[i]) == 'P' && ChessBoard.getRank(AvailableCaptureMoves[i][j]) == 8)
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
                            ChessBoard.updateChessBoard(AvailablePieces[i], AvailableCaptureMoves[i][j]);
                            ChessBoard.validatingMoves = false;

                            evaluation = -SearchCaptureMoves(-beta, -alpha);

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
                    else if (ChessBoard.getPieceAtIndex(AvailablePieces[i]) == 'p' && ChessBoard.getRank(AvailableCaptureMoves[i][j]) == 1)
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
                            ChessBoard.updateChessBoard(AvailablePieces[i], AvailableCaptureMoves[i][j]);
                            ChessBoard.validatingMoves = false;

                            evaluation = -SearchCaptureMoves(-beta, -alpha);

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
                        ChessBoard.updateChessBoard(AvailablePieces[i], AvailableCaptureMoves[i][j]);
                        ChessBoard.validatingMoves = false;

                        float evaluationScore = -SearchCaptureMoves(-beta, -alpha);

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

                        if (evaluationScore >= beta)
                        {
                            //Prune branch
                            return beta;
                        }
                        alpha = Math.Max(alpha, evaluationScore);

                    }

                }
            }

            return alpha;



        }



        //Minimax search algorithm
        public static float Search(int depth, float alpha, float beta, bool maximizingPlayer)
        {
            if (depth == 0)
            {
                return BoardEvaluation.getBoardEvaluation();
                //Quiescence search not used due to reduced performance
                //SearchCaptureMoves(alpha, beta);
            }


            //GET LIST OF AVAILABLE PIECE INDEXES
            List<int> AvailablePieces = getAvailablePieces(ChessBoard.playerTurn);
            //Get Number of possible Moves
            List<List<int>> AvailableMoves = getPieceMoves(AvailablePieces);



            //CHECK FOR CHECKMATE / STALEMATE
            int numberOfMoves = 0;
            for (int i = 0; i < AvailablePieces.Count; i++)
            {
                for (int j = 0; j < AvailableMoves[i].Count; j++)
                {
                    numberOfMoves++;
                }
            }

            if (numberOfMoves == 0)
            {
                //Checkmate
                if (Piece.getAttackedSquares(ChessBoard.getOppositeColor(ChessBoard.playerTurn)).Contains(ChessBoard.getKingLocation(ChessBoard.playerTurn)))
                {
                    switch (ChessBoard.playerTurn)
                    {
                        case 'w':
                            return float.NegativeInfinity;

                        case 'b':
                            return float.PositiveInfinity;
                    }
                }
                //Stalemate
                else
                {
                    return 0;
                }

            }



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

                                beta = Math.Min(beta, evaluation);

                                if (beta <= alpha)
                                {
                                    //Prune branch
                                    break;
                                }

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

                                beta = Math.Min(beta, evaluation);

                                if (beta <= alpha)
                                {
                                    //Prune branch
                                    break;
                                }

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
                            ChessBoard.playerTurn = 'w';
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


                            alpha = Math.Max(alpha, evaluation);

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
                                ChessBoard.playerTurn = 'b';
                                ChessBoard.promotionPiece = promoteOptions[k];
                                ChessBoard.validatingMoves = true;
                                ChessBoard.updateChessBoard(AvailablePieces[i], AvailableMoves[i][j]);
                                ChessBoard.validatingMoves = false;

                                float evaluation = Search(depth - 1, alpha, beta, true);

                                if (evaluation < minEvaluation)
                                {
                                    bestPromotion = promoteOptions[k];
                                    minEvaluation = evaluation;
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

                                beta = Math.Min(beta, evaluation);

                                if (beta <= alpha)
                                {
                                    //Prune branch
                                    break;
                                }

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
                                ChessBoard.playerTurn = 'b';
                                ChessBoard.promotionPiece = promoteOptions[k];
                                ChessBoard.validatingMoves = true;
                                ChessBoard.updateChessBoard(AvailablePieces[i], AvailableMoves[i][j]);
                                ChessBoard.validatingMoves = false;

                                float evaluation = Search(depth - 1, alpha, beta, true);


                                if (evaluation < minEvaluation)
                                {
                                    bestPromotion = promoteOptions[k];
                                    minEvaluation = evaluation;
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

                                beta = Math.Min(beta, evaluation);

                                if (beta <= alpha)
                                {
                                    //Prune branch
                                    break;
                                }

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
                            ChessBoard.playerTurn = 'b';
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

                            
                            beta = Math.Min(beta, evaluation);

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


        //All moves
        public static List<List<int>> getPieceMoves(List<int> AvailablePieces)
        {
            List<List<int>> AllAvailableMoves = new List<List<int>>();

            for (int i = 0; i < AvailablePieces.Count; i++)
            {
                AllAvailableMoves.Add(Piece.AvailableMoves(ChessBoard.getPieceAtIndex(AvailablePieces[i]), AvailablePieces[i]));
            }

            return AllAvailableMoves;

        }


        //Capture moves
        public static List<List<int>> getPieceCaptureMoves(List<int> AvailablePieces)
        {
            List<List<int>> AllAvailableCaptureMoves = new List<List<int>>();

            for (int i = 0; i < AvailablePieces.Count; i++)
            {
                AllAvailableCaptureMoves.Add(Piece.AvailableCaptureMoves(ChessBoard.getPieceAtIndex(AvailablePieces[i]), AvailablePieces[i]));
            }

            return AllAvailableCaptureMoves;

        }
    }
}
