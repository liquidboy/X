using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace SampleAStar
{
  public class Map
  {
    public int MAP_WIDTH = 20;
    public int MAP_HEIGHT = 20;

    private int[] map;

    private static readonly Map instance = new Map();
    public static Map Instance
    {
      get
      {
        return instance;
      }
    }

    public Map()
    {
    }

    public int GetMap(int x, int y)
    {
      if (x < 0 || x >= MAP_WIDTH || y < 0 || y >= MAP_HEIGHT)
      {
        return 9;
      }

      return map[(y * MAP_WIDTH) + x];
    }

    public void SetMap(Point dimension, int[] newMap)
    {
      map = newMap;
      MAP_WIDTH = (int)dimension.X;
      MAP_HEIGHT = (int)dimension.Y;
    }
  }


}
