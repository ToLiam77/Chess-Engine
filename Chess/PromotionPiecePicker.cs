using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Chess
{
    public partial class PromotionPiecePicker : Form
    {
        public PromotionPiecePicker()
        {
            InitializeComponent();
        }

        private void PromotionPiecePicker_Load(object sender, EventArgs e)
        {
            //Setup button icons
            switch (ChessBoard.playerTurn)
            {
                case 'w':
                    btn_queen.BackgroundImage = Properties.Resources.queen_white;
                    btn_rook.BackgroundImage = Properties.Resources.rook_white;
                    btn_bishop.BackgroundImage = Properties.Resources.bishop_white;
                    btn_knight.BackgroundImage = Properties.Resources.knight_white;
                    break;
                case 'b':
                    btn_queen.BackgroundImage = Properties.Resources.queen_black;
                    btn_rook.BackgroundImage = Properties.Resources.rook_black;
                    btn_bishop.BackgroundImage = Properties.Resources.bishop_black;
                    btn_knight.BackgroundImage = Properties.Resources.knight_black;
                    break;
            }
        }

        private void Queen_Clicked(object sender, EventArgs e)
        {
            switch (ChessBoard.playerTurn)
            {
                case 'w':
                    ChessBoard.promotionPiece = 'Q';
                    ChessBoard.updateChessBoard(ChessBoard.pawnToPromote, ChessBoard.pawnPromoteLocation);
                    break;
                case 'b':
                    ChessBoard.promotionPiece = 'q';
                    ChessBoard.updateChessBoard(ChessBoard.pawnToPromote, ChessBoard.pawnPromoteLocation);
                    break;
            }
            this.Close();
        }

        private void Rook_Clicked(object sender, EventArgs e)
        {
            switch (ChessBoard.playerTurn)
            {
                case 'w':
                    ChessBoard.promotionPiece = 'R';
                    ChessBoard.updateChessBoard(ChessBoard.pawnToPromote, ChessBoard.pawnPromoteLocation);
                    break;
                case 'b':
                    ChessBoard.promotionPiece = 'r';
                    ChessBoard.updateChessBoard(ChessBoard.pawnToPromote, ChessBoard.pawnPromoteLocation);
                    break;
            }
            this.Close();
        }

        private void Bishop_Clicked(object sender, EventArgs e)
        {
            switch (ChessBoard.playerTurn)
            {
                case 'w':
                    ChessBoard.promotionPiece = 'B';
                    ChessBoard.updateChessBoard(ChessBoard.pawnToPromote, ChessBoard.pawnPromoteLocation);
                    break;
                case 'b':
                    ChessBoard.promotionPiece = 'b';
                    ChessBoard.updateChessBoard(ChessBoard.pawnToPromote, ChessBoard.pawnPromoteLocation);
                    break;
            }
            this.Close();
        }

        private void Knight_Clicked(object sender, EventArgs e)
        {
            switch (ChessBoard.playerTurn)
            {
                case 'w':
                    ChessBoard.promotionPiece = 'N';
                    ChessBoard.updateChessBoard(ChessBoard.pawnToPromote, ChessBoard.pawnPromoteLocation);
                    break;
                case 'b':
                    ChessBoard.promotionPiece = 'n';
                    ChessBoard.updateChessBoard(ChessBoard.pawnToPromote, ChessBoard.pawnPromoteLocation);
                    break;
            }
            this.Close();
        }


    }
}
