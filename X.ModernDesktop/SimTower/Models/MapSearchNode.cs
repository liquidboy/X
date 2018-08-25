using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.ModernDesktop.SimTower.Models.Item;

namespace X.ModernDesktop.SimTower.Models
{
  internal class MapSearchNode
  {
    public IPrototype parent_item { get; set; }


    public Slot start_point { get; set; }
    public Slot end_point { get; set; }
    public bool serviceRoute { get; set; }

    AStarPathfinder pathfinder = null;


    public MapSearchNode(MapNode mp){

    }

    public MapSearchNode(AStarPathfinder _pathfinder)
    {
      start_point = new Slot(0, 0);
      pathfinder = _pathfinder;
    }

    public bool GetSuccessors(AStarPathfinder astarsearch, MapSearchNode parent_node) {
      throw new NotImplementedException();
    }

    public float GoalDistanceEstimate(MapSearchNode nodeGoal) {
      throw new NotImplementedException();
    }

    public bool IsGoal(MapSearchNode nodeGoal) {
      throw new NotImplementedException();
    }

    public bool IsSameState(MapSearchNode rhs) {
      throw new NotImplementedException();
    }

    public float GetCost(MapSearchNode successor)
    {
      throw new NotImplementedException();
    }
  
  }
}
