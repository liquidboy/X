using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.ModernDesktop.SimTower.Models.Item
{
  class Condo : Item, IPrototype
  {
    public Condo() { }

    public string Id => "condo";

    public string Name => "Condo";

    public int Price => 200000;

    private Slot _Size = new Slot(16, 1);
    public Slot Size { get => _Size; set => _Size = value; }

    public int Icon => (int)IconNumbers.ICON_CONDO;

    public int EntranceOffset { get => _EntranceOffset; set => _EntranceOffset = value; }
    public int ExitOffset { get => _ExitOffset; set => _ExitOffset = value; }

    public Item Make()
    {
      throw new NotImplementedException();
    }
  }
}
