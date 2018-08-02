using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.ModernDesktop.SimTower.Models.Item
{
  class PartyHall : Item, IPrototype
  {
    public PartyHall() { }

    public string Id => "partyhall";

    public string Name => "Party Hall";

    public int Price => 500000;

    public Slot Size => new Slot(24,2);

    public int Icon => (int)IconNumbers.ICON_PARTYHALL;

    public int EntranceOffset { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int ExitOffset { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public Item Make()
    {
      throw new NotImplementedException();
    }
  }
}
