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

    public int FloorLevelDebug { get => _floorLevel; set => SetProperty(ref _floorLevel, value); }


    private const int maxZSteps = 10;

    public GameMap gameMap { get; set; }
    private Factory itemFactory;
    List<X.ModernDesktop.SimTower.Models.Item.Item> items;
    Dictionary<int, List<X.ModernDesktop.SimTower.Models.Item.Item>> itemsByFloor;
    Dictionary<string, List<X.ModernDesktop.SimTower.Models.Item.Item>> itemsByType;
    Dictionary<int, Floor> floorItems;
    private int _floorLevel;

    public Board()
    {
      initBoard(100, 200, 15);
    }
    public Board(int xSlots, int ySlots, int groundSlotY)
    {
      initBoard(xSlots, ySlots, groundSlotY);
    }

    private void initBoard(int xSlots, int ySlots, int groundSlotY)
    {
      gameMap = new GameMap();
      itemFactory = new Factory();
      items = new List<Item.Item>();
      itemsByFloor = new Dictionary<int, List<Item.Item>>();
      itemsByType = new Dictionary<string, List<Item.Item>>();
      floorItems = new Dictionary<int, Floor>();

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
        Math.Min(CurrentSelection.SlotStart.X, CurrentSelection.SlotEnd.X),
        Math.Max(CurrentSelection.SlotStart.X, CurrentSelection.SlotEnd.X));
    }

    #endregion

    private void ExtendFloor(int floorSlotY, int minSlotX, int maxSlotX)
    {
      FloorLevelDebug = floorSlotY;
      
      

      Floor f = (Floor)itemFactory.Make(itemFactory.prototypesById["floor"], new Slot(minSlotX, floorSlotY));
      f.Size = new Slot(maxSlotX - minSlotX, f.Size.Y);

      //items.Add(f);
      //if (itemsByType.ContainsKey(f.Id)) itemsByType[f.Id].Add(f);
      //else itemsByType.Add(f.Id, new List<Item.Item>() { f });

      if (floorItems.ContainsKey(floorSlotY)) {
        var existingFloor = floorItems[floorSlotY];
      }

      floorItems.Add(floorSlotY, f);



    }

  }
}
