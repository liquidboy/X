using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.ModernDesktop.SimTower.Models.Item
{
  class Cinema : Item, IPrototype
  {
    public Cinema() { }

    public string Id => "cinema";

    public string Name => "Movie Theatre";

    public int Price => 500000;

    private Slot _Size = new Slot(31, 2);
    public Slot Size { get => _Size; set => _Size = value; }

    public int Icon => (int)IconNumbers.ICON_CINEMA;

    public int EntranceOffset { get => 1; set => _EntranceOffset = value; }
    public int ExitOffset { get => _ExitOffset; set => _ExitOffset = value; }

    public Item Make()
    {
      throw new NotImplementedException();
    }
  }
}
