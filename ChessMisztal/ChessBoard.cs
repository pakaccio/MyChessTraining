using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace ChessMisztal
{
    public class ChessBoard : INotifyPropertyChanged
    {
        public bool IsWhiteTurn { get; private set; } = true;
        private Piece[,] _board;
        protected ChessboardView _chessboardView;
        public Piece[,] Board
        {
            get => _board;
            private set
            {
                if (_board != value)
                {
                    _board = value;
                    OnPropertyChanged(nameof(Board));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ChessBoard()
        {
            Board = new Piece[8, 8];
            InitializeBoard();
            
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                
                {
                    Piece piece = Board[x, y];
                    if (piece != null)
                    {
                        piece.SetChessBoardAndChessboardView(this, _chessboardView);
                    }
                }
            }
        }
        public ChessBoard(bool isChess960 = false)
        {
            Board = new Piece[8, 8];
            if (isChess960)
            {
                InitializeBoard960();
            }
            else
            {
                InitializeBoard();
            }
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)

                {
                    Piece piece = Board[x, y];
                    if (piece != null)
                    {
                        piece.SetChessBoardAndChessboardView(this, _chessboardView);
                    }
                }
            }
        }

        public ChessBoard(ChessboardView chessboardView)
        {
            _chessboardView = chessboardView;

            //debug
            Console.WriteLine($"ChessBoard constructor called. _chessboardView is null: {_chessboardView == null}");

            Board = new Piece[8, 8];
            InitializeBoard();

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    Piece piece = Board[x, y];
                    if (piece != null)
                    {
                        piece.SetChessBoardAndChessboardView(this, _chessboardView);
                    }
                }
            }
        }

        public ChessBoard(ChessBoard other)
        {
            Board = new Piece[8, 8];
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    Board[x, y] = other.Board[x, y];
                }
            }

            IsWhiteTurn = other.IsWhiteTurn;
        }

        private void InitializeBoard()
        {
            for (int i = 0; i < 8; i++)
            {
                Board[i, 1] = new Pawn(true);
                Board[i, 6] = new Pawn(false);
            }

            Board[0, 0] = new Rook(true);
            Board[7, 0] = new Rook(true);
            Board[0, 7] = new Rook(false);
            Board[7, 7] = new Rook(false);

            Board[1, 0] = new Knight(true);
            Board[6, 0] = new Knight(true);
            Board[1, 7] = new Knight(false);
            Board[6, 7] = new Knight(false);

            Board[2, 0] = new Bishop(true);
            Board[5, 0] = new Bishop(true);
            Board[2, 7] = new Bishop(false);
            Board[5, 7] = new Bishop(false);

            Board[3, 0] = new Queen(true);
            Board[3, 7] = new Queen(false);

            Board[4, 0] = new King(true);
            Board[4, 7] = new King(false);
        }

        public void InitializeBoard960()
        {
            // Create an array of pieces for one side
            Piece[] pieces = new Piece[8]
            {
        new Rook(true),
        new Knight(true),
        new Bishop(true),
        new Queen(true),
        new King(true),
        new Bishop(true),
        new Knight(true),
        new Rook(true)
            };

            // Shuffle the pieces
            Random rng = new Random();
            int n = pieces.Length;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Piece temp = pieces[k];
                pieces[k] = pieces[n];
                pieces[n] = temp;
            }

            // Place the shuffled pieces on the board
            for (int i = 0; i < 8; i++)
            {
                Board[i, 0] = pieces[i];
                Board[i, 1] = new Pawn(true);
                // Place the black pieces
                if (pieces[i] is Rook)
                {
                    Board[i, 7] = new Rook(false);
                }
                if (pieces[i] is Queen)
                {
                    Board[i, 7] = new Queen(false);
                }
                if (pieces[i] is King)
                {
                    Board[i, 7] = new King(false);
                }
                if (pieces[i] is Bishop)
                {
                    Board[i, 7] = new Bishop(false);
                }
                else if (pieces[i] is Knight)
                {
                    Board[i, 7] = new Knight(false);
                }
                // Add similar code for other types of pieces
                Board[i, 6] = new Pawn(false);
            }

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)

                {
                    Piece piece = Board[x, y];
                    if (piece != null)
                    {
                        piece.SetChessBoardAndChessboardView(this, _chessboardView);
                    }
                }
            }
            OnPropertyChanged(nameof(Board));
        }

        public bool MovePiece(int fromX, int fromY, int toX, int toY)
        {
            Piece piece = Board[fromX, fromY];

            if (piece == null)
                return false;

            if (piece.IsWhite == IsWhiteTurn && piece.IsValidMove(fromX, fromY, toX, toY, Board))
            {
                // Simulate the move
                Piece originalPiece = Board[toX, toY];
                Board[toX, toY] = piece;
                Board[fromX, fromY] = null;

                bool isKingInCheck = IsKingInCheck(IsWhiteTurn);

                // Undo the move
                Board[fromX, fromY] = piece;
                Board[toX, toY] = originalPiece;

                if (isKingInCheck)
                {
                    // The move would result in the current player's king being in check, so it's not a valid move
                    return false;
                }

                // The move is valid, so make it for real this time
                Board[toX, toY] = piece;
                Board[fromX, fromY] = null;

                // Switch the active player
                IsWhiteTurn = !IsWhiteTurn;

                return true;
            }

            return false;
        }

        public bool IsKingInCheck(bool isWhite)
        {
            int kingX = 0;
            int kingY = 0;
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    Piece piece = Board[x, y];
                    if (piece != null && piece.IsWhite == isWhite && piece is King)
                    {
                        kingX = x;
                        kingY = y;
                        break;
                    }
                }
            }

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    Piece piece = Board[x, y];
                    if (piece != null && piece.IsWhite != isWhite && piece.IsValidMove(x, y, kingX, kingY, Board))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool IsKingInCheckmate(bool isWhite)
        {
            if (!IsKingInCheck(isWhite)) // Not in check, so can't be checkmate
            {
                return false;
            }

            // Find the position of the king
            int kingX = 0;
            int kingY = 0;
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    Piece piece = Board[x, y];
                    if (piece != null && piece.IsWhite == isWhite && piece is King)
                    {
                        kingX = x;
                        kingY = y;
                        break;
                    }
                }
            }

            // Check if there is any legal move the king can make that would get it out of check
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    int newX = kingX + dx;
                    int newY = kingY + dy;
                    if (newX >= 0 && newX < 8 && newY >= 0 && newY < 8)
                    {
                        // Simulate the move
                        Piece originalPiece = Board[newX, newY];
                        Board[newX, newY] = Board[kingX, kingY];
                        Board[kingX, kingY] = null;

                        bool wasInCheck = IsKingInCheck(isWhite);

                        // Undo the move
                        Board[kingX, kingY] = Board[newX, newY];
                        Board[newX, newY] = originalPiece;

                        if (!wasInCheck)
                        {
                            // Found a legal move that gets the king out of check
                            return false;
                        }
                    }
                }
            }

            // Didn't find any move that gets the king out of check, so it's checkmate
            return true;
        }

        public bool IsStalemate(bool isWhite)
        {
            if (IsKingInCheck(isWhite)) // If the king is in check, it can't be stalemate
            {
                return false;
            }

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    Piece piece = Board[x, y];
                    if (piece != null && piece.IsWhite == isWhite)
                    {
                        for (int dx = -1; dx <= 1; dx++)
                        {
                            for (int dy = -1; dy <= 1; dy++)
                            {
                                int newX = x + dx;
                                int newY = y + dy;
                                if (newX >= 0 && newX < 8 && newY >= 0 && newY < 8 && piece.IsValidMove(x, y, newX, newY, Board))
                                {
                                    // Simulate the move
                                    Piece originalPiece = Board[newX, newY];
                                    Board[newX, newY] = piece;
                                    Board[x, y] = null;

                                    bool wasInCheck = IsKingInCheck(isWhite);

                                    // Undo the move
                                    Board[x, y] = piece;
                                    Board[newX, newY] = originalPiece;

                                    if (!wasInCheck)
                                    {
                                        // Found a legal move that doesn't result in check
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Didn't find any legal move that doesn't result in check, so it's stalemate
            return true;
        }

        public string ToFEN()
        {
            StringBuilder fen = new StringBuilder();

            for (int y = 7; y >= 0; y--)
            {
                int emptySquares = 0;
                for (int x = 0; x < 8; x++)
                {
                    Piece piece = Board[x, y];
                    if (piece == null)
                    {
                        emptySquares++;
                    }
                    else
                    {
                        if (emptySquares != 0)
                        {
                            fen.Append(emptySquares);
                            emptySquares = 0;
                        }
                        fen.Append(piece.GetFENChar());
                    }
                }
                if (emptySquares != 0)
                {
                    fen.Append(emptySquares);
                }
                if (y != 0)
                {
                    fen.Append('/');
                }
            }

            // Add placeholders for turn, castling, en passant, halfmove, and fullmove counters
            fen.Append(" w KQkq - 0 1");

            return fen.ToString();
        }
    }
}
