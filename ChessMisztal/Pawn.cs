using System;
using System.Collections.Generic;
using System.Text;

namespace ChessMisztal
{
    public class Pawn : Piece
    {
        public Pawn(bool isWhite) : base(isWhite)
        {
            Name = "Pawn";
            PieceButton.Source = isWhite ? "white_pawn" : "black_pawn";
        }

        public override bool IsValidMove(int fromX, int fromY, int toX, int toY, Piece[,] board)
        {
            int direction = IsWhite ? -1 : 1;
            int startRow = IsWhite ? 6 : 1;
            int moveDistance = direction * (toY - fromY);

            // Pawns move one square forward, but two squares on their first move
            bool isSingleMove = moveDistance == direction;
            bool isDoubleMove = moveDistance == 2 * direction && fromY == startRow;

            if (fromX == toX) // Moving straight
            {
                if ((isSingleMove || isDoubleMove) && board[toX, toY] == null)
                {
                    // No piece at the destination
                    return true;
                }
            }
            else if (Math.Abs(fromX - toX) == 1 && isSingleMove) // Diagonal capture
            {
                Piece destinationPiece = board[toX, toY];
                if (destinationPiece != null && destinationPiece.IsWhite != IsWhite)
                {
                    // Capturing an opponent's piece
                    return true;
                }
            }

            // Invalid move for a pawn
            return false;
        }
        protected override void OnPieceClicked(object sender, EventArgs e)
        {
            base.OnPieceClicked(sender, e);
            //TODO: implement
        }
        public override char GetFENChar()
        {
            return IsWhite ? 'P' : 'p';
        }
    }
}
