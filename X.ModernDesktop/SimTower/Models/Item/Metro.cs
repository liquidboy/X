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

    private Slot _Size = new Slot(30, 3);
    public Slot Size { get => _Size; set => _Size = value; }

    public int Icon => (int)IconNumbers.ICON_METRO;

    public int EntranceOffset { get => 2; set => _EntranceOffset = value; }
    public int ExitOffset { get => 2; set => _ExitOffset = value; }


    public Item Make()
    {
      throw new NotImplementedException();
    }
  }
}
