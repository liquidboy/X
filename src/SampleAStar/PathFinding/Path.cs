using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleAStar
{
  public class Path : List<NodePosition>
  {
    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      for (int i = 0; i < Count; ++i)
      {
        sb.Append(string.Format("Node {0}: {1}, {2}", i, this[i].x, this[i].y));

        if (i < Count - 1)
        {
          sb.Append(" - ");
        }
      }

      return sb.ToString();
    }
  }
}
