using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Chess
{
    internal class BoardEvaluation
    {
        public static float Evaluation = 0;

        public static void getBoardEvaluation()
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

            //CHECK FOR CHECKMATE / STALEMATE
            if (ChessBoard.GameOver)
            {
                if (ChessBoard.GameResult.Contains("stalemate"))
                {
                    Evaluation = 0;
                }

                if (ChessBoard.GameResult.Contains("checkmate"))
                {

                }
            }
            



            //Debug.WriteLine(Evaluation);
        }
    }
}
