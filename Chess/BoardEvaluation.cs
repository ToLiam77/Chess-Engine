using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Chess
{
    internal class BoardEvaluation
    {
        public static float Evaluation = 0;

        public static float getBoardEvaluation()
        {
            Evaluation = 0;

            //COUNT PIECE VALUES
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    switch (ChessBoard.chessBoard[i, j])
                    {
                        case 'P':
                            Evaluation++;
                            break;

                        case 'R':
                            Evaluation += 5;
                            break;

                        case 'N':
                            Evaluation += 3;
                            break;

                        case 'B':
                            Evaluation += 3;
                            break;

                        case 'Q':
                            Evaluation += 9;
                            break;

                        case 'p':
                            Evaluation--;
                            break;

                        case 'r':
                            Evaluation -= 5;
                            break;

                        case 'n':
                            Evaluation -= 3;
                            break;

                        case 'b':
                            Evaluation -= 3;
                            break;

                        case 'q':
                            Evaluation -= 9;
                            break;

                    }
                }
            }



            //POSITIONAL (legal moves count)
            float posRatio = 0.03f;

            int[,] heatMapCenter = new int[,]
            {
                {1, 1, 1, 1, 1, 1, 1, 1 },
                {1, 1, 1, 1, 1, 1, 1, 1 },
                {1, 1, 2, 2, 2, 2, 1, 1 },
                {1, 1, 2, 3, 3, 2, 1, 1 },
                {1, 1, 2, 3, 3, 2, 1, 1 },
                {1, 1, 2, 2, 2, 2, 1, 1 },
                {1, 1, 1, 1, 1, 1, 1, 1 },
                {1, 1, 1, 1, 1, 1, 1, 1 }
            };

            List<int> AttackedSquaresWhite = Piece.getAttackedSquares('w');
            for (int i = 0; i < AttackedSquaresWhite.Count; i++)
            {
                Evaluation += posRatio * heatMapCenter[8 - ChessBoard.getRank(AttackedSquaresWhite[i]), ChessBoard.getFile(AttackedSquaresWhite[i]) - 1];
            }

            List<int> AttackedSquaresBlack = Piece.getAttackedSquares('b');
            for (int i = 0; i < AttackedSquaresBlack.Count; i++)
            {
                Evaluation += -posRatio * heatMapCenter[8 - ChessBoard.getRank(AttackedSquaresBlack[i]), ChessBoard.getFile(AttackedSquaresBlack[i]) - 1];
            }




            //ENCOURAGE KING CASTLING 
            int[,] kingPositionMap = new int[,]
            {
                {1, 1, 2, 1, 1, 1, 2, 1 },
                {1, 1, 1, 1, 1, 1, 1, 1 },
                {1, 1, 1, 1, 1, 1, 1, 1 },
                {1, 1, 1, 1, 1, 1, 1, 1 },
                {1, 1, 1, 1, 1, 1, 1, 1 },
                {1, 1, 1, 1, 1, 1, 1, 1 },
                {1, 1, 1, 1, 1, 1, 1, 1 },
                {1, 1, 2, 1, 1, 1, 2, 1 }
            };

            Evaluation += 0.3f * kingPositionMap[8 - ChessBoard.getRank(ChessBoard.getKingLocation('w')), ChessBoard.getFile(ChessBoard.getKingLocation('w')) - 1];
            Evaluation -= 0.3f * kingPositionMap[8 - ChessBoard.getRank(ChessBoard.getKingLocation('b')), ChessBoard.getFile(ChessBoard.getKingLocation('b')) - 1];



            //DISCOURAGE EARLY QUEEN MOVE
            if (ChessBoard.moveCounter < 10)
            {
                //Black
                if (ChessBoard.chessBoard[0, 3] != 'q')
                {
                    Evaluation += 0.3f;
                }

                //White
                if (ChessBoard.chessBoard[7, 3] != 'Q')
                {
                    Evaluation -= 0.3f;
                }
            }



            return Evaluation;
        }
    }
}
