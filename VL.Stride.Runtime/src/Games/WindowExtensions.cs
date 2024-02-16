using SharpDX.MediaFoundation;
using Stride.Audio;
using Stride.Core.Yaml.Tokens;
using Stride.Games;
using Stride.Graphics;
using System;
using System.Buffers.Text;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Security;
using static Stride.Graphics.Buffer;

namespace VL.Stride.Games
{
    public static class WindowExtensions
    {
        public static void BringToFront(this GameWindow window)
        {
            try
            { 
                var field = Type.GetType("Stride.Games.GameWindowSDL, Stride.Games")?.GetField("window", BindingFlags.Instance | BindingFlags.NonPublic);
                if (field != null)
                {
                    var sdlWindow = field.GetValue(window) as GameFormSDL;
                    if (sdlWindow != null)
                        sdlWindow.BringToFront();
                    return;
                }
            }
            catch { }

            try
            {
                var field = Type.GetType("Stride.Games.GameWindowWinforms, Stride.Games")?.GetField("form", BindingFlags.Instance | BindingFlags.NonPublic);
                if (field != null)
                {
                    var winformsWindow = (dynamic)field.GetValue(window);
                    if (winformsWindow != null)
                        winformsWindow.Activate();
                    return;
                }
            }
            catch { }
        }
    }

    [SuppressUnmanagedCodeSecurity]
    public static class WindowExtensionsNative
    {
        // https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-showwindow
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, ShowWindowCommand Cmd);

        public static bool ShowWindow(GameWindow gameWindow, ShowWindowCommand Cmd)
        {
            var hWnd = gameWindow.NativeWindow.Handle;

            return ShowWindow(hWnd, Cmd);
        }


        //https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setactivewindow
        [DllImport("user32.dll")]
        private static extern nint SetActiveWindow(IntPtr hWnd);

        public static bool SetActiveWindow(GameWindow gameWindow)
        {
            var hWnd = gameWindow.NativeWindow.Handle;

            return SetActiveWindow(hWnd) != IntPtr.Zero;
        }


        // https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setforegroundwindow
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        public static bool SetForegroundWindow(GameWindow gameWindow)
        {
            var hWnd = gameWindow.NativeWindow.Handle;

            return SetForegroundWindow(hWnd);
        }


        // https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-bringwindowtotop
        [DllImport("user32.dll")]
        private static extern bool BringWindowToTop(IntPtr hWnd);

        public static bool BringWindowToTop(GameWindow gameWindow)
        {
            var hWnd = gameWindow.NativeWindow.Handle;

            return BringWindowToTop(hWnd);
        }
    }

    public enum ShowWindowCommand
    {
        /// <summary>Hides the window and activates another window.</summary>
        SW_HIDE = 0,

        /// <summary>
        /// Activates and displays a window. If the window is minimized or maximized, Windows restores it to its original size and position.
        /// An application should specify this flag when displaying the window for the first time.
        /// </summary>
        SW_SHOWNORMAL = 1,

        /// <summary>
        /// Activates and displays a window. If the window is minimized or maximized, Windows restores it to its original size and position.
        /// An application should specify this flag when displaying the window for the first time.
        /// </summary>
        SW_NORMAL = 1,

        /// <summary>Activates the window and displays it as a minimized window.</summary>
        SW_SHOWMINIMIZED = 2,

        /// <summary>Activates the window and displays it as a maximized window.</summary>
        SW_SHOWMAXIMIZED = 3,

        /// <summary>Maximizes the specified window.</summary>
        SW_MAXIMIZE = 3,

        /// <summary>Displays a window in its most recent size and position. The active window remains active.</summary>
        SW_SHOWNOACTIVATE = 4,

        /// <summary>Activates the window and displays it in its current size and position.</summary>
        SW_SHOW = 5,

        /// <summary>Minimizes the specified window and activates the next top-level window in the z-order.</summary>
        SW_MINIMIZE = 6,

        /// <summary>Displays the window as a minimized window. The active window remains active.</summary>
        SW_SHOWMINNOACTIVE = 7,

        /// <summary>Displays the window in its current state. The active window remains active.</summary>
        SW_SHOWNA = 8,

        /// <summary>
        /// Activates and displays the window. If the window is minimized or maximized, Windows restores it to its original size and
        /// position. An application should specify this flag when restoring a minimized window.
        /// </summary>
        SW_RESTORE = 9,

        /// <summary>
        /// Sets the show state based on the SW_ flag specified in the STARTUPINFO structure passed to the CreateProcess function by the
        /// program that started the application. An application should call ShowWindow with this flag to set the initial show state of its
        /// main window.
        /// </summary>
        SW_SHOWDEFAULT = 10,

        /// <summary>
        /// Minimizes a window, even if the thread that owns the window is not responding. This flag should only be used when minimizing
        /// windows from a different thread.
        /// </summary>
        SW_FORCEMINIMIZE = 11,
    }

}
