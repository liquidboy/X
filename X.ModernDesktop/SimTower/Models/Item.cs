using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.ModernDesktop.SimTower.Models
{
  class Item : Prototype
  {
    public int Layer { get; set; }
    public Slot Position { get; set; }

    public override Slot Size => new Slot(1,1);

    public override string Id => "floor";

    public override string Name => "floor";

    public override int Price => 500;

    public override int Icon => (int)IconNumbers.ICON_FLOOR;
  }
}
