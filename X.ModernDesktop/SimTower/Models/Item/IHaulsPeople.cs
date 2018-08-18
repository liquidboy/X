using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.ModernDesktop.SimTower.Models.Item
{
  interface IHaulsPeople
  {
    bool ConnectsToFloor(int floor);
    void AddPerson();
    void RemovePerson();
  }
}
