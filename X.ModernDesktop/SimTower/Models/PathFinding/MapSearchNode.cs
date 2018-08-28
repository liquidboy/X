using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.ModernDesktop.SimTower.Models.Item;

namespace X.ModernDesktop.SimTower.Models.PathFinding
{
  internal class MapSearchNode
  {
    public Item.Item parent_item { get; set; }


    public Slot start_point { get; set; }
    public Slot end_point { get; set; }
    public bool serviceRoute { get; set; }

    AStarPathfinder pathfinder = null;
    public MapNode mapNode { get; set; }

    public float g { get; set; }
    public float h { get; set; }

    public const int MAX_WALKING_DIST = 80;
    public const int WALKING_COST = 1;
    public const int FLOOR_COST = 5;
    public const int ESCALATOR_COST = 10;
    public const int STAIRS_COST = 30;
    public const int ELEVATOR_COST = STAIRS_COST * 3 + WALKING_COST * MAX_WALKING_DIST;
    public const int EXPRESS_COST = 20;
    public const int INHIBITORY_COST = 10000;


    public MapSearchNode(MapNode mp){
      mapNode = mp;
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
      if (nodeGoal.mapNode.floorNode != null)
      {
        if (mapNode.floorNode != null)
          h = (float)(Math.Abs(end_point.Y - mapNode.position.Y) * FLOOR_COST);
        else
          h = (float)(Math.Abs(end_point.X - mapNode.position.X) * WALKING_COST + Math.Abs(end_point.Y - mapNode.position.Y) * FLOOR_COST);
      }
      else
      {
        if (mapNode.floorNode != null)
          h = (float)(Math.Abs(nodeGoal.mapNode.position.X - start_point.X) * WALKING_COST + Math.Abs(nodeGoal.mapNode.position.Y - start_point.Y) * FLOOR_COST);
        else
          h = (float)(Math.Abs(nodeGoal.mapNode.position.X - mapNode.position.X) * WALKING_COST + Math.Abs(nodeGoal.mapNode.position.Y - mapNode.position.Y) * FLOOR_COST);
      }

      return h;
    }

    public bool IsGoal(MapSearchNode nodeGoal) {
      if (mapNode == nodeGoal.mapNode)
      {
        nodeGoal.g = g;
        nodeGoal.h = h;
        return true;
      }
      else return false;
    }

    public bool IsSameState(MapSearchNode rhs) {
      return (mapNode == rhs.mapNode);
    }

    public float GetCost(MapSearchNode successor)
    {
      //Item::Item* i = getItemOnRoute(&successor); // Discover item that lies on this node, given the next step
      //successor.parent_item = i;
      //successor.numStairs = numStairs;
      //successor.numEscalators = numEscalators;
      //successor.numElevators = numElevators;
      //successor.g = g;

      //int traverse_cost = 0;
      //if (!i)
      //{
      //  // NULL item. Next step is traversal on current floor.
      //  if (mapNode->position.y == 0 && successor.mapNode->position.y == 0)
      //    traverse_cost = 0; // Movement in main lobby is free(?)
      //  else if (!mapNode->floorNode && start_point.x > INT_MIN) // This is a floor start node, use start_point to compute cost instead
      //    traverse_cost = std::abs(successor.mapNode->position.x - start_point.x) * WALKING_COST;
      //  else if (!successor.mapNode->floorNode && mapNode->position.y == end_point.y)
      //  {
      //    // Successor is a floor end node, use end_point to compute cost instead
      //    int end_walking_dist = std::abs(end_point.x - mapNode->position.x);

      //    // Check if final transport node is too far away from destination, and adjust cost accordingly
      //    if (end_walking_dist > MAX_WALKING_DIST)
      //      traverse_cost = (int)(MAX_WALKING_DIST * WALKING_COST + (float)end_walking_dist / MAX_WALKING_DIST * INHIBITORY_COST);
      //    else
      //      traverse_cost = end_walking_dist * WALKING_COST;
      //  }
      //  else
      //    traverse_cost = std::abs(successor.mapNode->position.x - mapNode->position.x) * WALKING_COST;
      //}
      //else if (i->isStairlike())
      //{
      //  if (i->prototype->icon == 2)
      //  {
      //    traverse_cost += STAIRS_COST;
      //    successor.numStairs++;
      //  }
      //  else
      //  {
      //    traverse_cost += ESCALATOR_COST;
      //    successor.numEscalators++;
      //  }

      //  traverse_cost += FLOOR_COST;
      //}
      //else if (i->isElevator())
      //{
      //  if (i != parent_item)
      //  {
      //    // NOTE: In future when hotel rooms are implemented, 
      //    // there will need to be another check here to include Service elevator costs.
      //    if (i->prototype->icon == 4)
      //      traverse_cost += ELEVATOR_COST;
      //    else
      //      traverse_cost += EXPRESS_COST;
      //    successor.numElevators++;
      //  }

      //  traverse_cost += std::abs(successor.mapNode->position.y - mapNode->position.y) * FLOOR_COST;
      //}
      //else traverse_cost = INHIBITORY_COST; // ERROR: Should not end up here. INHIBITORY_COST should deter further expansion from this node.

      //successor.g += (float)traverse_cost;
      //return (float)traverse_cost;
      throw new NotImplementedException();
    }
  
  }
}
