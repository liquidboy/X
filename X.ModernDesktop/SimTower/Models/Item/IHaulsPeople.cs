using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.ModernDesktop.SimTower.Models.Item
{
  interface IHaulsPeople
  {
    bool ConnectsToFloor(int floor, int overrideSizeY);
    // return Position.Y == floor || (Position.Y + Size.Y - 1) == floor;
    void AddPerson();
    void RemovePerson();
  }
}
