using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using DrawingFont = System.Drawing.Font;
using DrawingFontStyle = System.Drawing.FontStyle;
using DrawingPoint = System.Drawing.Point;
using DrawingRectangle = System.Drawing.Rectangle;
using DrawingSize = System.Drawing.Size;
using TextFont = iTextSharp.text.Font;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        private readonly Color BgTop = Color.FromArgb(75, 55, 60);
        private readonly Color BgBottom = Color.FromArgb(35, 30, 33);

        private const decimal BgnToEurRate = 1.95583m;

        private int step = 1;
        private SeatMapControl seatMap;
        private RadioButton selectedTripRadioButton;

        private readonly List<string> Cities = new List<string>
        {
            "Варна", "София", "Бургас", "Пловдив", "Русе", "В. Търново"
        };

        private readonly Dictionary<string, decimal> RoutePrice = new Dictionary<string, decimal>();
        private readonly List<Trip> CurrentTrips = new List<Trip>();
        private Trip SelectedTrip;

        private readonly Dictionary<string, HashSet<int>> Occupied = new Dictionary<string, HashSet<int>>();
        private readonly Random rnd = new Random();

        private bool ticketPurchased;
        private TicketData purchasedTicket;

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

        private sealed class TicketData
        {
            public string TicketNumber;
            public string PassengerFirstName;
            public string PassengerLastName;
            public string PassengerEmail;
            public string From;
            public string To;
            public DateTime Date;
            public string Depart;
            public string Arrive;
            public string Vehicle;
            public string Company;
            public string SeatNumbers;
            public string DiscountText;
            public decimal PriceEuro;
            public DateTime CreatedAt;
            public string StatusText;
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
            UpdateStep4State();
        }

        private void BuildPrices()
        {
            RoutePrice.Clear();

            AddRoute("Варна", "София", ToEuro(35m));
            AddRoute("Варна", "Бургас", ToEuro(18m));
            AddRoute("Варна", "Пловдив", ToEuro(32m));
            AddRoute("Варна", "Русе", ToEuro(22m));
            AddRoute("Варна", "В. Търново", ToEuro(24m));

            AddRoute("Пловдив", "София", ToEuro(16m));
            AddRoute("Бургас", "София", ToEuro(28m));
            AddRoute("Русе", "София", ToEuro(24m));
            AddRoute("В. Търново", "София", ToEuro(18m));

            AddRoute("Бургас", "Пловдив", ToEuro(20m));
            AddRoute("Пловдив", "Русе", ToEuro(27m));

            AddRoute("Русе", "Бургас", ToEuro(30m));
            AddRoute("Русе", "В. Търново", ToEuro(14m));
            AddRoute("Бургас", "В. Търново", ToEuro(22m));
            AddRoute("Пловдив", "В. Търново", ToEuro(17m));
        }

        private decimal ToEuro(decimal leva)
        {
            return Math.Round(leva / BgnToEurRate, 2, MidpointRounding.AwayFromZero);
        }

        private void AddRoute(string from, string to, decimal priceEuro)
        {
            RoutePrice[from + "|" + to] = priceEuro;
            RoutePrice[to + "|" + from] = priceEuro;
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
            cmbFrom.SelectedIndexChanged += delegate
            {
                RefreshPrice();
                InvalidateUnpurchasedTicket();
            };

            cmbTo.SelectedIndexChanged += delegate
            {
                RefreshPrice();
                InvalidateUnpurchasedTicket();
            };

            dtDate.ValueChanged += delegate
            {
                RefreshPrice();
                InvalidateUnpurchasedTicket();
            };

            chkStudent.CheckedChanged += delegate
            {
                if (chkStudent.Checked) chkPensioner.Checked = false;
                RefreshPrice();
                InvalidateUnpurchasedTicket();
            };

            chkPensioner.CheckedChanged += delegate
            {
                if (chkPensioner.Checked) chkStudent.Checked = false;
                RefreshPrice();
                InvalidateUnpurchasedTicket();
            };

            txtFirstName.TextChanged += delegate
            {
                if (!ticketPurchased) RefreshSummary();
            };

            txtLastName.TextChanged += delegate
            {
                if (!ticketPurchased) RefreshSummary();
            };

            txtEmail.TextChanged += delegate
            {
                if (!ticketPurchased) RefreshSummary();
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
                    BgDialog.ShowWarning("Изберете курс.", "Валидация");
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
                    BgDialog.ShowWarning("Изберете място от схемата.", "Валидация");
                    return;
                }

                RefreshSummary();
                GoStep(4);
            };

            btnBack4.Click += BtnBack4_Click;
            btnBuy.Click += delegate { DoBuy(); };
            btnPdf.Click += delegate { ExportTicketToPdf(); };
            btnPrint.Click += delegate { PrintTicket(); };
            btnDeleteTicket.Click += delegate { DeleteTicketRequest(); };
        }

        private void BtnBack4_Click(object sender, EventArgs e)
        {
            if (ticketPurchased)
            {
                bool confirmed = BgDialog.ShowQuestion(
                    "Сигурни ли сте? Ще получите копие на билета на посочения имейл.",
                    "Потвърждение");

                if (!confirmed)
                    return;
            }

            GoStep(3);
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

            tripsList.Width = contentWidth;
            tripsList.Height = 248;

            btnBack2.Left = 0;
            btnBack2.Top = 314;

            btnNext2.Left = 160;
            btnNext2.Top = 314;

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

            lblSummary.Width = contentWidth - 20;

            btnBack4.Left = 0;
            btnBack4.Top = 314;

            if (ticketPurchased)
            {
                btnPdf.Left = 140;
                btnPdf.Top = 314;

                btnPrint.Left = 300;
                btnPrint.Top = 314;

                btnDeleteTicket.Left = 460;
                btnDeleteTicket.Top = 314;
            }
            else
            {
                btnBuy.Left = 160;
                btnBuy.Top = 314;
            }

            UpdateProgress();
        }

        private void GoStep(int newStep)
        {
            step = newStep;

            step1.Visible = step == 1;
            step2.Visible = step == 2;
            step3.Visible = step == 3;
            step4.Visible = step == 4;

            if (step == 4 && !ticketPurchased)
                RefreshSummary();

            UpdateProgress();
            RefreshPrice();
            LayoutResponsive();
            UpdateStep4State();
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
                BgDialog.ShowWarning("Моля изберете градове.", "Валидация");
                return false;
            }

            if ((string)cmbFrom.SelectedItem == (string)cmbTo.SelectedItem)
            {
                BgDialog.ShowWarning("Градът „От“ и „До“ не може да са еднакви.", "Валидация");
                return false;
            }

            return true;
        }

        private bool ValidateAllBeforeBuy()
        {
            if (!ValidateStep1())
                return false;

            if (SelectedTrip == null)
            {
                BgDialog.ShowWarning("Няма избран курс.", "Валидация");
                return false;
            }

            if (seatMap.SelectedSeats.Count == 0)
            {
                BgDialog.ShowWarning("Изберете място от схемата.", "Валидация");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                BgDialog.ShowWarning("Моля въведете име.", "Валидация");
                txtFirstName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                BgDialog.ShowWarning("Моля въведете фамилия.", "Валидация");
                txtLastName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                BgDialog.ShowWarning("Моля въведете имейл.", "Валидация");
                txtEmail.Focus();
                return false;
            }

            if (!IsValidEmail(txtEmail.Text.Trim()))
            {
                BgDialog.ShowWarning(
                    "Имейлът не е с валидна структура.\n\nПример за валиден имейл:\nvm3333@gmail.com",
                    "Невалиден имейл");
                txtEmail.Focus();
                return false;
            }

            return true;
        }

        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return Regex.IsMatch(
                email.Trim(),
                @"^[^@\s]+@[^@\s]+\.[^@\s]{2,}$",
                RegexOptions.IgnoreCase);
        }

        private decimal CalculatePrice()
        {
            string from = cmbFrom.SelectedItem as string;
            string to = cmbTo.SelectedItem as string;

            if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to))
                return 0m;

            decimal basePrice;
            if (!RoutePrice.TryGetValue(from + "|" + to, out basePrice))
                return 0m;

            decimal price = basePrice;

            if (chkStudent.Checked) price *= 0.70m;
            else if (chkPensioner.Checked) price *= 0.50m;

            if (price < 0m) price = 0m;

            return Math.Round(price, 2, MidpointRounding.AwayFromZero);
        }

        private void RefreshPrice()
        {
            decimal shownPrice = ticketPurchased && purchasedTicket != null
                ? purchasedTicket.PriceEuro
                : CalculatePrice();

            lblPrice.Text = "Цена: " + shownPrice.ToString("0.00") + " €";
        }

        private void LoadTrips()
        {
            tripsList.SuspendLayout();
            tripsList.Controls.Clear();
            CurrentTrips.Clear();
            SelectedTrip = null;
            selectedTripRadioButton = null;

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
                BgDialog.ShowWarning("Няма възможен курс за избраната дата и час.", "Няма курс");
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
            rb.Location = new DrawingPoint(row.Width - 24, 23);
            rb.Width = 20;
            rb.Tag = t;
            rb.CheckedChanged += TripRadioButton_CheckedChanged;
            row.Controls.Add(rb);

            Label route = new Label();
            route.Text = t.From + " - " + t.To;
            route.AutoSize = true;
            route.Font = new DrawingFont("Segoe UI Semibold", 10.5F, DrawingFontStyle.Bold);
            route.Location = new DrawingPoint(12, 10);
            row.Controls.Add(route);

            Label vehicle = new Label();
            vehicle.Text = "🚌 " + t.Vehicle + "  •  " + t.Company;
            vehicle.AutoSize = true;
            vehicle.ForeColor = Color.FromArgb(120, 120, 120);
            vehicle.Location = new DrawingPoint(12, 36);
            row.Controls.Add(vehicle);

            Label times = new Label();
            times.Text = t.Depart + " - " + t.Arrive;
            times.AutoSize = true;
            times.Font = new DrawingFont("Segoe UI Semibold", 10F, DrawingFontStyle.Bold);
            times.Location = new DrawingPoint(260, 22);
            row.Controls.Add(times);

            Label free = new Label();
            free.Text = "☑ има свободни места";
            free.AutoSize = true;
            free.ForeColor = Color.FromArgb(40, 140, 70);
            free.Location = new DrawingPoint(row.Width - 210, 22);
            row.Controls.Add(free);

            return row;
        }

        private void TripRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton current = sender as RadioButton;
            if (current == null) return;

            if (current.Checked)
            {
                if (selectedTripRadioButton != null && selectedTripRadioButton != current)
                    selectedTripRadioButton.Checked = false;

                selectedTripRadioButton = current;
                SelectedTrip = current.Tag as Trip;
                InvalidateUnpurchasedTicket();
            }
            else
            {
                if (selectedTripRadioButton == current)
                {
                    selectedTripRadioButton = null;
                    SelectedTrip = null;
                }
            }
        }

        private void LoadSeatsForTrip()
        {
            if (SelectedTrip == null)
                return;

            seatMap.ClearSelection();
            seatMap.MaxSelectable = 1;

            HashSet<int> occ = GetOccupiedForTrip(TripKey(SelectedTrip));
            seatMap.SetOccupiedSeats(occ);
            UpdateSeatCounters();
            InvalidateUnpurchasedTicket();
        }

        private void UpdateSeatCounters()
        {
            int chosen = seatMap.SelectedSeats.Count;
            int left = seatMap.MaxSelectable - chosen;

            lblChosen.Text = "Избрани места: " + chosen;
            lblRemaining.Text = "Остават: " + left;

            if (!ticketPurchased)
                RefreshSummary();
        }

        private void BuildLegend()
        {
            legend.Controls.Clear();

            Label title = new Label();
            title.Text = "Информация";
            title.AutoSize = true;
            title.ForeColor = Color.FromArgb(120, 120, 120);
            title.Location = new DrawingPoint(12, 10);
            legend.Controls.Add(title);

            legend.Controls.Add(LegendRow(12, 34, Color.FromArgb(245, 245, 245), Color.FromArgb(200, 200, 200), "Свободно"));
            legend.Controls.Add(LegendRow(12, 58, Color.FromArgb(210, 50, 55), Color.FromArgb(180, 30, 35), "Заето"));
            legend.Controls.Add(LegendRow(12, 82, Color.FromArgb(245, 190, 60), Color.FromArgb(210, 160, 40), "Избрано"));
        }

        private Control LegendRow(int x, int y, Color fill, Color border, string text)
        {
            Panel row = new Panel();
            row.Location = new DrawingPoint(x, y);
            row.Size = new DrawingSize(170, 20);

            Panel sw = new Panel();
            sw.Location = new DrawingPoint(0, 2);
            sw.Size = new DrawingSize(16, 16);
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
            lbl.Location = new DrawingPoint(24, 1);
            row.Controls.Add(lbl);

            return row;
        }

        private void RefreshSummary()
        {
            if (ticketPurchased && purchasedTicket != null)
            {
                lblStep4Header.Text = "Заявката е приета";
                lblSummary.Text = BuildPurchasedTicketSummaryText(purchasedTicket);
                RefreshPrice();
                return;
            }

            lblStep4Header.Text = "Вашият избор";

            if (SelectedTrip == null)
            {
                lblSummary.Text = "Изберете курс и място, за да видите обобщението.";
                return;
            }

            string seats = seatMap.SelectedSeats.Count == 0
                ? "—"
                : string.Join(", ", seatMap.SelectedSeats.OrderBy(s => s).Select(s => s.ToString()).ToArray());

            decimal price = CalculatePrice();

            lblSummary.Text =
                "Маршрут: " + SelectedTrip.From + " → " + SelectedTrip.To + "\r\n" +
                "Дата: " + SelectedTrip.Date.ToString("dd.MM.yyyy") + "\r\n" +
                "Час: " + SelectedTrip.Depart + " - " + SelectedTrip.Arrive + "\r\n" +
                "Превозвач: " + SelectedTrip.Company + " / " + SelectedTrip.Vehicle + "\r\n" +
                "Място: " + seats + "\r\n" +
                "Пътник: " + txtFirstName.Text.Trim() + " " + txtLastName.Text.Trim() + "\r\n" +
                "Имейл: " + txtEmail.Text.Trim() + "\r\n" +
                "Отстъпка: " + GetDiscountText() + "\r\n" +
                "Цена: " + price.ToString("0.00") + " €";
        }

        private string GetDiscountText()
        {
            if (chkStudent.Checked) return "Ученическа (-30%)";
            if (chkPensioner.Checked) return "Пенсионерска (-50%)";
            return "Няма";
        }

        private void DoBuy()
        {
            if (ticketPurchased)
                return;

            if (!ValidateAllBeforeBuy())
                return;

            HashSet<int> occ = GetOccupiedForTrip(TripKey(SelectedTrip));

            foreach (int s in seatMap.SelectedSeats)
            {
                if (occ.Contains(s))
                {
                    BgDialog.ShowWarning("Мястото вече е заето. Изберете друго.", "Заето място");
                    LoadSeatsForTrip();
                    GoStep(3);
                    return;
                }
            }

            foreach (int s in seatMap.SelectedSeats)
                occ.Add(s);

            TicketData ticket = BuildTicketDataFromCurrentSelection();
            purchasedTicket = ticket;
            ticketPurchased = true;

            RefreshSummary();
            UpdateStep4State();

            BgDialog.ShowInfo(
                "Успешно закупен билет!\n\n" + BuildMessageBoxTicketText(ticket),
                "Успех");
        }

        private TicketData BuildTicketDataFromCurrentSelection()
        {
            return new TicketData
            {
                TicketNumber = GenerateTicketNumber(),
                PassengerFirstName = txtFirstName.Text.Trim(),
                PassengerLastName = txtLastName.Text.Trim(),
                PassengerEmail = txtEmail.Text.Trim(),
                From = SelectedTrip.From,
                To = SelectedTrip.To,
                Date = SelectedTrip.Date,
                Depart = SelectedTrip.Depart,
                Arrive = SelectedTrip.Arrive,
                Vehicle = SelectedTrip.Vehicle,
                Company = SelectedTrip.Company,
                SeatNumbers = string.Join(", ", seatMap.SelectedSeats.OrderBy(s => s).Select(s => s.ToString()).ToArray()),
                DiscountText = GetDiscountText(),
                PriceEuro = CalculatePrice(),
                CreatedAt = DateTime.Now,
                StatusText = "Заявката ще бъде обработена и копие на билета ще получите на посочения имейл."
            };
        }

        private string GenerateTicketNumber()
        {
            string datePart = DateTime.Now.ToString("yyMMdd");
            string randomPart = rnd.Next(100000, 999999).ToString();
            return "ATG-" + datePart + "-" + randomPart;
        }

        private string BuildMessageBoxTicketText(TicketData ticket)
        {
            return
                "Номер на билет: " + ticket.TicketNumber + "\n" +
                "Пътник: " + ticket.PassengerFirstName + " " + ticket.PassengerLastName + "\n" +
                "Имейл: " + ticket.PassengerEmail + "\n" +
                "Маршрут: " + ticket.From + " → " + ticket.To + "\n" +
                "Дата: " + ticket.Date.ToString("dd.MM.yyyy") + "\n" +
                "Час: " + ticket.Depart + " - " + ticket.Arrive + "\n" +
                "Превозвач: " + ticket.Company + " / " + ticket.Vehicle + "\n" +
                "Място: " + ticket.SeatNumbers + "\n" +
                "Отстъпка: " + ticket.DiscountText + "\n" +
                "Цена: " + ticket.PriceEuro.ToString("0.00") + " €";
        }

        private string BuildPurchasedTicketSummaryText(TicketData ticket)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Вашата заявка за билет е приета успешно.");
            sb.AppendLine(ticket.StatusText);
            sb.AppendLine();
            sb.AppendLine("Номер на билет: " + ticket.TicketNumber);
            sb.AppendLine("Пътник: " + ticket.PassengerFirstName + " " + ticket.PassengerLastName);
            sb.AppendLine("Имейл: " + ticket.PassengerEmail);
            sb.AppendLine("Маршрут: " + ticket.From + " → " + ticket.To);
            sb.AppendLine("Дата: " + ticket.Date.ToString("dd.MM.yyyy"));
            sb.AppendLine("Час: " + ticket.Depart + " - " + ticket.Arrive);
            sb.AppendLine("Превозвач: " + ticket.Company + " / " + ticket.Vehicle);
            sb.AppendLine("Място: " + ticket.SeatNumbers);
            sb.AppendLine("Отстъпка: " + ticket.DiscountText);
            sb.AppendLine("Цена: " + ticket.PriceEuro.ToString("0.00") + " €");

            return sb.ToString();
        }

        private void UpdateStep4State()
        {
            bool purchased = ticketPurchased && purchasedTicket != null;

            btnBuy.Visible = !purchased;
            btnPdf.Visible = purchased;
            btnPrint.Visible = purchased;
            btnDeleteTicket.Visible = purchased;

            txtFirstName.ReadOnly = purchased;
            txtLastName.ReadOnly = purchased;
            txtEmail.ReadOnly = purchased;

            lblStep4Header.Text = purchased ? "Заявката е приета" : "Вашият избор";

            if (!purchased)
                RefreshSummary();
            else
                lblSummary.Text = BuildPurchasedTicketSummaryText(purchasedTicket);

            RefreshPrice();
            LayoutResponsive();
        }

        private void DeleteTicketRequest()
        {
            if (!ticketPurchased || purchasedTicket == null)
                return;

            bool confirmed = BgDialog.ShowQuestion(
                "Сигурни ли сте, че искате да изтриете заявката?",
                "Изтриване на заявка");

            if (!confirmed)
                return;

            ticketPurchased = false;
            purchasedTicket = null;

            RefreshSummary();
            UpdateStep4State();

            BgDialog.ShowInfo("Заявката за билет беше изтрита.", "Изтрито");
        }

        private void InvalidateUnpurchasedTicket()
        {
            if (ticketPurchased)
                return;

            purchasedTicket = null;
            RefreshSummary();
        }

        private void ExportTicketToPdf()
        {
            if (!ticketPurchased || purchasedTicket == null)
            {
                BgDialog.ShowInfo("Няма закупен билет за експортиране.", "Информация");
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "Запази билет като PDF";
                sfd.Filter = "PDF файл (*.pdf)|*.pdf";
                sfd.FileName = purchasedTicket.TicketNumber + ".pdf";

                if (sfd.ShowDialog(this) != DialogResult.OK)
                    return;

                try
                {
                    GeneratePdfTicket(sfd.FileName, purchasedTicket);
                    BgDialog.ShowInfo("PDF билетът беше генериран успешно.", "Готово");
                }
                catch (Exception ex)
                {
                    BgDialog.ShowError("Възникна проблем при генерирането на PDF.\n\n" + ex.Message, "Грешка");
                }
            }
        }

        private void PrintTicket()
        {
            if (!ticketPurchased || purchasedTicket == null)
            {
                BgDialog.ShowInfo("Няма закупен билет за принтиране.", "Информация");
                return;
            }

            try
            {
                using (PrintDialog pd = new PrintDialog())
                {
                    pd.AllowSomePages = false;
                    pd.AllowSelection = false;
                    pd.UseEXDialog = true;
                    pd.Document = CreateTicketPrintDocument();

                    if (pd.ShowDialog(this) == DialogResult.OK)
                    {
                        pd.Document.PrintController = new StandardPrintController();
                        pd.Document.Print();
                    }
                }
            }
            catch (Exception ex)
            {
                BgDialog.ShowError("Възникна проблем при принтирането.\n\n" + ex.Message, "Грешка");
            }
        }

        private PrintDocument CreateTicketPrintDocument()
        {
            PrintDocument doc = new PrintDocument();
            doc.DocumentName = "Билет " + purchasedTicket.TicketNumber;
            doc.DefaultPageSettings.Margins = new Margins(40, 40, 40, 40);
            doc.DefaultPageSettings.Landscape = false;
            doc.PrintPage += TicketPrintDocument_PrintPage;
            return doc;
        }

        private void TicketPrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            e.Graphics.Clear(Color.White);

            DrawingRectangle bounds = e.MarginBounds;
            DrawProfessionalTicket(e.Graphics, bounds, purchasedTicket);

            e.HasMorePages = false;
        }

        private void DrawProfessionalTicket(Graphics g, DrawingRectangle bounds, TicketData ticket)
        {
            Color brandRed = Color.FromArgb(185, 0, 0);
            Color softGray = Color.FromArgb(243, 243, 243);
            Color lineGray = Color.FromArgb(225, 225, 225);
            Color textGray = Color.FromArgb(90, 90, 90);
            Color successGreen = Color.FromArgb(33, 115, 70);

            using (Brush white = new SolidBrush(Color.White))
                g.FillRectangle(white, bounds);

            DrawingRectangle outerCard = new DrawingRectangle(bounds.Left + 10, bounds.Top + 10, bounds.Width - 20, bounds.Height - 20);

            using (GraphicsPath cardPath = RoundedRect(outerCard, 18))
            using (SolidBrush cardBrush = new SolidBrush(Color.White))
            using (Pen borderPen = new Pen(lineGray))
            {
                g.FillPath(cardBrush, cardPath);
                g.DrawPath(borderPen, cardPath);
            }

            DrawingRectangle headerBar = new DrawingRectangle(outerCard.Left, outerCard.Top, outerCard.Width, 52);
            using (GraphicsPath headerPath = TopRoundedRect(headerBar, 18))
            using (SolidBrush redBrush = new SolidBrush(brandRed))
            {
                g.FillPath(redBrush, headerPath);
            }

            using (DrawingFont fBrand = new DrawingFont("Segoe UI Semibold", 17f, DrawingFontStyle.Bold))
            using (Brush bWhite = new SolidBrush(Color.White))
            {
                g.DrawString("AUTOGARA", fBrand, bWhite, outerCard.Left + 20, outerCard.Top + 10);
            }

            int contentX = outerCard.Left + 28;
            int contentW = outerCard.Width - 56;
            int y = outerCard.Top + 74;

            using (DrawingFont fTitle = new DrawingFont("Segoe UI Semibold", 20f, DrawingFontStyle.Bold))
            using (Brush bBlack = new SolidBrush(Color.FromArgb(30, 30, 30)))
            {
                g.DrawString("Автобусен билет", fTitle, bBlack, contentX, y);
            }

            y += 34;

            using (DrawingFont fSub = new DrawingFont("Segoe UI", 10f))
            using (Brush bGray = new SolidBrush(textGray))
            {
                g.DrawString("Професионален електронен билет", fSub, bGray, contentX, y);
            }

            y += 22;

            using (Pen p = new Pen(brandRed, 3f))
            {
                g.DrawLine(p, contentX, y, contentX + 200, y);
            }

            using (Pen pLine = new Pen(lineGray, 1f))
            {
                g.DrawLine(pLine, contentX + 212, y, contentX + contentW, y);
            }

            y += 24;

            DrawingRectangle statusBox = new DrawingRectangle(contentX, y, contentW, 50);
            using (SolidBrush sb = new SolidBrush(Color.FromArgb(241, 249, 243)))
            using (Pen pen = new Pen(Color.FromArgb(182, 224, 197)))
            {
                g.FillRectangle(sb, statusBox);
                g.DrawRectangle(pen, statusBox);
            }

            using (DrawingFont fStatus = new DrawingFont("Segoe UI Semibold", 10.5f, DrawingFontStyle.Bold))
            using (Brush bStatus = new SolidBrush(successGreen))
            {
                g.DrawString("Заявката е приета успешно", fStatus, bStatus, statusBox.Left + 12, statusBox.Top + 7);
            }

            using (DrawingFont fStatusText = new DrawingFont("Segoe UI", 9.5f))
            using (Brush bStatusText = new SolidBrush(textGray))
            {
                g.DrawString("Копие на билета ще бъде изпратено на: " + ticket.PassengerEmail,
                    fStatusText, bStatusText, statusBox.Left + 12, statusBox.Top + 25);
            }

            y += 70;

            DrawingRectangle leftBlock = new DrawingRectangle(contentX, y, (contentW - 20) / 2, 245);
            DrawingRectangle rightBlock = new DrawingRectangle(leftBlock.Right + 20, y, (contentW - 20) / 2, 245);

            DrawInfoBlock(g, leftBlock, "Детайли за пътуване", new[]
            {
                Tuple.Create("Маршрут", ticket.From + " → " + ticket.To),
                Tuple.Create("Дата", ticket.Date.ToString("dd.MM.yyyy")),
                Tuple.Create("Час", ticket.Depart + " - " + ticket.Arrive),
                Tuple.Create("Превозвач", ticket.Company),
                Tuple.Create("Автобус", ticket.Vehicle),
                Tuple.Create("Място", ticket.SeatNumbers)
            }, softGray, lineGray, textGray);

            DrawInfoBlock(g, rightBlock, "Детайли за билета", new[]
            {
                Tuple.Create("Номер на билет", ticket.TicketNumber),
                Tuple.Create("Пътник", ticket.PassengerFirstName + " " + ticket.PassengerLastName),
                Tuple.Create("Имейл", ticket.PassengerEmail),
                Tuple.Create("Отстъпка", ticket.DiscountText),
                Tuple.Create("Цена", ticket.PriceEuro.ToString("0.00") + " €"),
                Tuple.Create("Създадено", ticket.CreatedAt.ToString("dd.MM.yyyy HH:mm"))
            }, softGray, lineGray, textGray);

            y += 270;

            DrawingRectangle footerBox = new DrawingRectangle(contentX, y, contentW, 70);
            using (SolidBrush sb = new SolidBrush(Color.FromArgb(250, 250, 250)))
            using (Pen p = new Pen(lineGray))
            {
                g.FillRectangle(sb, footerBox);
                g.DrawRectangle(p, footerBox);
            }

            using (DrawingFont fFootTitle = new DrawingFont("Segoe UI Semibold", 10f, DrawingFontStyle.Bold))
            using (DrawingFont fFootText = new DrawingFont("Segoe UI", 9f))
            using (Brush bTitle = new SolidBrush(Color.FromArgb(45, 45, 45)))
            using (Brush bText = new SolidBrush(textGray))
            {
                g.DrawString("Важно", fFootTitle, bTitle, footerBox.Left + 12, footerBox.Top + 10);
                g.DrawString(
                    "Моля, представете този билет при качване. Съхранете документа или отпечатано копие до края на пътуването.",
                    fFootText,
                    bText,
                    new RectangleF(footerBox.Left + 12, footerBox.Top + 28, footerBox.Width - 24, 28));
            }
        }

        private void DrawInfoBlock(Graphics g, DrawingRectangle rect, string title, Tuple<string, string>[] rows, Color softGray, Color lineGray, Color textGray)
        {
            using (SolidBrush sb = new SolidBrush(softGray))
            using (Pen p = new Pen(lineGray))
            {
                g.FillRectangle(sb, rect);
                g.DrawRectangle(p, rect);
            }

            using (DrawingFont fTitle = new DrawingFont("Segoe UI Semibold", 11f, DrawingFontStyle.Bold))
            using (Brush bTitle = new SolidBrush(Color.FromArgb(35, 35, 35)))
            {
                g.DrawString(title, fTitle, bTitle, rect.Left + 12, rect.Top + 12);
            }

            int y = rect.Top + 44;

            using (DrawingFont fLabel = new DrawingFont("Segoe UI Semibold", 9.5f, DrawingFontStyle.Bold))
            using (DrawingFont fValue = new DrawingFont("Segoe UI", 9.5f))
            using (Brush bLabel = new SolidBrush(Color.FromArgb(60, 60, 60)))
            using (Brush bValue = new SolidBrush(textGray))
            using (Pen linePen = new Pen(Color.FromArgb(230, 230, 230)))
            {
                for (int i = 0; i < rows.Length; i++)
                {
                    g.DrawString(rows[i].Item1, fLabel, bLabel, rect.Left + 12, y);
                    g.DrawString(rows[i].Item2, fValue, bValue, new RectangleF(rect.Left + 145, y, rect.Width - 157, 28));

                    if (i < rows.Length - 1)
                        g.DrawLine(linePen, rect.Left + 12, y + 24, rect.Right - 12, y + 24);

                    y += 34;
                }
            }
        }

        private void GeneratePdfTicket(string filePath, TicketData ticket)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);

            string regularFontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
            string boldFontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arialbd.ttf");

            BaseFont bfRegular = BaseFont.CreateFont(regularFontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            BaseFont bfBold = BaseFont.CreateFont(boldFontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            TextFont fBrand = new TextFont(bfBold, 16f, TextFont.NORMAL, BaseColor.WHITE);
            TextFont fTitle = new TextFont(bfBold, 20f, TextFont.NORMAL, new BaseColor(30, 30, 30));
            TextFont fSub = new TextFont(bfRegular, 10f, TextFont.NORMAL, new BaseColor(90, 90, 90));
            TextFont fBlockTitle = new TextFont(bfBold, 11f, TextFont.NORMAL, new BaseColor(35, 35, 35));
            TextFont fLabel = new TextFont(bfBold, 9.5f, TextFont.NORMAL, new BaseColor(60, 60, 60));
            TextFont fValue = new TextFont(bfRegular, 9.5f, TextFont.NORMAL, new BaseColor(90, 90, 90));
            TextFont fStatusTitle = new TextFont(bfBold, 10.5f, TextFont.NORMAL, new BaseColor(33, 115, 70));
            TextFont fStatusText = new TextFont(bfRegular, 9.5f, TextFont.NORMAL, new BaseColor(90, 90, 90));
            TextFont fFooterTitle = new TextFont(bfBold, 10f, TextFont.NORMAL, new BaseColor(45, 45, 45));
            TextFont fFooterText = new TextFont(bfRegular, 9f, TextFont.NORMAL, new BaseColor(90, 90, 90));

            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            using (Document doc = new Document(iTextSharp.text.PageSize.A4, 40, 40, 40, 40))
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();

                PdfContentByte cb = writer.DirectContent;
                float pageWidth = doc.PageSize.Width;
                float pageHeight = doc.PageSize.Height;

                float cardX = 40f;
                float cardY = 60f;
                float cardW = pageWidth - 80f;
                float cardH = pageHeight - 120f;

                BaseColor brandRed = new BaseColor(185, 0, 0);
                BaseColor softGray = new BaseColor(243, 243, 243);
                BaseColor lineGray = new BaseColor(225, 225, 225);
                BaseColor statusGreen = new BaseColor(241, 249, 243);
                BaseColor statusGreenBorder = new BaseColor(182, 224, 197);

                DrawPdfRoundedRectangle(cb, cardX, cardY, cardW, cardH, 18f, BaseColor.WHITE, lineGray, 1f);
                DrawPdfTopBar(cb, cardX, cardY + cardH - 52f, cardW, 52f, brandRed);

                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("AUTOGARA", fBrand), cardX + 20f, cardY + cardH - 32f, 0);

                float contentX = cardX + 28f;
                float contentY = cardY + cardH - 82f;
                float contentW = cardW - 56f;

                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Автобусен билет", fTitle), contentX, contentY, 0);
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Професионален електронен билет", fSub), contentX, contentY - 20f, 0);

                cb.SetColorStroke(brandRed);
                cb.SetLineWidth(3f);
                cb.MoveTo(contentX, contentY - 32f);
                cb.LineTo(contentX + 200f, contentY - 32f);
                cb.Stroke();

                cb.SetColorStroke(lineGray);
                cb.SetLineWidth(1f);
                cb.MoveTo(contentX + 212f, contentY - 32f);
                cb.LineTo(contentX + contentW, contentY - 32f);
                cb.Stroke();

                float statusY = contentY - 86f;
                DrawPdfRectangle(cb, contentX, statusY, contentW, 50f, statusGreen, statusGreenBorder, 1f);
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Заявката е приета успешно", fStatusTitle), contentX + 12f, statusY + 32f, 0);
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Копие на билета ще бъде изпратено на: " + ticket.PassengerEmail, fStatusText), contentX + 12f, statusY + 14f, 0);

                float blocksY = statusY - 265f;
                float blockW = (contentW - 20f) / 2f;
                float blockH = 245f;

                DrawPdfInfoBlock(cb, contentX, blocksY, blockW, blockH, "Детайли за пътуване", new[]
                {
                    Tuple.Create("Маршрут", ticket.From + " → " + ticket.To),
                    Tuple.Create("Дата", ticket.Date.ToString("dd.MM.yyyy")),
                    Tuple.Create("Час", ticket.Depart + " - " + ticket.Arrive),
                    Tuple.Create("Превозвач", ticket.Company),
                    Tuple.Create("Автобус", ticket.Vehicle),
                    Tuple.Create("Място", ticket.SeatNumbers)
                }, softGray, lineGray, fBlockTitle, fLabel, fValue);

                DrawPdfInfoBlock(cb, contentX + blockW + 20f, blocksY, blockW, blockH, "Детайли за билета", new[]
                {
                    Tuple.Create("Номер на билет", ticket.TicketNumber),
                    Tuple.Create("Пътник", ticket.PassengerFirstName + " " + ticket.PassengerLastName),
                    Tuple.Create("Имейл", ticket.PassengerEmail),
                    Tuple.Create("Отстъпка", ticket.DiscountText),
                    Tuple.Create("Цена", ticket.PriceEuro.ToString("0.00") + " €"),
                    Tuple.Create("Създадено", ticket.CreatedAt.ToString("dd.MM.yyyy HH:mm"))
                }, softGray, lineGray, fBlockTitle, fLabel, fValue);

                float footerY = blocksY - 90f;
                DrawPdfRectangle(cb, contentX, footerY, contentW, 70f, new BaseColor(250, 250, 250), lineGray, 1f);

                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Важно", fFooterTitle), contentX + 12f, footerY + 48f, 0);

                ColumnText ct = new ColumnText(cb);
                ct.SetSimpleColumn(
                    new Phrase("Моля, представете този билет при качване. Съхранете документа или отпечатано копие до края на пътуването.", fFooterText),
                    contentX + 12f,
                    footerY + 10f,
                    contentX + contentW - 12f,
                    footerY + 38f,
                    14f,
                    Element.ALIGN_LEFT);
                ct.Go();

                doc.Close();
            }
        }

        private void DrawPdfInfoBlock(PdfContentByte cb, float x, float y, float w, float h, string title, Tuple<string, string>[] rows,
            BaseColor fill, BaseColor border, TextFont titleFont, TextFont labelFont, TextFont valueFont)
        {
            DrawPdfRectangle(cb, x, y, w, h, fill, border, 1f);

            ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(title, titleFont), x + 12f, y + h - 18f, 0);

            float rowY = y + h - 48f;

            for (int i = 0; i < rows.Length; i++)
            {
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(rows[i].Item1, labelFont), x + 12f, rowY, 0);

                ColumnText ct = new ColumnText(cb);
                ct.SetSimpleColumn(
                    new Phrase(rows[i].Item2, valueFont),
                    x + 145f,
                    rowY - 12f,
                    x + w - 12f,
                    rowY + 10f,
                    13f,
                    Element.ALIGN_LEFT);
                ct.Go();

                if (i < rows.Length - 1)
                {
                    cb.SetColorStroke(new BaseColor(230, 230, 230));
                    cb.SetLineWidth(1f);
                    cb.MoveTo(x + 12f, rowY - 18f);
                    cb.LineTo(x + w - 12f, rowY - 18f);
                    cb.Stroke();
                }

                rowY -= 34f;
            }
        }

        private void DrawPdfRectangle(PdfContentByte cb, float x, float y, float w, float h, BaseColor fill, BaseColor border, float borderWidth)
        {
            cb.SaveState();
            cb.SetColorFill(fill);
            cb.SetColorStroke(border);
            cb.SetLineWidth(borderWidth);
            cb.Rectangle(x, y, w, h);
            cb.FillStroke();
            cb.RestoreState();
        }

        private void DrawPdfRoundedRectangle(PdfContentByte cb, float x, float y, float w, float h, float radius, BaseColor fill, BaseColor border, float borderWidth)
        {
            cb.SaveState();
            cb.SetColorFill(fill);
            cb.SetColorStroke(border);
            cb.SetLineWidth(borderWidth);
            cb.RoundRectangle(x, y, w, h, radius);
            cb.FillStroke();
            cb.RestoreState();
        }

        private void DrawPdfTopBar(PdfContentByte cb, float x, float y, float w, float h, BaseColor fill)
        {
            cb.SaveState();
            cb.SetColorFill(fill);
            cb.RoundRectangle(x, y, w, h, 18f);
            cb.Fill();
            cb.RestoreState();

            cb.SaveState();
            cb.SetColorFill(fill);
            cb.Rectangle(x, y, w, h - 18f);
            cb.Fill();
            cb.RestoreState();
        }

        private GraphicsPath TopRoundedRect(DrawingRectangle bounds, int radius)
        {
            int d = radius * 2;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(bounds.Left, bounds.Top, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Top, d, d, 270, 90);
            path.AddLine(bounds.Right, bounds.Bottom, bounds.Right, bounds.Top + radius);
            path.AddLine(bounds.Left, bounds.Bottom, bounds.Left, bounds.Top + radius);
            path.CloseFigure();
            return path;
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

            if (a.TotalHours >= 24)
                a = a - TimeSpan.FromHours(24);

            return a.ToString(@"hh\:mm");
        }

        private void Hero_Paint(object sender, PaintEventArgs e)
        {
            DrawingRectangle r = hero.ClientRectangle;

            using (LinearGradientBrush br = new LinearGradientBrush(r, BgTop, BgBottom, 90f))
            {
                e.Graphics.FillRectangle(br, r);
            }
        }

        private void Card_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            DrawingRectangle r = card.ClientRectangle;
            r.Inflate(-1, -1);

            using (GraphicsPath path = RoundedRect(r, 18))
            using (Pen pen = new Pen(Color.FromArgb(235, 235, 235)))
            {
                card.Region = new Region(path);
                e.Graphics.DrawPath(pen, path);
            }
        }

        private static GraphicsPath RoundedRect(DrawingRectangle bounds, int radius)
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