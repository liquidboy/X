using SumoNinjaMonkey.Framework.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using X.ModernDesktop.SimTower.Models.PathFinding;

namespace X.ModernDesktop.SimTower.Models.Item
{
  class Item : BindableBase, IDrawable
  {
    internal int _EntranceOffset;
    internal int _ExitOffset;
    internal Board _board;

    private bool _IsDirty = false;
    private Slot _position;
    private UIElement _control;
    private bool _IsInVisualTree = false;
    private int _ZIndex = 1;

    public bool IsDirty { get => _IsDirty; set => _IsDirty = value; }
    public Slot Position { get => _position; set => SetDataMarkDirtyAndRaisePropertyChanged(ref _position, value); }
    public UIElement Control { get => _control; set => SetDataAndMarkDirty(ref _control, value); }
    public bool IsInVisualTree { get => _IsInVisualTree; set => SetDataAndMarkDirty(ref _IsInVisualTree, value); }
    public int ZIndex { get => _ZIndex; set => SetDataAndMarkDirty(ref _ZIndex, value); }

    
    Route lobbyRoute;

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

    public void SetDataMarkDirtyAndRaisePropertyChanged<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
      SetDataAndMarkDirty(ref field, value);
      OnPropertyChanged(propertyName);
    }
    
    public void UpdateRoutes()
    {
      if (_board!=null &&  !(this is IHaulsPeople) && Position.Y != 0)
      {
        lobbyRoute = _board.FindRoute(_board.mainLobby, this);
      } else {
        lobbyRoute?.Clear();
      }
    }
  }
}
