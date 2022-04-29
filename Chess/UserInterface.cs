using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess
{
    public partial class UserInterface : Form
    {

        public static bool wasMoveMade = false;
        PictureBox previousMove_From;
        PictureBox previousMove_To;

        public UserInterface()
        {
            InitializeComponent();
        }

        PictureBox[,] Tile;
        PictureBox selectedPiece = null;


    private void Form1_Load(object sender, EventArgs e)
        {
            //Select Game mode
            ModePicker pickMode = new ModePicker();
            pickMode.ShowDialog();


            //Colors
            var dark = Color.DarkSeaGreen;
            var white = Color.Honeydew;

            var dark_MoveIndicator = ColorTranslator.FromHtml("#b8db67");
            var white_MoveIndicator = ColorTranslator.FromHtml("#faff78");

            var dark_OptionIndicator = Color.Tan;
            var white_OptionIndicator = Color.Wheat;


            //Drawing chessboard
            int n = 8;
            Tile = new PictureBox[n, n];
            int left = 2, top = 2;


            Color[] colors = new Color[2];

            for (int i = 0; i < n; i++)
            {
                left = 2;
                if (i % 2 == 0)
                {
                    colors[0] = dark;
                    colors[1] = white;
                }
                else
                {
                    colors[0] = white;
                    colors[1] = dark;
                }

                for (int j = 0; j < n; j++)
                {
                    Tile[i, j] = new PictureBox
                    {
                        BackColor = colors[(j % 2 == 0) ? 1 : 0],
                        Location = new Point(left, top),
                        Size = new Size(60, 60),
                        Name = ChessBoard.getSquareIndex(i, j).ToString()
                    
                    };

                    left += 60;
                    Board.Controls.Add(Tile[i, j]);

                    Tile[i, j].SizeMode = PictureBoxSizeMode.CenterImage;

                    //Mouse hover color change
                    Color tileColor = Tile[i, j].BackColor;

                    Tile[i, j].MouseHover += (sender2, e2) =>
                    {
                        PictureBox p = sender2 as PictureBox;
                        if (p.Image != null && p.BackColor == white || p.Image != null && p.BackColor == dark)
                            p.BackColor = Color.LightSalmon;
                    };

                    Tile[i, j].MouseLeave += (sender2, e2) =>
                    {
                        PictureBox p = sender2 as PictureBox;
                        if (p.BackColor == Color.LightSalmon) p.BackColor = tileColor;
                    };



                    //Tile Click 
                    Tile[i, j].Click += (sender3, e3) =>
                    {
                        wasMoveMade = false;
                        PictureBox clickedTile = sender3 as PictureBox;

                        int PieceIndex = Int32.Parse(clickedTile.Name);

                        //Piece Move or capture
                        if (clickedTile.BackColor == Color.Wheat || clickedTile.BackColor == Color.Tan)
                        {
                            if (selectedPiece != clickedTile)
                            {
                                //Ask to choose promotion piece
                                switch (ChessBoard.getPieceAtIndex(Int32.Parse(selectedPiece.Name)))
                                {
                                    case 'P':
                                        if (ChessBoard.getRank(PieceIndex) == 8)
                                        {
                                            ChessBoard.pawnToPromote = Int32.Parse(selectedPiece.Name);
                                            ChessBoard.pawnPromoteLocation = Int32.Parse(clickedTile.Name);

                                            PromotionPiecePicker promotionPiece = new PromotionPiecePicker();
                                            ChessBoard.waitingForPromotionResponse = true;
                                            promotionPiece.ShowDialog();

                                            previousMove_From = selectedPiece;
                                            previousMove_To = clickedTile;
                                            SetupBoard();
                                        }
                                        break;
                                    case 'p':
                                        if (ChessBoard.getRank(PieceIndex) == 1)
                                        {
                                            ChessBoard.pawnToPromote = Int32.Parse(selectedPiece.Name);
                                            ChessBoard.pawnPromoteLocation = Int32.Parse(clickedTile.Name);

                                            PromotionPiecePicker promotionPiece = new PromotionPiecePicker();
                                            ChessBoard.waitingForPromotionResponse = true;
                                            promotionPiece.ShowDialog();

                                            previousMove_From = selectedPiece;
                                            previousMove_To = clickedTile;
                                            SetupBoard();
                                        }
                                        break;
                                }

                                //Make move
                                if (!ChessBoard.waitingForPromotionResponse)
                                {
                                    ChessBoard.updateChessBoard(Int32.Parse(selectedPiece.Name), Int32.Parse(clickedTile.Name));
                                    SetupBoard();
                                    previousMove_From = selectedPiece;
                                    previousMove_To = clickedTile;
                                }
                                ChessBoard.waitingForPromotionResponse = false;

                                //Game is finished
                                if (ChessBoard.GameOver)
                                {
                                    string message = ChessBoard.GameResult;
                                    string title = "Game over";
                                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                                    DialogResult result = MessageBox.Show(message, title, buttons);

                                    if (result == DialogResult.OK)
                                    {
                                        this.Close();
                                    }
                                }

                                //Generate a response
                                if (!ChessBoard.GameOver)
                                {
                                    if (MoveGeneration.playingVsAI)
                                    {
                                        SetBaseColors();
                                        MoveGeneration.generateMove();
                                        previousMove_From = Tile[8 - ChessBoard.getRank(MoveGeneration.moveIndex_From), ChessBoard.getFile(MoveGeneration.moveIndex_From) - 1];              
                                        previousMove_To = Tile[8 - ChessBoard.getRank(MoveGeneration.moveIndex_To), ChessBoard.getFile(MoveGeneration.moveIndex_To) - 1];
                                        SetupBoard();
                                        wasMoveMade = true;

                                        //Check again if game is finished
                                        if (ChessBoard.GameOver)
                                        {
                                            string message = ChessBoard.GameResult;
                                            string title = "Game over";
                                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                                            DialogResult result = MessageBox.Show(message, title, buttons);

                                            if (result == DialogResult.OK)
                                            {
                                                this.Close();
                                            }
                                        }


                                    }
                                }
                                
                            }
                            
                        }


                        //Piece previous move displayed
                        SetBaseColors();
                        if (previousMove_From != null)
                        {
                            if (previousMove_From.BackColor == white)
                            {
                                previousMove_From.BackColor = white_MoveIndicator;
                            }
                            else if (previousMove_From.BackColor == dark)
                            {
                                previousMove_From.BackColor = dark_MoveIndicator;
                            }

                            if (previousMove_To.BackColor == white)
                            {
                                previousMove_To.BackColor = white_MoveIndicator;
                            }
                            else if (previousMove_To.BackColor == dark)
                            {
                                previousMove_To.BackColor = dark_MoveIndicator;
                            }

                        }


                        //Piece available moves displayed
                        List<int> AvailableMovesList = Piece.AvailableMoves(ChessBoard.getPieceAtIndex(PieceIndex), PieceIndex);

                        if (AvailableMovesList != null && !wasMoveMade)
                        {
                            selectedPiece = clickedTile;

                            for (int i = 0; i < AvailableMovesList.Count; i++)
                            {
                                if (Tile[8 - ChessBoard.getRank(AvailableMovesList[i]), ChessBoard.getFile(AvailableMovesList[i]) - 1].BackColor == dark ||
                                    Tile[8 - ChessBoard.getRank(AvailableMovesList[i]), ChessBoard.getFile(AvailableMovesList[i]) - 1].BackColor == dark_MoveIndicator)
                                {
                                    Tile[8 - ChessBoard.getRank(AvailableMovesList[i]), ChessBoard.getFile(AvailableMovesList[i]) - 1].BackColor = dark_OptionIndicator;
                                }
                                else if (Tile[8 - ChessBoard.getRank(AvailableMovesList[i]), ChessBoard.getFile(AvailableMovesList[i]) - 1].BackColor == white ||
                                         Tile[8 - ChessBoard.getRank(AvailableMovesList[i]), ChessBoard.getFile(AvailableMovesList[i]) - 1].BackColor == white_MoveIndicator)
                                {
                                    Tile[8 - ChessBoard.getRank(AvailableMovesList[i]), ChessBoard.getFile(AvailableMovesList[i]) - 1].BackColor = white_OptionIndicator;
                                }

                            }

                        }

                    };

                }
                top += 60;
            }

            SetupBoard();

        }


        public void SetupBoard()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    switch (ChessBoard.chessBoard[i, j])
                    {
                        //Empty square
                        case '0':
                            Tile[i, j].Image = null;
                            break;
                        //Black pieces
                        case 'r':
                            Tile[i, j].Image = Properties.Resources.rook_black;
                            break;
                        case 'n':
                            Tile[i, j].Image = Properties.Resources.knight_black;
                            break;
                        case 'b':
                            Tile[i, j].Image = Properties.Resources.bishop_black;
                            break;
                        case 'q':
                            Tile[i, j].Image = Properties.Resources.queen_black;
                            break;
                        case 'k':
                            Tile[i, j].Image = Properties.Resources.king_black;
                            break;
                        case 'p':
                            Tile[i, j].Image = Properties.Resources.pawn_black;
                            break;

                        //White pieces
                        case 'R':
                            Tile[i, j].Image = Properties.Resources.rook_white;
                            break;
                        case 'N':
                            Tile[i, j].Image = Properties.Resources.knight_white;
                            break;
                        case 'B':
                            Tile[i, j].Image = Properties.Resources.bishop_white;
                            break;
                        case 'Q':
                            Tile[i, j].Image = Properties.Resources.queen_white;
                            break;
                        case 'K':
                            Tile[i, j].Image = Properties.Resources.king_white;
                            break;
                        case 'P':
                            Tile[i, j].Image = Properties.Resources.pawn_white;
                            break;
                    }
                }
            }

        }

        public void SetBaseColors()
        {
            Color[] colors = new Color[2];
            var dark = Color.DarkSeaGreen;
            var white = Color.Honeydew;

            int n = 8;
            for (int i = 0; i < n; i++)
            {
                if (i % 2 == 0)
                {
                    colors[0] = dark;
                    colors[1] = white;
                }
                else
                {
                    colors[0] = white;
                    colors[1] = dark;
                }

                for (int j = 0; j < n; j++)
                {
                    Tile[i, j].BackColor = colors[(j % 2 == 0) ? 1 : 0];
                }
            }
        }
    }
}
