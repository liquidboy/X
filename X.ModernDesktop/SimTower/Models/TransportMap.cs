using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.ModernDesktop.SimTower.Models.Item;
using static X.ModernDesktop.SimTower.Models.MapNode;

namespace X.ModernDesktop.SimTower.Models
{
  class TransportMap
  {
    private SortedDictionary<string, MapNode> peopleMovementMap;
    private SortedDictionary<int, List<MapNode>> mapNodesByFloor;
    private SortedDictionary<int, FloorNode> floorNodes;

    public TransportMap()
    {
      peopleMovementMap = new SortedDictionary<string, MapNode>();
      mapNodesByFloor = new SortedDictionary<int, List<MapNode>>();
      floorNodes = new SortedDictionary<int, FloorNode>();
    }

    public void clear()
    {
      peopleMovementMap.Clear();
      mapNodesByFloor.Clear();
    }

    public void removeNode(Slot slot, IPrototype item)
    {
      return;
      if (item is null) return;
      if (!(item is IHaulsPeople)) return;
      if (!peopleMovementMap.ContainsKey(slot.ToString())) return;

      MapNode n = peopleMovementMap[slot.ToString()];


      // Update all neighour links
      if (item is IHaulsPeople && slot.Y == item.Position.Y)
      {
        n.neighbours[(int)Direction.UP].neighbours[(int)Direction.DOWN] = null;
        n.neighbours[(int)Direction.UP].transportItems[(int)Direction.DOWN] = null;
        n.neighbours[(int)Direction.UP] = null;
        n.transportItems[(int)Direction.UP] = null;
      }
      else if (item is Elevator)
      {
        n.HasElevator = false;
        if (item.Icon == (int)IconNumbers.ICON_ELEVATOR_SERVICE) n.HasServiceElevator = false;

        // Link upper & lower floor node to skip this node
        if (n.neighbours[(int)Direction.UP] != null)
        {
          n.neighbours[(int)Direction.UP].neighbours[(int)Direction.DOWN] = n.neighbours[(int)Direction.DOWN];
          if (n.neighbours[(int)Direction.DOWN] == null) n.neighbours[(int)Direction.UP].transportItems[(int)Direction.DOWN] = null;
        }

        if (n.neighbours[(int)Direction.DOWN] != null)
        {
          n.neighbours[(int)Direction.DOWN].neighbours[(int)Direction.UP] = n.neighbours[(int)Direction.UP];
          if (n.neighbours[(int)Direction.UP] == null) n.neighbours[(int)Direction.DOWN].transportItems[(int)Direction.UP] = null;
        }

        n.neighbours[(int)Direction.UP] = null;
        n.transportItems[(int)Direction.UP] = null;
        n.neighbours[(int)Direction.DOWN] = null;
        n.transportItems[(int)Direction.DOWN] = null;
      }



      // Delete and erase only if no other overlapping item
      if (!n.HasElevator && !n.HasServiceElevator &&
        n.transportItems[(int)Direction.UP] == null &&
        n.transportItems[(int)Direction.DOWN] == null)
      {


        if (n.neighbours[(int)MapNode.Direction.LEFT] != null)
        {
          n.neighbours[(int)MapNode.Direction.LEFT].neighbours[(int)MapNode.Direction.RIGHT] = n.neighbours[(int)MapNode.Direction.RIGHT];
        }

        if (n.neighbours[(int)MapNode.Direction.RIGHT] != null)
        {
          n.neighbours[(int)MapNode.Direction.RIGHT].neighbours[(int)MapNode.Direction.LEFT] = n.neighbours[(int)MapNode.Direction.LEFT];
        }

        peopleMovementMap.Remove(slot.ToString());

        var mnl = mapNodesByFloor[slot.Y];
        //mnl.Clear();
        mnl.ForEach(x => {
          if (x == n)
          {
            x.Status = 1; // mark for deletion
          }
        });
        mnl.RemoveAll(x => x.Status == 1); // using status lets delete

      }
    }

    public MapNode findNode(Slot slot, X.ModernDesktop.SimTower.Models.Item.Item item)
    {
      if (item is null) return null;

      if (floorNodes.ContainsKey(slot.Y)) return floorNodes[slot.Y];
      else return null;

      throw new NotImplementedException();
    }



    public (FloorNode, MapNode) createFloorAndNodeIfApplicable(Slot slot, IPrototype item)
    {

      // Create corresponding FloorNode if not available
      FloorNode f;
      if (!floorNodes.ContainsKey(slot.Y))
      {
        if (!mapNodesByFloor.ContainsKey(slot.Y)) mapNodesByFloor[slot.Y] = new List<MapNode>();
        f = new FloorNode(mapNodesByFloor[slot.Y]);
        floorNodes[slot.Y] = f;
        f.position = new Slot(int.MinValue, slot.Y);
      }
      else f = floorNodes[slot.Y];

      if (!(item is IHaulsPeople)) return (f, null); // Building item, do not add into transport

      // Create/Get MapNode for above created floor
      MapNode n;
      if (!peopleMovementMap.ContainsKey(slot.ToString()))
      {
        n = new MapNode(f);
        peopleMovementMap[slot.ToString()] = n;
        n.position = slot;

        // Just insert node if list is empty
        if (mapNodesByFloor[slot.Y].Count == 0) mapNodesByFloor[slot.Y].Add(n);
        else
        {
          // Look for nearby nodes on same floor to insert
          MapNode left = null;
          MapNode right = null;
          foreach (MapNode node in mapNodesByFloor[slot.Y])
          {
            if (node.position.X < slot.X) left = node;
            else
            {
              right = node;
              var nodeIndex = mapNodesByFloor[slot.Y].IndexOf(node);
              mapNodesByFloor[slot.Y].Insert(nodeIndex, n);
              break;
            }
          }

          if (left != null)
          {
            if (right == null) mapNodesByFloor[slot.Y].Add(n); // Insert node as last node in list
            n.neighbours[(int)MapNode.Direction.LEFT] = left;
            left.neighbours[(int)MapNode.Direction.RIGHT] = n;
          }

          if (right != null)
          {
            n.neighbours[(int)MapNode.Direction.RIGHT] = right;
            right.neighbours[(int)MapNode.Direction.LEFT] = n;
          }
        }

      }
      else
      {
        n = peopleMovementMap[slot.ToString()];
      }

      return (f, n);
    }

    public MapNode extendNode(Slot slotBeforeExtending, Slot sizeBeforeExtending, IPrototype itemAfterExtending)
    {
      (FloorNode f, MapNode n) ret = createFloorAndNodeIfApplicable(slotBeforeExtending, itemAfterExtending);

      if (itemAfterExtending is Elevator)
      {
        // Link to upper/lower floor node
        var e = (Elevator)itemAfterExtending;

        var expandedUp = itemAfterExtending.Position.Y > slotBeforeExtending.Y;
        if (expandedUp)
        {
          var topIndex = itemAfterExtending.Position.Y;

          if (!e.ConnectsToFloor(topIndex, sizeBeforeExtending.Y))
          {
            var ep = new Slot(itemAfterExtending.Position.X, topIndex);
            MapNode upper = peopleMovementMap.ContainsKey(ep.ToString()) ? peopleMovementMap[ep.ToString()] : addNode(ep, itemAfterExtending);

            ret.n.neighbours[(int)MapNode.Direction.UP] = upper;
            ret.n.transportItems[(int)MapNode.Direction.UP] = itemAfterExtending;

            //if (upper.neighbours[(int)MapNode.Direction.DOWN] != null)
            //{
            //  ret.n.neighbours[(int)MapNode.Direction.DOWN] = upper.neighbours[(int)MapNode.Direction.DOWN];
            //  ret.n.transportItems[(int)MapNode.Direction.DOWN] = itemAfterExtending;
            //}

            upper.neighbours[(int)MapNode.Direction.DOWN] = ret.n;
            upper.transportItems[(int)MapNode.Direction.DOWN] = itemAfterExtending;

          }
        }
        else
        { //expanded down
          var bottomIndex = itemAfterExtending.Position.Y - itemAfterExtending.Size.Y + 1;
          var topIndex = bottomIndex+1;
          if (!e.ConnectsToFloor(bottomIndex, sizeBeforeExtending.Y))
          {
            var epLower = new Slot(itemAfterExtending.Position.X, bottomIndex);
            var epUpper = new Slot(itemAfterExtending.Position.X, topIndex);
            MapNode lower = peopleMovementMap.ContainsKey(epLower.ToString()) ? peopleMovementMap[epLower.ToString()] : addNode(epLower, itemAfterExtending);
            MapNode upper = peopleMovementMap.ContainsKey(epUpper.ToString()) ? peopleMovementMap[epUpper.ToString()] : addNode(epUpper, itemAfterExtending);

            upper.neighbours[(int)MapNode.Direction.DOWN] = lower;
            upper.transportItems[(int)MapNode.Direction.DOWN] = itemAfterExtending;
            //ret.n.neighbours[(int)MapNode.Direction.DOWN] = lower;
            //ret.n.transportItems[(int)MapNode.Direction.DOWN] = itemAfterExtending;

            //if (lower.neighbours[(int)MapNode.Direction.UP] != null)
            //{
            //  ret.n.neighbours[(int)MapNode.Direction.UP] = lower.neighbours[(int)MapNode.Direction.UP];
            //  ret.n.transportItems[(int)MapNode.Direction.UP] = itemAfterExtending;
            //}

            lower.neighbours[(int)MapNode.Direction.UP] = upper;
            lower.transportItems[(int)MapNode.Direction.UP] = itemAfterExtending;
          }
        }

      }

      return ret.n;
    }

    public MapNode addNode(Slot slot, IPrototype item)
    {
      if (item is null) return null;

      (FloorNode f, MapNode n) ret = createFloorAndNodeIfApplicable(slot, item);

      //if (ret.n == null) return null;

      //if (item is IHaulsPeople && slot.Y == item.Position.Y)
      //{
      //  // If stairlike, automatically create pair node above and link accordingly
      //  // assert(n->neighbours[MapNode::UP] == NULL);
      //  // assert(n->transportItems[MapNode::UP] == NULL);
      //  MapNode n_upper = addNode(new Slot(slot.X, slot.Y + 1), item);

      //  ret.n.neighbours[(int)MapNode.Direction.UP] = n_upper;
      //  ret.n.transportItems[(int)MapNode.Direction.UP] = item;

      //  // assert(n_upper->neighbours[MapNode::DOWN] == NULL);
      //  // assert(n_upper->transportItems[MapNode::DOWN] == NULL);
      //  n_upper.neighbours[(int)MapNode.Direction.DOWN] = ret.n;
      //  n_upper.transportItems[(int)MapNode.Direction.DOWN] = item;
      //}
      //else if (item is Elevator)
      //{
      //  ret.n.HasElevator = true;
      //  if (item.Icon == (int)IconNumbers.ICON_ELEVATOR_SERVICE) ret.n.HasServiceElevator = true;

      //  // Link to upper/lower floor node
      //  var e = (Elevator)item;

      //  var topIndex = item.Position.Y + 1;
      //  for (int i = slot.Y + 1; i < item.Position.Y; i++)
      //  {
      //    if (e.ConnectsToFloor(i))
      //    {
      //      var ep = new Slot(slot.X, i);
      //      if (!peopleMovementMap.ContainsKey(ep)) addNode(ep, item);
      //      else
      //      {
      //        MapNode upper = peopleMovementMap[ep];
      //        ret.n.neighbours[(int)MapNode.Direction.UP] = upper;
      //        ret.n.transportItems[(int)MapNode.Direction.UP] = item;

      //        if (upper.neighbours[(int)MapNode.Direction.DOWN] != null)
      //        {
      //          ret.n.neighbours[(int)MapNode.Direction.DOWN] = upper.neighbours[(int)MapNode.Direction.DOWN];
      //          ret.n.transportItems[(int)MapNode.Direction.DOWN] = item;
      //        }

      //        upper.neighbours[(int)MapNode.Direction.DOWN] = ret.n;
      //        upper.transportItems[(int)MapNode.Direction.DOWN] = item;
      //      }
      //      break;
      //    }
      //  }
      //  var bottomIndex = item.Position.Y - item.Size.Y - 1;
      //  for (int i = slot.Y - 1; i >= bottomIndex; i--)
      //  {
      //    if (e.ConnectsToFloor(i))
      //    {
      //      var ep = new Slot(slot.X, i);
      //      //if (!peopleMovementMap.ContainsKey(ep)) addNode(ep, item);
      //      //else
      //      //{
      //      // assert(gameMap.count(MapNode::Point(p.x, i)) != 0);
      //      MapNode lower = peopleMovementMap[ep];
      //      ret.n.neighbours[(int)MapNode.Direction.DOWN] = lower;
      //      ret.n.transportItems[(int)MapNode.Direction.DOWN] = item;

      //      if (lower.neighbours[(int)MapNode.Direction.UP] != null)
      //      {
      //        ret.n.neighbours[(int)MapNode.Direction.UP] = lower.neighbours[(int)MapNode.Direction.UP];
      //        ret.n.transportItems[(int)MapNode.Direction.UP] = item;
      //      }

      //      lower.neighbours[(int)MapNode.Direction.UP] = ret.n;
      //      lower.transportItems[(int)MapNode.Direction.UP] = item;
      //      //}
      //      break;
      //    }
      //  }
      //}

      return ret.n;
    }
  }
}
