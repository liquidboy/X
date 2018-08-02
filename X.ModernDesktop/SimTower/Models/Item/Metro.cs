using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.ModernDesktop.SimTower.Models.Item
{
  class Metro : Item, IPrototype
  {
    public Metro() { }

    public string Id => "metro";

    public string Name => "Metro Station";

    public int Price => 1000000;

    public Slot Size => new Slot(30,3);

    public int Icon => (int)IconNumbers.ICON_METRO;

    public int EntranceOffset { get => 2; set => throw new NotImplementedException(); }
    public int ExitOffset { get => 2; set => throw new NotImplementedException(); }
  }
}
