namespace WindowsFormsApp2
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel hero;
        private System.Windows.Forms.Panel topBar;
        private System.Windows.Forms.Label lblBrand;

        private System.Windows.Forms.Panel card;
        private System.Windows.Forms.Panel cardHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Panel progressTrack;
        private System.Windows.Forms.Panel progressFill;

        private System.Windows.Forms.Panel stepHost;

        private System.Windows.Forms.Panel step1;
        private System.Windows.Forms.Panel step2;
        private System.Windows.Forms.Panel step3;
        private System.Windows.Forms.Panel step4;

        private System.Windows.Forms.Panel footer;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.Panel footerLine;

        private System.Windows.Forms.Label lblStep1Header;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.ComboBox cmbFrom;
        private System.Windows.Forms.ComboBox cmbTo;
        private System.Windows.Forms.DateTimePicker dtDate;
        private System.Windows.Forms.CheckBox chkStudent;
        private System.Windows.Forms.CheckBox chkPensioner;
        private System.Windows.Forms.Button btnNext1;

        private System.Windows.Forms.Label lblStep2Header;
        private System.Windows.Forms.FlowLayoutPanel tripsList;
        private System.Windows.Forms.Button btnBack2;
        private System.Windows.Forms.Button btnNext2;

        private System.Windows.Forms.Label lblStep3Header;
        private System.Windows.Forms.Panel pnlSeatHost;
        private System.Windows.Forms.Label lblChosen;
        private System.Windows.Forms.Label lblRemaining;
        private System.Windows.Forms.Panel legend;
        private System.Windows.Forms.Button btnBack3;
        private System.Windows.Forms.Button btnNext3;

        private System.Windows.Forms.Label lblStep4Header;
        private System.Windows.Forms.Label lblSummary;
        private System.Windows.Forms.Label lblFirstName;
        private System.Windows.Forms.Label lblLastName;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Button btnBack4;
        private System.Windows.Forms.Button btnPdf;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnBuy;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.hero = new System.Windows.Forms.Panel();
            this.topBar = new System.Windows.Forms.Panel();
            this.lblBrand = new System.Windows.Forms.Label();
            this.card = new System.Windows.Forms.Panel();
            this.stepHost = new System.Windows.Forms.Panel();
            this.step1 = new System.Windows.Forms.Panel();
            this.lblStep1Header = new System.Windows.Forms.Label();
            this.lblFrom = new System.Windows.Forms.Label();
            this.cmbFrom = new System.Windows.Forms.ComboBox();
            this.lblTo = new System.Windows.Forms.Label();
            this.cmbTo = new System.Windows.Forms.ComboBox();
            this.lblDate = new System.Windows.Forms.Label();
            this.dtDate = new System.Windows.Forms.DateTimePicker();
            this.chkStudent = new System.Windows.Forms.CheckBox();
            this.chkPensioner = new System.Windows.Forms.CheckBox();
            this.btnNext1 = new System.Windows.Forms.Button();
            this.step2 = new System.Windows.Forms.Panel();
            this.lblStep2Header = new System.Windows.Forms.Label();
            this.tripsList = new System.Windows.Forms.FlowLayoutPanel();
            this.btnBack2 = new System.Windows.Forms.Button();
            this.btnNext2 = new System.Windows.Forms.Button();
            this.step3 = new System.Windows.Forms.Panel();
            this.lblStep3Header = new System.Windows.Forms.Label();
            this.pnlSeatHost = new System.Windows.Forms.Panel();
            this.lblChosen = new System.Windows.Forms.Label();
            this.lblRemaining = new System.Windows.Forms.Label();
            this.legend = new System.Windows.Forms.Panel();
            this.btnBack3 = new System.Windows.Forms.Button();
            this.btnNext3 = new System.Windows.Forms.Button();
            this.step4 = new System.Windows.Forms.Panel();
            this.lblStep4Header = new System.Windows.Forms.Label();
            this.lblSummary = new System.Windows.Forms.Label();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.lblLastName = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.btnBack4 = new System.Windows.Forms.Button();
            this.btnPdf = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnBuy = new System.Windows.Forms.Button();
            this.footer = new System.Windows.Forms.Panel();
            this.lblPrice = new System.Windows.Forms.Label();
            this.footerLine = new System.Windows.Forms.Panel();
            this.cardHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.progressTrack = new System.Windows.Forms.Panel();
            this.progressFill = new System.Windows.Forms.Panel();
            this.hero.SuspendLayout();
            this.topBar.SuspendLayout();
            this.card.SuspendLayout();
            this.stepHost.SuspendLayout();
            this.step1.SuspendLayout();
            this.step2.SuspendLayout();
            this.step3.SuspendLayout();
            this.step4.SuspendLayout();
            this.footer.SuspendLayout();
            this.cardHeader.SuspendLayout();
            this.progressTrack.SuspendLayout();
            this.SuspendLayout();
            // 
            // hero
            // 
            this.hero.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(35)))), ((int)(((byte)(40)))));
            this.hero.Controls.Add(this.topBar);
            this.hero.Controls.Add(this.card);
            this.hero.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hero.Location = new System.Drawing.Point(0, 0);
            this.hero.Name = "hero";
            this.hero.Size = new System.Drawing.Size(1400, 760);
            this.hero.TabIndex = 0;
            // 
            // topBar
            // 
            this.topBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.topBar.Controls.Add(this.lblBrand);
            this.topBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.topBar.Location = new System.Drawing.Point(0, 0);
            this.topBar.Name = "topBar";
            this.topBar.Size = new System.Drawing.Size(1400, 54);
            this.topBar.TabIndex = 0;
            // 
            // lblBrand
            // 
            this.lblBrand.AutoSize = true;
            this.lblBrand.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold);
            this.lblBrand.ForeColor = System.Drawing.Color.White;
            this.lblBrand.Location = new System.Drawing.Point(16, 12);
            this.lblBrand.Name = "lblBrand";
            this.lblBrand.Size = new System.Drawing.Size(130, 30);
            this.lblBrand.TabIndex = 0;
            this.lblBrand.Text = "AUTOGARA";
            // 
            // card
            // 
            this.card.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.card.BackColor = System.Drawing.Color.White;
            this.card.Controls.Add(this.stepHost);
            this.card.Controls.Add(this.footer);
            this.card.Controls.Add(this.cardHeader);
            this.card.Location = new System.Drawing.Point(280, 100);
            this.card.Name = "card";
            this.card.Size = new System.Drawing.Size(840, 560);
            this.card.TabIndex = 1;
            // 
            // stepHost
            // 
            this.stepHost.BackColor = System.Drawing.Color.White;
            this.stepHost.Controls.Add(this.step1);
            this.stepHost.Controls.Add(this.step2);
            this.stepHost.Controls.Add(this.step3);
            this.stepHost.Controls.Add(this.step4);
            this.stepHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stepHost.Location = new System.Drawing.Point(0, 82);
            this.stepHost.Name = "stepHost";
            this.stepHost.Padding = new System.Windows.Forms.Padding(24, 0, 24, 10);
            this.stepHost.Size = new System.Drawing.Size(840, 416);
            this.stepHost.TabIndex = 0;
            // 
            // step1
            // 
            this.step1.BackColor = System.Drawing.Color.White;
            this.step1.Controls.Add(this.lblStep1Header);
            this.step1.Controls.Add(this.lblFrom);
            this.step1.Controls.Add(this.cmbFrom);
            this.step1.Controls.Add(this.lblTo);
            this.step1.Controls.Add(this.cmbTo);
            this.step1.Controls.Add(this.lblDate);
            this.step1.Controls.Add(this.dtDate);
            this.step1.Controls.Add(this.chkStudent);
            this.step1.Controls.Add(this.chkPensioner);
            this.step1.Controls.Add(this.btnNext1);
            this.step1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.step1.Location = new System.Drawing.Point(24, 0);
            this.step1.Name = "step1";
            this.step1.Size = new System.Drawing.Size(792, 406);
            this.step1.TabIndex = 0;
            // 
            // lblStep1Header
            // 
            this.lblStep1Header.AutoSize = true;
            this.lblStep1Header.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold);
            this.lblStep1Header.Location = new System.Drawing.Point(0, 12);
            this.lblStep1Header.Name = "lblStep1Header";
            this.lblStep1Header.Size = new System.Drawing.Size(213, 25);
            this.lblStep1Header.TabIndex = 0;
            this.lblStep1Header.Text = "За къде ще пътувате?";
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.lblFrom.Location = new System.Drawing.Point(0, 62);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(69, 13);
            this.lblFrom.TabIndex = 1;
            this.lblFrom.Text = "Тръгвам от:";
            // 
            // cmbFrom
            // 
            this.cmbFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFrom.Location = new System.Drawing.Point(0, 84);
            this.cmbFrom.Name = "cmbFrom";
            this.cmbFrom.Size = new System.Drawing.Size(320, 21);
            this.cmbFrom.TabIndex = 2;
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.lblTo.Location = new System.Drawing.Point(360, 62);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(70, 13);
            this.lblTo.TabIndex = 3;
            this.lblTo.Text = "Пътувам до:";
            // 
            // cmbTo
            // 
            this.cmbTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTo.Location = new System.Drawing.Point(360, 84);
            this.cmbTo.Name = "cmbTo";
            this.cmbTo.Size = new System.Drawing.Size(320, 21);
            this.cmbTo.TabIndex = 4;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.lblDate.Location = new System.Drawing.Point(0, 144);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(101, 13);
            this.lblDate.TabIndex = 5;
            this.lblDate.Text = "Дата на тръгване:";
            // 
            // dtDate
            // 
            this.dtDate.Location = new System.Drawing.Point(0, 166);
            this.dtDate.Name = "dtDate";
            this.dtDate.Size = new System.Drawing.Size(320, 20);
            this.dtDate.TabIndex = 6;
            // 
            // chkStudent
            // 
            this.chkStudent.AutoSize = true;
            this.chkStudent.Location = new System.Drawing.Point(0, 242);
            this.chkStudent.Name = "chkStudent";
            this.chkStudent.Size = new System.Drawing.Size(95, 17);
            this.chkStudent.TabIndex = 7;
            this.chkStudent.Text = "Ученик (-30%)";
            // 
            // chkPensioner
            // 
            this.chkPensioner.AutoSize = true;
            this.chkPensioner.Location = new System.Drawing.Point(160, 242);
            this.chkPensioner.Name = "chkPensioner";
            this.chkPensioner.Size = new System.Drawing.Size(114, 17);
            this.chkPensioner.TabIndex = 8;
            this.chkPensioner.Text = "Пенсионер (-50%)";
            // 
            // btnNext1
            // 
            this.btnNext1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnNext1.FlatAppearance.BorderSize = 0;
            this.btnNext1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNext1.ForeColor = System.Drawing.Color.White;
            this.btnNext1.Location = new System.Drawing.Point(0, 318);
            this.btnNext1.Name = "btnNext1";
            this.btnNext1.Size = new System.Drawing.Size(220, 44);
            this.btnNext1.TabIndex = 9;
            this.btnNext1.Text = "Продължи";
            this.btnNext1.UseVisualStyleBackColor = false;
            // 
            // step2
            // 
            this.step2.BackColor = System.Drawing.Color.White;
            this.step2.Controls.Add(this.lblStep2Header);
            this.step2.Controls.Add(this.tripsList);
            this.step2.Controls.Add(this.btnBack2);
            this.step2.Controls.Add(this.btnNext2);
            this.step2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.step2.Location = new System.Drawing.Point(24, 0);
            this.step2.Name = "step2";
            this.step2.Size = new System.Drawing.Size(792, 406);
            this.step2.TabIndex = 1;
            this.step2.Visible = false;
            // 
            // lblStep2Header
            // 
            this.lblStep2Header.AutoSize = true;
            this.lblStep2Header.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold);
            this.lblStep2Header.Location = new System.Drawing.Point(0, 12);
            this.lblStep2Header.Name = "lblStep2Header";
            this.lblStep2Header.Size = new System.Drawing.Size(136, 25);
            this.lblStep2Header.TabIndex = 0;
            this.lblStep2Header.Text = "Изберете курс";
            // 
            // tripsList
            // 
            this.tripsList.AutoScroll = true;
            this.tripsList.BackColor = System.Drawing.Color.White;
            this.tripsList.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.tripsList.Location = new System.Drawing.Point(0, 50);
            this.tripsList.Name = "tripsList";
            this.tripsList.Size = new System.Drawing.Size(792, 248);
            this.tripsList.TabIndex = 1;
            this.tripsList.WrapContents = false;
            // 
            // btnBack2
            // 
            this.btnBack2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.btnBack2.FlatAppearance.BorderSize = 0;
            this.btnBack2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.btnBack2.Location = new System.Drawing.Point(0, 314);
            this.btnBack2.Name = "btnBack2";
            this.btnBack2.Size = new System.Drawing.Size(140, 44);
            this.btnBack2.TabIndex = 2;
            this.btnBack2.Text = "Назад";
            this.btnBack2.UseVisualStyleBackColor = false;
            // 
            // btnNext2
            // 
            this.btnNext2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnNext2.FlatAppearance.BorderSize = 0;
            this.btnNext2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNext2.ForeColor = System.Drawing.Color.White;
            this.btnNext2.Location = new System.Drawing.Point(160, 314);
            this.btnNext2.Name = "btnNext2";
            this.btnNext2.Size = new System.Drawing.Size(220, 44);
            this.btnNext2.TabIndex = 3;
            this.btnNext2.Text = "Напред";
            this.btnNext2.UseVisualStyleBackColor = false;
            // 
            // step3
            // 
            this.step3.BackColor = System.Drawing.Color.White;
            this.step3.Controls.Add(this.lblStep3Header);
            this.step3.Controls.Add(this.pnlSeatHost);
            this.step3.Controls.Add(this.lblChosen);
            this.step3.Controls.Add(this.lblRemaining);
            this.step3.Controls.Add(this.legend);
            this.step3.Controls.Add(this.btnBack3);
            this.step3.Controls.Add(this.btnNext3);
            this.step3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.step3.Location = new System.Drawing.Point(24, 0);
            this.step3.Name = "step3";
            this.step3.Size = new System.Drawing.Size(792, 406);
            this.step3.TabIndex = 2;
            this.step3.Visible = false;
            // 
            // lblStep3Header
            // 
            this.lblStep3Header.AutoSize = true;
            this.lblStep3Header.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold);
            this.lblStep3Header.Location = new System.Drawing.Point(0, 12);
            this.lblStep3Header.Name = "lblStep3Header";
            this.lblStep3Header.Size = new System.Drawing.Size(149, 25);
            this.lblStep3Header.TabIndex = 0;
            this.lblStep3Header.Text = "Изберете място";
            // 
            // pnlSeatHost
            // 
            this.pnlSeatHost.BackColor = System.Drawing.Color.White;
            this.pnlSeatHost.Location = new System.Drawing.Point(140, 20);
            this.pnlSeatHost.Name = "pnlSeatHost";
            this.pnlSeatHost.Size = new System.Drawing.Size(360, 270);
            this.pnlSeatHost.TabIndex = 1;
            // 
            // lblChosen
            // 
            this.lblChosen.AutoSize = true;
            this.lblChosen.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblChosen.Location = new System.Drawing.Point(560, 120);
            this.lblChosen.Name = "lblChosen";
            this.lblChosen.Size = new System.Drawing.Size(121, 19);
            this.lblChosen.TabIndex = 2;
            this.lblChosen.Text = "Избрани места: 0";
            // 
            // lblRemaining
            // 
            this.lblRemaining.AutoSize = true;
            this.lblRemaining.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblRemaining.Location = new System.Drawing.Point(560, 148);
            this.lblRemaining.Name = "lblRemaining";
            this.lblRemaining.Size = new System.Drawing.Size(74, 19);
            this.lblRemaining.TabIndex = 3;
            this.lblRemaining.Text = "Остават: 1";
            // 
            // legend
            // 
            this.legend.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.legend.Location = new System.Drawing.Point(535, 186);
            this.legend.Name = "legend";
            this.legend.Size = new System.Drawing.Size(190, 110);
            this.legend.TabIndex = 4;
            // 
            // btnBack3
            // 
            this.btnBack3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.btnBack3.FlatAppearance.BorderSize = 0;
            this.btnBack3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.btnBack3.Location = new System.Drawing.Point(0, 360);
            this.btnBack3.Name = "btnBack3";
            this.btnBack3.Size = new System.Drawing.Size(140, 44);
            this.btnBack3.TabIndex = 5;
            this.btnBack3.Text = "Назад";
            this.btnBack3.UseVisualStyleBackColor = false;
            // 
            // btnNext3
            // 
            this.btnNext3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnNext3.FlatAppearance.BorderSize = 0;
            this.btnNext3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNext3.ForeColor = System.Drawing.Color.White;
            this.btnNext3.Location = new System.Drawing.Point(160, 360);
            this.btnNext3.Name = "btnNext3";
            this.btnNext3.Size = new System.Drawing.Size(220, 44);
            this.btnNext3.TabIndex = 6;
            this.btnNext3.Text = "Продължи";
            this.btnNext3.UseVisualStyleBackColor = false;
            // 
            // step4
            // 
            this.step4.BackColor = System.Drawing.Color.White;
            this.step4.Controls.Add(this.lblStep4Header);
            this.step4.Controls.Add(this.lblSummary);
            this.step4.Controls.Add(this.lblFirstName);
            this.step4.Controls.Add(this.lblLastName);
            this.step4.Controls.Add(this.lblEmail);
            this.step4.Controls.Add(this.txtFirstName);
            this.step4.Controls.Add(this.txtLastName);
            this.step4.Controls.Add(this.txtEmail);
            this.step4.Controls.Add(this.btnBack4);
            this.step4.Controls.Add(this.btnPdf);
            this.step4.Controls.Add(this.btnPrint);
            this.step4.Controls.Add(this.btnBuy);
            this.step4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.step4.Location = new System.Drawing.Point(24, 0);
            this.step4.Name = "step4";
            this.step4.Size = new System.Drawing.Size(792, 406);
            this.step4.TabIndex = 3;
            this.step4.Visible = false;
            // 
            // lblStep4Header
            // 
            this.lblStep4Header.AutoSize = true;
            this.lblStep4Header.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold);
            this.lblStep4Header.Location = new System.Drawing.Point(0, 12);
            this.lblStep4Header.Name = "lblStep4Header";
            this.lblStep4Header.Size = new System.Drawing.Size(133, 25);
            this.lblStep4Header.TabIndex = 0;
            this.lblStep4Header.Text = "Вашият избор";
            // 
            // lblSummary
            // 
            this.lblSummary.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSummary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(55)))));
            this.lblSummary.Location = new System.Drawing.Point(0, 56);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Size = new System.Drawing.Size(740, 130);
            this.lblSummary.TabIndex = 1;
            this.lblSummary.Text = "—";
            // 
            // lblFirstName
            // 
            this.lblFirstName.AutoSize = true;
            this.lblFirstName.Location = new System.Drawing.Point(0, 200);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(29, 13);
            this.lblFirstName.TabIndex = 2;
            this.lblFirstName.Text = "Име";
            // 
            // lblLastName
            // 
            this.lblLastName.AutoSize = true;
            this.lblLastName.Location = new System.Drawing.Point(260, 200);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(55, 13);
            this.lblLastName.TabIndex = 3;
            this.lblLastName.Text = "Фамилия";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(520, 200);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(39, 13);
            this.lblEmail.TabIndex = 4;
            this.lblEmail.Text = "Имейл";
            // 
            // txtFirstName
            // 
            this.txtFirstName.Location = new System.Drawing.Point(0, 220);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(220, 20);
            this.txtFirstName.TabIndex = 5;
            // 
            // txtLastName
            // 
            this.txtLastName.Location = new System.Drawing.Point(260, 220);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(220, 20);
            this.txtLastName.TabIndex = 6;
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(520, 220);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(220, 20);
            this.txtEmail.TabIndex = 7;
            // 
            // btnBack4
            // 
            this.btnBack4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.btnBack4.FlatAppearance.BorderSize = 0;
            this.btnBack4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.btnBack4.Location = new System.Drawing.Point(0, 300);
            this.btnBack4.Name = "btnBack4";
            this.btnBack4.Size = new System.Drawing.Size(120, 44);
            this.btnBack4.TabIndex = 8;
            this.btnBack4.Text = "Назад";
            this.btnBack4.UseVisualStyleBackColor = false;
            // 
            // btnPdf
            // 
            this.btnPdf.BackColor = System.Drawing.Color.White;
            this.btnPdf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPdf.Location = new System.Drawing.Point(140, 300);
            this.btnPdf.Name = "btnPdf";
            this.btnPdf.Size = new System.Drawing.Size(140, 44);
            this.btnPdf.TabIndex = 9;
            this.btnPdf.Text = "Запази PDF";
            this.btnPdf.UseVisualStyleBackColor = false;
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.Color.White;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrint.Location = new System.Drawing.Point(300, 300);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(140, 44);
            this.btnPrint.TabIndex = 10;
            this.btnPrint.Text = "Принтирай";
            this.btnPrint.UseVisualStyleBackColor = false;
            // 
            // btnBuy
            // 
            this.btnBuy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnBuy.FlatAppearance.BorderSize = 0;
            this.btnBuy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBuy.ForeColor = System.Drawing.Color.White;
            this.btnBuy.Location = new System.Drawing.Point(460, 300);
            this.btnBuy.Name = "btnBuy";
            this.btnBuy.Size = new System.Drawing.Size(220, 44);
            this.btnBuy.TabIndex = 11;
            this.btnBuy.Text = "Купи билет";
            this.btnBuy.UseVisualStyleBackColor = false;
            // 
            // footer
            // 
            this.footer.BackColor = System.Drawing.Color.White;
            this.footer.Controls.Add(this.lblPrice);
            this.footer.Controls.Add(this.footerLine);
            this.footer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.footer.Location = new System.Drawing.Point(0, 498);
            this.footer.Name = "footer";
            this.footer.Size = new System.Drawing.Size(840, 62);
            this.footer.TabIndex = 1;
            // 
            // lblPrice
            // 
            this.lblPrice.AutoSize = true;
            this.lblPrice.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.lblPrice.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.lblPrice.Location = new System.Drawing.Point(24, 18);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(113, 21);
            this.lblPrice.TabIndex = 0;
            this.lblPrice.Text = "Цена: 0.00 лв.";
            // 
            // footerLine
            // 
            this.footerLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.footerLine.Dock = System.Windows.Forms.DockStyle.Top;
            this.footerLine.Location = new System.Drawing.Point(0, 0);
            this.footerLine.Name = "footerLine";
            this.footerLine.Size = new System.Drawing.Size(840, 1);
            this.footerLine.TabIndex = 1;
            // 
            // cardHeader
            // 
            this.cardHeader.BackColor = System.Drawing.Color.White;
            this.cardHeader.Controls.Add(this.lblTitle);
            this.cardHeader.Controls.Add(this.lblSubtitle);
            this.cardHeader.Controls.Add(this.progressTrack);
            this.cardHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.cardHeader.Location = new System.Drawing.Point(0, 0);
            this.cardHeader.Name = "cardHeader";
            this.cardHeader.Size = new System.Drawing.Size(840, 82);
            this.cardHeader.TabIndex = 2;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(24, 16);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(281, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Онлайн автобусни билети";
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
            this.lblSubtitle.Location = new System.Drawing.Point(24, 43);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(212, 13);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "Купи своя билет бързо, лесно и сигурно";
            // 
            // progressTrack
            // 
            this.progressTrack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.progressTrack.Controls.Add(this.progressFill);
            this.progressTrack.Location = new System.Drawing.Point(24, 68);
            this.progressTrack.Name = "progressTrack";
            this.progressTrack.Size = new System.Drawing.Size(790, 4);
            this.progressTrack.TabIndex = 2;
            // 
            // progressFill
            // 
            this.progressFill.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.progressFill.Location = new System.Drawing.Point(0, 0);
            this.progressFill.Name = "progressFill";
            this.progressFill.Size = new System.Drawing.Size(198, 4);
            this.progressFill.TabIndex = 0;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(1400, 760);
            this.Controls.Add(this.hero);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Автогара – Онлайн билети";
            this.hero.ResumeLayout(false);
            this.topBar.ResumeLayout(false);
            this.topBar.PerformLayout();
            this.card.ResumeLayout(false);
            this.stepHost.ResumeLayout(false);
            this.step1.ResumeLayout(false);
            this.step1.PerformLayout();
            this.step2.ResumeLayout(false);
            this.step2.PerformLayout();
            this.step3.ResumeLayout(false);
            this.step3.PerformLayout();
            this.step4.ResumeLayout(false);
            this.step4.PerformLayout();
            this.footer.ResumeLayout(false);
            this.footer.PerformLayout();
            this.cardHeader.ResumeLayout(false);
            this.cardHeader.PerformLayout();
            this.progressTrack.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}