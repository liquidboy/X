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

    private Slot _Size = new Slot(8, 2);
    public Slot Size { get => _Size; set => _Size = value; }

    public int Icon => (int)IconNumbers.ICON_STAIRS;

    public int EntranceOffset { get => _EntranceOffset; set => _EntranceOffset = value; }
    public int ExitOffset { get => _ExitOffset; set => _ExitOffset = value; }


    public Item Make()
    {
      throw new NotImplementedException();
    }
  }
}
