using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.ModernDesktop.SimTower
{
  static class Utilities
  {
    public static int RoundUp(int toRound, int slotSize)
    {
      if (toRound % slotSize == 0) return toRound;
      return (slotSize - toRound % slotSize) + toRound;
    }

    public static int RoundDown(int toRound, int slotSize)
    {
      return toRound - toRound % slotSize;
    }
  }
}
