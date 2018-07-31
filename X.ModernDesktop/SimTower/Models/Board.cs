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



    public bool IsCurrentSlotVisible { get; set; }
    public Slot CurrentSlotPositionStart { get; set; }
    public Slot CurrentSlotStart { get; set; }
    public Slot CurrentSlotPositionMiddle { get; set; }
    public Slot CurrentSlotMiddle { get; set; }
    public Slot CurrentSlotPositionEnd { get; set; }
    public Slot CurrentSlotEnd { get; set; }

    public Slot CurrentSelectionXY { get; set; }
    public Slot CurrentSelectionWH { get; set; }


    private const int maxZSteps = 10;

    public event PropertyChangedEventHandler PropertyChanged;

    public GameMap gameMap { get; set; }

    public Board() {
      gameMap = new GameMap();
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

    }

    protected void RaisePropertyChanged(string name)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }



    #region Board Pointer Events

    public void OnPointerExited(object sender, PointerRoutedEventArgs e)
    {
      IsCurrentSlotVisible = false;
      RaisePropertyChanged("IsCurrentSlotVisible");
    }

    public void OnPointerEntered(object sender, PointerRoutedEventArgs e)
    {
      IsCurrentSlotVisible = true;
      RaisePropertyChanged("IsCurrentSlotVisible");
    }

    public void OnPointerMoved(object sender, PointerRoutedEventArgs e)
    {
      if (!startSelection) return;

      var pt = e.GetCurrentPoint((Windows.UI.Xaml.UIElement)sender);
      CurrentSlotPositionMiddle = new Slot(Utilities.RoundDown((int)pt.Position.X, (int)SlotDimension.X), Utilities.RoundDown((int)pt.Position.Y, (int)SlotDimension.Y));
      CurrentSlotMiddle = new Slot((int)(CurrentSlotPositionMiddle.X / SlotDimension.X), (int)(CurrentSlotPositionMiddle.Y / SlotDimension.Y));

      RaisePropertyChanged("CurrentSlotPositionMiddle");
      RaisePropertyChanged("CurrentSlotMiddle");

      int x1 = 0; int y1 = 0;
      x1 = Math.Min(CurrentSlotPositionStart.X, CurrentSlotPositionMiddle.X);
      y1 = Math.Min(CurrentSlotPositionStart.Y, CurrentSlotPositionMiddle.Y);
      CurrentSelectionXY = new Slot(x1, y1);

      int x2 = 0; int y2 = 0;
      x2 = Math.Max(CurrentSlotPositionStart.X, CurrentSlotPositionMiddle.X);
      y2 = Math.Max(CurrentSlotPositionStart.Y, CurrentSlotPositionMiddle.Y);

      int w = 0; int h = 0;
      w = (int)(((x2 - x1) + 1) * SlotDimension.X);
      h = (int)(((y2 - y1) + 1) * SlotDimension.Y);
      CurrentSelectionWH = new Slot(w, h);

      RaisePropertyChanged("CurrentSelectionXY");
      RaisePropertyChanged("CurrentSelectionWH");
    }

    bool startSelection = false;

    public void OnPointerPressed(object sender, PointerRoutedEventArgs e)
    {
      startSelection = true;

      var pt = e.GetCurrentPoint((Windows.UI.Xaml.UIElement)sender);
      CurrentSlotPositionStart = new Slot(Utilities.RoundDown((int)pt.Position.X, (int)SlotDimension.X), Utilities.RoundDown((int)pt.Position.Y, (int)SlotDimension.Y));
      CurrentSlotStart = new Slot((int)(CurrentSlotPositionStart.X / SlotDimension.X), (int)(CurrentSlotPositionStart.Y / SlotDimension.Y));
      RaisePropertyChanged("CurrentSlotPositionStart");
      RaisePropertyChanged("CurrentSlotStart");
    }

    public void OnPointerReleased(object sender, PointerRoutedEventArgs e)
    {
      startSelection = false;

      var pt = e.GetCurrentPoint((Windows.UI.Xaml.UIElement)sender);
      CurrentSlotPositionEnd = new Slot(Utilities.RoundDown((int)pt.Position.X, (int)SlotDimension.X), Utilities.RoundDown((int)pt.Position.Y, (int)SlotDimension.Y));
      CurrentSlotEnd = new Slot((int)(CurrentSlotPositionEnd.X / SlotDimension.X), (int)(CurrentSlotPositionEnd.Y / SlotDimension.Y));

      RaisePropertyChanged("CurrentSlotPositionEnd");
      RaisePropertyChanged("CurrentSlotEnd");
    }

    #endregion
  }
}
