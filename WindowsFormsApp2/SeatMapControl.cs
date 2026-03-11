using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public sealed class SeatMapControl : Control
    {
        public event EventHandler SelectionChanged;

        private readonly HashSet<int> occupied = new HashSet<int>();
        private readonly HashSet<int> selected = new HashSet<int>();
        private int hoveredSeat = -1;

        public int SeatCount { get; private set; } = 60;
        public int MaxSelectable { get; set; } = 1;

        public IReadOnlyCollection<int> SelectedSeats
        {
            get { return selected.ToList().AsReadOnly(); }
        }

        public SeatMapControl()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.UserPaint |
                ControlStyles.ResizeRedraw,
                true);

            BackColor = Color.White;
            Font = new Font("Segoe UI", 9F);
            Cursor = Cursors.Hand;
        }

        public void SetOccupiedSeats(IEnumerable<int> seats)
        {
            occupied.Clear();

            if (seats != null)
            {
                foreach (int s in seats)
                {
                    if (s >= 1 && s <= SeatCount)
                        occupied.Add(s);
                }
            }

            List<int> remove = new List<int>();
            foreach (int s in selected)
            {
                if (occupied.Contains(s))
                    remove.Add(s);
            }

            for (int i = 0; i < remove.Count; i++)
                selected.Remove(remove[i]);

            Invalidate();
            RaiseChanged();
        }

        public void ClearSelection()
        {
            selected.Clear();
            Invalidate();
            RaiseChanged();
        }

        public bool TrySelectSeat(int seatNo)
        {
            if (seatNo < 1 || seatNo > SeatCount) return false;
            if (occupied.Contains(seatNo)) return false;

            if (selected.Contains(seatNo))
            {
                selected.Remove(seatNo);
                Invalidate();
                RaiseChanged();
                return true;
            }

            if (selected.Count >= MaxSelectable) return false;

            selected.Add(seatNo);
            Invalidate();
            RaiseChanged();
            return true;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            int? seat = HitTest(e.Location);
            if (!seat.HasValue) return;

            if (!TrySelectSeat(seat.Value))
                System.Media.SystemSounds.Beep.Play();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            int newHover = HitTest(e.Location) ?? -1;
            if (newHover != hoveredSeat)
            {
                hoveredSeat = newHover;
                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            hoveredSeat = -1;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(BackColor);

            Rectangle outer = ClientRectangle;
            outer.Inflate(-8, -8);

            using (Brush b = new SolidBrush(Color.FromArgb(248, 248, 248)))
                g.FillRoundedRectangle(b, outer, 16);

            using (Pen p = new Pen(Color.FromArgb(225, 225, 225)))
                g.DrawRoundedRectangle(p, outer, 16);

            Rectangle inner = new Rectangle(outer.Left + 14, outer.Top + 14, outer.Width - 28, outer.Height - 28);

            using (Brush b = new SolidBrush(Color.White))
                g.FillRoundedRectangle(b, inner, 10);

            using (Pen p = new Pen(Color.FromArgb(222, 222, 222)))
                g.DrawRoundedRectangle(p, inner, 10);

            Rectangle driver = new Rectangle(inner.Left + 12, inner.Top + 10, inner.Width - 24, 30);

            using (Brush b = new SolidBrush(Color.FromArgb(246, 246, 246)))
                g.FillRoundedRectangle(b, driver, 6);

            using (Pen p = new Pen(Color.FromArgb(226, 226, 226)))
                g.DrawRoundedRectangle(p, driver, 6);

            using (Brush b = new SolidBrush(Color.FromArgb(120, 120, 120)))
            using (Font f = new Font("Segoe UI", 8.5F, FontStyle.Italic))
            {
                g.DrawString("Шофьор", f, b, driver.Left + 10, driver.Top + 6);
            }

            Rectangle seatArea = new Rectangle(inner.Left + 14, driver.Bottom + 10, inner.Width - 28, inner.Height - 58);
            DrawSeats(g, seatArea);
        }

        private void DrawSeats(Graphics g, Rectangle area)
        {
            int rows = 15;
            int seatGapX = 8;
            int seatGapY = 6;
            int aisle = 28;

            int availableWidth = area.Width - aisle - seatGapX * 3;
            int seatWidth = availableWidth / 4;
            int seatHeight = (area.Height - seatGapY * (rows - 1)) / rows;

            if (seatWidth < 30) seatWidth = 30;
            if (seatHeight < 14) seatHeight = 14;

            int totalWidth = seatWidth * 4 + seatGapX * 3 + aisle;
            int startX = area.Left + (area.Width - totalWidth) / 2;
            int startY = area.Top + 4;

            for (int i = 0; i < SeatCount; i++)
            {
                int seatNo = i + 1;
                int row = i / 4;
                int col = i % 4;

                int x = startX;
                if (col == 0) x = startX;
                else if (col == 1) x = startX + seatWidth + seatGapX;
                else if (col == 2) x = startX + seatWidth * 2 + seatGapX * 2 + aisle;
                else x = startX + seatWidth * 3 + seatGapX * 3 + aisle;

                int y = startY + row * (seatHeight + seatGapY);

                Rectangle seatRect = new Rectangle(x, y, seatWidth, seatHeight);
                DrawSeat(g, seatRect, seatNo);
            }
        }

        private void DrawSeat(Graphics g, Rectangle rect, int seatNo)
        {
            bool isOccupied = occupied.Contains(seatNo);
            bool isSelected = selected.Contains(seatNo);
            bool isHovered = hoveredSeat == seatNo && !isOccupied;

            Color fill = Color.FromArgb(245, 245, 245);
            Color border = Color.FromArgb(196, 196, 196);
            Color textColor = Color.FromArgb(85, 85, 85);

            if (isOccupied)
            {
                fill = Color.FromArgb(212, 47, 53);
                border = Color.FromArgb(182, 32, 38);
                textColor = Color.White;
            }
            else if (isSelected)
            {
                fill = Color.FromArgb(243, 190, 49);
                border = Color.FromArgb(211, 161, 33);
                textColor = Color.FromArgb(90, 65, 0);
            }
            else if (isHovered)
            {
                fill = Color.FromArgb(236, 244, 255);
                border = Color.FromArgb(66, 133, 244);
                textColor = Color.FromArgb(30, 60, 120);
            }

            Rectangle shadow = new Rectangle(rect.Left, rect.Top + 1, rect.Width, rect.Height);
            using (Brush sb = new SolidBrush(Color.FromArgb(18, 0, 0, 0)))
                g.FillRoundedRectangle(sb, shadow, 6);

            using (Brush b = new SolidBrush(fill))
                g.FillRoundedRectangle(b, rect, 6);

            using (Pen p = new Pen(border, isHovered ? 2f : 1f))
                g.DrawRoundedRectangle(p, rect, 6);

            Rectangle topLine = new Rectangle(rect.Left + 3, rect.Top + 2, rect.Width - 6, 3);
            using (Brush b = new SolidBrush(Color.FromArgb(40, Color.White)))
                g.FillRoundedRectangle(b, topLine, 2);

            using (Brush tb = new SolidBrush(textColor))
            using (Font f = new Font("Segoe UI", 8F, FontStyle.Bold))
            {
                string s = seatNo.ToString();
                SizeF sz = g.MeasureString(s, f);
                float tx = rect.Left + (rect.Width - sz.Width) / 2f;
                float ty = rect.Top + (rect.Height - sz.Height) / 2f - 0.5f;
                g.DrawString(s, f, tb, tx, ty);
            }
        }

        private int? HitTest(Point p)
        {
            Rectangle outer = ClientRectangle;
            outer.Inflate(-8, -8);

            Rectangle inner = new Rectangle(outer.Left + 14, outer.Top + 14, outer.Width - 28, outer.Height - 28);
            Rectangle driver = new Rectangle(inner.Left + 12, inner.Top + 10, inner.Width - 24, 30);
            Rectangle area = new Rectangle(inner.Left + 14, driver.Bottom + 10, inner.Width - 28, inner.Height - 58);

            int rows = 15;
            int seatGapX = 8;
            int seatGapY = 6;
            int aisle = 28;

            int availableWidth = area.Width - aisle - seatGapX * 3;
            int seatWidth = availableWidth / 4;
            int seatHeight = (area.Height - seatGapY * (rows - 1)) / rows;

            if (seatWidth < 30) seatWidth = 30;
            if (seatHeight < 14) seatHeight = 14;

            int totalWidth = seatWidth * 4 + seatGapX * 3 + aisle;
            int startX = area.Left + (area.Width - totalWidth) / 2;
            int startY = area.Top + 4;

            for (int i = 0; i < SeatCount; i++)
            {
                int seatNo = i + 1;
                int row = i / 4;
                int col = i % 4;

                int x = startX;
                if (col == 0) x = startX;
                else if (col == 1) x = startX + seatWidth + seatGapX;
                else if (col == 2) x = startX + seatWidth * 2 + seatGapX * 2 + aisle;
                else x = startX + seatWidth * 3 + seatGapX * 3 + aisle;

                int y = startY + row * (seatHeight + seatGapY);
                Rectangle seatRect = new Rectangle(x, y, seatWidth, seatHeight);

                if (seatRect.Contains(p))
                    return seatNo;
            }

            return null;
        }

        private void RaiseChanged()
        {
            if (SelectionChanged != null)
                SelectionChanged(this, EventArgs.Empty);
        }
    }

    internal static class SeatMapGraphicsExtensions
    {
        public static void FillRoundedRectangle(this Graphics g, Brush brush, Rectangle bounds, int radius)
        {
            using (GraphicsPath path = RoundedRect(bounds, radius))
                g.FillPath(brush, path);
        }

        public static void DrawRoundedRectangle(this Graphics g, Pen pen, Rectangle bounds, int radius)
        {
            using (GraphicsPath path = RoundedRect(bounds, radius))
                g.DrawPath(pen, path);
        }

        private static GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int d = radius * 2;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(bounds.Left, bounds.Top, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Top, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.Left, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}