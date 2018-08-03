using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.ModernDesktop.SimTower.Models.Item
{
  class Office : Item, IPrototype
  {
    public Office() { }

    public string Id => "office";

    public string Name => "Office";

    public int Price => 40000;

    private Slot _Size = new Slot(9, 1);
    public Slot Size { get => _Size; set => _Size = value; }

    public int Icon => (int)IconNumbers.ICON_OFFICE;

    public int EntranceOffset { get => _EntranceOffset; set => _EntranceOffset = value; }
    public int ExitOffset { get => _ExitOffset; set => _ExitOffset = value; }


    public Item Make()
    {
      throw new NotImplementedException();
    }
  }
}
