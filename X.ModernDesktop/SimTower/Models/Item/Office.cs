using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace X.ModernDesktop.SimTower.Models.Item
{
  class Office : Item, IPrototype
  {
    public Office() { }

    public string Id => "office";

    public string Name => "Office";

    public int Price => 50000;

    private Slot _Size = new Slot(2, 1);
    public Slot Size { get => _Size; set => _Size = value; }

    public int Icon => (int)IconNumbers.ICON_OFFICE;

    public int EntranceOffset { get => _EntranceOffset; set => _EntranceOffset = value; }
    public int ExitOffset { get => _ExitOffset; set => _ExitOffset = value; }

    public int MaxInstancesPerFloor => 30;

    public bool KeepGrowingSameInstanceX => false;
    public bool KeepGrowingSameInstanceY => false;

    public IPrototype Make()
    {
      return new Office();
    }

    public UIElement MakeUI()
    {
      return new Views.Office();
    }

    public void FirstTimeDraw()
    {

    }

    public void AddToItem(IPrototype itemToAdd)
    {

    }

  }
}
