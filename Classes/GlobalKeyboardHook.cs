﻿using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WinFormsApp1.Classes
{
    /// <summary>
    /// Modified from https://stackoverflow.com/questions/604410/global-keyboard-capture-in-c-sharp-application
    /// </summary>
    public class GlobalKeyboardHook : IDisposable
    {
        public event EventHandler<GlobalKeyboardHookEventArgs> KeyboardPressed;

        private IntPtr _windowsHookHandle;
        private IntPtr _user32LibraryHandle;
        private HookProc _hookProc;

        public const int WH_KEYBOARD_LL = 13;
        public const int VkSnapshot = 0x2c;
        public const int KfAltdown = 0x2000;
        public const int LlkhfAltdown = KfAltdown >> 8;

        public enum KeyboardState
        {
            KeyDown = 0x0100,
            KeyUp = 0x0101,
            SysKeyDown = 0x0104,
            SysKeyUp = 0x0105
        }

        public GlobalKeyboardHook()
        {
            _windowsHookHandle = IntPtr.Zero;
            _user32LibraryHandle = IntPtr.Zero;
            _hookProc = LowLevelKeyboardProc;

            _user32LibraryHandle = LoadLibrary("User32");
            if (_user32LibraryHandle == IntPtr.Zero)
            {
                int errorCode = Marshal.GetLastWin32Error();
                throw new Win32Exception(
                    errorCode,
                    $"Failed to load library 'User32.dll'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}"
                );
            }

            _windowsHookHandle = SetWindowsHookEx(WH_KEYBOARD_LL, _hookProc, _user32LibraryHandle, 0);
            if (_windowsHookHandle == IntPtr.Zero)
            {
                int errorCode = Marshal.GetLastWin32Error();
                throw new Win32Exception(
                    errorCode,
                    $"Failed to adjust keyboard hooks for '{Process.GetCurrentProcess().ProcessName}'." +
                    $" Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}."
                );
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool FreeLibrary(IntPtr hModule);

        [DllImport("USER32", SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, int dwThreadId);

        [DllImport("USER32", SetLastError = true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hHook);

        [DllImport("USER32", SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hHook, int code, IntPtr wParam, IntPtr lParam);

        public IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            bool fEatKeyStroke = false;
            var wParamTyped = wParam.ToInt32();

            if (Enum.IsDefined(typeof(KeyboardState), wParamTyped))
            {
                object o = Marshal.PtrToStructure(lParam, typeof(LowLevelKeyboardInputEvent));
                LowLevelKeyboardInputEvent p = (LowLevelKeyboardInputEvent)o;

                var eventArguments = new GlobalKeyboardHookEventArgs(p, (KeyboardState)wParamTyped);
                EventHandler<GlobalKeyboardHookEventArgs> handler = KeyboardPressed;
                handler?.Invoke(this, eventArguments);

                fEatKeyStroke = eventArguments.Handled;
            }

            return fEatKeyStroke ? (IntPtr)1 : CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_windowsHookHandle != IntPtr.Zero)
                {
                    if (!UnhookWindowsHookEx(_windowsHookHandle))
                    {
                        int errorCode = Marshal.GetLastWin32Error();
                        throw new Win32Exception(
                            errorCode,
                            $"Failed to remove keyboard hooks for '{Process.GetCurrentProcess().ProcessName}'." +
                            $" Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}."
                        );
                    }
                    _windowsHookHandle = IntPtr.Zero;

                    _hookProc -= LowLevelKeyboardProc;
                }
            }

            if (_user32LibraryHandle != IntPtr.Zero)
            {
                if (!FreeLibrary(_user32LibraryHandle))
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    throw new Win32Exception(
                        errorCode,
                        $"Failed to unload library 'User32.dll'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}."
                    );
                }
                _user32LibraryHandle = IntPtr.Zero;
            }
        }

        ~GlobalKeyboardHook()
        {
            Dispose(false);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct LowLevelKeyboardInputEvent
        {
            public int VirtualCode;
            public int HardwareScanCode;
            public int Flags;
            public int TimeStamp;
            public IntPtr AdditionalInformation;
        }
    }

    public class GlobalKeyboardHookEventArgs : HandledEventArgs
    {
        public GlobalKeyboardHook.KeyboardState KeyboardState { get; private set; }
        public GlobalKeyboardHook.LowLevelKeyboardInputEvent KeyboardData { get; private set; }

        public GlobalKeyboardHookEventArgs(GlobalKeyboardHook.LowLevelKeyboardInputEvent keyboardData, GlobalKeyboardHook.KeyboardState keyboardState)
        {
            KeyboardData = keyboardData;
            KeyboardState = keyboardState;
        }
    }
}
