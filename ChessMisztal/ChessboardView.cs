using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Xamarin.Forms;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Effects;

namespace ChessMisztal
{
    public class ChessboardView : Grid
    {
        public static readonly BindableProperty ChessBoardProperty =
            BindableProperty.Create(nameof(ChessBoard), typeof(ChessBoard), typeof(ChessboardView), default(ChessBoard), BindingMode.Default, propertyChanged: ChessBoardPropertyChanged);
        public static readonly BindableProperty ChessboardTouchedEventArgsCommandProperty =
    BindableProperty.Create(nameof(ChessboardTouchedEventArgsCommand), typeof(ICommand), typeof(ChessboardView), default(ICommand), BindingMode.Default);

        private ChessBoard _chessBoard;
        public List<ImageButton> _possibleMoveButtons { get; set; }

        public ChessBoard ChessBoard
        {
            get => (ChessBoard)GetValue(ChessBoardProperty);
            set => SetValue(ChessBoardProperty, value);
        }

        public ICommand ChessboardTouchedEventArgsCommand
        {
            get => (ICommand)GetValue(ChessboardTouchedEventArgsCommandProperty);
            set => SetValue(ChessboardTouchedEventArgsCommandProperty, value);
        }

        private static void ChessBoardPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ChessboardView)bindable;
            control._chessBoard = (ChessBoard)newValue;
            control.UpdateBoard();
        }

        public static readonly BindableProperty ChessboardTouchedCommandProperty =
            BindableProperty.Create(nameof(ChessboardTouchedCommand), typeof(ICommand), typeof(ChessboardView), default(ICommand), BindingMode.Default);

        public ICommand ChessboardTouchedCommand
        {
            get => (ICommand)GetValue(ChessboardTouchedCommandProperty);
            set => SetValue(ChessboardTouchedCommandProperty, value);
        }

        public ChessboardView()
        {
            _possibleMoveButtons = new List<ImageButton>();

            // Create the grid with 8 rows and 8 columns
            for (int i = 0; i < 8; i++)
            {
                RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            // Create the background for each square
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    BoxView square = new BoxView
                    {
                        BackgroundColor = (row + col) % 2 == 0 ? Color.White : Color.Gray
                    };

                    Children.Add(square, col, row);
                }
            }
        }

        public List<ImageButton> _pieceButtons = new List<ImageButton>();
        public List<ImageButton> _moveButtons = new List<ImageButton>();

        public void UpdateBoard()
        {
            // Remove previous piece images from the board
            foreach (ImageButton pieceButton in _pieceButtons)
            {
                Children.Remove(pieceButton);
            }
            foreach (ImageButton moveButton in _moveButtons)
            {
                Children.Remove(moveButton);
            }
            foreach (ImageButton possibleMoveButton in _possibleMoveButtons)
            {
                Children.Remove(possibleMoveButton);
            }

            _possibleMoveButtons.Clear();
            _moveButtons.Clear();
            _pieceButtons.Clear();

            if (_chessBoard == null)
            {
                System.Diagnostics.Debug.WriteLine("ChessBoard is null");
                return;
            }

            if (_chessBoard.Board == null)
            {
                System.Diagnostics.Debug.WriteLine("ChessBoard's Board is null");
                return;
            }

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Piece piece = _chessBoard.Board[col, row];
                    if (piece != null)
                    {
                        ImageButton pieceButton = piece.PieceButton;

                        _pieceButtons.Add(pieceButton);
                        Children.Add(pieceButton, col, row);
                    }
                }
            }
        }
        private void PieceTouched(Point point, int col, int row)
        {
            // Convert the touch point to the grid cell coordinate
            ChessboardTouchedCommand?.Execute(new Point(col, row));
        }

    }
}