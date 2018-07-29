using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.ModernDesktop.SimTower.Models
{
  class GameMap
  {
    public SortedDictionary<Slot, MapNode> gameMap;
    public SortedDictionary<int, List<MapNode>> mapNodesByFloor;
    public SortedDictionary<int, FloorNode> floorNodes;

    MapNode addNode(Slot slot) {
      //if (floorNodes.Count(slot.Y) == 0) {
      //}
      return null;
    }
  }
}
