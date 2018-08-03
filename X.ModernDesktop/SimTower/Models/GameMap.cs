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

    public GameMap() {
      gameMap = new SortedDictionary<Slot, MapNode>();
      mapNodesByFloor = new SortedDictionary<int, List<MapNode>>();
      floorNodes = new SortedDictionary<int, FloorNode>();
    }

    MapNode addNode(Slot slot) {
      
      FloorNode f;
      if (!floorNodes.ContainsKey(slot.Y))
      {
        f = new FloorNode(mapNodesByFloor[slot.Y]);
        floorNodes[slot.Y] = f;
        f.position = new Slot(int.MinValue, slot.Y);
      }
      else f = floorNodes[slot.Y];

      MapNode n;
      if (!gameMap.ContainsKey(slot))
      {
        n = new MapNode(f);
        gameMap[slot] = n;
        n.position = slot;

        if (mapNodesByFloor[slot.Y] == null) mapNodesByFloor[slot.Y].Add(n);
        else {
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
            if (right == null) mapNodesByFloor[slot.Y].Add(n);
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


      //if (item->isStairlike() && p.y == item->position.y)
      //{
      //  // If stairlike, automatically create pair node above and link accordingly
      //  assert(n->neighbours[MapNode::UP] == NULL);
      //  assert(n->transportItems[MapNode::UP] == NULL);
      //  MapNode* n_upper = addNode(MapNode::Point(p.x, p.y + 1), item);

      //  n->neighbours[MapNode::UP] = n_upper;
      //  n->transportItems[MapNode::UP] = item;

      //  assert(n_upper->neighbours[MapNode::DOWN] == NULL);
      //  assert(n_upper->transportItems[MapNode::DOWN] == NULL);
      //  n_upper->neighbours[MapNode::DOWN] = n;
      //  n_upper->transportItems[MapNode::DOWN] = item;
      //}
      //else if (item->isElevator())
      //{
      //  n->hasElevator = true;
      //  if (item->prototype->icon == 5) n->hasServiceElevator = true;
      //  // Link to upper/lower floor node
      //  Item::Elevator::Elevator* e = (Item::Elevator::Elevator*)item;
      //  for (int i = p.y + 1; i < item->position.y + item->size.y; i++)
      //  {
      //    if (e->connectsFloor(i))
      //    {
      //      MapNode::Point ep(p.x, i);
      //      if (gameMap.count(ep) == 0) addNode(ep, item);
      //      else
      //      {
      //        MapNode* upper = gameMap[ep];
      //        n->neighbours[MapNode::UP] = upper;
      //        n->transportItems[MapNode::UP] = item;

      //        if (upper->neighbours[MapNode::DOWN])
      //        {
      //          n->neighbours[MapNode::DOWN] = upper->neighbours[MapNode::DOWN];
      //          n->transportItems[MapNode::DOWN] = item;
      //        }

      //        upper->neighbours[MapNode::DOWN] = n;
      //        upper->transportItems[MapNode::DOWN] = item;
      //      }
      //      break;
      //    }
      //  }

      //  for (int i = p.y - 1; i >= item->position.y; i--)
      //  {
      //    if (e->connectsFloor(i))
      //    {
      //      assert(gameMap.count(MapNode::Point(p.x, i)) != 0);
      //      MapNode* lower = gameMap[MapNode::Point(p.x, i)];
      //      n->neighbours[MapNode::DOWN] = lower;
      //      n->transportItems[MapNode::DOWN] = item;

      //      if (lower->neighbours[MapNode::UP])
      //      {
      //        n->neighbours[MapNode::UP] = lower->neighbours[MapNode::UP];
      //        n->transportItems[MapNode::UP] = item;
      //      }

      //      lower->neighbours[MapNode::UP] = n;
      //      lower->transportItems[MapNode::UP] = item;
      //      break;
      //    }
      //  }
      //}

      return n;
    }
  }
}
