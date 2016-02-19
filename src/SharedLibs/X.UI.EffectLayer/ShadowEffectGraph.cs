using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace X.UI.EffectLayer
{
    class ShadowEffectGraph : IEffectGraph
    {
        public ICanvasImage Output
        {
            get
            {
                return shadow;
            }
        }

        ShadowEffect shadow = new ShadowEffect()
        {
            ShadowColor = Colors.Black,
            BlurAmount = 3f
        };
        
        public ShadowEffectGraph()
        {

        }

        private void _setup(ICanvasImage source, float amount, Color shadowColor)
        {
            shadow.Source = source;
            shadow.BlurAmount = amount;
            shadow.ShadowColor = shadowColor;
        }

        public void Setup(ICanvasImage source, params dynamic[] args)
        {
            _setup(source, args[0], args[1]);
        }
    }
}
