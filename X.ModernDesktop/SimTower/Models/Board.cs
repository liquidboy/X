using SumoNinjaMonkey.Framework.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml;
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

    public string SelectedTool { get; set; }

    public int FloorLevelDebug { get => _floorLevel; set => SetProperty(ref _floorLevel, value); }


    private const int maxZSteps = 10;

    public GameMap gameMap { get; set; }
    private Factory itemFactory;
    List<X.ModernDesktop.SimTower.Models.Item.Item> items;
    Dictionary<int, List<X.ModernDesktop.SimTower.Models.Item.Item>> itemsByFloor;
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
      items = new List<Item.Item>();
      itemsByFloor = new Dictionary<int, List<Item.Item>>();
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
      CurrentSelection.ChangeHoverCursor(e.GetCurrentPoint((UIElement)sender), SlotDimension);
      CurrentSelection.ChangeSelection(e.GetCurrentPoint((UIElement)sender), SlotDimension);
    }

    public void OnPointerPressed(object sender, PointerRoutedEventArgs e)
    {
      CurrentSelection.BeginSelection(e.GetCurrentPoint((UIElement)sender), SlotDimension);
    }

    public void OnPointerReleased(object sender, PointerRoutedEventArgs e)
    {
      CurrentSelection.ChangeSelection(e.GetCurrentPoint((UIElement)sender), SlotDimension);
      CurrentSelection.EndSelection(e.GetCurrentPoint((UIElement)sender), SlotDimension);



      // TEST : draw floor
      switch (SelectedTool) {
        case "floor":
          LayFloor(
            floorFromSlot(CurrentSelection.SlotEnd.Y),
            Math.Min(CurrentSelection.SlotStart.X, CurrentSelection.SlotEnd.X),
            Math.Max(CurrentSelection.SlotStart.X, CurrentSelection.SlotEnd.X));
          break;
        default:
          ProcessItem(
            SelectedTool,
            floorFromSlot(CurrentSelection.SlotEnd.Y),
            Math.Min(CurrentSelection.SlotStart.X, CurrentSelection.SlotEnd.X));
          break;
      }
      
      Draw();
    }

    #endregion

    private void ProcessItem(string itemId, int floorSlotY, int startSlotX)
    {
      //no tool choosen
      if (itemId == null) return;

      // only allow placing on existing layed out floors
      if (floorSlotY > 0 && !floorItems.ContainsKey(floorSlotY - 1)) return;
      if (floorSlotY < 0 && !floorItems.ContainsKey(floorSlotY + 1)) return;

      // get toolbar prototype to know what we are making
      var prototype = itemFactory.prototypesById[itemId];

      // create collection to hold items on a given floor
      var itemsOnFloor = itemsByFloor.ContainsKey(floorSlotY) ? itemsByFloor[floorSlotY] : new List<Item.Item>();
      if (itemsOnFloor.Count == 0) itemsByFloor.Add(floorSlotY, itemsOnFloor);

      // try extending item
      if (ExtendItem(floorSlotY, startSlotX, prototype, itemsOnFloor)) return;

      // otherwise additem
      AddItem(floorSlotY, startSlotX, prototype, itemsOnFloor);

    }


    private void AddItem(int floorSlotY, int startSlotX, IPrototype prototype, List<Item.Item> itemsOnFloor)
    {
      // get items on a given floor of prototype's type
      var foundItemsOnFloorOfGivenType = itemsOnFloor.Where(x => ((IPrototype)x).Id == prototype.Id).ToList();

      // check MAX rule of prototype per floor, don't allow more than Max
      if (foundItemsOnFloorOfGivenType.Count <= prototype.MaxInstancesPerFloor) 
      {
        if(foundItemsOnFloorOfGivenType.Count == prototype.MaxInstancesPerFloor)
        {
          if (foundItemsOnFloorOfGivenType.Count == 1 && prototype.KeepGrowingSameInstanceX) {
            // todo: allow the one instance on this level to keep growing
            var itemFound = (IPrototype)foundItemsOnFloorOfGivenType[0];

            var x1 = Math.Min(itemFound.Position.X, startSlotX);
            var x2 = Math.Max(itemFound.Position.X, startSlotX) + 1;

            itemFound.Position = new Slot(x1, itemFound.Position.Y);
            itemFound.Size = new Slot(x2 - x1, itemFound.Size.Y);

            return;
          }
          return; //don't allow more than the max number
        }
        else if (foundItemsOnFloorOfGivenType.Count == 0)
        {
          // create new item for adding to collections
          var newItem = itemFactory.Make(prototype, position: new Slot(startSlotX, floorSlotY), size: new Slot(prototype.Size.X, 1));

          // add to generic collection of items across ALL floors
          items.Add(newItem);

          // add to collection on given floor
          itemsOnFloor.Add(newItem);

          // todo: add to collection grouped by type, across all floors
        }
      }
      
    }

    private bool ExtendItem(int floorSlotY, int startSlotX, IPrototype prototype, List<Item.Item> itemsOnFloor) {
      
      // get collection of items on adjacent floors
      var itemsOnFloorAbove = itemsByFloor.ContainsKey(floorSlotY + 1) ? itemsByFloor[floorSlotY + 1] : new List<Item.Item>();
      var itemsOnFloorBelow = itemsByFloor.ContainsKey(floorSlotY - 1) ? itemsByFloor[floorSlotY - 1] : new List<Item.Item>();

      // get items on a given floor of prototype's type
      var foundItemsOnFloorOfGivenType = itemsOnFloor.Where(x => ((IPrototype)x).Id == prototype.Id).ToList();
      var foundItemsOnAboveFloorOfGivenType = itemsOnFloorAbove.Where(x => ((IPrototype)x).Id == prototype.Id).ToList();
      var foundItemsOnBelowFloorOfGivenType = itemsOnFloorBelow.Where(x => ((IPrototype)x).Id == prototype.Id).ToList();

      // if can grow y then use above/below and grow in Y direction +/- 1
      if (prototype.KeepGrowingSameInstanceY && (foundItemsOnAboveFloorOfGivenType.Count > 0 || foundItemsOnBelowFloorOfGivenType.Count > 0))
      {
        //todo : handle multiple items (currently only dealing with first one)
        var itemAbove = foundItemsOnAboveFloorOfGivenType.Count > 0 ? (IPrototype)foundItemsOnAboveFloorOfGivenType.First() : null;
        var itemBelow = foundItemsOnBelowFloorOfGivenType.Count > 0 ? (IPrototype)foundItemsOnBelowFloorOfGivenType.First() : null;

        if (itemAbove != null && itemBelow != null && itemAbove.Position.X == itemBelow.Position.X && itemBelow.Position.X == startSlotX)
        {
          //todo : need to delete one of them
        }
        else if (itemBelow != null)
        {
          //todo: grow up in y direction
          var newSlotY = itemBelow.Position.Y + 1;
          itemBelow.Size = new Slot(itemBelow.Size.X, itemBelow.Size.Y + 1);
          itemBelow.Position = new Slot(itemBelow.Position.X, newSlotY);

          itemsOnFloor.Add((Item.Item)itemBelow);
        }
        else if (itemAbove != null)
        {
          //todo: grow down in y direction
          itemAbove.Size = new Slot(itemAbove.Size.X, itemAbove.Size.Y + 1);
          itemAbove.Position = new Slot(itemAbove.Position.X, itemAbove.Position.Y);

          itemsOnFloor.Add((Item.Item)itemAbove);
        }

        return true;
      }

      return false;
    }

    private void LayFloor(int floorSlotY, int minSlotX, int maxSlotX)
    {
      FloorLevelDebug = floorSlotY;

      //floor cannot be created in patches along the same level
      //floor can only be created in blocks of 1 height
      //ground floor - allow floor to start anywhere 
      //above ground - needs to be placed above an existing floor tile
      //below ground - needs to be placed below an existing floor tile


      if (floorSlotY > 0 && !floorItems.ContainsKey(floorSlotY - 1)) return;
      if (floorSlotY < 0 && !floorItems.ContainsKey(floorSlotY + 1)) return;

      var minMaxFloor = floorMinMax(floorSlotY, minSlotX, maxSlotX);
      
      if (floorItems.ContainsKey(floorSlotY)) // EXTEND EXISTING FLOOR
      {
        var existingFloor = floorItems[floorSlotY];

        var x1 = Math.Min(minMaxFloor.minSlotXAllowed, existingFloor.Position.X);
        var x2 = Math.Max(minMaxFloor.maxSlotXAllowed, existingFloor.Position.X + existingFloor.Size.X);

        existingFloor.Position = new Slot(x1, floorSlotY);
        existingFloor.Size = new Slot(x2 - x1, 1);

        gameMap.removeNode(existingFloor.Position, existingFloor);
        gameMap.addNode(existingFloor.Position, existingFloor);

      }
      else { //NEW FLOOR
        Floor newFloor = (Floor)itemFactory.Make(itemFactory.prototypesById["floor"],
        position: new Slot(minMaxFloor.minSlotXAllowed, floorSlotY), 
        size: new Slot(minMaxFloor.maxSlotXAllowed - minMaxFloor.minSlotXAllowed, 1));

        floorItems.Add(floorSlotY, newFloor);
        gameMap.addNode(newFloor.Position, newFloor);
      }
    }

    private (int minSlotXAllowed, int maxSlotXAllowed) floorMinMax(int floorSlotY, int minSlotX, int maxSlotX) {
      // get adjance floor to ensure level is created correctly
      Floor adjacentFloor = null;
      if (floorSlotY > 0) { adjacentFloor = floorItems[floorSlotY - 1]; }
      else if (floorSlotY < 0) adjacentFloor = floorItems[floorSlotY + 1];

      // ensure min left compared to adjacent level
      int minSlotXAllowed = minSlotX;
      if (adjacentFloor != null)
      {
        int tempMinSlotXAllowed = adjacentFloor.Position.X;
        tempMinSlotXAllowed = minSlotX < tempMinSlotXAllowed ? tempMinSlotXAllowed : minSlotX;
        minSlotXAllowed = tempMinSlotXAllowed;
      }

      // ensure max right compared to adjacent level
      int maxSlotXAllowed = maxSlotX;
      if (adjacentFloor != null)
      {
        int tempMaxSlotXAllowed = adjacentFloor.Position.X + adjacentFloor.Size.X;
        tempMaxSlotXAllowed = maxSlotX > tempMaxSlotXAllowed ? tempMaxSlotXAllowed : maxSlotX;
        maxSlotXAllowed = tempMaxSlotXAllowed;
      }

      return (minSlotXAllowed, maxSlotXAllowed);
    }



    private void Draw() {
      if (_renderSurface != null) {
        foreach (var floorItem in floorItems)
        {
          var floor = floorItem.Value;
          if (floor.IsDirty) {
            var ctl = floor.IsInVisualTree ? floor.Control : floorItem.Value.MakeUI();
            DrawItem(ctl, floor, floor.Size.X, floor.Size.Y);
          }
        }
        foreach (var otherItem in items) {
          if (otherItem.IsDirty) {
            var p = (IPrototype)otherItem;
            var ctl = otherItem.IsInVisualTree ? otherItem.Control : p.MakeUI();
            DrawItem(ctl, otherItem, p.Size.X-1, p.Size.Y);
          }
        }
      }
    }

    private void DrawItem(UIElement uie, Item.Item item, int sizeX, int sizeY) {
      var ctlFE = (FrameworkElement)uie;
      try
      {
        ctlFE.Width = (sizeX + 1) * SlotDimension.X;
        ctlFE.Height = sizeY * SlotDimension.Y;
        ctlFE.SetValue(Canvas.LeftProperty, item.Position.X * SlotDimension.X);
        ctlFE.SetValue(Canvas.TopProperty, (AboveGroundSlotsAvailable - item.Position.Y - 1) * SlotDimension.Y);
        ctlFE.DataContext = item;

        if (!item.IsInVisualTree)
        {
          item.Control = ctlFE;
          _renderSurface.Children.Add(item.Control);
          item.IsInVisualTree = true;
        }
      }
      catch
      {

      }
    }

    public void SetTool(string tool) {
      SelectedTool = tool;
    }
  }
}
