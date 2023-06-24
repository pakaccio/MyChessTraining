using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.Effects;


namespace ChessMisztal
{
    public partial class MainPage : ContentPage
    {
        private ChessBoard chessBoard;

        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = new ChessViewModel();

            var viewModel = new ChessViewModel();
            var chessboardView = new ChessboardView();
            chessBoard = new ChessBoard(chessboardView);
            viewModel.ChessboardView = chessboardView;
            viewModel.CurrentChessBoard = chessBoard;
        }

        //private void NewGame960_Click(object sender, EventArgs e)
        //{
        //    chessBoard = new ChessBoard(true);
        //    MyChessViewModel.CurrentChessBoard = chessBoard;
        //}


    }
}