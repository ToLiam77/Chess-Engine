using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Chess
{
    internal class MoveGeneration
    {
        public static bool playingVsAI = false;

        public static int moveIndex_From;
        public static int moveIndex_To;

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
    }
}
