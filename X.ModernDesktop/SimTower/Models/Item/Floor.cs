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
    
    private Slot _Size = new Slot(1, 1);
    public Slot Size { get => _Size; set => _Size = value; }
    
    public int Icon => (int)IconNumbers.ICON_FLOOR;

    public int EntranceOffset { get => _EntranceOffset; set => _EntranceOffset = value; }
    public int ExitOffset { get => _ExitOffset; set => _ExitOffset = value; }


    public Item Make()
    {
      // todo : make an item and do stuff with it
      return this; 
    }
  }
}
