using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.ModernDesktop.SimTower.Models.Item;

namespace X.ModernDesktop.SimTower.Models.PathFinding
{
  class PathFinder
  {
    AStarPathfinder astarsearch ;
    AStarPathfinder.SearchState SearchState;

    public PathFinder() {
      astarsearch = new AStarPathfinder();
    }

    public Route FindRoute(MapNode start_mapnode, MapNode end_mapnode, Item.Item start_item, Item.Item end_item, bool serviceRoute) {

      Slot start_point = new Slot(start_item.Position.X + ((IPrototype)start_item).Size.X , start_mapnode.position.Y);
      Slot end_point = new Slot(end_item.Position.X + ((IPrototype)end_item).Size.X, end_mapnode.position.Y);

      MapSearchNode nodeStart = new MapSearchNode(start_mapnode);
      nodeStart.parent_item = start_item;
      nodeStart.start_point = start_point;
      nodeStart.end_point = end_point;
      nodeStart.serviceRoute = serviceRoute;
      MapSearchNode nodeEnd = new MapSearchNode(end_mapnode);
      astarsearch.SetStartAndGoalStates(nodeStart, nodeEnd);

      int SearchSteps = 0;
      do
      {
        SearchState = astarsearch.SearchStep();
        SearchSteps++;
      } while (SearchState == AStarPathfinder.SearchState.Searching);

      Route r = new Route();
      buildRoute(r, start_item, end_item);
      clear();

      return r;
    }


    void clear()
    {
      if (SearchState == AStarPathfinder.SearchState.Succeeded) astarsearch.FreeSolutionNodes();
      //astarsearch.EnsureMemoryFreed();
    }

    void buildRoute(Route r, Item.Item start_item, Item.Item end_item)
    {
      if (SearchState != AStarPathfinder.SearchState.Succeeded)
      {
        r.Clear();
        return;
      }

      if (start_item == null || end_item == null)
      {
        r.Clear();
        return;
      }

      //MapSearchNode end_node = astarsearch.GetSolutionEnd();
      //MapSearchNode start_node = astarsearch.GetSolutionStart();

      //if (start_node.IsSameState(end_node))
      //{
      //  //r.add(start_item, start_node.mapNode.position.Y);
      //  //r.add(end_item, end_node.mapNode.position.Y);
      //  return;
      //}

      //MapSearchNode n = start_node;
      //MapSearchNode n_child;
      //r.add(start_item, n.mapNode.position.Y);
      //n = astarsearch.GetSolutionNext();
      //while (n != end_node)
      //{
      //  n_child = astarsearch.GetSolutionNext();
      //  Item.Item i = n_child.parent_item;
      //  if (i != null)
      //  {
      //    int toFloor;
      //    if (i is IHaulsPeople && !(i is Elevator)) toFloor = n_child.mapNode.position.Y;
      //    else toFloor = n.mapNode.position.Y;

      //    Route::Node & rn_prev = r.nodes.back();
      //    if (i is Elevator && rn_prev.item->isElevator() && i == rn_prev.item)
      //    {
      //      // Moving along same elevator
      //      rn_prev.toFloor = toFloor;
      //    }
      //    else r.add(i, toFloor);
      //  }
      //  n = n_child;
      //};
      //r.add(end_item, n.mapNode.position.Y);
      //r.updateScore((int)Math.Abs(end_node.g + end_node.h));
    }
  }
}
