using System;
using System.Collections.Generic;
using System.Text;

namespace ChessMisztal
{
    public class Knight : Piece
    {
        public Knight(bool isWhite) : base(isWhite)
        {
            Name = "Knight";
            PieceButton.Source = isWhite ? "white_knight.png" : "black_knight.png";
        }
        public override bool IsValidMove(int fromX, int fromY, int toX, int toY, Piece[,] board)
        {
            int xDiff = Math.Abs(fromX - toX);
            int yDiff = Math.Abs(fromY - toY);

            // Knights move in an L shape (two squares in one direction and one square in the other direction)
            if ((xDiff == 2 && yDiff == 1) || (xDiff == 1 && yDiff == 2))
            {
                Piece destinationPiece = board[toX, toY];
                if (destinationPiece != null && destinationPiece.IsWhite == IsWhite)
                {
                    // Can't capture a piece of the same color
                    return false;
                }
                return true;
            }

            // Invalid move for a knight
            return false;
        }
        protected override void OnPieceClicked(object sender, EventArgs e)
        {
            base.OnPieceClicked(sender, e);
            //TODO: implement
        }
        public override char GetFENChar()
        {
            return IsWhite ? 'N' : 'n';
        }
    }
}
