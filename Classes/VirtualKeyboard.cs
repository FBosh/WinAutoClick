using System.Runtime.InteropServices;

namespace WinFormsApp1.Classes
{
    public class KeyboardOperations : BaseOperations
    {
        public enum KBFlags
        {
            WM_APPCOMMAND = 0x319,

            VOLUME_MUTE = 0x80000,
            VOLUME_DOWN = 0x90000,
            VOLUME_UP = 0xA0000,
            MEDIA_PLAY_PAUSE = 0xE0000,

            WH_KEYBOARD_LL = 13,
            WM_KEYDOWN = 0x0100,
            WM_KEYUP = 0x101,

            KEYEVENTF_EXTENDEDKEY = 0x0001,
            KEYEVENTF_KEYUP = 0x0002
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessageW(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVK, byte bScan, uint dwFlags, uint dwExtraInfo);

        public static void Press_KB(Control ctrl, KBFlags key)
        {
            SendMessageW(ctrl.Handle, (int)KBFlags.WM_APPCOMMAND, ctrl.Handle, (IntPtr)key);
        }

        public static void Press_KB(Keys key, int holding)
        {
            keybd_event((byte)key, 0, (uint)KBFlags.KEYEVENTF_EXTENDEDKEY | 0, 0);
            Thread.Sleep(holding);
            keybd_event((byte)key, 0, (uint)(KBFlags.KEYEVENTF_EXTENDEDKEY | KBFlags.KEYEVENTF_KEYUP), 0);
        }
    }
}
