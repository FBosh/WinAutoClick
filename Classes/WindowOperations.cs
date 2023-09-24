using System.Runtime.InteropServices;

namespace WinFormsApp1.Classes
{
    public class WindowOperations : BaseOperations
    {
        public enum WindowFlags
        {
            SWP_NOMOVE = 0x2,
            SWP_NOSIZE = 1,
            SWP_NOZORDER = 0x4,
            SWP_SHOWWINDOW = 0x0040
        }

        public enum WindowLocation
        {
            TOP_LEFT,
            TOP_MID,
            TOP_RIGHT,
            MID,
            BOTTOM_LEFT,
            BOTTOM_MID,
            BOTTOM_RIGHT
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, ref Rect lpRect);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        public static IntPtr SetWindowPosition(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags)
        {
            return SetWindowPos(hWnd, hWndInsertAfter, x, y, cx, cy, wFlags);
        }

        public static bool GetWindowRectangle(IntPtr hWnd, ref Rect lpRect) => GetWindowRect(hWnd, ref lpRect);

        public static void SetWindowLocation(IntPtr hWnd, WindowLocation wl)
        {
            var r = new Rect();
            GetWindowRect(hWnd, ref r);

            var currentScreen = Screen.FromHandle(hWnd);
            int x, y;

            //var offset = 8;
            var offset = 2;

            var taskbarRect = new Rect();
            GetWindowRect(FindWindow("Shell_traywnd", ""), ref taskbarRect);

            var taskbarHeight = Math.Abs(taskbarRect.Top - taskbarRect.Bottom);

            x = wl.ToString().Contains("LEFT") ? -offset + currentScreen.Bounds.Left
                : wl.ToString().Contains("RIGHT") ? offset + currentScreen.Bounds.Width - (r.Right - r.Left) + currentScreen.Bounds.Left
                : (currentScreen.Bounds.Width - (r.Right - r.Left)) / 2 + currentScreen.Bounds.Left;

            y = wl.ToString().Contains("TOP") ? currentScreen.Bounds.Top
                : wl.ToString().Contains("BOTTOM") ? currentScreen.Bounds.Height - (r.Bottom - r.Top) + currentScreen.Bounds.Top - taskbarHeight
                : (currentScreen.Bounds.Height - (r.Bottom - r.Top)) / 2 + currentScreen.Bounds.Top;

            SetWindowPos(
                hWnd,
                0,
                x,
                y,
                r.Right - r.Left,   //taskmgr:752
                r.Bottom - r.Top,   //taskmgr:626
                (int)WindowFlags.SWP_SHOWWINDOW
            );
        }

        public struct Rect
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }

            public override string ToString()
            {
                return $"Left: {Left}, Top: {Top}, Right: {Right}, Bottom: {Bottom}";
            }
        }
    }
}
