using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.ModernDesktop.SimTower.Models.Item
{
  class Elevator : Item, IPrototype
  {
    public Elevator() { }

    public string Id => "elevator";

    public string Name => "Elevator";

    public int Price => 20000;

    public Slot Size => new Slot(8,2);

    public int Icon => (int)IconNumbers.ICON_ELEVATOR;

    public int EntranceOffset { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int ExitOffset { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public Item Make()
    {
      throw new NotImplementedException();
    }
  }
}
