using SumoNinjaMonkey.Framework.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.ModernDesktop.SimTower.Models.Item;

namespace X.ModernDesktop.SimTower.Models
{
  class SelectedTool : BindableBase
  {
    private IPrototype _toolPrototype;

    public IPrototype ToolPrototype { get => _toolPrototype; set => SetProperty(ref _toolPrototype, value); }


  }
}
