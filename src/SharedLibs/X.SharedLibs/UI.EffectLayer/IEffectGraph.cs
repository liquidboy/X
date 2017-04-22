using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.UI.EffectLayer
{
    interface IEffectGraph : IDisposable
    {
        ICanvasImage Output { get; }
        void Setup(ICanvasImage source, params dynamic[] args);
    }
}
