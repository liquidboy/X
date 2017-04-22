using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Viewer.SketchFlow
{
    public enum eActionTypes
    {
        CreateFromStamp,
        CloseStamp,

        ToggleGridMarkers,

        MoveTopLeft,
        ToolbarTopRight,
        RotateBottomLeft,
        ResizeBottomRight,
        ResizeCenterRight,
        CenterLeft
    }
}
