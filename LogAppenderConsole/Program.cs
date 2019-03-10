using System;
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
