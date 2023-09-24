namespace WinFormsApp1
{
    partial class Form1
    {
        private Label lbInterval, lbKB, lbInfo, lbStartupTime, lbLatestActionTime;
        private Button btnStart, btnLocate;
        private TextBox tbInterval;
        private ComboBox cbbKB;
        private CheckBox ckbDisableDelay, ckbDisableMouseClick;

        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);

            dispose();
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        //private void InitializeComponent()
        //{
        //    this.lbInterval = new System.Windows.Forms.Label();
        //    this.lbKB = new System.Windows.Forms.Label();
        //    this.lbInfo = new System.Windows.Forms.Label();
        //    this.btnStart = new System.Windows.Forms.Button();
        //    this.tbInterval = new System.Windows.Forms.TextBox();
        //    this.SuspendLayout();
        //    // 
        //    // lbInterval
        //    // 
        //    this.lbInterval.Location = new System.Drawing.Point(0, 0);
        //    this.lbInterval.Name = "lbInterval";
        //    this.lbInterval.Size = new System.Drawing.Size(100, 23);
        //    this.lbInterval.TabIndex = 0;
        //    // 
        //    // lbKB
        //    // 
        //    this.lbKB.Location = new System.Drawing.Point(0, 0);
        //    this.lbKB.Name = "lbKB";
        //    this.lbKB.Size = new System.Drawing.Size(100, 23);
        //    this.lbKB.TabIndex = 1;
        //    // 
        //    // lbInfo
        //    // 
        //    this.lbInfo.Location = new System.Drawing.Point(0, 0);
        //    this.lbInfo.Name = "lbInfo";
        //    this.lbInfo.Size = new System.Drawing.Size(100, 23);
        //    this.lbInfo.TabIndex = 2;
        //    // 
        //    // btnStart
        //    // 
        //    this.btnStart.Location = new System.Drawing.Point(0, 0);
        //    this.btnStart.Name = "btnStart";
        //    this.btnStart.Size = new System.Drawing.Size(75, 23);
        //    this.btnStart.TabIndex = 3;
        //    this.btnStart.Click += new System.EventHandler(this.btn1_click);
        //    // 
        //    // tbInterval
        //    // 
        //    this.tbInterval.Location = new System.Drawing.Point(0, 0);
        //    this.tbInterval.Name = "tbInterval";
        //    this.tbInterval.Size = new System.Drawing.Size(100, 23);
        //    this.tbInterval.TabIndex = 4;
        //    this.tbInterval.GotFocus += new System.EventHandler(this.tbInterval_onFocus);
        //    this.tbInterval.LostFocus += new System.EventHandler(this.tbInterval_offFocus);
        //    // 
        //    // Form1
        //    // 
        //    this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        //    this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        //    this.ClientSize = new System.Drawing.Size(800, 450);
        //    this.Controls.Add(this.lbInterval);
        //    this.Controls.Add(this.lbKB);
        //    this.Controls.Add(this.lbInfo);
        //    this.Controls.Add(this.btnStart);
        //    this.Controls.Add(this.tbInterval);
        //    this.MaximizeBox = false;
        //    this.Name = "Form1";
        //    this.Text = "Form1";
        //    this.ResumeLayout(false);
        //    this.PerformLayout();

        //}

        #endregion

        private void initUI()
        {
            components = new System.ComponentModel.Container();
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(640, 360);
            Text = "Bosh";

            Name = "Form1";
            MaximizeBox = false;

            KeyPreview = true;
            //KeyPress += new KeyPressEventHandler(Form1_KeyPress);
            KeyUp += new KeyEventHandler(Form1_KeyUp);
            Click += new EventHandler(Form1_Click);

            FormBorderStyle = FormBorderStyle.FixedSingle;

            SuspendLayout();

            lbInterval = new Label()
            {
                AutoSize = true,
                Location = new Point(30, 30),
                Name = "lbInterval",
                //Size = new Size(42, 15),
                //TabIndex = 0,
                Text = "Interval(Millisecond)"
            };

            lbKB = new Label()
            {
                AutoSize = true,
                Location = new Point(lbInterval.Location.X, lbInterval.Bounds.Bottom + DefaultMargin.All),
                Name = "lbKB",
                //Size = new Size(99, 99),
                //TabIndex = 0,
                Text = "Keyboard key"
            };

            lbInfo = new Label()
            {
                AutoSize = true,
                Location = new Point(lbKB.Location.X, lbKB.Bounds.Bottom + DefaultMargin.All),
                Name = "lbInfo",
                Text = $"Start after {delay / 1000} seconds after click start button"
            };

            btnStart = new Button()
            {
                Name = "btnStart",
                Text = "Begin",
                AutoSize = true,
                //Location = new Point(label1.Bounds.Right, label1.Location.Y)
            };

            tbInterval = new TextBox()
            {
                Name = "tbInterval",
                Text = TB_INTERVAL_HINT,
                ForeColor = Color.Gray,
                Size = new Size(160, DefaultSize.Height),
                MaxLength = 5
            };

            cbbKB = new ComboBox()
            {
                Name = "cbbKB",
                AutoSize = true,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            lbStartupTime = new Label()
            {
                Name = "lbStartupTime",
                Text = $"Start: {DateTime.Now}",
                AutoSize = true,
            };

            lbLatestActionTime = new Label()
            {
                Name = "lbLatestActionTime",
                AutoSize = true
            };

            ckbDisableDelay = new CheckBox()
            {
                Name = "ckbDisableDelay",
                Text = "Disable delay"
            };

            btnLocate = new()
            {
                Name = "btnLocate",
                Text = "Locate windows"
            };

            ckbDisableMouseClick = new()
            {
                Name = "ckbDisableMouseClick",
                Text = "Disable mouse click",
                AutoSize = true
            };

            Controls.AddRange(new Control[]
                {
                    lbInterval, lbKB, lbInfo, btnStart, tbInterval,
                    cbbKB, lbStartupTime, lbLatestActionTime, ckbDisableDelay, btnLocate ,ckbDisableMouseClick
                }
            );

            btnStart.Click += new EventHandler(btnStart_click);
            btnStart.Location = new Point(lbInfo.Location.X, lbInfo.Bounds.Bottom + DefaultMargin.All);

            tbInterval.Location = new Point(lbInterval.Bounds.Right, getAverageY(tbInterval, lbInterval));
            tbInterval.GotFocus += new EventHandler(tbInterval_onFocus);
            tbInterval.LostFocus += new EventHandler(tbInterval_offFocus);

            cbbKB.Location = new Point(lbKB.Bounds.Right, getAverageY(cbbKB, lbKB));
            cbbKB.Items.AddRange(arrKeys);
            cbbKB.SelectedIndex = 0;

            lbStartupTime.Location = new Point(btnStart.Location.X, btnStart.Bounds.Bottom + DefaultMargin.All);

            lbLatestActionTime.Location = new Point(lbStartupTime.Location.X, lbStartupTime.Bounds.Bottom + DefaultMargin.All);

            ckbDisableDelay.Location = new Point(tbInterval.Bounds.Right + DefaultMargin.All, getAverageY(ckbDisableDelay, tbInterval));
            ckbDisableDelay.CheckStateChanged += new EventHandler(ckbDisableDelay_CheckStateChanged);

            btnLocate.Click += new EventHandler(btnLocate_Click);
            btnLocate.Location = new Point(btnStart.Bounds.Right + DefaultMargin.All, btnStart.Location.Y);

            ckbDisableMouseClick.Location = new Point(ckbDisableDelay.Location.X, getAverageY(ckbDisableMouseClick, lbKB));

            //AutoScaleDimensions = new SizeF(7F, 15F);
            ResumeLayout(false);
            PerformLayout();
        }

        private int getAverageY(Control cSelf, Control cOpp) => cOpp.Location.Y - cSelf.Height / 2 + cOpp.Height / 2;
    }
}
