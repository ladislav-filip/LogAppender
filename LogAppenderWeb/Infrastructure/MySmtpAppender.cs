using System.Web;
using log4net.Appender;
using log4net.Core;

namespace LogAppenderWeb.Infrastructure
{
    public class MySmtpAppender : SmtpAppender
    {
        protected override void Append(LoggingEvent loggingEvent)
        {            
            //log4net.GlobalContext.Properties["AssemblyVersion"] = "3.00";
            //log4net.ThreadContext.Properties["Uziv"] = HttpContext.Current.User.Identity.Name;
            base.Append(loggingEvent);
        }
    }
}