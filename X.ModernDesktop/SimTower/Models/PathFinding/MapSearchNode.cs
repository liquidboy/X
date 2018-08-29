using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.ModernDesktop.SimTower.Models.Item;
using static X.ModernDesktop.SimTower.Models.PathFinding.MapNode;

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

    int numStairs;
    int numEscalators;
    int numElevators;

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
      MapSearchNode n;
      MapNode left = null;
      MapNode right = null;
      bool ret;

      if (parent_node != null && mapNode.floorNode != null)
      {
        // Start node which is also a floor node.
        // Find and add nearest transport nodes to the left/right (based on start_point, which should be populated)
        List<MapNode> nodesOnFloor = mapNode.nodesOnFloor;
        //assert(nodesOnFloor != NULL);
        foreach(var nodex in nodesOnFloor)
        //for (List<MapNode> const_iterator i = nodesOnFloor->begin(); i != nodesOnFloor->end(); i++)
        {
          //MapNode node = *i;
          if (nodex.position.X <= start_point.X && canTransfer(this, nodex, Direction.LEFT)) left = nodex;
          else if (canTransfer(this, nodex, Direction.RIGHT))
          {
            right = nodex;
            break;
          }
        }

        if (left != null)
        {
          createNode(out n, left);
          ret = astarsearch.AddSuccessor(n);
          if (!ret) return false;
        }

        if (right != null)
        {
          createNode(out n, right);
          ret = astarsearch.AddSuccessor(n);
          if (!ret) return false;
        }

        return true;
      }

      //if (mapNode.position.Y == end_point.Y && astarsearch.GetSolutionEnd().mapNode.floorNode != null)
      //{
      //  // Reached the same floor as end node, and end node is a floor node.
      //  // Add destination floor node
      //  if (mapNode.floorNode != null) return false; // ERROR: All transport nodes must contain pointer to their respective floor node.

      //  createNode(out n, mapNode.floorNode);
      //  ret = astarsearch.AddSuccessor(n);
      //  if (!ret) return false;
      //  else return true;
      //}

      MapNode node = mapNode.neighbours[(int)Direction.LEFT];
      while (node != null && left == null)
      {
        if (!canTransfer(this, node, Direction.LEFT))
        {
          node = node.neighbours[(int)Direction.LEFT];
          continue;
        }

        left = node;
      }

      node = mapNode.neighbours[(int)Direction.RIGHT];
      while (node != null && right == null)
      {
        if (!canTransfer(this, node, Direction.RIGHT))
        {
          node = node.neighbours[(int)Direction.RIGHT];
          continue;
        }

        right = node;
      }

      if (left != null)
      {
        createNode(out n, left);
        ret = astarsearch.AddSuccessor(n);
        if (!ret) return false;
      }

      if (right != null)
      {
        createNode(out n, right);
        ret = astarsearch.AddSuccessor(n);
        if (!ret) return false;
      }

      if (mapNode.neighbours[(int)Direction.UP] != null)
      {
        node = mapNode.neighbours[(int)Direction.UP];
        if (mapNode.HasElevator)
        {
          while (node != null)
          {
            createNode(out n, node);
            ret = astarsearch.AddSuccessor(n);
            if (!ret) return false;
            node = node.neighbours[(int)Direction.UP];
          }
        }
        else if (canTransfer(this, node, Direction.UP))
        {
          createNode(out n, node);
          ret = astarsearch.AddSuccessor(n);
          if (!ret) return false;
        }
      }

      if (mapNode.neighbours[(int)Direction.DOWN] != null)
      {
        node = mapNode.neighbours[(int)Direction.DOWN];
        if (mapNode.HasElevator)
        {
          while (node != null)
          {
            createNode(out n, node);
            ret = astarsearch.AddSuccessor(n);
            if (!ret) return false;
            node = node.neighbours[(int)Direction.DOWN];
          }
        }
        else if (canTransfer(this, node, Direction.DOWN))
        {
          createNode(out n, node);
          ret = astarsearch.AddSuccessor(n);
          if (!ret) return false;
        }
      }

      return true;
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
      Item.Item i = getItemOnRoute(successor); // Discover item that lies on this node, given the next step
      successor.parent_item = i;
      successor.numStairs = numStairs;
      successor.numEscalators = numEscalators;
      successor.numElevators = numElevators;
      successor.g = g;

      int traverse_cost = 0;
      if (i != null)
      {
        // NULL item. Next step is traversal on current floor.
        if (mapNode.position.Y == 0 && successor.mapNode.position.Y == 0)
          traverse_cost = 0; // Movement in main lobby is free(?)
        else if (mapNode.floorNode != null && start_point.X > int.MinValue) // This is a floor start node, use start_point to compute cost instead
          traverse_cost = Math.Abs(successor.mapNode.position.X - start_point.X) * WALKING_COST;
        else if (successor.mapNode.floorNode != null && mapNode.position.Y == end_point.Y)
        {
          // Successor is a floor end node, use end_point to compute cost instead
          int end_walking_dist = Math.Abs(end_point.X - mapNode.position.X);

          // Check if final transport node is too far away from destination, and adjust cost accordingly
          if (end_walking_dist > MAX_WALKING_DIST)
            traverse_cost = (int)(MAX_WALKING_DIST * WALKING_COST + (float)end_walking_dist / MAX_WALKING_DIST * INHIBITORY_COST);
          else
            traverse_cost = end_walking_dist * WALKING_COST;
        }
        else
          traverse_cost = Math.Abs(successor.mapNode.position.X - mapNode.position.X) * WALKING_COST;
      }
      else if (i is IHaulsPeople && !(i is Elevator))
      {
        if (((IPrototype)i).Icon == 2)
        {
          traverse_cost += STAIRS_COST;
          successor.numStairs++;
        }
        else
        {
          traverse_cost += ESCALATOR_COST;
          successor.numEscalators++;
        }

        traverse_cost += FLOOR_COST;
      }
      else if (i is Elevator)
      {
        if (i != parent_item)
        {
          // NOTE: In future when hotel rooms are implemented, 
          // there will need to be another check here to include Service elevator costs.
          if (((IPrototype)i).Icon == 4)
            traverse_cost += ELEVATOR_COST;
          else
            traverse_cost += EXPRESS_COST;
          successor.numElevators++;
        }

        traverse_cost += Math.Abs(successor.mapNode.position.Y - mapNode.position.Y) * FLOOR_COST;
      }
      else traverse_cost = INHIBITORY_COST; // ERROR: Should not end up here. INHIBITORY_COST should deter further expansion from this node.

      successor.g += (float)traverse_cost;
      return (float)traverse_cost;
    }

    private Item.Item getItemOnRoute(MapSearchNode successor) {
      // NOTE: Calling this on the start and end nodes will not give correct results.
      // Always use known start and destination items instead.

      MapNode s_mapnode;

	    if(successor != null || successor.mapNode != null) return null;
	    else s_mapnode = successor.mapNode;

	    if(mapNode.neighbours[(int)Direction.UP] == s_mapnode)
		     return (Item.Item)mapNode.transportItems[(int)Direction.UP];
	    else if(mapNode.neighbours[(int)Direction.DOWN] == s_mapnode)
		     return (Item.Item)mapNode.transportItems[(int)Direction.DOWN];
	    else if(mapNode.transportItems[(int)Direction.UP] != null && mapNode.transportItems[(int)Direction.UP] == s_mapnode.transportItems[(int)Direction.DOWN])
		    return (Item.Item)mapNode.transportItems[(int)Direction.UP];
	    else if(mapNode.transportItems[(int)Direction.DOWN] != null && mapNode.transportItems[(int)Direction.DOWN] == s_mapnode.transportItems[(int)Direction.UP])
		    return (Item.Item)mapNode.transportItems[(int)Direction.DOWN];
	    else
		    return null;
    }

    bool canTransfer(MapSearchNode start, MapNode dest, Direction dir){
	    /* 
		    Returns true if transfer to destination transport has not exceed transfer limits.
		    Returns false otherwise.
		    Maximum transfer limits are:
			    2 elevators (no more than 1 elevator if journey has already used stairs or escalators)
			    3 stairs
			    6 escalators
			    1 stair, 2 escalators OR 2 stairs, 1 escalator
	    */

	    if((dir == Direction.UP || dir == Direction.DOWN)) {
		    // Once in an elevator node, travel is allowed for as many floors available in the elevator shaft
		    if(start.mapNode.HasElevator) return true;

		    // Hence we only check limits for stairlike travel
		    if(start.mapNode.transportItems[(int)dir].Icon == 2) {
			    if(start.numStairs > 2 - start.numEscalators) return false;
		    } else {
			    if(start.numEscalators > 5) return false;
			    if(start.numStairs > 0 && start.numEscalators > 2 - start.numStairs) return false;
		    }
	    } else {
		    if(dest.HasElevator) {
			    if((!start.serviceRoute && dest.HasServiceElevator) || (start.serviceRoute && !dest.HasServiceElevator)) return false;
			    if(start.numElevators > 1) return false;
			    if(start.numElevators == 1 && (start.mapNode.position.Y % 15 != 0 || start.numStairs > 0 || start.numEscalators > 0)) return false;
		    }
		    // Transit to other stairlike nodes (moving left/right) has no limits.
		    // Limit only applies when actually using (moving up/down) the stairlike.
	    }

	    return true;
    }


    void createNode(out MapSearchNode n, MapNode node) {
      n = new MapSearchNode(node);
      n.end_point = end_point;
	    n.serviceRoute = serviceRoute;
    }

  }
}
