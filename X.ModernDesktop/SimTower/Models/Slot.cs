using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.ModernDesktop.SimTower.Models
{
  public class Slot 
  {
    public int X { get; private set; }
    public int Y { get; private set; }

    public Slot(int x, int y) {
      X = x;
      Y = y;
    }
    
    public override string ToString() {
      return $"x: {X} y: {Y}";
    }
  }
}
