using SumoNinjaMonkey.Framework.Collections;
using System;
using System.Collections.Generic;
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
    public SelectedTool CurrentSelectedTool { get; set; }

    public int FloorLevel { get => _floorLevel; set => SetProperty(ref _floorLevel, value); }


    private const int maxZSteps = 10;

    public GameMap gameMap { get; set; }
    private Factory itemFactory;
    List<X.ModernDesktop.SimTower.Models.Item.Item> items;
    Dictionary<int, List<X.ModernDesktop.SimTower.Models.Item.Item>> itemSetByInt;
    Dictionary<int, Floor> floorItems;
    private int _floorLevel;

    public Board()
    {
      gameMap = new GameMap();
      itemFactory = new Factory();
      items = new List<Item.Item>();
      itemSetByInt = new Dictionary<int, List<Item.Item>>();
      floorItems = new Dictionary<int, Floor>();

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
      CurrentSelectedTool = new SelectedTool();
    }

    private int floorFromSlot(int slotY)
    {
      var floor = 0;
      if (slotY < AboveGroundSlotsAvailable)
      {
        //above ground
        floor = AboveGroundSlotsAvailable - slotY;
      }
      else
      {
        //below ground
        floor = -1 * (slotY - AboveGroundSlotsAvailable);
      }
      return floor - 1;
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
      ExtendFloor(
        floorFromSlot(CurrentSelection.SlotEnd.Y),
        Math.Min(CurrentSelection.SelectionXY.X, CurrentSelection.SelectionWH.X),
        Math.Max(CurrentSelection.SelectionXY.X, CurrentSelection.SelectionWH.X));
    }

    #endregion

    private void ExtendFloor(int floor, int minX, int maxX)
    {
      FloorLevel = floor;

      // Look for existing floor to extend
      //Floor f = floorItems[floor];



      //Floor f = (Floor)itemFactory.Make(itemFactory.prototypesById["floor"], CurrentSelection.SelectionXY);

    }

  }
}
