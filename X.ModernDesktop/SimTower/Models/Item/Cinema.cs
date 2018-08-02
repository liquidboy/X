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

    public Slot Size => new Slot(31,2);

    public int Icon => (int)IconNumbers.ICON_CINEMA;

    public int EntranceOffset { get => 1; set => throw new NotImplementedException(); }
    public int ExitOffset { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
  }
}
