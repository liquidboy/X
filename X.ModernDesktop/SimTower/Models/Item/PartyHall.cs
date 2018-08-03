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

    private Slot _Size = new Slot(24, 2);
    public Slot Size { get => _Size; set => _Size = value; }

    public int Icon => (int)IconNumbers.ICON_PARTYHALL;

    public int EntranceOffset { get => _EntranceOffset; set => _EntranceOffset = value; }
    public int ExitOffset { get => _ExitOffset; set => _ExitOffset = value; }


    public Item Make()
    {
      throw new NotImplementedException();
    }
  }
}
