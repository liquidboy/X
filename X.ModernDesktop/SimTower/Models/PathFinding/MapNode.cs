using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.ModernDesktop.SimTower.Models.Item;

namespace X.ModernDesktop.SimTower.Models.PathFinding
{
  internal class MapNode
  {
    internal Slot position;
    internal MapNode[] neighbours = new MapNode[4];
    internal IPrototype[] transportItems = new IPrototype[2];
    public int Status = 0;  //1 = delete

    internal enum Direction :int {
      UP = 0,
      DOWN = 1,
      LEFT = 2,
      RIGHT = 3
    }

    public bool HasElevator { get; set; }
    public bool HasServiceElevator { get; set; }


    public FloorNode floorNode { get; set; }
    internal List<MapNode> nodesOnFloor;

    public MapNode(FloorNode floor) {
      HasElevator = false;
      HasServiceElevator = false;
      floorNode = floor;
      nodesOnFloor = null;
      init();
    }

    void init() {
      neighbours[(int)Direction.UP] = null;
      neighbours[(int)Direction.DOWN] = null;
      neighbours[(int)Direction.LEFT] = null;
      neighbours[(int)Direction.RIGHT] = null;

      transportItems[(int)Direction.UP] = null;
      transportItems[(int)Direction.DOWN] = null;
    }


  }
}
