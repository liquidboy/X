using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace X.ModernDesktop.SimTower.Models.Item
{
  class Lobby : Item, IPrototype
  {
    public Lobby() { }
    public Lobby(Board board) { _board = board; }

    public string Id => "lobby";

    public string Name => "Lobby";

    public int Price => 20000;

    private Slot _Size = new Slot(1, 1);
    public Slot Size { get => _Size; set => _Size = value; }

    public int Icon => (int)IconNumbers.ICON_LOBBY;

    public int EntranceOffset { get => _EntranceOffset; set => _EntranceOffset = value; }
    public int ExitOffset { get => _ExitOffset; set => _ExitOffset = value; }

    public int MaxInstancesPerFloor => 1;

    public bool KeepGrowingSameInstanceX => true;
    public bool KeepGrowingSameInstanceY => false;

    public IPrototype Make(Board board)
    {
      return new Lobby(board);
    }

    public UIElement MakeUI()
    {
      return new Views.Lobby();
    }

    public void FirstTimeDraw()
    {

    }

    public new void Init() {
      if (Position.Y == 0 && _board != null) {
        _board.mainLobby = this;
      }
    }

    public void AddToItem(IPrototype itemToAdd)
    {

    }
  }
}
