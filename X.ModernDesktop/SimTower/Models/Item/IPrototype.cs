using Windows.UI.Xaml;

namespace X.ModernDesktop.SimTower.Models.Item
{
  interface IPrototype
  {
    string Id { get; }
    string Name { get; }
    int Price { get; }
    int ZIndex { get; }
    Slot Size { get; set; }
    Slot Position { get; set; }
    int Icon { get; }
    int EntranceOffset { get; set; }
    int ExitOffset { get; set; }
    UIElement Control { get; set; }
    bool IsInVisualTree { get; set; }
    bool IsDirty { get; set; }
    int MaxInstancesPerFloor { get; }
    bool KeepGrowingSameInstanceX { get; }
    bool KeepGrowingSameInstanceY { get; }

    IPrototype Make();
    UIElement MakeUI();
    void Init();
    void FirstTimeDraw();

    void AddToItem(IPrototype itemToAdd);

  }
}
