using EvantHandling.Objects;
using System.Security.Cryptography.Xml;

namespace EvantHandling
{
    public partial class Form1 : Form
    {
        List<BaseObject> objects = new();
        Player player;
        Marker marker;
        public Form1()
        {
            InitializeComponent();
            timer1.Enabled = true;
            timer1.Interval = 20;
            timer1.Tick -= timer1_Tick;
            timer1.Tick += timer1_Tick;
            pbMain.MouseClick -= pbMain_MouseClick;
            pbMain.MouseClick += pbMain_MouseClick;
            player = new Player(pbMain.Width / 2, pbMain.Height / 2, 0);
            player.OnMarkerOverlap += (m) =>
            {
                objects.Remove(m);
                marker = null;
            };
            marker = new Marker(pbMain.Width / 2 + 50, pbMain.Height / 2 + 50, 0);

            objects.Add(marker);
            objects.Add(player);
            objects.Add(new MyRectangle(50, 50, 0));
            objects.Add(new MyRectangle(100, 100, 45));

        }

        private void pbMain_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.White);

            foreach (var obj in objects.ToList())
            {
                if (obj != player && player.Overlaps(obj, g))
                {
                    player.Overlap(obj);
                    obj.Overlap(player);
                }
            }

            foreach (var obj in objects)
            {
                g.Transform = obj.GetTransform();
                obj.Render(g);
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (marker != null)
            {
                float dx = marker.X - player.X;
                float dy = marker.Y - player.Y;

                float length = MathF.Sqrt(dx * dx + dy * dy);
                if (length > 0)
                {
                    dx /= length;
                    dy /= length;

                    player.X += dx * 2;
                    player.Y += dy * 2;

                    player.Angle = MathF.Atan2(dy, dx) * 180 / MathF.PI;
                }
            }                  

            pbMain.Invalidate();
        }

        private void pbMain_MouseClick(object sender, MouseEventArgs e)
        {
            if (marker == null)
            {
                marker = new Marker(0, 0, 0);
                objects.Add(marker);
            }

            marker.X = e.X;
            marker.Y = e.Y;

            pbMain.Invalidate();
        }
    }
}
