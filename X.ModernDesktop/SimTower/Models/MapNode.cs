using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.ModernDesktop.SimTower.Models
{
  class MapNode
  {
    Slot position;
    MapNode neighbours;

    enum Direction {
      UP = 0,
      DOWN = 1,
      LEFT = 2,
      RIGHT = 3
    }

    bool hasElevator;
    bool hasServiceElevator;

  }
}
