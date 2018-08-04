using SumoNinjaMonkey.Framework.Collections;
using System;
using System.ComponentModel;
using System.Numerics;
using Windows.UI.Input;

namespace X.ModernDesktop.SimTower.Models
{
  class SelectedArea : BindableBase
  {
    private Slot _SlotPositionHover;
    private Slot _SlotHover;

    private Slot _SlotPositionStart;
    private Slot _SlotStart;
    private Slot _SlotPositionMiddle;
    private Slot _SlotMiddle;
    private Slot _SelectionXY;
    private Slot _SelectionWH;
    private Slot _SlotPositionEnd;
    private Slot _SlotEnd;
    private bool _isHoverVisible;
    private bool _isSelectionVisible;

    public bool IsInSelectionMode { get; set; }

    public bool IsHoverCursorVisible { get => _isHoverVisible; set => SetProperty(ref _isHoverVisible, value); }
    public bool IsSelectionVisible { get => _isSelectionVisible; set => SetProperty(ref _isSelectionVisible, value); }

    public Slot SlotPositionHover { get => _SlotPositionHover; set => SetProperty(ref _SlotPositionHover, value); }
    public Slot SlotHover { get => _SlotHover; set => SetProperty(ref _SlotHover, value); }

    public Slot SlotPositionStart { get => _SlotPositionStart; set => SetProperty(ref _SlotPositionStart, value); }
    public Slot SlotStart { get => _SlotStart; set => SetProperty(ref _SlotStart, value); }

    public Slot SlotPositionDelta { get => _SlotPositionMiddle; set => SetProperty(ref _SlotPositionMiddle, value); }
    public Slot SlotDelta { get => _SlotMiddle; set => SetProperty(ref _SlotMiddle, value); }
    public Slot SelectionXY { get => _SelectionXY; set => SetProperty(ref _SelectionXY, value); }
    public Slot SelectionWH { get => _SelectionWH; set => SetProperty(ref _SelectionWH, value); }


    public Slot SlotPositionEnd { get => _SlotPositionEnd; set => SetProperty(ref _SlotPositionEnd, value); }
    public Slot SlotEnd { get => _SlotEnd; set => SetProperty(ref _SlotEnd, value); }


    public void BeginSelection(PointerPoint pt, Vector2 slotDimension)
    {
      IsHoverCursorVisible = false;
      IsInSelectionMode = true;
      SetStartSlotDetails(pt, slotDimension);
    }
    public void EndSelection(PointerPoint pt, Vector2 slotDimension)
    {
      IsHoverCursorVisible = true;
      IsInSelectionMode = false;
      SetEndSlotDetails(pt, slotDimension);
    }

    public void ChangeSelection(PointerPoint pt, Vector2 slotDimension)
    {
      if (!IsInSelectionMode) return;
      SetSelectionSlotDetails(pt, slotDimension);
      SetEndSlotDetails(pt, slotDimension);
    }
    public void ChangeHoverCursor(PointerPoint pt, Vector2 slotDimension)
    {
      SetHoverSlotDetails(pt, slotDimension);
    }

    private void SetHoverSlotDetails(PointerPoint pt, Vector2 slotDimension)
    {
      if (IsInSelectionMode) return;
      SlotPositionHover = new Slot(Utilities.RoundDown((int)pt.Position.X, (int)slotDimension.X), Utilities.RoundDown((int)pt.Position.Y, (int)slotDimension.Y));
      SlotHover = new Slot((int)(SlotPositionHover.X / slotDimension.X), (int)(SlotPositionHover.Y / slotDimension.Y));
    }

    private void SetStartSlotDetails(PointerPoint pt, Vector2 slotDimension)
    {
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
      SlotPositionDelta = new Slot(Utilities.RoundDown((int)pt.Position.X, (int)slotDimension.X), Utilities.RoundDown((int)pt.Position.Y, (int)slotDimension.Y));
      SlotDelta = new Slot((int)(SlotPositionDelta.X / slotDimension.X), (int)(SlotPositionDelta.Y / slotDimension.Y));

      int x1 = 0; int y1 = 0;
      x1 = Math.Min(SlotPositionStart.X, SlotPositionDelta.X);
      y1 = Math.Min(SlotPositionStart.Y, SlotPositionDelta.Y);
      SelectionXY = new Slot(x1, y1);

      int x2 = 0; int y2 = 0;
      x2 = Math.Max(SlotPositionStart.X, SlotPositionDelta.X);
      y2 = Math.Max(SlotPositionStart.Y, SlotPositionDelta.Y);

      int w = 0; int h = 0;
      w = (int)(Math.Abs(x2 - x1) + slotDimension.X);
      h = (int)(Math.Abs(y2 - y1) + slotDimension.Y);
      SelectionWH = new Slot(Utilities.RoundDown(w, (int)slotDimension.X), Utilities.RoundDown(h, (int)slotDimension.Y));
    }

  }
}
