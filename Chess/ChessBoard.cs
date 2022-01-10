using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Chess
{
    public static class ChessBoard
    {
        //Globally used variables
        public static bool validatingMoves = false;

        public static char playerTurn = 'w';

        public static int moveCounter = 0; //Counting moves for both sides

        public static bool GameOver = false;
        public static String GameResult;

        public static char promotionPiece = ' ';
        public static bool waitingForPromotionResponse = false;
        public static int pawnToPromote = 0;
        public static int pawnPromoteLocation = 0;

        public static bool EnPassantAvailable = false;
        public static int pawnToEnPassant = 0;

        public static bool hasKingMoved_white = false;
        public static bool hasKingMoved_black = false;
        public static bool hasRookMoved_queenside_white = false;
        public static bool hasRookMoved_kingside_white = false;
        public static bool hasRookMoved_queenside_black = false;
        public static bool hasRookMoved_kingside_black = false;
        ////////////////////////////



        public static char[,] chessBoard = new char[,]
        {
            {'r', 'n', 'b', 'q', 'k', 'b', 'n', 'r'},
            {'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p'},
            {'0', '0', '0', '0', '0', '0', '0', '0'},
            {'0', '0', '0', '0', '0', '0', '0', '0'},
            {'0', '0', '0', '0', '0', '0', '0', '0'},
            {'0', '0', '0', '0', '0', '0', '0', '0'},
            {'P', 'P', 'P', 'P', 'P', 'P', 'P', 'P'},
            {'R', 'N', 'B', 'Q', 'K', 'B', 'N', 'R'}
        };

        public static void updateChessBoard(int startingIndex, int targetIndex)
        {
            char pieceToMove = getPieceAtIndex(startingIndex);
            char squareToOcupy = getPieceAtIndex(targetIndex);

            //MAKE UPDATE
            chessBoard[8 - getRank(startingIndex), getFile(startingIndex) - 1] = '0';
            chessBoard[8 - getRank(targetIndex), getFile(targetIndex) - 1] = pieceToMove;

            if (!validatingMoves)
            {
                moveCounter++;
            }


            //EN PASSANT
            if (!validatingMoves)
            {
                EnPassantAvailable = false;
            }
            
            if (pieceToMove == 'p' || pieceToMove == 'P')
            {
                //Storing previously moved pawn for En Passant
                if (Math.Abs(getRank(startingIndex) - getRank(targetIndex)) == 2)
                {
                    EnPassantAvailable = true;
                    pawnToEnPassant = targetIndex;
                }

                //En Passant
                if (squareToOcupy == '0')
                {
                    if (Math.Abs(startingIndex - targetIndex) == 7 || Math.Abs(startingIndex - targetIndex) == 9)
                    {
                        chessBoard[8 - getRank(pawnToEnPassant), getFile(pawnToEnPassant) - 1] = '0';
                    }
                }
               
            }


            //PAWN PROMOTION
            if (pieceToMove == 'P' && getRank(targetIndex) == 8)
            {
                chessBoard[0, getFile(targetIndex) - 1] = promotionPiece;
            }
            else if (pieceToMove == 'p' && getRank(targetIndex) == 1)
            {
                chessBoard[7, getFile(targetIndex) - 1] = promotionPiece;
            }


            //CASTLING
            switch (pieceToMove)
            {
                case 'k':
                    //Castling (black)
                    if (Math.Abs(startingIndex - targetIndex) == 2)
                    {
                        if (targetIndex > startingIndex)
                        {
                            chessBoard[0, 7] = '0';
                            chessBoard[0, 5] = 'r';
                        }
                        else
                        {
                            chessBoard[0, 0] = '0';
                            chessBoard[0, 3] = 'r';
                        }
                    }
                    break;

                case 'K':
                    //Castling (white)
                    if (Math.Abs(startingIndex - targetIndex) == 2)
                    {
                        Debug.Write("test");
                        if (targetIndex > startingIndex)
                        {
                            chessBoard[7, 7] = '0';
                            chessBoard[7, 5] = 'R';
                        }
                        else
                        {
                            chessBoard[7, 0] = '0';
                            chessBoard[7, 3] = 'R';
                        }
                    }
                    break;

            }

            //Removing castling rights
            if (!validatingMoves)
            {
                switch (pieceToMove)
                {
                    case 'k':
                        hasKingMoved_black = true;
                        break;

                    case 'K':
                        hasKingMoved_white = true;
                        break;

                    case 'r':
                        if (getFile(startingIndex) == 1)
                        {
                            hasRookMoved_queenside_black = true;
                        }
                        else if (getFile(startingIndex) == 8)
                        {
                            hasRookMoved_kingside_black = true;
                        }
                        break;

                    case 'R':
                        if (getFile(startingIndex) == 1)
                        {
                            hasRookMoved_queenside_white = true;
                        }
                        else if (getFile(startingIndex) == 8)
                        {
                            hasRookMoved_kingside_white = true;
                        }
                        break;

                }

            }
            


            //CHECK FOR END OF GAME
            if (!validatingMoves)
            {
                changePlayerTurn();

                char movedColor = getPieceColor(targetIndex);
                List<int> attackedSquares = Piece.getAttackedSquares(movedColor);


                for (int i = 0; i < attackedSquares.Count; i++)
                {
                    //Verifying if oponent has available moves
                    for (int j = 0; j < 8; j++)
                    {
                        for (int k = 0; k < 8; k++)
                        {
                            switch (playerTurn)
                            {
                                case 'w':
                                    if (Char.IsUpper(chessBoard[j, k]))
                                    {
                                        if (Piece.AvailableMoves(chessBoard[j, k], getSquareIndex(j, k)).Count > 0)
                                        {
                                            checkDrawByInsufficientMaterial();
                                            return;
                                        }
                                    }
                                    break;

                                case 'b':
                                    if (Char.IsLower(chessBoard[j, k]))
                                    {
                                        if (Piece.AvailableMoves(chessBoard[j, k], getSquareIndex(j, k)).Count > 0)
                                        {
                                            checkDrawByInsufficientMaterial();
                                            return;
                                        }
                                    }
                                    break;
                            }

                        }
                    }

                    GameOver = true;

                    //If no available moves, see if checkmate or stalemate 
                    if (attackedSquares[i] == getKingLocation(getOppositeColor(movedColor)))
                    {
                        String movedColorString = "";
                        switch (movedColor)
                        {
                            case 'b':
                                movedColorString = "Black";
                                break;
                            case 'w':
                                movedColorString = "White";
                                break;
                        }
                        GameResult = movedColorString + " wins by checkmate";
                        return;
                    }
                    else
                    {
                        GameResult = "Game is drawn by stalemate";
                    }
                }

                
            }

        }
        

        public static void changePlayerTurn()
        {
            if (playerTurn == 'w')
            {
                playerTurn = 'b';
            }
            else if (playerTurn == 'b')
            {
                playerTurn = 'w';
            }
        }

        public static char getPieceAtIndex(int SquareIndex)
        {
            return chessBoard[8 - getRank(SquareIndex), getFile(SquareIndex) - 1];
        }

        public static char getPieceColor(int SquareIndex)
        {
            char selectedSquare = getPieceAtIndex(SquareIndex);

            if (Char.IsUpper(selectedSquare))
            {
                return 'w';
            }
            else if (Char.IsLower(selectedSquare))
            {
                return 'b';
            }

            return '0';
        }

        public static char getOppositeColor(char color)
        {
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
        public static int getKingLocation(char color)
        {
            char pieceToSearch = '0';

            if (color == 'w')
            {
                pieceToSearch = 'K';
            }
            else if (color == 'b')
            {
                pieceToSearch = 'k';
            }

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (chessBoard[i, j] == pieceToSearch)
                    {
                        return getSquareIndex(i, j);
                    }
                }
            }

            return 0;
        }

        //Board Index
        public static int getSquareIndex(int i, int j)
        {
            int[,] BoardIndex = new int[,]
            {
                {56, 57, 58, 59, 60, 61, 62, 63},
                {48, 49, 50, 51, 52, 53, 54, 55},
                {40, 41, 42, 43, 44, 45, 46, 47},
                {32, 33, 34, 35, 36, 37, 38, 39},
                {24, 25, 26, 27, 28, 29, 30, 31},
                {16, 17, 18, 19, 20, 21, 22, 23},
                {8,  9,  10, 11, 12, 13, 14, 15},
                {0,  1,  2,  3,  4,  5,  6,  7 }
            };

            return BoardIndex[i, j];
        }



        public static int getRank(int SquareIndex)
        {
            return (SquareIndex / 8) + 1;
        }

        public static int getFile(int SquareIndex)
        {
            return (SquareIndex % 8) + 1;
        }



        public static int ChessNotationToIndex(String squareInNotation)
        {
            int squareIndex = 0;
            char[] components = squareInNotation.ToCharArray();

            squareIndex += 8 * (components[1] - 1);
            squareIndex += char.ToUpper(components[0]) - 64;

            return squareIndex;
        }


        public static void setupBoardFEN(String fen)
        {
            String[] FenSubsections = fen.Split(' ');

            //Piece placement
            String[] rankRayout = FenSubsections[0].Split('/');

            int updatingX = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < rankRayout[i].Length; j++)
                {
                    if (Char.IsDigit(rankRayout[i][j]))
                    {
                        int skipDigit = rankRayout[i][j] - '0';
                        for (int k = 0; k < skipDigit; k++)
                        {
                            chessBoard[i, updatingX] = '0';
                            updatingX++;
                        }
                        
                    }
                    else
                    {
                        chessBoard[i, updatingX] = rankRayout[i][j];
                        updatingX++;
                    }
                    
                }
                updatingX = 0;

            }

            //Player Turn
            if (FenSubsections[1].Contains('w'))
            {
                playerTurn = 'w';
            }
            else
            {
                playerTurn = 'b';
            }



            //Castling permissions
            if (FenSubsections[2] == "-")
            {
                hasKingMoved_white = true;
                hasKingMoved_black = true;
            }
            else
            {
                if (!FenSubsections[2].Contains("K"))
                {
                    hasRookMoved_kingside_white = true;
                }

                if (!FenSubsections[2].Contains("Q"))
                {
                    hasRookMoved_queenside_white = true;
                }
                
                if (!FenSubsections[2].Contains("k"))
                {
                    hasRookMoved_kingside_black = true;
                }

                if (!FenSubsections[2].Contains("q"))
                {
                    hasRookMoved_queenside_black = true;
                }
            }

            //En Passant pawn
            if (FenSubsections[3] != "-")
            {
                if (playerTurn == 'w')
                {
                    pawnToEnPassant = ChessNotationToIndex(FenSubsections[3]) - 8;
                }
                else
                {
                    pawnToEnPassant = ChessNotationToIndex(FenSubsections[3]) + 8;
                }
            }
            

            //Move Count
            moveCounter = Int32.Parse(FenSubsections[4]) + Int32.Parse(FenSubsections[5]);
        }

        public static void checkDrawByInsufficientMaterial()
        {
            //Draw by insufficient material
            List<char> PiecesOnBoard = new List<char>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (chessBoard[i, j] != '0')
                    {
                        PiecesOnBoard.Add(chessBoard[i, j]);
                    }
                }
            }

            List<List<char>> materialCombinations = new List<List<char>>();
            materialCombinations.Add(new List<char> { 'K', 'k' });
            materialCombinations.Add(new List<char> { 'K', 'k', 'b' });
            materialCombinations.Add(new List<char> { 'K', 'B', 'k' });
            materialCombinations.Add(new List<char> { 'K', 'k', 'n' });
            materialCombinations.Add(new List<char> { 'K', 'N', 'k' });
            materialCombinations.Add(new List<char> { 'K', 'B', 'k', 'b' });
            materialCombinations.Add(new List<char> { 'K', 'N', 'k', 'n' });
            materialCombinations.Add(new List<char> { 'K', 'R', 'k', 'r' });
            materialCombinations.Add(new List<char> { 'K', 'R', 'k', 'n' });
            materialCombinations.Add(new List<char> { 'K', 'R', 'k', 'b' });
            materialCombinations.Add(new List<char> { 'K', 'N', 'k', 'r' });
            materialCombinations.Add(new List<char> { 'K', 'B', 'k', 'r' });
            materialCombinations.Add(new List<char> { 'K', 'N', 'N', 'k' });
            materialCombinations.Add(new List<char> { 'K', 'k', 'n', 'n' });



            for (int i = 0;i < materialCombinations.Count; i++)
            {
                bool isInsufficientMaterial = Enumerable.SequenceEqual(PiecesOnBoard.OrderBy(e => e), materialCombinations[i].OrderBy(e => e));

                if (isInsufficientMaterial)
                {
                    GameOver = true;
                    GameResult = "Draw by insufficient material";
                }
            }
            
        }

    }
}
