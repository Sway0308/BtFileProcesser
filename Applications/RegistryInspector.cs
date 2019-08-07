using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace BtFileProcesserNet
{
    class RegistryInspector
    {
        static void Main(string[] args)
        {
            var timer = new Timer(60 * 60 * 1000);  //每一小時檢查一次
            timer.Elapsed += (s, e) => {
                Console.WriteLine(DateTime.Now);
                DeleteRegistryKey();
                Console.ReadLine();
            };
            Console.WriteLine("Start timer");
            timer.Start();
            HandleConsole();
        }

        private static void HandleConsole()
        {
            var result = Console.ReadLine();
            if (result.ToLower() != "exit")
                HandleConsole();
        }

        private static void DeleteRegistryKey()
        {
            //讀取Registry Key位置
            using (var RegK = Registry.LocalMachine.OpenSubKey(@"Software\Policies\Google\Chrome", RegistryKeyPermissionCheck.ReadWriteSubTree))
            {
                var value = RegK.GetValue("RemoteAccessHostFirewallTraversal")?.ToString();
                if (value != null)
                {
                    Console.WriteLine($"Registry Value = {value}");
                    Console.WriteLine($"Start delete registry");
                    RegK.DeleteValue("RemoteAccessHostFirewallTraversal", true);
                    Console.WriteLine($"finish delete registry");
                }
                else
                {
                    Console.WriteLine("No Registry");
                }
            }
        }
    }
}
