using System;
using System.Collections.Generic;
using System.Text;

namespace ChessMisztal
{
    public class Bishop : Piece
    {
        public Bishop(bool isWhite) : base(isWhite)
        {
            Name = "Bishop";
            PieceButton.Source = isWhite ? "white_bishop.png" : "black_bishop.png";
        }

        public override bool IsValidMove(int fromX, int fromY, int toX, int toY, Piece[,] board)
        {
            int xDiff = Math.Abs(fromX - toX);
            int yDiff = Math.Abs(fromY - toY);

            // Bishops move diagonally
            if (xDiff == yDiff)
            {
                // Check for pieces in the path of the bishop
                int xStep = (toX > fromX) ? 1 : -1;
                int yStep = (toY > fromY) ? 1 : -1;

                int x = fromX + xStep;
                int y = fromY + yStep;

                while (x != toX && y != toY)
                {
                    if (board[x, y] != null)
                    {
                        // There's a piece blocking the bishop's path
                        return false;
                    }

                    x += xStep;
                    y += yStep;
                }

                Piece destinationPiece = board[toX, toY];
                if (destinationPiece != null && destinationPiece.IsWhite == IsWhite)
                {
                    // Can't capture a piece of the same color
                    return false;
                }
                return true;
            }

            // Invalid move for a bishop
            return false;
        }
        protected override void OnPieceClicked(object sender, EventArgs e)
        {
            base.OnPieceClicked(sender, e);
            //TODO: implement
        }
        public override char GetFENChar()
        {
            return IsWhite ? 'B' : 'b';
        }
    }
}
