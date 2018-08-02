using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.ModernDesktop.SimTower.Models.Item
{
  enum IconNumbers
  {
    ICON_LOBBY = 0,
    ICON_FLOOR = 1,
    ICON_STAIRS = 2,
    ICON_ELEVATOR = 3,
    ICON_OFFICE = 7,
    ICON_FASTFOOD = 11,
    ICON_RESTAURANT = 12,
    ICON_CINEMA = 14,
    ICON_PARTYHALL = 15,
    ICON_METRO = 19,
    ICON_CONDO = 24,
  };

  class Factory
  {
    List<IPrototype> prototypes;
    Dictionary<string, IPrototype> prototypesById;

    Factory() {
      prototypes = new List<IPrototype>();
      prototypesById = new Dictionary<string, IPrototype>();

      prototypes.Add(Cinema.makePrototype());
      prototypes.Add(Condo.makePrototype());
      prototypes.Add(Elevator.makePrototype());
      prototypes.Add(FastFood.makePrototype());
      prototypes.Add(Floor.makePrototype());
      prototypes.Add(Lobby.makePrototype());
      prototypes.Add(Metro.makePrototype());
      prototypes.Add(Office.makePrototype());
      prototypes.Add(PartyHall.makePrototype());
      prototypes.Add(Restaurant.makePrototype());
      prototypes.Add(Stairs.makePrototype());
    }

    public Item Make(IPrototype prototype, Slot position) {
      Item item = prototype.Make();
      item.Position = position;
      item.Init();
      return item;
    }

    public Item Make(string prototypeId, Slot position) {
      IPrototype prototype = prototypesById[prototypeId];
      return Make(prototype, position);
    }
  }
}
