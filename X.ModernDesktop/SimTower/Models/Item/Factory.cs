﻿using System;
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
    public List<IPrototype> prototypes;
    public Dictionary<string, IPrototype> prototypesById;

    public Factory() {
      prototypes = new List<IPrototype>();
      prototypesById = new Dictionary<string, IPrototype>();
      LoadPrototypes();
    }

    private void LoadPrototypes() {
      prototypes.Add(Cinema.makePrototype<Cinema>());
      prototypes.Add(Condo.makePrototype<Condo>());
      prototypes.Add(Elevator.makePrototype<Elevator>());
      prototypes.Add(FastFood.makePrototype<FastFood>());
      prototypes.Add(Floor.makePrototype<Floor>());
      prototypes.Add(Lobby.makePrototype<Lobby>());
      prototypes.Add(Metro.makePrototype<Metro>());
      prototypes.Add(Office.makePrototype<Office>());
      prototypes.Add(PartyHall.makePrototype<PartyHall>());
      prototypes.Add(Restaurant.makePrototype<Restaurant>());
      prototypes.Add(Stairs.makePrototype<Stairs>());

      foreach(var prototype in prototypes)
      {
        prototypesById.Add(prototype.Id, prototype);
      }
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
