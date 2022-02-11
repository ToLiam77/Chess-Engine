using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chess;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Diagnostics;


namespace Chess_Test
{
    [TestClass]
    public class ChessRulesTests
    {
        public static void countPossiblePositions(long[] NodesAtDepth, string FEN)
        {

            for (int i = 0; i < NodesAtDepth.Length; i++)
            {
                ChessBoard.setupBoardFEN(FEN);
                long foundPositions = getNodesAtDepth(i + 1);

                Console.WriteLine("Number of moves found at depth " + (i + 1) + ": " + foundPositions);
                Console.WriteLine("Correct number of moves at depth " + (i + 1) + ": " + NodesAtDepth[i]);
                Console.WriteLine("");
                Assert.IsTrue(foundPositions == NodesAtDepth[i]);
            }

        }

        public static int getNodesAtDepth(int depth)
        {
            int numPositions = 0;

            if (depth == 0)
            {
                return 1;
            }
            

            //GET LIST OF AVAILABLE PIECE INDEXES
            List<int> AvailablePieces = getAvailablePieces(ChessBoard.playerTurn);

            //Get Number of possible Moves
            List<List<int>> AvailableMoves = getPieceMoves(AvailablePieces);


            for (int i = 0; i < AvailablePieces.Count; i++)
            {
                for (int j = 0; j < AvailableMoves[i].Count; j++)
                {

                    #region Pawn promotion case
                    //Pawn promotion case
                    if (ChessBoard.getPieceAtIndex(AvailablePieces[i]) == 'P' && ChessBoard.getRank(AvailableMoves[i][j]) == 8)
                    {
                        char[] promoteOptions = { 'Q', 'R', 'B', 'N' };
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

                            ChessBoard.promotionPiece =  promoteOptions[k];
                            ChessBoard.updateChessBoard(AvailablePieces[i], AvailableMoves[i][j]);

                            int nodesAtDepth = getNodesAtDepth(depth - 1);

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

                            numPositions += nodesAtDepth;
                            ChessBoard.changePlayerTurn();
                        }
                    }
                    else if (ChessBoard.getPieceAtIndex(AvailablePieces[i]) == 'p' && ChessBoard.getRank(AvailableMoves[i][j]) == 1)
                    {
                        char[] promoteOptions = { 'q', 'r', 'b', 'n' };
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
                            ChessBoard.updateChessBoard(AvailablePieces[i], AvailableMoves[i][j]);

                            int nodesAtDepth = getNodesAtDepth(depth - 1);

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

                            numPositions += nodesAtDepth;
                            ChessBoard.changePlayerTurn();
                        }
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

                        ChessBoard.updateChessBoard(AvailablePieces[i], AvailableMoves[i][j]);

                        int nodesAtDepth = getNodesAtDepth(depth - 1);

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

                        numPositions += nodesAtDepth;
                        ChessBoard.changePlayerTurn();
                    }

                }
            }


            return numPositions;
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



        //TESTS
        [TestMethod]
        public void Position1()
        {
            string FEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
            ChessBoard.setupBoardFEN(FEN);

            //Nodes to expect at depths
            long[] NodesAtDepth = { 20, 400, 8902, 197281 }; //, 4865609 }; 119060324, 3195901860 };


            //SetCastlingValues();
            countPossiblePositions(NodesAtDepth, FEN);
        }

        [TestMethod]
        public void Position2()
        {
            string FEN = "r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq - 0 1";
            ChessBoard.setupBoardFEN(FEN);

            //Nodes to expect at depths
            long[] NodesAtDepth = { 48, 2039, 97862 }; //, 4085603, 193690690, 8031647685 };


            //SetCastlingValues();
            countPossiblePositions(NodesAtDepth, FEN);
        }

        [TestMethod]
        public void Position3()
        {
            string FEN = "8/2p5/3p4/KP5r/1R3p1k/8/4P1P1/8 w - - 0 1";
            ChessBoard.setupBoardFEN(FEN);

            //Nodes to expect at depths
            long[] NodesAtDepth = { 14, 191, 2812, 43238, 674624 };


            //SetCastlingValues();
            countPossiblePositions(NodesAtDepth, FEN);
        }

        [TestMethod]
        public void Position4()
        {
            string FEN = "r3k2r/Pppp1ppp/1b3nbN/nP6/BBP1P3/q4N2/Pp1P2PP/R2Q1RK1 w kq - 0 1";
            ChessBoard.setupBoardFEN(FEN);

            //Nodes to expect at depths
            long[] NodesAtDepth = { 6, 264, 9467, 422333 };


            //SetCastlingValues();
            countPossiblePositions(NodesAtDepth, FEN);
        }

        [TestMethod]
        public void Position5()
        {
            string FEN = "rnbq1k1r/pp1Pbppp/2p5/8/2B5/8/PPP1NnPP/RNBQK2R w KQ - 1 8";
            ChessBoard.setupBoardFEN(FEN);

            //Nodes to expect at depths
            long[] NodesAtDepth = { 44, 1486, 62379 };


            //SetCastlingValues();
            countPossiblePositions(NodesAtDepth, FEN);
        }

        [TestMethod]
        public void Position6()
        {
            string FEN = "r4rk1/1pp1qppp/p1np1n2/2b1p1B1/2B1P1b1/P1NP1N2/1PP1QPPP/R4RK1 w - - 0 10 ";
            ChessBoard.setupBoardFEN(FEN);

            //Nodes to expect at depths
            long[] NodesAtDepth = { 46, 2079, 89890 };


            //SetCastlingValues();
            countPossiblePositions(NodesAtDepth, FEN);
        }

        [TestMethod]
        public void TestCase_Castle()
        {
            string FEN = "k7/p1p5/P1P5/8/8/7p/7P/4K2R w K - 0 1";
            ChessBoard.setupBoardFEN(FEN);

            //Nodes to expect at depths
            long[] NodesAtDepth = { 8, 8, 98, 191, 2870};


            //SaveCastlingValues();
            countPossiblePositions(NodesAtDepth, FEN);
        }

        [TestMethod]
        public void TestCase_Promotion()
        {
            string FEN = "k1b5/1p1p1P2/pP1P4/P7/8/8/8/1K6 w - - 0 1";
            ChessBoard.setupBoardFEN(FEN);

            //Nodes to expect at depths
            long[] NodesAtDepth = { 9, 9, 103, 105, 1492, 1599, 26440, 35530 };


            //SaveCastlingValues();
            countPossiblePositions(NodesAtDepth, FEN);
        }

    }
}
