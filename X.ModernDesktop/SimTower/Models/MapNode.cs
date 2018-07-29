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
    MapNode[] neighbours = new MapNode[4];

    enum Direction {
      UP = 0,
      DOWN = 1,
      LEFT = 2,
      RIGHT = 3
    }

    bool hasElevator;
    bool hasServiceElevator;


    FloorNode floorNode;
    internal List<MapNode> nodesOnFloor;

    public MapNode(FloorNode floor) {
      hasElevator = false;
      hasServiceElevator = false;
      floorNode = floor;
      nodesOnFloor = null;
      init();
    }

    void init() {
      neighbours[(int)Direction.UP] = null;
      neighbours[(int)Direction.DOWN] = null;
      neighbours[(int)Direction.LEFT] = null;
      neighbours[(int)Direction.RIGHT] = null;
    }


  }
}
