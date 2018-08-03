using SumoNinjaMonkey.Framework.Collections;
using System.Numerics;
using Windows.UI.Xaml.Input;
using X.ModernDesktop.SimTower.Models.Item;

namespace X.ModernDesktop.SimTower.Models
{
  class Board : BindableBase
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



    public SelectedArea CurrentSelection { get; set; }



    private const int maxZSteps = 10;

    public GameMap gameMap { get; set; }

    public Board()
    {
      gameMap = new GameMap();
      initBoard(100, 200, 15);
    }
    public Board(int xSlots, int ySlots, int groundSlotY)
    {
      initBoard(xSlots, ySlots, groundSlotY);
    }

    private void initBoard(int xSlots, int ySlots, int groundSlotY)
    {
      SlotsAvailable = new Vector3(xSlots, ySlots, maxZSteps);
      GroundLevelSlotPositionY = groundSlotY;
      AboveGroundSlotsAvailable = ySlots - groundSlotY;
      BelowGroundSlotsAvailable = groundSlotY;
      SlotDimension = new Vector2(40, 40);

      BoardHeight = (int)(SlotsAvailable.Y * SlotDimension.Y);
      BoardWidth = (int)(SlotsAvailable.X * SlotDimension.X);

      AboveGroundDimension = new Vector4(0, 0, BoardWidth, AboveGroundSlotsAvailable * SlotDimension.Y);
      BelowGroundDimension = new Vector4(0, AboveGroundDimension.W, BoardWidth, AboveGroundDimension.W + (BelowGroundSlotsAvailable * SlotDimension.Y));

      CurrentSelection = new SelectedArea();

    }


    #region Board Pointer Events

    public void OnPointerExited(object sender, PointerRoutedEventArgs e)
    {
      CurrentSelection.IsVisible = false;
    }

    public void OnPointerEntered(object sender, PointerRoutedEventArgs e)
    {
      CurrentSelection.IsVisible = true;
    }

    public void OnPointerMoved(object sender, PointerRoutedEventArgs e)
    {
      CurrentSelection.ChangeSelection(e.GetCurrentPoint((Windows.UI.Xaml.UIElement)sender), SlotDimension);
    }

    public void OnPointerPressed(object sender, PointerRoutedEventArgs e)
    {
      CurrentSelection.BeginSelection(e.GetCurrentPoint((Windows.UI.Xaml.UIElement)sender), SlotDimension);
    }

    public void OnPointerReleased(object sender, PointerRoutedEventArgs e)
    {
      CurrentSelection.EndSelection(e.GetCurrentPoint((Windows.UI.Xaml.UIElement)sender), SlotDimension);
      DrawFloor();
    }

    #endregion

    IPrototype toolPrototype;

    private void DrawFloor()
    {

    }
  }
}
