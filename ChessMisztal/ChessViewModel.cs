using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TouchTracking;

namespace ChessMisztal
{
    public class ChessViewModel : INotifyPropertyChanged
    {
        public ICommand ChessboardTouchedCommand { get; set; }
        public ICommand NewGameCommand { get; set; }
        public ICommand NewGame960Command { get; set; }
        public ICommand NewGameAICommand { get; set; }
        public ICommand UndoMoveCommand { get; set; }
        private ChessBoard _currentChessBoard;

        public ChessBoard CurrentChessBoard
        {
            get => _currentChessBoard;
            set
            {
                if (_currentChessBoard != value)
                {
                    _currentChessBoard = value;
                    OnPropertyChanged(nameof(CurrentChessBoard));

                    for (int x = 0; x < 8; x++)
                    {
                        for (int y = 0; y < 8; y++)
                        {
                            Piece piece = _currentChessBoard.Board[x, y];
                            if (piece != null)
                            {
                                piece.SetChessBoardAndChessboardView(_currentChessBoard, _chessboardView);
                            }
                        }
                    }
                }
            }
        }

        private ChessboardView _chessboardView;
        public ChessboardView ChessboardView
        {
            get => _chessboardView;
            set
            {
                if (_chessboardView != value)
                {
                    _chessboardView = value;
                    OnPropertyChanged(nameof(ChessboardView));
                }
            }
        }

        private (int X, int Y)? _selectedPiece;

        public (int X, int Y)? SelectedPiece
        {
            get => _selectedPiece;
            private set
            {
                _selectedPiece = value;
                OnPropertyChanged(nameof(SelectedPiece));
            }
        }

        public List<(int X, int Y)> PossibleMoves { get; private set; } = new List<(int X, int Y)>();

        public ChessViewModel()
        {
            _chessboardView = new ChessboardView(); // Create the ChessboardView first
            CurrentChessBoard = new ChessBoard(); // Then set the CurrentChessBoard
            ChessboardTouchedCommand = new Command<object>(OnChessboardTouched);
            NewGameCommand = new Command(NewGame);
            NewGame960Command = new Command(NewGame960);
            NewGameAICommand = new Command(NewGameAI);
            UndoMoveCommand = new Command(UndoMove);

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    Piece piece = CurrentChessBoard.Board[x, y];
                    if (piece != null)
                    {
                        piece.SetChessBoardAndChessboardView(CurrentChessBoard, _chessboardView);
                    }
                }
            }

        }

        public bool IsWhiteTurn => _currentChessBoard.IsWhiteTurn;

        private void OnChessboardTouched(object args)
        {
            if (args == null)
            {
                Console.WriteLine("Touch event arguments were null.");
                return;
            }

            var touchPoint = (Point)args;
            var cellSize = (double)Application.Current.MainPage.Width / 8;
            var cell = new Point(Math.Floor(touchPoint.X / cellSize), Math.Floor(touchPoint.Y / cellSize));

            int col = (int)cell.X;
            int row = (int)cell.Y;

            if (!_selectedPiece.HasValue)
            {
                // Select the piece
                Piece selectedPiece = _currentChessBoard.Board[col, row];
                if (selectedPiece != null && selectedPiece.IsWhite == _currentChessBoard.IsWhiteTurn)
                {
                    _selectedPiece = (col, row);
                    // TODO: Update the possible moves

                }
            }
            else
            {
                // Attempt to move the piece
                bool moveSuccessful = _currentChessBoard.MovePiece(_selectedPiece.Value.X, _selectedPiece.Value.Y, col, row);
                if (moveSuccessful)
                {
                    OnPropertyChanged(nameof(CurrentChessBoard));
                }
                else
                {
                    // TODO: Provide feedback for invalid move
                    Console.WriteLine("Invalid Move: That is not a valid move. Please try again.");
                }

                // Deselect the piece
                _selectedPiece = null;
            }
        }

        private void NewGame()
        {
            Console.WriteLine($"NewGame called. ChessboardView is null: {ChessboardView == null}");
            CurrentChessBoard = new ChessBoard(false);

        }
        private void NewGameAI()
        {
            Console.WriteLine($"NewGame called. ChessboardView is null: {ChessboardView == null}");
            CurrentChessBoard = new ChessBoard(false);

        }
        private void UndoMove()
        {
            //...

        }
        private void NewGame960()
        {
            Console.WriteLine($"NewGame960 called. ChessboardView is null: {ChessboardView == null}");
            CurrentChessBoard = new ChessBoard(true);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}