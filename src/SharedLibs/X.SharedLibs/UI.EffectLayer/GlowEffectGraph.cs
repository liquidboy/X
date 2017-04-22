using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.UI.EffectLayer
{
    class GlowEffectGraph : IEffectGraph
    {
        ICanvasImage _cl;

        public ICanvasImage Output
        {
            get
            {
                return blur;
            }
        }

        MorphologyEffect morphology = new MorphologyEffect()
        {
            Mode = MorphologyEffectMode.Dilate,
            Width = 1,
            Height = 1
        };

        GaussianBlurEffect blur = new GaussianBlurEffect()
        {
            BlurAmount = 0,
            BorderMode = EffectBorderMode.Soft
        };

        public GlowEffectGraph()
        {
            blur.Source = morphology;
        }

        private void _setup(ICanvasImage source, float amount)
        {
            if (amount == 0) return;

            if (_cl != null) { _cl.Dispose(); _cl = null; }
            _cl = source;

            morphology.Source = _cl;
            
            var halfAmount = Math.Min(amount / 2, 100);
            morphology.Width = (int)Math.Ceiling(halfAmount);
            morphology.Height = (int)Math.Ceiling(halfAmount);
            blur.BlurAmount = halfAmount;
        }

        public void Setup(ICanvasImage source, params dynamic[] args)
        {
            _setup(source, args[0]);
        }

        public void Dispose()
        {
            _cl?.Dispose();
            _cl = null;
            morphology?.Dispose();
            morphology = null;
            blur?.Dispose();
            blur = null;
        }
    }
}
