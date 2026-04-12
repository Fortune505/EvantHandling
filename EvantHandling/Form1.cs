using EvantHandling.Objects;
using System.Security.Cryptography.Xml;

namespace EvantHandling
{
    public partial class Form1 : Form
    {
        List<BaseObject> objects = new();
        Player player;
        Marker marker;
        Random rnd = new Random();
        int score = 0;
        public Form1()
        {
            InitializeComponent();
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

            for (int i = 0; i < 2; i++)
            {
                var greenCircle = new MyRectangle(rnd.Next(30, pbMain.Width - 30), rnd.Next(30, pbMain.Height - 30), 0);
                greenCircle.OnOverlap += (sender, otherObj) =>
                {
                    if (otherObj is Player)
                    {
                        score++;
                        lblScore.Text = $"Очки: {score}";

                        sender.X = rnd.Next(30, pbMain.Width - 30);
                        sender.Y = rnd.Next(30, pbMain.Height - 30);
                    }
                };

                objects.Add(greenCircle);
            }
                  
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
                    txtLog.Text = $"[{DateTime.Now:HH:mm:ss:ff}] Игрок пересекся с {obj}\n" + txtLog.Text;
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
