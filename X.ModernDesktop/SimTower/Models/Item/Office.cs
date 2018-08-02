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

    public Slot Size => new Slot(9,1);

    public int Icon => (int)IconNumbers.ICON_OFFICE;

    public int EntranceOffset { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int ExitOffset { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public Item Make()
    {
      throw new NotImplementedException();
    }
  }
}
