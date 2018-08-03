using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.ModernDesktop.SimTower.Models.Item
{
  interface IDrawable
  {
    bool IsDirty { get; set; }

    void SetDataAndMarkDirty<T>(ref T field, T value);

  }
}
