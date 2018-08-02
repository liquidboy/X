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

    public Slot Size => new Slot(24,1);

    public int Icon => (int)IconNumbers.ICON_RESTAURANT;

    public int EntranceOffset { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int ExitOffset { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
  }
}
