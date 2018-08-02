using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.ModernDesktop.SimTower.Models.Item
{
  class FastFood : Item, IPrototype
  {
    public FastFood() { }

    public string Id => "fastfood";

    public string Name => "Fast Food";

    public int Price => 100000;

    public Slot Size => new Slot(16,1);

    public int Icon => (int)IconNumbers.ICON_FASTFOOD;

    public int EntranceOffset { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int ExitOffset { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
  }
}
