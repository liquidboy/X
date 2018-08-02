using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.ModernDesktop.SimTower.Models.Item
{
  class Item
  {
    public static IPrototype makePrototype() {
      Type t = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;
      IPrototype p = (IPrototype)Activator.CreateInstance(t);
      p.EntranceOffset = 0;
      p.ExitOffset = 0;
      return p;
    }
  }
}
