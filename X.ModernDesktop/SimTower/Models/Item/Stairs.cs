using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.ModernDesktop.SimTower.Models.Item
{
  class Stairs : Item, IPrototype
  {
    public Stairs() { }

    public string Id => "stairs";

    public string Name => "Stairs";

    public int Price => 5000;

    public Slot Size => new Slot(8,2);

    public int Icon => (int)IconNumbers.ICON_STAIRS;

    public int EntranceOffset { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int ExitOffset { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public Item Make()
    {
      throw new NotImplementedException();
    }
  }
}
