using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public static class BgDialog
    {
        public static void ShowInfo(string text, string title)
        {
            using (BgMessageForm form = new BgMessageForm(text, title, false))
            {
                form.ShowDialog();
            }
        }

        public static void ShowWarning(string text, string title)
        {
            using (BgMessageForm form = new BgMessageForm(text, title, false))
            {
                form.ShowDialog();
            }
        }

        public static void ShowError(string text, string title)
        {
            using (BgMessageForm form = new BgMessageForm(text, title, false))
            {
                form.ShowDialog();
            }
        }

        public static bool ShowQuestion(string text, string title)
        {
            using (BgMessageForm form = new BgMessageForm(text, title, true))
            {
                return form.ShowDialog() == DialogResult.Yes;
            }
        }
    }

    public sealed class BgMessageForm : Form
    {
        public BgMessageForm(string text, string title, bool isQuestion)
        {
            Text = title;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterScreen;
            MinimizeBox = false;
            MaximizeBox = false;
            ShowInTaskbar = false;
            BackColor = Color.White;
            Font = new Font("Segoe UI", 9F);
            AutoScaleMode = AutoScaleMode.None;

            Label lblTitle = new Label();
            lblTitle.AutoSize = false;
            lblTitle.Text = title;
            lblTitle.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(35, 35, 35);
            lblTitle.Location = new Point(22, 18);
            lblTitle.Size = new Size(540, 28);
            Controls.Add(lblTitle);

            Label lblText = new Label();
            lblText.AutoSize = false;
            lblText.Text = text;
            lblText.ForeColor = Color.FromArgb(70, 70, 70);
            lblText.Location = new Point(22, 56);
            lblText.Size = new Size(540, 150);
            lblText.MaximumSize = new Size(540, 0);
            Controls.Add(lblText);

            Size measured = TextRenderer.MeasureText(
                text,
                lblText.Font,
                new Size(540, 0),
                TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl);

            int textHeight = measured.Height + 14;
            if (textHeight < 90) textHeight = 90;

            lblText.Height = textHeight;

            if (isQuestion)
            {
                ClientSize = new Size(590, textHeight + 120);

                Button btnYes = CreateMainButton("Да");
                btnYes.Location = new Point(342, textHeight + 72);
                btnYes.DialogResult = DialogResult.Yes;
                Controls.Add(btnYes);

                Button btnNo = CreateSecondaryButton("Не");
                btnNo.Location = new Point(460, textHeight + 72);
                btnNo.DialogResult = DialogResult.No;
                Controls.Add(btnNo);

                AcceptButton = btnYes;
                CancelButton = btnNo;
            }
            else
            {
                ClientSize = new Size(590, textHeight + 120);

                Button btnOk = CreateMainButton("Добре");
                btnOk.Location = new Point(460, textHeight + 72);
                btnOk.DialogResult = DialogResult.OK;
                Controls.Add(btnOk);

                AcceptButton = btnOk;
                CancelButton = btnOk;
            }
        }

        private Button CreateMainButton(string text)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Size = new Size(110, 36);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = Color.FromArgb(185, 0, 0);
            btn.ForeColor = Color.White;
            return btn;
        }

        private Button CreateSecondaryButton(string text)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Size = new Size(110, 36);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btn.BackColor = Color.White;
            btn.ForeColor = Color.FromArgb(50, 50, 50);
            return btn;
        }
    }
}