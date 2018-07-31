using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace X.ModernDesktop.SimTower.Views
{
  public sealed partial class Board : UserControl
  {
    Models.Board GameBoard { get; set; }

    public Board()
    {
      this.InitializeComponent();
      this.InitializeBoard();
    }

    private void InitializeBoard() {
      GameBoard = new Models.Board(30,50,10);
      this.layoutRoot.DataContext = GameBoard;
    }

  }
}
