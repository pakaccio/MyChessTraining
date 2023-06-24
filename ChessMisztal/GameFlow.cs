using System;
using System.Collections.Generic;
using System.Text;

namespace ChessMisztal
{
    public class GameFlow
    {
        public ChessBoard Board { get; private set; }
        public bool IsWhiteTurn { get; private set; }

        public GameFlow()
        {
            Board = new ChessBoard();
            IsWhiteTurn = true; // White usually starts in chess
        }

        public bool TryMove(int fromX, int fromY, int toX, int toY)
        {
            Piece piece = Board.Board[fromX, fromY];

            // Ensure the piece being moved belongs to the player whose turn it is
            if ((piece.IsWhite && IsWhiteTurn) || (!piece.IsWhite && !IsWhiteTurn))
            {
                if (Board.MovePiece(fromX, fromY, toX, toY))
                {
                    IsWhiteTurn = !IsWhiteTurn;
                    return true;
                }
            }
            return false;
        }

        public bool IsGameOver()
        {
            // TODO: Implement game over logic
            return false;
        }
    }
}
