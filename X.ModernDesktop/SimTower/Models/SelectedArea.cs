using SumoNinjaMonkey.Framework.Collections;
using System;
using System.ComponentModel;
using System.Numerics;
using Windows.UI.Input;

namespace X.ModernDesktop.SimTower.Models
{
  class SelectedArea : BindableBase
  {
    private Slot _SlotPositionStart;
    private Slot _SlotStart;
    private Slot _SlotPositionMiddle;
    private Slot _SlotMiddle;
    private Slot _SelectionXY;
    private Slot _SelectionWH;
    private Slot _SlotPositionEnd;
    private Slot _SlotEnd;
    private bool _isVisible;

    public bool IsVisible { get => _isVisible; set => SetProperty(ref _isVisible, value); }

    public bool IsInSelectionMode { get; set; }


    public Slot SlotPositionStart { get => _SlotPositionStart; set => SetProperty(ref _SlotPositionStart, value); }
    public Slot SlotStart { get => _SlotStart; set => SetProperty(ref _SlotStart, value); }


    public Slot SlotPositionMiddle { get => _SlotPositionMiddle; set => SetProperty(ref _SlotPositionMiddle, value); }
    public Slot SlotMiddle { get => _SlotMiddle; set => SetProperty(ref _SlotMiddle, value); }
    public Slot SelectionXY { get => _SelectionXY; set => SetProperty(ref _SelectionXY, value); }
    public Slot SelectionWH { get => _SelectionWH; set => SetProperty(ref _SelectionWH, value); }


    public Slot SlotPositionEnd { get => _SlotPositionEnd; set => SetProperty(ref _SlotPositionEnd, value); }
    public Slot SlotEnd { get => _SlotEnd; set => SetProperty(ref _SlotEnd, value); }


    public void BeginSelection(PointerPoint pt, Vector2 slotDimension) {
      IsInSelectionMode = true;
      SetStartSlotDetails(pt, slotDimension);
    }
    public void EndSelection(PointerPoint pt, Vector2 slotDimension) {
      IsInSelectionMode = false;
      SetEndSlotDetails(pt, slotDimension);
    }

    public void ChangeSelection(PointerPoint pt, Vector2 slotDimension) {
      if (!IsInSelectionMode) return;
      SetSelectionSlotDetails(pt, slotDimension);
      SetEndSlotDetails(pt, slotDimension);
    }


    private void SetStartSlotDetails(PointerPoint pt, Vector2 slotDimension)
    {
      IsInSelectionMode = true;
      SlotPositionStart = new Slot(Utilities.RoundDown((int)pt.Position.X, (int)slotDimension.X), Utilities.RoundDown((int)pt.Position.Y, (int)slotDimension.Y));
      SlotStart = new Slot((int)(SlotPositionStart.X / slotDimension.X), (int)(SlotPositionStart.Y / slotDimension.Y));
    }

    private void SetEndSlotDetails(PointerPoint pt, Vector2 slotDimension)
    {
      SlotPositionEnd = new Slot(Utilities.RoundDown((int)pt.Position.X, (int)slotDimension.X), Utilities.RoundDown((int)pt.Position.Y, (int)slotDimension.Y));
      SlotEnd = new Slot((int)(SlotPositionEnd.X / slotDimension.X), (int)(SlotPositionEnd.Y / slotDimension.Y));
    }

    private void SetSelectionSlotDetails(PointerPoint pt, Vector2 slotDimension)
    {
      SlotPositionMiddle = new Slot(Utilities.RoundDown((int)pt.Position.X, (int)slotDimension.X), Utilities.RoundDown((int)pt.Position.Y, (int)slotDimension.Y));
      SlotMiddle = new Slot((int)(SlotPositionMiddle.X / slotDimension.X), (int)(SlotPositionMiddle.Y / slotDimension.Y));

      int x1 = 0; int y1 = 0;
      x1 = Math.Min(SlotPositionStart.X, SlotPositionMiddle.X);
      y1 = Math.Min(SlotPositionStart.Y, SlotPositionMiddle.Y);
      SelectionXY = new Slot(x1, y1);

      int x2 = 0; int y2 = 0;
      x2 = Math.Max(SlotPositionStart.X, SlotPositionMiddle.X);
      y2 = Math.Max(SlotPositionStart.Y, SlotPositionMiddle.Y);

      int w = 0; int h = 0;
      w = (int)(Math.Abs(x2 - x1) + slotDimension.X);
      h = (int)(Math.Abs(y2 - y1) + slotDimension.Y);
      SelectionWH = new Slot(Utilities.RoundDown(w, (int)slotDimension.X), Utilities.RoundDown(h, (int)slotDimension.Y));
    }

  }
}
