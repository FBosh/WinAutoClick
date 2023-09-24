using WinFormsApp1.Classes;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private readonly Thread t1;
        private readonly string TB_INTERVAL_HINT = $"Mininum: {MIN_INTERVAL}";
        private bool isLoop;
        private readonly object[] arrKeys;
        private object keyKB = string.Empty;

        private int count;
        private const int DEFAULT_INTERVAL = 1000;

        private bool isHoldingCTRL;

        private readonly GlobalKeyboardHook _globalKeyboardHook;

        private bool isFreshStart = true;

        private int countTotal;

        private const int delay = 3000;
        private int interval;

        private const int MIN_INTERVAL = 250;

        public Form1()
        {
            //InitializeComponent();

            arrKeys = new[] { (object)"None", Keys.F5, Keys.F6, Keys.F7, Keys.F8, Keys.F9, Keys.F10, Keys.F11, Keys.F12 };

            initUI();

            setup();

            _globalKeyboardHook = new();
            _globalKeyboardHook.KeyboardPressed += OnKeyPressed;

            t1 = new Thread(() => Run()) { IsBackground = true, Priority = ThreadPriority.Highest };
        }

        private void btnStart_click(object sender, EventArgs e)
        {
            isLoop = !isLoop;
            btnStart.Text = isLoop ? "Stop" : "Go";

            ckbDisableDelay.Enabled = !isLoop;
            ckbDisableMouseClick.Enabled = !isLoop;
            tbInterval.Enabled = !isLoop;
            cbbKB.Enabled = !isLoop;

            if (isLoop)
            {
                setup();
                if ($"{t1.ThreadState}".Contains($"{ThreadState.Unstarted}")) t1.Start();

                btnStart.Enabled = false;
            }
            else
            {
                isFreshStart = true;

                countTotal += count;
                lbStartupTime.Text = $"{lbStartupTime.Text[..(lbStartupTime.Text.Contains("Total") ? lbStartupTime.Text.IndexOf(',') : lbStartupTime.Text.Length)]}"
                    + $", Total: {countTotal}";
                count = 0;
            }
        }

        private void tbInterval_onFocus(object sender, EventArgs e)
        {
            if (tbInterval.Text != TB_INTERVAL_HINT) return;

            tbInterval.Text = string.Empty;
            tbInterval.ForeColor = Color.Black;
        }

        private void tbInterval_offFocus(object sender, EventArgs e)
        {
            if (tbInterval.Text.Length > 0) return;

            tbInterval.Text = TB_INTERVAL_HINT;
            tbInterval.ForeColor = Color.Gray;
        }

        private static void println(string msg) => System.Diagnostics.Debug.WriteLine($"{DateTime.Now}: {msg}");

        private async void Run()
        {
            var holding = 100;

            while (true)
            {
                if (!isLoop)
                {
                    Thread.Sleep(100);

                    continue;
                }

                if (isFreshStart)
                {
                    await Task.Delay(delay);
                    isFreshStart = false;

                    btnStart.Invoke(() => { btnStart.Enabled = true; });

                    continue;
                }

                if (keyKB is Keys) KeyboardOperations.Press_KB((Keys)keyKB, holding);

                PerformMouseLeftClick(holding);

                lbLatestActionTime.Invoke(() =>
                {
                    lbLatestActionTime.Text = $"Run {++count} at: {DateTime.Now}.{DateTime.Now.Millisecond}, Total: {count + countTotal}\n{lbLatestActionTime.Text}";

                    if (lbLatestActionTime.Text.Count(c => c == '\n') >= 10)
                    {
                        lbLatestActionTime.Text = lbLatestActionTime.Text[..lbLatestActionTime.Text.LastIndexOf('\n')];
                    }
                });

                Thread.Sleep(ckbDisableDelay.CheckState != CheckState.Checked ? interval - holding : 100);
            }
        }

        private void setup()
        {
            keyKB = arrKeys[cbbKB.SelectedIndex];
            interval = int.TryParse(tbInterval.Text, out _) && int.Parse(tbInterval.Text) > MIN_INTERVAL ? int.Parse(tbInterval.Text) : DEFAULT_INTERVAL;
        }

        //private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    var hasControl = (ModifierKeys & Keys.Control) == Keys.Control;
        //    println($"???= {hasControl && e.KeyChar == (char)Keys.S}");
        //}

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.Control | Keys.S)) btnStart.PerformClick();
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            //Classes.KeyboardOperations.Press_KB(this, Classes.KeyboardOperations.KBFlags.VOLUME_MUTE);

            foreach (var p in System.Diagnostics.Process.GetProcesses())
            {
                if (p.ProcessName == "Taskmgr")
                {
                    WindowOperations.SetWindowLocation(p.MainWindowHandle, WindowOperations.WindowLocation.MID);

                    break;
                }
            }
        }

        private void dispose()
        {
            println("dispose()");

            _globalKeyboardHook.Dispose();
        }

        private void OnKeyPressed(object sender, GlobalKeyboardHookEventArgs e)
        {
            var key = (Keys)e.KeyboardData.VirtualCode;

            if (key == Keys.LControlKey || key == Keys.RControlKey)
            {
                if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyDown) isHoldingCTRL = true;
                else if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyUp) isHoldingCTRL = false;
            }

            if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyUp)
            {
                if (isHoldingCTRL && key == Keys.Q) btnStart.PerformClick();

                if (isHoldingCTRL && key == Keys.W && ckbDisableMouseClick.Enabled)
                {
                    ckbDisableMouseClick.CheckState = ckbDisableMouseClick.CheckState == CheckState.Checked ? CheckState.Unchecked : CheckState.Checked;
                }
            }
        }

        private void ckbDisableDelay_CheckStateChanged(object sender, EventArgs e)
        {
            tbInterval.Enabled = ckbDisableDelay.CheckState != CheckState.Checked;
            tbInterval.Text = ckbDisableDelay.CheckState != CheckState.Checked ? TB_INTERVAL_HINT : string.Empty;
        }

        private void btnLocate_Click(object sender, EventArgs e)
        {
            var index = 0;

            WindowOperations.WindowLocation getNewLocation() => index switch
            {
                0 => WindowOperations.WindowLocation.MID,
                1 => WindowOperations.WindowLocation.TOP_LEFT,
                2 => WindowOperations.WindowLocation.TOP_RIGHT,
                3 => WindowOperations.WindowLocation.BOTTOM_LEFT,
                4 => WindowOperations.WindowLocation.BOTTOM_RIGHT,
                _ => WindowOperations.WindowLocation.TOP_MID
            };

            foreach (var p in System.Diagnostics.Process.GetProcesses())
            {
                if (p.ProcessName == "Fairyland")
                {
                    WindowOperations.SetWindowLocation(p.MainWindowHandle, getNewLocation());
                    index++;
                }
            }
        }

        private void PerformMouseLeftClick(int holding)
        {
            if (ckbDisableMouseClick.CheckState == CheckState.Checked) return;

            MouseOperations.ClickLeft(ckbDisableDelay.CheckState != CheckState.Checked ? holding : 10);
        }
    }
}
