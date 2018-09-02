using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleAStar
{
  public struct NodePosition
  {
    public int x;
    public int y;

    public NodePosition(int x, int y)
    {
      this.x = x;
      this.y = y;
    }
  }
}
