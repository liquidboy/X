using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace X.ModernDesktop.SimTower.Models.Item
{
  class Item : IDrawable
  {
    internal int _EntranceOffset;
    internal int _ExitOffset;


    private bool _IsDirty = false;
    private Slot _position;
    private UIElement _control;
    private bool _IsInVisualTree = false;

    public bool IsDirty { get => _IsDirty; set => _IsDirty = value; }
    public Slot Position { get => _position; set => SetDataAndMarkDirty(ref _position, value); }
    public UIElement Control { get => _control; set => SetDataAndMarkDirty(ref _control, value); }
    public bool IsInVisualTree { get => _IsInVisualTree; set => SetDataAndMarkDirty(ref _IsInVisualTree, value); }


    public static IPrototype makePrototype<T>()
    {
      //var currentMethod = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;
      IPrototype p = (IPrototype)Activator.CreateInstance(typeof(T));
      p.EntranceOffset = 0;
      p.ExitOffset = 0;
      return p;
    }

    public void Init()
    {

    }

    public void SetDataAndMarkDirty<T>(ref T field, T value)
    {
      field = value;
      IsDirty = true;
    }
  }
}
