using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        private readonly Color BgTop = Color.FromArgb(75, 55, 60);
        private readonly Color BgBottom = Color.FromArgb(35, 30, 33);

        private int step = 1;
        private SeatMapControl seatMap;

        private readonly List<string> Cities = new List<string>
        {
            "Варна", "София", "Бургас", "Пловдив", "Русе", "В. Търново"
        };

        private readonly Dictionary<string, decimal> RoutePrice = new Dictionary<string, decimal>();
        private readonly List<Trip> CurrentTrips = new List<Trip>();
        private Trip SelectedTrip;

        private readonly Dictionary<string, HashSet<int>> Occupied = new Dictionary<string, HashSet<int>>();
        private readonly Random rnd = new Random(7);

        private sealed class Trip
        {
            public string From;
            public string To;
            public DateTime Date;
            public string Depart;
            public string Arrive;
            public string Vehicle;
            public string Company;
        }

        public Form1()
        {
            InitializeComponent();

            BuildPrices();
            LoadStep1Data();
            BuildLegend();
            WireUi();

            hero.Paint += Hero_Paint;
            card.Paint += Card_Paint;

            seatMap = new SeatMapControl();
            seatMap.Dock = DockStyle.Fill;
            seatMap.MaxSelectable = 1;
            seatMap.SelectionChanged += delegate { UpdateSeatCounters(); };
            pnlSeatHost.Controls.Add(seatMap);

            CenterCard();
            Resize += delegate
            {
                CenterCard();
                LayoutResponsive();
            };

            GoStep(1);
            RefreshPrice();
            LayoutResponsive();
        }

        private void BuildPrices()
        {
            AddRoute("Варна", "София", 35m);
            AddRoute("Варна", "Бургас", 18m);
            AddRoute("Варна", "Пловдив", 32m);
            AddRoute("Варна", "Русе", 22m);
            AddRoute("Варна", "В. Търново", 24m);
            AddRoute("Пловдив", "София", 16m);
            AddRoute("Бургас", "София", 28m);
            AddRoute("Русе", "София", 24m);
            AddRoute("В. Търново", "София", 18m);
            AddRoute("Бургас", "Пловдив", 20m);
            AddRoute("Пловдив", "Русе", 27m);
        }

        private void AddRoute(string from, string to, decimal price)
        {
            RoutePrice[from + "|" + to] = price;
            RoutePrice[to + "|" + from] = price;
        }

        private void LoadStep1Data()
        {
            cmbFrom.Items.Clear();
            cmbTo.Items.Clear();

            for (int i = 0; i < Cities.Count; i++)
            {
                cmbFrom.Items.Add(Cities[i]);
                cmbTo.Items.Add(Cities[i]);
            }

            cmbFrom.SelectedItem = "Варна";
            cmbTo.SelectedItem = "София";

            dtDate.MinDate = DateTime.Today;
            dtDate.Value = DateTime.Today;
        }

        private void WireUi()
        {
            cmbFrom.SelectedIndexChanged += delegate { RefreshPrice(); };
            cmbTo.SelectedIndexChanged += delegate { RefreshPrice(); };
            dtDate.ValueChanged += delegate { RefreshPrice(); };

            chkStudent.CheckedChanged += delegate
            {
                if (chkStudent.Checked) chkPensioner.Checked = false;
                RefreshPrice();
            };

            chkPensioner.CheckedChanged += delegate
            {
                if (chkPensioner.Checked) chkStudent.Checked = false;
                RefreshPrice();
            };

            btnNext1.Click += delegate
            {
                if (!ValidateStep1()) return;
                LoadTrips();
                GoStep(2);
            };

            btnBack2.Click += delegate { GoStep(1); };

            btnNext2.Click += delegate
            {
                if (SelectedTrip == null)
                {
                    MessageBox.Show("Изберете курс.", "Валидация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                LoadSeatsForTrip();
                GoStep(3);
            };

            btnBack3.Click += delegate { GoStep(2); };

            btnNext3.Click += delegate
            {
                if (seatMap.SelectedSeats.Count == 0)
                {
                    MessageBox.Show("Изберете място от схемата.", "Валидация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                RefreshSummary();
                GoStep(4);
            };

            btnBack4.Click += delegate { GoStep(3); };
            btnBuy.Click += delegate { DoBuy(); };
        }

        private void CenterCard()
        {
            int availableWidth = hero.ClientSize.Width - 48;
            if (availableWidth < 860) availableWidth = 860;
            if (availableWidth > 980) availableWidth = 980;

            card.Width = availableWidth;

            int left = (hero.ClientSize.Width - card.Width) / 2;
            if (left < 16) left = 16;

            card.Left = left;
            card.Top = 90;
            card.Invalidate();
        }

        private void LayoutResponsive()
        {
            int contentWidth = card.Width - 48;

            progressTrack.Width = card.Width - 50;

            int comboGap = 40;
            int comboWidth = (contentWidth - comboGap) / 2;
            if (comboWidth < 240) comboWidth = 240;

            cmbFrom.Left = 0;
            cmbFrom.Width = comboWidth;
            cmbFrom.DropDownWidth = comboWidth;

            lblTo.Left = contentWidth - comboWidth;
            cmbTo.Left = contentWidth - comboWidth;
            cmbTo.Width = comboWidth;
            cmbTo.DropDownWidth = comboWidth;

            dtDate.Width = comboWidth;

            // STEP 2
            tripsList.Width = contentWidth;
            tripsList.Height = 248;

            btnBack2.Left = 0;
            btnBack2.Top = 314;

            btnNext2.Left = 160;
            btnNext2.Top = 314;

            // STEP 3 - оправен layout без застъпване
            pnlSeatHost.Width = 360;
            pnlSeatHost.Height = 270;
            pnlSeatHost.Left = 140;
            pnlSeatHost.Top = 20;

            lblChosen.Left = 560;
            lblChosen.Top = 110;

            lblRemaining.Left = 560;
            lblRemaining.Top = 138;

            legend.Left = 535;
            legend.Top = 176;
            legend.Width = 190;
            legend.Height = 110;

            btnBack3.Left = 0;
            btnBack3.Top = 322;

            btnNext3.Left = 160;
            btnNext3.Top = 322;

            // STEP 4
            lblSummary.Width = contentWidth - 20;

            btnBack4.Left = 0;
            btnBack4.Top = 314;

            btnBuy.Left = 160;
            btnBuy.Top = 314;
        }

        private void GoStep(int newStep)
        {
            step = newStep;
            step1.Visible = step == 1;
            step2.Visible = step == 2;
            step3.Visible = step == 3;
            step4.Visible = step == 4;
            UpdateProgress();
            RefreshPrice();
            LayoutResponsive();
        }

        private void UpdateProgress()
        {
            int w = progressTrack.Width;
            int fill;

            if (step == 1) fill = (int)(w * 0.25);
            else if (step == 2) fill = (int)(w * 0.50);
            else if (step == 3) fill = (int)(w * 0.75);
            else fill = w;

            progressFill.Width = fill;
        }

        private bool ValidateStep1()
        {
            if (cmbFrom.SelectedItem == null || cmbTo.SelectedItem == null)
            {
                MessageBox.Show("Моля изберете градове.", "Валидация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if ((string)cmbFrom.SelectedItem == (string)cmbTo.SelectedItem)
            {
                MessageBox.Show("Градът 'От' и 'До' не може да са еднакви.", "Валидация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            return true;
        }

        private decimal CalculatePrice()
        {
            string from = cmbFrom.SelectedItem as string;
            string to = cmbTo.SelectedItem as string;
            if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to)) return 0m;

            decimal basePrice = 0m;
            RoutePrice.TryGetValue(from + "|" + to, out basePrice);

            decimal price = basePrice;

            if (chkStudent.Checked) price *= 0.70m;
            else if (chkPensioner.Checked) price *= 0.50m;

            if (price < 0m) price = 0m;
            return Math.Round(price, 2);
        }

        private void RefreshPrice()
        {
            lblPrice.Text = "Цена: " + CalculatePrice().ToString("0.00") + " лв.";
        }

        private void LoadTrips()
        {
            tripsList.SuspendLayout();
            tripsList.Controls.Clear();
            CurrentTrips.Clear();
            SelectedTrip = null;

            string from = cmbFrom.SelectedItem as string;
            string to = cmbTo.SelectedItem as string;
            DateTime date = dtDate.Value.Date;

            lblStep2Header.Text = from + " → " + to + "   " + date.ToString("yyyy-MM-dd");

            string[] vehicles = { "SCANIA Irizar", "Setra", "Neoplan", "Mercedes Tourismo" };
            string[] departTimes = { "01:45", "07:15", "10:45", "11:45", "14:15", "16:15", "19:15", "20:15" };

            DateTime now = DateTime.Now;

            for (int i = 0; i < departTimes.Length; i++)
            {
                DateTime departDateTime = date.Add(TimeSpan.Parse(departTimes[i] + ":00"));

                if (date == DateTime.Today && departDateTime <= now)
                    continue;

                Trip t = new Trip();
                t.From = from;
                t.To = to;
                t.Date = date;
                t.Depart = departTimes[i];
                t.Arrive = AddDuration(departTimes[i], "03:00");
                t.Vehicle = vehicles[i % vehicles.Length];
                t.Company = "АвтоПревоз " + ((i % 3) + 1);

                if (date.DayOfWeek == DayOfWeek.Sunday && t.Depart == "20:15")
                    continue;

                CurrentTrips.Add(t);
            }

            if (CurrentTrips.Count == 0)
            {
                tripsList.ResumeLayout();
                MessageBox.Show("Няма възможен курс за избраната дата и час.", "Няма курс", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                GoStep(1);
                return;
            }

            for (int i = 0; i < CurrentTrips.Count; i++)
                tripsList.Controls.Add(MakeTripRow(CurrentTrips[i]));

            tripsList.ResumeLayout();
        }

        private Control MakeTripRow(Trip t)
        {
            Panel row = new Panel();
            row.Width = tripsList.ClientSize.Width - 24;
            if (row.Width < 640) row.Width = 640;
            row.Height = 64;
            row.BackColor = Color.White;
            row.Margin = new Padding(0, 0, 0, 0);

            Panel line = new Panel();
            line.Dock = DockStyle.Bottom;
            line.Height = 1;
            line.BackColor = Color.FromArgb(240, 240, 240);
            row.Controls.Add(line);

            RadioButton rb = new RadioButton();
            rb.Location = new Point(row.Width - 24, 23);
            rb.Width = 20;
            rb.CheckedChanged += delegate
            {
                if (rb.Checked) SelectedTrip = t;
            };
            row.Controls.Add(rb);

            Label route = new Label();
            route.Text = t.From + " - " + t.To;
            route.AutoSize = true;
            route.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold);
            route.Location = new Point(12, 10);
            row.Controls.Add(route);

            Label vehicle = new Label();
            vehicle.Text = "🚌 " + t.Vehicle + "  •  " + t.Company;
            vehicle.AutoSize = true;
            vehicle.ForeColor = Color.FromArgb(120, 120, 120);
            vehicle.Location = new Point(12, 36);
            row.Controls.Add(vehicle);

            Label times = new Label();
            times.Text = t.Depart + " - " + t.Arrive;
            times.AutoSize = true;
            times.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            times.Location = new Point(260, 22);
            row.Controls.Add(times);

            Label free = new Label();
            free.Text = "☑ има свободни места";
            free.AutoSize = true;
            free.ForeColor = Color.FromArgb(40, 140, 70);
            free.Location = new Point(row.Width - 210, 22);
            row.Controls.Add(free);

            return row;
        }

        private void LoadSeatsForTrip()
        {
            if (SelectedTrip == null) return;

            seatMap.ClearSelection();
            seatMap.MaxSelectable = 1;

            HashSet<int> occ = GetOccupiedForTrip(TripKey(SelectedTrip));
            seatMap.SetOccupiedSeats(occ);
            UpdateSeatCounters();
        }

        private void UpdateSeatCounters()
        {
            int chosen = seatMap.SelectedSeats.Count;
            int left = seatMap.MaxSelectable - chosen;
            lblChosen.Text = "Избрани места: " + chosen;
            lblRemaining.Text = "Остават: " + left;
        }

        private void BuildLegend()
        {
            legend.Controls.Clear();

            Label title = new Label();
            title.Text = "Информация";
            title.AutoSize = true;
            title.ForeColor = Color.FromArgb(120, 120, 120);
            title.Location = new Point(12, 10);
            legend.Controls.Add(title);

            legend.Controls.Add(LegendRow(12, 34, Color.FromArgb(245, 245, 245), Color.FromArgb(200, 200, 200), "Свободно"));
            legend.Controls.Add(LegendRow(12, 58, Color.FromArgb(210, 50, 55), Color.FromArgb(180, 30, 35), "Заето"));
            legend.Controls.Add(LegendRow(12, 82, Color.FromArgb(245, 190, 60), Color.FromArgb(210, 160, 40), "Избрано"));
        }

        private Control LegendRow(int x, int y, Color fill, Color border, string text)
        {
            Panel row = new Panel();
            row.Location = new Point(x, y);
            row.Size = new Size(170, 20);

            Panel sw = new Panel();
            sw.Location = new Point(0, 2);
            sw.Size = new Size(16, 16);
            sw.BackColor = fill;
            sw.Paint += delegate (object sender, PaintEventArgs e)
            {
                using (Pen p = new Pen(border))
                    e.Graphics.DrawRectangle(p, 0, 0, 15, 15);
            };
            row.Controls.Add(sw);

            Label lbl = new Label();
            lbl.Text = text;
            lbl.AutoSize = true;
            lbl.Location = new Point(24, 1);
            row.Controls.Add(lbl);

            return row;
        }

        private void RefreshSummary()
        {
            if (SelectedTrip == null) return;

            string seats = string.Join(", ", seatMap.SelectedSeats.OrderBy(s => s).Select(s => s.ToString()).ToArray());
            decimal price = CalculatePrice();

            lblSummary.Text =
                "Маршрут: " + SelectedTrip.From + " → " + SelectedTrip.To + "\r\n" +
                "Дата: " + SelectedTrip.Date.ToString("dd.MM.yyyy") + "\r\n" +
                "Час: " + SelectedTrip.Depart + " - " + SelectedTrip.Arrive + "\r\n" +
                "Превозвач: " + SelectedTrip.Company + " / " + SelectedTrip.Vehicle + "\r\n" +
                "Място: " + seats + "\r\n" +
                "Отстъпка: " + (chkStudent.Checked ? "Ученическа (-30%)" : chkPensioner.Checked ? "Пенсионерска (-50%)" : "Няма") + "\r\n" +
                "Цена: " + price.ToString("0.00") + " лв.";
        }

        private void DoBuy()
        {
            if (SelectedTrip == null)
            {
                MessageBox.Show("Няма избран курс.", "Грешка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            HashSet<int> occ = GetOccupiedForTrip(TripKey(SelectedTrip));

            foreach (int s in seatMap.SelectedSeats)
            {
                if (occ.Contains(s))
                {
                    MessageBox.Show("Мястото вече е заето. Изберете друго.", "Заето място", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LoadSeatsForTrip();
                    GoStep(3);
                    return;
                }
            }

            foreach (int s in seatMap.SelectedSeats)
                occ.Add(s);

            RefreshSummary();

            MessageBox.Show("Успешно закупен билет!\n\n" + lblSummary.Text, "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

            GoStep(2);
            LoadTrips();
        }

        private string TripKey(Trip t)
        {
            return t.From + "|" + t.To + "|" + t.Date.ToString("yyyy-MM-dd") + "|" + t.Depart + "|" + t.Company;
        }

        private HashSet<int> GetOccupiedForTrip(string key)
        {
            HashSet<int> set;
            if (!Occupied.TryGetValue(key, out set))
            {
                set = new HashSet<int>();
                int count = rnd.Next(10, 22);
                for (int i = 0; i < count; i++)
                    set.Add(rnd.Next(1, 61));
                Occupied[key] = set;
            }
            return set;
        }

        private static string AddDuration(string startHHmm, string durHHmm)
        {
            TimeSpan s = TimeSpan.Parse(startHHmm + ":00");
            TimeSpan d = TimeSpan.Parse(durHHmm + ":00");
            TimeSpan a = s + d;
            if (a.TotalHours >= 24) a = a - TimeSpan.FromHours(24);
            return a.ToString(@"hh\:mm");
        }

        private void Hero_Paint(object sender, PaintEventArgs e)
        {
            Rectangle r = hero.ClientRectangle;
            using (System.Drawing.Drawing2D.LinearGradientBrush br =
                new System.Drawing.Drawing2D.LinearGradientBrush(r, BgTop, BgBottom, 90f))
            {
                e.Graphics.FillRectangle(br, r);
            }
        }

        private void Card_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            Rectangle r = card.ClientRectangle;
            r.Inflate(-1, -1);
            int radius = 18;

            using (System.Drawing.Drawing2D.GraphicsPath path = RoundedRect(r, radius))
            using (Pen pen = new Pen(Color.FromArgb(235, 235, 235)))
            {
                card.Region = new Region(path);
                e.Graphics.DrawPath(pen, path);
            }
        }

        private static System.Drawing.Drawing2D.GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int d = radius * 2;
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(bounds.Left, bounds.Top, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Top, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.Left, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}