using log4net.Appender;
using log4net.Core;

namespace LogAppenderConsole
{
    public class MySmtpAppender : SmtpAppender
    {
        protected override void Append(LoggingEvent loggingEvent)
        {
            log4net.GlobalContext.Properties["AssemblyVersion"] = "3.00";
            log4net.ThreadContext.Properties["Uziv"] = "LF";
            base.Append(loggingEvent);
        }
    }
}