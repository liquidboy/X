using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace SampleAStar
{
  class MazeGenerator
  {
    private Stack<Point> stack = new Stack<Point>();
    private Random rand = new Random();
    private int[,] maze;
    private Point dimension;

    enum MazeNodeType {
      NoAccess = 9,
      Free = 1
    }

    public int[] GetGeneratedMazeAsSingleDimensionArray(bool useTestMap = false) {

      if (useTestMap) {
        return getTestMap();
      } else generateMaze();

      int[] ret = new int[maze.Length];

      for (int y = 0; y < dimension.Y; y++)
      {
        for (int x = 0; x < dimension.X; x++)
        {
          var indexPosition = (int)(y * dimension.X) + x;
          ret[indexPosition] = maze[x, y] == 1 ? (int)MazeNodeType.Free : (int)MazeNodeType.NoAccess;
        }
      }

      //Buffer.BlockCopy(maze, 0, ret, 0, maze.Length);
      return ret;
      //return maze.Cast<int>().ToArray();
    }

    public MazeGenerator(Point dimension)
    {
      maze = new int[(int)dimension.X, (int)dimension.Y];
      this.dimension = dimension;
    }


    private void generateMaze()
    {
      stack.Push(new Point(0, 0));
      while (stack.Count() != 0)
      {
        Point next = stack.Pop();
        if (validNextNode(next))
        {
          maze[(int)next.X, (int)next.Y] = 1;
          List<Point> neighbors = findNeighbors(next);
          randomlyAddNodesToStack(neighbors);
        }
      }
    }


    private Boolean validNextNode(Point node)
    {
      int numNeighboringOnes = 0;
      for (int y = (int)node.Y - 1; y < (int)node.Y + 2; y++)
      {
        for (int x = (int)node.X - 1; x < (int)node.X + 2; x++)
        {
          if (pointOnGrid(x, y) && pointNotNode(node, x, y) && maze[x, y] == 1)
          {
            numNeighboringOnes++;
          }
        }
      }
      return (numNeighboringOnes < 3) && maze[(int)node.X, (int)node.Y] != 1;
    }

    private void randomlyAddNodesToStack(List<Point> nodes)
    {
      int targetIndex;
      while (nodes.Count() != 0)
      {
        targetIndex = rand.Next(nodes.Count());
        stack.Push(nodes[targetIndex]);
        nodes.RemoveAt(targetIndex);
      }
    }

    private List<Point> findNeighbors(Point node)
    {
      List<Point> neighbors = new List<Point>();
      for (int y = (int)node.Y - 1; y < node.Y + 2; y++)
      {
        for (int x = (int)node.X - 1; x < node.X + 2; x++)
        {
          if (pointOnGrid(x, y) && pointNotCorner(node, x, y)
              && pointNotNode(node, x, y))
          {
            neighbors.Add(new Point(x, y));
          }
        }
      }
      return neighbors;
    }

    private int[] getTestMap()
    {
      return new int[20 * 20] { 
     // 0001020304050607080910111213141516171819
        1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,   // 00
    	  1,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,1,   // 01
    	  1,9,9,1,1,9,9,9,1,9,1,9,1,9,1,9,9,9,1,1,   // 02
    	  1,9,9,1,1,9,9,9,1,9,1,9,1,9,1,9,9,9,1,1,   // 03
    	  1,9,1,1,1,1,9,9,1,9,1,9,1,1,1,1,9,9,1,1,   // 04
    	  1,9,1,1,9,1,1,1,1,9,1,1,1,1,9,1,1,1,1,1,   // 05
    	  1,9,9,9,9,1,1,1,1,1,1,9,9,9,9,1,1,1,1,1,   // 06
    	  1,9,9,9,9,9,9,9,9,1,1,1,9,9,9,9,9,9,9,1,   // 07
    	  1,9,1,1,1,1,1,1,1,1,1,9,1,1,1,1,1,1,1,1,   // 08
    	  1,9,1,9,9,9,9,9,9,9,1,1,9,9,9,9,9,9,9,1,   // 09
    	  1,9,1,1,1,1,9,1,1,9,1,1,1,1,1,1,1,1,1,1,   // 10
    	  1,9,9,9,9,9,1,9,1,9,1,9,9,9,9,9,1,1,1,1,   // 11
    	  1,9,1,9,1,9,9,9,1,9,1,9,1,9,1,9,9,9,1,1,   // 12
    	  1,9,1,9,1,9,9,9,1,9,1,9,1,9,1,9,9,9,1,1,   // 13
    	  1,9,1,1,1,1,9,9,1,9,1,9,1,1,1,1,9,9,1,1,   // 14
    	  1,9,1,1,9,1,1,1,1,9,1,1,1,1,9,1,1,1,1,1,   // 15
    	  1,9,9,9,9,1,1,1,1,1,1,9,9,9,9,1,1,1,1,1,   // 16
    	  1,1,9,9,9,9,9,9,9,1,1,1,9,9,9,1,9,9,9,9,   // 17
    	  1,9,1,1,1,1,1,1,1,1,1,9,1,1,1,1,1,1,1,1,   // 18
    	  1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,   // 19
      };
    }
    
    private Boolean pointOnGrid(int x, int y)
    {
      return x >= 0 && y >= 0 && x < dimension.X && y < dimension.Y;
    }


    private Boolean pointNotCorner(Point node, int x, int y)
    {
      return (x == node.X || y == node.Y);
    }

    private Boolean pointNotNode(Point node, int x, int y)
    {
      return !(x == node.X && y == node.Y);
    }

  }
  
}
