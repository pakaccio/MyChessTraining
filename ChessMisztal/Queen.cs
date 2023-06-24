using System;
using System.Collections.Generic;
using System.Text;

namespace ChessMisztal
{
    public class Queen : Piece
    {
        public Queen(bool isWhite) : base(isWhite)
        {
            Name = "Queen";
            PieceButton.Source = isWhite ? "white_queen.png" : "black_queen.png";
        }

        public override bool IsValidMove(int fromX, int fromY, int toX, int toY, Piece[,] board)
        {
            int xDiff = Math.Abs(fromX - toX);
            int yDiff = Math.Abs(fromY - toY);

            // Queens can move horizontally, vertically, or diagonally
            bool isHorizontalOrVertical = (fromX == toX) || (fromY == toY);
            bool isDiagonal = (xDiff == yDiff);

            if (isHorizontalOrVertical || isDiagonal)
            {
                // Check for pieces in the path of the queen
                int xStep = (toX == fromX) ? 0 : (toX > fromX ? 1 : -1);
                int yStep = (toY == fromY) ? 0 : (toY > fromY ? 1 : -1);

                int x = fromX + xStep;
                int y = fromY + yStep;

                while (x != toX || y != toY)
                {
                    if (board[x, y] != null)
                    {
                        // There's a piece blocking the queen's path
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

            // Invalid move for a queen
            return false;
        }
        protected override void OnPieceClicked(object sender, EventArgs e)
        {
            base.OnPieceClicked(sender, e);
            //TODO: implement
        }
        public override char GetFENChar()
        {
            return IsWhite ? 'Q' : 'q';
        }
    }
}
