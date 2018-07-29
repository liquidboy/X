using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Input;

namespace X.ModernDesktop.SimTower.Models
{
  class Board : INotifyPropertyChanged
  {
    public Vector3 SlotsAvailable { get; private set; }
    public int GroundLevelSlotPositionY { get; private set; }
    public Vector2 SlotDimension { get; set; }

    public int AboveGroundSlotsAvailable { get; private set; }
    public int BelowGroundSlotsAvailable { get; private set; }
    public int BoardHeight { get; private set; }
    public int BoardWidth { get; private set; }
    public Vector4 AboveGroundDimension { get; set; }
    public Vector4 BelowGroundDimension { get; set; }

    public Windows.UI.Xaml.Visibility CurrentSlotVisible { get; set; }

    public bool IsCurrentSlotVisible { get; set; }
    public int CurrentSlotPositionX { get; set; }
    public int CurrentSlotPositionY { get; set; }

    private const int maxZSteps = 10;

    public event PropertyChangedEventHandler PropertyChanged;

    public Board() {
      initBoard(100, 200, 15);
    }
    public Board(int xSlots, int ySlots, int groundSlotY) {
      initBoard(xSlots, ySlots, groundSlotY);
    }

    private void initBoard(int xSlots, int ySlots, int groundSlotY) {
      SlotsAvailable = new Vector3(xSlots, ySlots, maxZSteps);
      GroundLevelSlotPositionY = groundSlotY;
      AboveGroundSlotsAvailable = ySlots - groundSlotY;
      BelowGroundSlotsAvailable = groundSlotY;
      SlotDimension = new Vector2(40, 40);

      BoardHeight = (int)(SlotsAvailable.Y * SlotDimension.Y);
      BoardWidth = (int)(SlotsAvailable.X * SlotDimension.X);

      AboveGroundDimension = new Vector4(0, 0, BoardWidth, AboveGroundSlotsAvailable * SlotDimension.Y);
      BelowGroundDimension = new Vector4(0, AboveGroundDimension.W, BoardWidth, AboveGroundDimension.W + (BelowGroundSlotsAvailable * SlotDimension.Y));

      //CurrentSlotVisible = Windows.UI.Xaml.Visibility.Visible;
      
    }

    protected void RaisePropertyChanged(string name)
    {
      if (PropertyChanged != null)
      {
        PropertyChanged(this, new PropertyChangedEventArgs(name));
      }
    }


    public void OnPointerExited(object sender, PointerRoutedEventArgs e)
    {
      //this.CurrentSlotVisible = Windows.UI.Xaml.Visibility.Collapsed;
      IsCurrentSlotVisible = false;
      RaisePropertyChanged("IsCurrentSlotVisible");
    }

    public void OnPointerEntered(object sender, PointerRoutedEventArgs e)
    {
      //this.CurrentSlotVisible = Windows.UI.Xaml.Visibility.Visible;
      IsCurrentSlotVisible = true;
      RaisePropertyChanged("IsCurrentSlotVisible");
    }

    public void OnPointerMoved(object sender, PointerRoutedEventArgs e)
    {
      var pt = e.GetCurrentPoint((Windows.UI.Xaml.UIElement)sender);
      CurrentSlotPositionX = RoundDown((int)pt.Position.X);
      CurrentSlotPositionY = RoundDown((int)pt.Position.Y);
      //CurrentSlotPosition = new Vector4(CurrentSlotPositionX, (int)pt.Position.Y, (int)pt.Position.X + SlotDimension.X, (int)pt.Position.Y + SlotDimension.Y);
      



      RaisePropertyChanged("CurrentSlotPositionX");
      RaisePropertyChanged("CurrentSlotPositionY");
    }

    int RoundUp(int toRound)
    {
      if (toRound % (int)SlotDimension.Y == 0) return toRound;
      return ((int)SlotDimension.Y - toRound % (int)SlotDimension.Y) + toRound;
    }

    int RoundDown(int toRound)
    {
      return toRound - toRound % (int)SlotDimension.Y;
    }
  }
}
