using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace X.ModernDesktop.SimTower.Models.Item
{
  class Elevator : Item, IHaulsPeople, IPrototype
  {
    public Elevator() { }

    public string Id => "elevator";

    public string Name => "Elevator";
    
    public int Price => 20000;

    private Slot _Size = new Slot(1, 3);
    private int _currentFloor = 0;
    private int _noCarts =1;

    public Slot Size { get => _Size; set => _Size = value; }

    public int NoCarts { get => _noCarts; set => SetDataMarkDirtyAndRaisePropertyChanged(ref _noCarts, value); }

    public int CurrentFloor { get => _currentFloor; set => SetDataMarkDirtyAndRaisePropertyChanged(ref _currentFloor, value); }

    public int Icon => (int)IconNumbers.ICON_ELEVATOR;

    public int EntranceOffset { get => _EntranceOffset; set => _EntranceOffset = value; }
    public int ExitOffset { get => _ExitOffset; set => _ExitOffset = value; }

    public int MaxInstancesPerFloor => 20;

    public bool KeepGrowingSameInstanceX => false;
    public bool KeepGrowingSameInstanceY => true;

    public new int ZIndex => 10;

    public IPrototype Make()
    {
      return new Elevator();
    }

    public UIElement MakeUI()
    {
      return new Views.Elevator();
    }

    public void FirstTimeDraw()
    {
      NoCarts = 1;
    }

    public void AddToItem(IPrototype itemToAdd)
    {
      if (itemToAdd is Elevator)
      {
        NoCarts++;
      }
    }

    public void AddPerson()
    {
      throw new NotImplementedException();
    }

    public void RemovePerson()
    {
      throw new NotImplementedException();
    }

    public bool ConnectsToFloor(int floor, int overrideSizeY = int.MinValue)
    {
      //if (!Enumerable.Range(Position.Y - Size.Y , Position.Y).Contains(floor)) return false;
      ////if (floor < Position.Y || floor >= Position.Y + Size.Y) return false;
      ////return !unservicedFloors.count(floor);
      //return true;

      if (floor >= Position.Y || floor <= Position.Y - (overrideSizeY == int.MinValue ? Size.Y : overrideSizeY )) return false;
      return true;
    }
  }
}
