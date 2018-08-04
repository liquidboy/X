using SumoNinjaMonkey.Framework.Collections;
using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using X.ModernDesktop.SimTower.Models.Item;


namespace X.ModernDesktop.SimTower.Models
{
  //GAME
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
    //List<X.ModernDesktop.SimTower.Models.Item.Item> items;
    //Dictionary<int, List<X.ModernDesktop.SimTower.Models.Item.Item>> itemsByFloor;
    //Dictionary<string, List<X.ModernDesktop.SimTower.Models.Item.Item>> itemsByType;
    Dictionary<int, Floor> floorItems;
    private int _floorLevel;

    private Canvas _renderSurface;

    public Board()
    {
      initBoard(100, 200, 15, null);
    }
    public Board(int xSlots, int ySlots, int groundSlotY, Canvas renderSurface)
    {
      initBoard(xSlots, ySlots, groundSlotY, renderSurface);
    }

    private void initBoard(int xSlots, int ySlots, int groundSlotY, Canvas renderSurface)
    {
      _renderSurface = renderSurface;
      gameMap = new GameMap();
      itemFactory = new Factory();
      //items = new List<Item.Item>();
      //itemsByFloor = new Dictionary<int, List<Item.Item>>();
      //itemsByType = new Dictionary<string, List<Item.Item>>();
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
      CurrentSelection.IsSelectionVisible = false;
      CurrentSelection.IsHoverCursorVisible = false;
    }

    public void OnPointerEntered(object sender, PointerRoutedEventArgs e)
    {
      CurrentSelection.IsSelectionVisible = false;
      CurrentSelection.IsHoverCursorVisible = true;
    }

    public void OnPointerMoved(object sender, PointerRoutedEventArgs e)
    {
      CurrentSelection.ChangeHoverCursor(e.GetCurrentPoint((Windows.UI.Xaml.UIElement)sender), SlotDimension);
      CurrentSelection.ChangeSelection(e.GetCurrentPoint((Windows.UI.Xaml.UIElement)sender), SlotDimension);
    }

    public void OnPointerPressed(object sender, PointerRoutedEventArgs e)
    {
      CurrentSelection.BeginSelection(e.GetCurrentPoint((Windows.UI.Xaml.UIElement)sender), SlotDimension);
    }

    public void OnPointerReleased(object sender, PointerRoutedEventArgs e)
    {
      CurrentSelection.ChangeSelection(e.GetCurrentPoint((Windows.UI.Xaml.UIElement)sender), SlotDimension);
      CurrentSelection.EndSelection(e.GetCurrentPoint((Windows.UI.Xaml.UIElement)sender), SlotDimension);



      // TEST : draw floor
      LayFloor(
        floorFromSlot(CurrentSelection.SlotEnd.Y),
        Math.Min(CurrentSelection.SlotStart.X, CurrentSelection.SlotEnd.X),
        Math.Max(CurrentSelection.SlotStart.X, CurrentSelection.SlotEnd.X));
    }

    #endregion

    private void LayFloor(int floorSlotY, int minSlotX, int maxSlotX)
    {
      FloorLevelDebug = floorSlotY;
    
      //floor cannot be created in patches along the same level
      //floor can only be created in blocks of 1 height
      //ground floor - allow floor to start anywhere 
      //above ground - needs to be placed above an existing floor tile
      //below ground - needs to be placed below an existing floor tile

      if (floorItems.ContainsKey(floorSlotY)) // EXTEND EXISTING FLOOR
      {
        var existingFloor = floorItems[floorSlotY];

        var x1 = Math.Min(minSlotX, existingFloor.Position.X);
        var x2 = Math.Max(maxSlotX, existingFloor.Position.X + existingFloor.Size.X);

        existingFloor.Position = new Slot(x1, floorSlotY);
        existingFloor.Size = new Slot(x2-x1, 1);
      }
      else { //NEW FLOOR
        Floor f = (Floor)itemFactory.Make(itemFactory.prototypesById["floor"],
        position: new Slot(minSlotX, floorSlotY), 
        size: new Slot(maxSlotX - minSlotX, 1));

        floorItems.Add(floorSlotY, f);
      }

      Draw();
    }

    private void Draw() {
      if (_renderSurface != null) {
        foreach (var floorItem in floorItems)
        {
          var floor = floorItem.Value;
          if (floor.IsDirty) {
            Views.Floor ctl = floor.IsInVisualTree ? (Views.Floor)floor.Control : new Views.Floor();

            ctl.Width = (floor.Size.X + 1) * SlotDimension.X;
            ctl.Height = floor.Size.Y * SlotDimension.Y;
            ctl.SetValue(Canvas.LeftProperty, floor.Position.X * SlotDimension.X);
            ctl.SetValue(Canvas.TopProperty, (AboveGroundSlotsAvailable - floor.Position.Y - 1) * SlotDimension.Y);
            ctl.DataContext = floor;

            if (!floor.IsInVisualTree) {
              floor.Control = ctl;
              _renderSurface.Children.Add(floor.Control);
              floor.IsInVisualTree = true;
            }

          }
        }
      }
    }


  }
}
