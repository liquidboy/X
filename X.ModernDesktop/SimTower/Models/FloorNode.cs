using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.ModernDesktop.SimTower.Models
{
  class FloorNode : MapNode
  {

    public FloorNode(List<MapNode> nodeList): base(null) {
      nodesOnFloor = nodeList;
    }
  }
}
