using Windows.UI.Xaml;

namespace X.ModernDesktop.SimTower.Models.Item
{
  interface IPrototype
  {
    string Id { get; }
    string Name { get; }
    int Price { get; }
    Slot Size { get; set; }
    Slot Position { get; set; }
    int Icon { get; }
    int EntranceOffset { get; set; }
    int ExitOffset { get; set; }
    UIElement Control { get; set; }
    bool IsInVisualTree { get; set; }
    int MaxInstancesPerFloor { get; }
    bool KeepGrowingSameInstanceX { get; }
    bool KeepGrowingSameInstanceY { get; }

    Item Make();
    UIElement MakeUI();

    void FirstTimeDraw();

  }
}
