using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public sealed class GradientPanel : Panel
    {
        public Color TopColor { get; set; }
        public Color BottomColor { get; set; }

        public GradientPanel()
        {
            DoubleBuffered = true;
            TopColor = Color.FromArgb(75, 55, 60);
            BottomColor = Color.FromArgb(35, 30, 33);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            using (var br = new System.Drawing.Drawing2D.LinearGradientBrush(
                ClientRectangle, TopColor, BottomColor, 90f))
            {
                e.Graphics.FillRectangle(br, ClientRectangle);
            }
        }
    }
}