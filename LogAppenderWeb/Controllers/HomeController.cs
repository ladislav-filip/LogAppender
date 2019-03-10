using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using LogAppenderWeb.Models;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;

namespace LogAppenderWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Session["lf"] = "aktualni hodnota";
            Helper.log.Info("Open home page...");
            ViewBag.Title = "Home Page";

            Helper.log.Error("Simulace chybove zpravy");

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            Helper.log.Info($"Logon user '{model.Login}'...");
            if (ModelState.IsValid)
            {
                if (model.Password != "123")
                {
                    Helper.log.Error("Invalid password!");
                    ModelState.AddModelError("Password", "Invalid password!");
                }
                else
                {
                    SignIn(model);
                    //FormsAuthentication.SetAuthCookie(model.Login, false);
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        public ActionResult Chyba()
        {
            throw new ApplicationException("Testovaci vyjimka.");
        }

        public async Task<ActionResult> Hodnoty()
        {
            Helper.log.Info("Open Hodnoty page...");
            var model = new FullsysModel();
            model.Content = await GetHodnotyAsync();
            return View(model);
        }

        public async Task<string> GetHodnotyAsync()
        {
            var uri = new Uri("http://www.fullsys.cz");
            using (HttpClient httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(uri);
                Helper.log.Debug($"Load content from: {uri.AbsolutePath}");
                return await response.Content.ReadAsStringAsync();
            }
        }

        private void SignIn(LoginModel model)
        {
            FormsAuthentication.SetAuthCookie(model.Login, false);
            System.Web.HttpContext.Current.Session["authorize"] = true;            
        }
    }
}
