using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.ModernDesktop.SimTower.Models.Item
{
  class Condo : Item, IPrototype
  {
    public Condo() { }

    public string Id => "condo";

    public string Name => "Condo";

    public int Price => 200000;

    public Slot Size => new Slot(16,1);

    public int Icon => (int)IconNumbers.ICON_CONDO;

    public int EntranceOffset { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int ExitOffset { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
  }
}
