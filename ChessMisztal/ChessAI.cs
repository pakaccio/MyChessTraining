using System;
using System.Collections.Generic;
using System.Text;

namespace ChessMisztal
{
    public class ChessAI
    {
        private int maxDepth;

        public ChessAI(int maxDepth)
        {
            this.maxDepth = maxDepth;
        }

        public (int, int, int, int) GetBestMove(Piece[,] board, bool isWhite)
        {
            int bestScore = isWhite ? int.MinValue : int.MaxValue;
            int fromX = -1, fromY = -1, toX = -1, toY = -1;

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    Piece piece = board[x, y];
                    if (piece != null && piece.IsWhite == isWhite)
                    {
                        for (int destX = 0; destX < 8; destX++)
                        {
                            for (int destY = 0; destY < 8; destY++)
                            {
                                if (piece.IsValidMove(x, y, destX, destY, board))
                                {
                                    Piece capturedPiece = board[destX, destY];
                                    board[destX, destY] = piece;
                                    board[x, y] = null;

                                    int score = Minimax(board, !isWhite, maxDepth - 1, int.MinValue, int.MaxValue);

                                    board[x, y] = piece;
                                    board[destX, destY] = capturedPiece;

                                    if (isWhite && score > bestScore || !isWhite && score < bestScore)
                                    {
                                        bestScore = score;
                                        fromX = x;
                                        fromY = y;
                                        toX = destX;
                                        toY = destY;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return (fromX, fromY, toX, toY);
        }

        private int Minimax(Piece[,] board, bool isWhite, int depth, int alpha, int beta)
        {
            if (depth == 0)
            {
                return EvaluateBoard(board);
            }

            int bestScore = isWhite ? int.MinValue : int.MaxValue;

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    Piece piece = board[x, y];
                    if (piece != null && piece.IsWhite == isWhite)
                    {
                        for (int destX = 0; destX < 8; destX++)
                        {
                            for (int destY = 0; destY < 8; destY++)
                            {
                                if (piece.IsValidMove(x, y, destX, destY, board))
                                {
                                    Piece capturedPiece = board[destX, destY];
                                    board[destX, destY] = piece;
                                    board[x, y] = null;

                                    int score = Minimax(board, !isWhite, depth - 1, alpha, beta);

                                    board[x, y] = piece;
                                    board[destX, destY] = capturedPiece;

                                    if (isWhite)
                                    {
                                        bestScore = Math.Max(bestScore, score);
                                        alpha = Math.Max(alpha, bestScore);
                                    }
                                    else
                                    {
                                        bestScore = Math.Min(bestScore, score);
                                        beta = Math.Min(beta, bestScore);
                                    }

                                    if (beta <= alpha)
                                    {
                                        return bestScore;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return bestScore;
        }
        private int EvaluateBoard(Piece[,] board)
        {
            int score = 0;

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    Piece piece = board[x, y];
                    if (piece != null)
                    {
                        int pieceValue = GetPieceValue(piece);
                        score += piece.IsWhite ? pieceValue : -pieceValue;
                    }
                }
            }

            return score;
        }

        private int GetPieceValue(Piece piece)
        {
            switch (piece.Name)
            {
                case "Pawn":
                    return 1;
                case "Knight":
                    return 3;
                case "Bishop":
                    return 3;
                case "Rook":
                    return 5;
                case "Queen":
                    return 9;
                default:
                    return 0;
            }
        }
    }
}