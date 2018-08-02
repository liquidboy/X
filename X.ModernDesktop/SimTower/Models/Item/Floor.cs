using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.ModernDesktop.SimTower.Models.Item
{
  class Floor : Item, IPrototype
  {
    public Floor() { }

    public string Id => "floor";

    public string Name => "Floor";

    public int Price => 500;

    public Slot Size => new Slot(1,1);

    public int Icon => (int)IconNumbers.ICON_FLOOR;

    public int EntranceOffset { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int ExitOffset { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
  }
}
