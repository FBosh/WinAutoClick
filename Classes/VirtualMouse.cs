using System.Runtime.InteropServices;

namespace WinFormsApp1.Classes
{
    public class MouseOperations : BaseOperations
    {
        [Flags]
        private enum MouseEventFlags
        {
            LeftDown = 0x00000002,
            LeftUp = 0x00000004,
            MiddleDown = 0x00000020,
            MiddleUp = 0x00000040,
            Move = 0x00000001,
            Absolute = 0x00008000,
            RightDown = 0x00000008,
            RightUp = 0x00000010
        }

        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(out MousePoint lpMousePoint);

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        private static void SetCursorPosition(MousePoint point) => SetCursorPos(point.X, point.Y);

        private static MousePoint GetCursorPosition()
        {
            MousePoint currentMousePoint;
            var gotPoint = GetCursorPos(out currentMousePoint);
            if (!gotPoint) currentMousePoint = new MousePoint(0, 0);

            return currentMousePoint;
        }

        private static void MouseEvent(MouseEventFlags value)
        {
            MousePoint position = GetCursorPosition();

            mouse_event((int)value, position.X, position.Y, 0, 0);
        }

        public static void ClickLeft(int holdingMillis = 0)
        {
            MouseEvent(MouseEventFlags.LeftDown);
            Thread.Sleep(holdingMillis);
            MouseEvent(MouseEventFlags.LeftUp);
        }

        public static void ClickRight(int holdingMillis = 0)
        {
            MouseEvent(MouseEventFlags.RightDown);
            Thread.Sleep(holdingMillis);
            MouseEvent(MouseEventFlags.RightUp);
        }

        public static void SetCursorPosition(int x, int y) => SetCursorPos(x, y);

        [StructLayout(LayoutKind.Sequential)]
        private struct MousePoint
        {
            public int X;
            public int Y;

            public MousePoint(int x, int y)
            {
                X = x;
                Y = y;
            }
        }
    }
}
