using System;
using System.Collections.Generic;
using System.Text;

namespace ChessMisztal
{
    public class King : Piece
    {
        public King(bool isWhite) : base(isWhite)
        {
            Name = "King";
            PieceButton.Source = isWhite ? "white_king.png" : "black_king.png";
        }

        public override bool IsValidMove(int fromX, int fromY, int toX, int toY, Piece[,] board)
        {
            int xDiff = Math.Abs(fromX - toX);
            int yDiff = Math.Abs(fromY - toY);

            // King can move one square in any direction
            if ((xDiff <= 1) && (yDiff <= 1))
            {
                Piece destinationPiece = board[toX, toY];
                if (destinationPiece != null && destinationPiece.IsWhite == IsWhite)
                {
                    // Can't capture a piece of the same color
                    return false;
                }
                return true;
            }

            // Invalid move for a king
            return false;
        }
        protected override void OnPieceClicked(object sender, EventArgs e)
        {
            base.OnPieceClicked(sender, e);
            //TODO: implement
        }

        public override char GetFENChar()
        {
            return IsWhite ? 'K' : 'k';
        }
    }
}
