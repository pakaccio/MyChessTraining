using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ChessMisztal
{
    public abstract class Piece
    {
        public string Name { get; protected set; }
        public bool IsWhite { get; private set; }
        public ImageButton PieceButton { get; protected set; }
        protected ChessBoard _chessBoard;
        protected ChessboardView _chessboardView;
         protected Piece(bool isWhite)
        {
            IsWhite = isWhite;
            PieceButton = new ImageButton
            {
              BackgroundColor = Color.Transparent,
            };
            PieceButton.Clicked += OnPieceClicked;

            //debug
            Console.WriteLine($"Piece constructor called. _chessboardView is null: {_chessboardView == null}");

        }

      

        public void SetChessBoardAndChessboardView(ChessBoard chessBoard, ChessboardView chessboardView)
        {
            _chessBoard = chessBoard;
            _chessboardView = chessboardView;

            //debug
            Console.WriteLine($"SetChessBoardAndChessboardView called. _chessBoard is null: {_chessBoard == null}, _chessboardView is null: {_chessboardView == null}");
        }

         public virtual List<(int row, int col)> GetPossibleMoves(int currentRow, int currentCol, Piece[,] board)
        {
            var possibleMoves = new List<(int row, int col)>();

            // Iterate over all squares on the board
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    // If the move to this square is valid, add it to the list of possible moves
                    if (IsValidMove(currentRow, currentCol, row, col, board))
                    {
                        possibleMoves.Add((row, col));
                    }
                }
            }

            return possibleMoves;
        }
    protected int _currentRow;
    protected int _currentCol;

     protected virtual void OnPieceClicked(object sender, EventArgs e)
    {
        var pieceButton = (ImageButton)sender;

            //debug
            Console.WriteLine($"_chessboardView is null: {_chessboardView == null}");
            if (_chessboardView != null)
            {
                Console.WriteLine($"_possibleMoveButtons is null: {_chessboardView._possibleMoveButtons == null}");
            }

            Piece clickedPiece = null;
         for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                Piece piece = _chessBoard.Board[row, col];
                if (piece != null && piece.PieceButton == pieceButton)
                {
                    clickedPiece = piece;
                    _currentRow = row;
                    _currentCol = col;
                    break;
                }
            }
            if (clickedPiece != null)
            {
                break;
            }
        }
        
        var possibleMoves = GetPossibleMoves(_currentRow, _currentCol, _chessBoard.Board);

            //debug
            Console.WriteLine($"Number of possible moves for clicked piece: {possibleMoves.Count}");

        foreach (var move in possibleMoves)
        {
            var moveButton = new ImageButton
            {
                Source = "black_king", // TODO: zmienic obraz
                BackgroundColor = Color.Red,
                CommandParameter = move
            };
            moveButton.Clicked += OnMoveClicked;

                _chessboardView._possibleMoveButtons.Add(moveButton);
            _chessboardView._moveButtons.Add(moveButton);
            _chessboardView.Children.Add(moveButton, move.col, move.row);

                //debug
                Console.WriteLine($"Added move button at row {move.row}, col {move.col}");
        }
    }

    protected virtual void OnMoveClicked(object sender, EventArgs e)
    {
        var moveButton = (ImageButton)sender;
        var movePosition = (ValueTuple<int, int>)moveButton.CommandParameter;
        int moveRow = movePosition.Item1;
        int moveCol = movePosition.Item2;

        bool moveSuccessful = _chessBoard.MovePiece(_currentRow, _currentCol, moveRow, moveCol);

        // Move the piece
        _chessBoard.MovePiece(_currentRow, _currentCol, moveRow, moveCol);

        if (moveSuccessful)
        {
            _chessboardView.UpdateBoard();

            foreach (var button in _chessboardView._possibleMoveButtons)
            {
                _chessboardView.Children.Remove(button);
            }
            _chessboardView._possibleMoveButtons.Clear();
        }

        else
        {
                //debug
            Console.WriteLine("Invalid move");
        }

    }

        public abstract bool IsValidMove(int fromX, int fromY, int toX, int toY, Piece[,] board);
        public abstract char GetFENChar();


        
    }
}
