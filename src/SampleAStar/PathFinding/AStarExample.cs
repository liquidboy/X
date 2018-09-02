using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace SampleAStar
{
  // NodePosition
  // Path
  // Map
  // MapSearchNode
  
  public class AStarExample
  {
    static FrameworkElement _visualGrid;

    public static void Start(FrameworkElement fe, Point end)
    {
      if (_visualGrid == null) _visualGrid = fe;
      AStarPathfinder pathfinder = new AStarPathfinder();
      Pathfind(new NodePosition(0, 0), new NodePosition((int)end.X, (int)end.Y), pathfinder);
    }

    static bool Pathfind(NodePosition startPos, NodePosition goalPos, AStarPathfinder pathfinder)
    {
      // Reset the allocated MapSearchNode pointer
      pathfinder.InitiatePathfind();

      // Create a start state
      MapSearchNode nodeStart = pathfinder.AllocateMapSearchNode(startPos);

      // Define the goal state
      MapSearchNode nodeEnd = pathfinder.AllocateMapSearchNode(goalPos);

      // Set Start and goal states
      pathfinder.SetStartAndGoalStates(nodeStart, nodeEnd);

      // Set state to Searching and perform the search
      AStarPathfinder.SearchState searchState = AStarPathfinder.SearchState.Searching;
      uint searchSteps = 0;

      do
      {
        searchState = pathfinder.SearchStep();
        searchSteps++;
      }
      while (searchState == AStarPathfinder.SearchState.Searching);

      // Search complete
      bool pathfindSucceeded = (searchState == AStarPathfinder.SearchState.Succeeded);

      if (pathfindSucceeded)
      {
        // Success
        Path newPath = new Path();
        int numSolutionNodes = 0; // Don't count the starting cell in the path length

        // Get the start node
        MapSearchNode node = pathfinder.GetSolutionStart();
        newPath.Add(node.position);
        ++numSolutionNodes;

        // Get all remaining solution nodes
        for (; ; )
        {
          node = pathfinder.GetSolutionNext();

          if (node == null)
          {
            break;
          }

          ++numSolutionNodes;
          newPath.Add(node.position);

          // DRAW PATH TO GOAL
          var vnName = $"vnx{node.position.x}y{node.position.y}";
          VisualNode foundVisualNode = _visualGrid.FindName(vnName) as VisualNode;
          foundVisualNode?.SetDot(true);
        };

        // Once you're done with the solution we can free the nodes up
        pathfinder.FreeSolutionNodes();


        Debug.WriteLine("Solution path length: " + numSolutionNodes);
        Debug.WriteLine("Solution: " + newPath.ToString());
      }
      else if (searchState == AStarPathfinder.SearchState.Failed)
      {
        // FAILED, no path to goal
        Debug.WriteLine("Pathfind FAILED!");
      }

      return pathfindSucceeded;
    }
  }
}