using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PinguRock.Controllers
{
    public class HomeController : Controller
    {
        // Vista principal de aplicativo
        // Muestra la información del proyecto
        public ActionResult Index()
        {
            return View();
        }
    }
}