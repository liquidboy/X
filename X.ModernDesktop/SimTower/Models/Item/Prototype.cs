using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.ModernDesktop.SimTower.Models.Item
{
  interface IPrototype
  {
    string Id { get; }
    string Name { get; }
    int Price { get; }
    Slot Size { get; }
    int Icon { get; }
    int EntranceOffset { get; set; }
    int ExitOffset { get; set;  }

  }
}
