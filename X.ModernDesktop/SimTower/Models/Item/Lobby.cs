using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.ModernDesktop.SimTower.Models.Item
{
  class Lobby : Item, IPrototype
  {
    public Lobby() { }

    public string Id => "lobby";

    public string Name => "Lobby";

    public int Price => 20000;

    private Slot _Size = new Slot(4, 1);
    public Slot Size { get => _Size; set => _Size = value; }

    public int Icon => (int)IconNumbers.ICON_LOBBY;

    public int EntranceOffset { get => _EntranceOffset; set => _EntranceOffset = value; }
    public int ExitOffset { get => _ExitOffset; set => _ExitOffset = value; }


    public Item Make()
    {
      throw new NotImplementedException();
    }
  }
}
