using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.ModernDesktop.SimTower.Models.Item
{
  class FastFood : Item, IPrototype
  {
    public FastFood() { }

    public string Id => "fastfood";

    public string Name => "Fast Food";

    public int Price => 100000;

    private Slot _Size = new Slot(16, 1);
    public Slot Size { get => _Size; set => _Size = value; }

    public int Icon => (int)IconNumbers.ICON_FASTFOOD;

    public int EntranceOffset { get => _EntranceOffset; set => _EntranceOffset = value; }
    public int ExitOffset { get => _ExitOffset; set => _ExitOffset = value; }

    public Item Make()
    {
      throw new NotImplementedException();
    }
  }
}
