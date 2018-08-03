using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.ModernDesktop.SimTower.Models.Item
{
  class Item
  {
    internal int _EntranceOffset;
    internal int _ExitOffset;


    public Slot Position { get; set; }

    public static IPrototype makePrototype<T>() {
      //var currentMethod = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;
      IPrototype p = (IPrototype)Activator.CreateInstance(typeof(T));
      p.EntranceOffset = 0;
      p.ExitOffset = 0;
      return p;
    }

    public void Init() {
    }

  }
}
