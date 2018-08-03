using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.ModernDesktop.SimTower.Models.Item
{
  class Restaurant : Item, IPrototype
  {
    public Restaurant() { }

    public string Id => "restaurant";

    public string Name => "Restaurant";

    public int Price => 200000;

    private Slot _Size = new Slot(24, 1);
    public Slot Size { get => _Size; set => _Size = value; }

    public int Icon => (int)IconNumbers.ICON_RESTAURANT;

    public int EntranceOffset { get => _EntranceOffset; set => _EntranceOffset = value; }
    public int ExitOffset { get => _ExitOffset; set => _ExitOffset = value; }


    public Item Make()
    {
      throw new NotImplementedException();
    }
  }
}
