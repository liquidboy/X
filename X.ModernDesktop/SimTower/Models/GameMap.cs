using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.ModernDesktop.SimTower.Models.Item;

namespace X.ModernDesktop.SimTower.Models
{
  class GameMap
  {
    private SortedDictionary<Slot, MapNode> gameMap;
    private SortedDictionary<int, List<MapNode>> mapNodesByFloor;
    private SortedDictionary<int, FloorNode> floorNodes;

    public GameMap() {
      gameMap = new SortedDictionary<Slot, MapNode>();
      mapNodesByFloor = new SortedDictionary<int, List<MapNode>>();
      floorNodes = new SortedDictionary<int, FloorNode>();
    }

    public void clear()
    {
      gameMap.Clear();
      mapNodesByFloor.Clear();
    }

    public void removeNode(Slot slot, X.ModernDesktop.SimTower.Models.Item.Item item)
    {
      if (item is null) return;
      if (!(item is IHaulsPeople)) return;
      if (!gameMap.ContainsKey(slot)) return;

      MapNode n = gameMap[slot];

      // Delete and erase only if no other overlapping item
      
      if (n.neighbours[(int)MapNode.Direction.LEFT]!=null)
      {
        n.neighbours[(int)MapNode.Direction.LEFT].neighbours[(int)MapNode.Direction.RIGHT] = n.neighbours[(int)MapNode.Direction.RIGHT];
      }

      if (n.neighbours[(int)MapNode.Direction.RIGHT]!=null)
      {
        n.neighbours[(int)MapNode.Direction.RIGHT].neighbours[(int)MapNode.Direction.LEFT] = n.neighbours[(int)MapNode.Direction.LEFT];
      }

      gameMap.Remove(slot);

      var mnl = mapNodesByFloor[slot.Y];
      //mnl.Clear();
      mnl.ForEach(x => {
        if (x == n) {
          x.Status = 1; // mark for deletion
        }
      });
      mnl.RemoveAll(x => x.Status == 1); // using status lets delete
      
    }

    public MapNode findNode(Slot slot, X.ModernDesktop.SimTower.Models.Item.Item item)
    {
      if (item is null) return null;

      if (floorNodes.ContainsKey(slot.Y)) return floorNodes[slot.Y];
      else return null;

      throw new NotImplementedException();
      return null;
    }

    public MapNode addNode(Slot slot, IPrototype item) {
      if (item is null) return null;

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

     if (!(item is IHaulsPeople)) return f; // Building item, do not add into transport

     // Create/Get MapNode for above created floor
      MapNode n;
      if (!gameMap.ContainsKey(slot))
      {
        n = new MapNode(f);
        gameMap[slot] = n;
        n.position = slot;

        // Just insert node if list is empty
        if (mapNodesByFloor[slot.Y].Count == 0) mapNodesByFloor[slot.Y].Add(n);
        else {
          // Look for nearby nodes on same floor to insert
          MapNode left = null;
          MapNode right = null;
          foreach (MapNode node in mapNodesByFloor[slot.Y]) {
            if (node.position.X < slot.X) left = node;
            else {
              right = node;
              var nodeIndex = mapNodesByFloor[slot.Y].IndexOf(node);
              mapNodesByFloor[slot.Y].Insert(nodeIndex, n);
              break;
            }
          }

          if (left != null) {
            if (right == null) mapNodesByFloor[slot.Y].Add(n); // Insert node as last node in list
            n.neighbours[(int)MapNode.Direction.LEFT] = left;
            left.neighbours[(int)MapNode.Direction.RIGHT] = n;
          }
          if (right != null) {
            n.neighbours[(int)MapNode.Direction.LEFT] = left;
            right.neighbours[(int)MapNode.Direction.LEFT] = n;
          }
        }

      }
      else {
        n = gameMap[slot];
      }


      if (item is IHaulsPeople && slot.Y == item.Position.Y)
      {
        // If stairlike, automatically create pair node above and link accordingly
      //  assert(n->neighbours[MapNode::UP] == NULL);
      //  assert(n->transportItems[MapNode::UP] == NULL);
        MapNode n_upper = addNode(new Slot(slot.X, slot.Y + 1), item);

        n.neighbours[(int)MapNode.Direction.UP] = n_upper;
        n.transportItems[(int)MapNode.Direction.UP] = item;

      //  assert(n_upper->neighbours[MapNode::DOWN] == NULL);
      //  assert(n_upper->transportItems[MapNode::DOWN] == NULL);
        n_upper.neighbours[(int)MapNode.Direction.DOWN] = n;
        n_upper.transportItems[(int)MapNode.Direction.DOWN] = item;
      }
      else if (item is Elevator)
      {
        n.HasElevator = true;
        if (item.Icon == (int)IconNumbers.ICON_ELEVATOR_SERVICE) n.HasServiceElevator = true;

        // Link to upper/lower floor node
        var e = (Elevator)item;
        for (int i = slot.Y + 1; i < item.Position.Y + item.Size.Y; i++)
        {
          if (e.ConnectsToFloor(i))
          {
            var ep = new Slot(slot.X, i);
            if (!gameMap.ContainsKey(ep)) addNode(ep, item);
            else
            {
              MapNode upper = gameMap[ep];
              n.neighbours[(int)MapNode.Direction.UP] = upper;
              n.transportItems[(int)MapNode.Direction.UP] = item;

              if (upper.neighbours[(int)MapNode.Direction.DOWN] != null)
              {
                n.neighbours[(int)MapNode.Direction.DOWN] = upper.neighbours[(int)MapNode.Direction.DOWN];
                n.transportItems[(int)MapNode.Direction.DOWN] = item;
              }

              upper.neighbours[(int)MapNode.Direction.DOWN] = n;
              upper.transportItems[(int)MapNode.Direction.DOWN] = item;
            }
            break;
          }
        }

        for (int i = slot.Y - 1; i >= item.Position.Y; i--)
        {
          if (e.ConnectsToFloor(i))
          {
      //      assert(gameMap.count(MapNode::Point(p.x, i)) != 0);
            MapNode lower = gameMap[new Slot(slot.X, i)];
            n.neighbours[(int)MapNode.Direction.DOWN] = lower;
            n.transportItems[(int)MapNode.Direction.DOWN] = item;

            if (lower.neighbours[(int)MapNode.Direction.UP] != null)
            {
              n.neighbours[(int)MapNode.Direction.UP] = lower.neighbours[(int)MapNode.Direction.UP];
              n.transportItems[(int)MapNode.Direction.UP] = item;
            }

            lower.neighbours[(int)MapNode.Direction.UP] = n;
            lower.transportItems[(int)MapNode.Direction.UP] = item;
            break;
          }
        }
      }

      return n;
    }
  }
}
