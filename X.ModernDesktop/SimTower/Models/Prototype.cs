using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.ModernDesktop.SimTower.Models
{
  abstract class Prototype
  {
    public abstract string Id { get; }
    public abstract string Name { get; }
    public abstract int Price { get; }
    public abstract Slot Size { get; }
    public abstract int Icon { get; }
  }
}
