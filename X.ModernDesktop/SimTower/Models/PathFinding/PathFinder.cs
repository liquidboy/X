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

      //unsigned int SearchSteps = 0;
      //do
      //{
      //  SearchState = astarsearch.SearchStep();
      //  SearchSteps++;
      //} while (SearchState == AStarSearch < MapSearchNode >::SEARCH_STATE_SEARCHING);

      Route r = new Route();
      buildRoute(r, start_item, end_item);
      clear();

      return r;
    }


    void clear()
    {
      //if (SearchState == AStarSearch < MapSearchNode >::SEARCH_STATE_SUCCEEDED) astarsearch.FreeSolutionNodes();
      //astarsearch.EnsureMemoryFreed();
    }

    void buildRoute(Route r, Item.Item start_item, Item.Item end_item)
    {

    }
  }
}
