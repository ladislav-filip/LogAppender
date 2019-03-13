using System;
using System.Management;
using System.Threading.Tasks;

namespace LogAppenderConsole
{
    /// <summary>
    /// https://stackify.com/log4net-guide-dotnet-logging/
    /// </summary>
    class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            var osInfo = System.Environment.OSVersion.VersionString;
            // definice základních globálních proměnných pro logování
            log4net.GlobalContext.Properties["OsInfo"] = osInfo;

            Console.WriteLine("OS version by Environment: " + osInfo);
            try
            {
                CheckVersionOsByRegistry();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try
            {
                CheckVersionOsByManagement();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try
            {
                CheckVersionOsByWin32();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }            

            Console.WriteLine();
            Console.WriteLine();

            // NDC = "nested diagnostic context"
            using (log4net.NDC.Push(Guid.NewGuid().ToString()))
            {
                log.Info("Hello logging world!");

                log.Error("Moje chyba");

                log.Info("Nasledujici zprava");

                Thr();
            }

            using (log4net.ThreadContext.Stacks["NDC"].Push("Kontext"))
            {
                log.Info("Zprava kontextu");
                LogMy("Dalsi volani v kontextu");
                log.Info("Konec kontextu je zde.");
            }

            Console.WriteLine("Hit enter");
            Console.ReadLine();
        }

        static void CheckVersionOsByRegistry()
        {
            var subKey = @"SOFTWARE\Wow6432Node\Microsoft\Windows NT\CurrentVersion";
            var key = Microsoft.Win32.Registry.LocalMachine;
            var skey = key.OpenSubKey(subKey);
            var name = skey.GetValue("ProductName").ToString();

            Console.WriteLine("OS version by registry: " + name);
        }

        static void CheckVersionOsByManagement()
        {
            var result = string.Empty;
            var searcher = new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem");
            foreach (var os in searcher.Get())
            {
                result = os["Caption"].ToString();
                break;
            }
            Console.WriteLine("OS version by management: " + result);
        }

        static void CheckVersionOsByWin32()
        {
            var result = $"Windows {ComputerInfo.WinMajorVersion}.{ComputerInfo.WinMinorVersion}";
            Console.WriteLine("OS version by Win32: " + result);
        }


        static void LogMy(string msg)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("Vnoreny kontext"))
            {
                log.Info(msg);
            }
        }

        static void Thr()
        {
            Task.Factory.StartNew(() =>
            {
                log.Error("CHYBA: Zprava poslana z vlakna");
            });
        }
    }
}
