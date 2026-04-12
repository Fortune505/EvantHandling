using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvantHandling.Objects
{
    class MyRectangle : BaseObject
    {
        public float Size = 30f;
        public float MaxSize = 30f;

        public Action<MyRectangle> OnSizeZero;
        public MyRectangle(float x, float y, float angle) : base(x, y, angle) { 
        
        }

        public void DecreaseSize()
        {
            Size -= 0.3f;

            if (Size <= 0)
            {
                Size = 0;
                OnSizeZero?.Invoke(this);
            }
        }

        public void ResetSize()
        {
            Size = MaxSize;
        }

        public override void Render(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.Green), -Size / 2, -Size / 2, Size, Size);
        }

        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-Size / 2, -Size / 2, Size, Size);
            return path;
        }
    }
}
