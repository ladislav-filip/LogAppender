using System.Web;
using log4net.Appender;
using log4net.Core;

namespace LogAppenderWeb.Infrastructure
{
    /// <summary>
    /// Pouze pro otestování funkčnosti vlastního SMTP appenderu.
    /// </summary>
    public class MySmtpAppender : SmtpAppender
    {
        protected override void Append(LoggingEvent loggingEvent)
        {            
            base.Append(loggingEvent);
        }
    }
}