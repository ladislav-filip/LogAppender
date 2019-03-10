using System.Collections.Generic;
using System.Web.Http;

namespace LogAppenderWeb.Controllers
{
    public class MyController : ApiController
    {
        public IEnumerable<string> Get()
        {
            return new string[] { "hodnota 1", "hodnota 2" };
        }
    }
}
