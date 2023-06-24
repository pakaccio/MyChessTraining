using System;
using System.Collections.Generic;
using System.Text;

namespace ChessMisztal
{
    public class Rook : Piece
    {
        public Rook(bool isWhite) : base(isWhite)
        {
            Name = "Rook";
            PieceButton.Source = isWhite ? "white_rook" : "black_rook";
        }

        public override bool IsValidMove(int fromX, int fromY, int toX, int toY, Piece[,] board)
        {
            if (fromX != toX && fromY != toY)
            {
                // Rooks can only move horizontally or vertically
                return false;
            }

            int xDirection = fromX < toX ? 1 : -1;
            int yDirection = fromY < toY ? 1 : -1;

            if (fromX == toX)
            {
                // Moving vertically
                for (int y = fromY + yDirection; y != toY; y += yDirection)
                {
                    if (board[fromX, y] != null)
                    {
                        // There's a piece in the way
                        return false;
                    }
                }
            }
            else
            {
                // Moving horizontally
                for (int x = fromX + xDirection; x != toX; x += xDirection)
                {
                    if (board[x, fromY] != null)
                    {
                        // There's a piece in the way
                        return false;
                    }
                }
            }

            Piece destinationPiece = board[toX, toY];
            if (destinationPiece != null && destinationPiece.IsWhite == IsWhite)
            {
                // Can't capture a piece of the same color
                return false;
            }

            return true;
        }
        protected override void OnPieceClicked(object sender, EventArgs e)
        {
            base.OnPieceClicked(sender, e);
            //TODO: implement
        }
        
        public override char GetFENChar()
        {
            return IsWhite ? 'R' : 'r';
        }
    }
}
