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

    public Slot Size => new Slot(4,1);

    public int Icon => (int)IconNumbers.ICON_LOBBY;

    public int EntranceOffset { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int ExitOffset { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
  }
}
