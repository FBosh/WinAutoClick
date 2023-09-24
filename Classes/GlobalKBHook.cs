using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WinFormsApp1.Classes
{
    public class GlobalKBHook : BaseOperations
    {
        private IntPtr _hookId = IntPtr.Zero;

        public enum KBHookFlags
        {
            WH_KEYBOARD_LL = 13,
            WM_KEYDOWN = 0x0100,
            WM_KEYUP = 0x101
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        private IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using var p = Process.GetCurrentProcess();
            using var m = p.MainModule;

            return SetWindowsHookEx((int)KBHookFlags.WH_KEYBOARD_LL, proc, GetModuleHandle(m.ModuleName), 0);
        }

        public void Unhook() => UnhookWindowsHookEx(_hookId);

        public void Hook(LowLevelKeyboardProc proc) => _hookId = SetHook(proc);

        public delegate void LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
    }
}
