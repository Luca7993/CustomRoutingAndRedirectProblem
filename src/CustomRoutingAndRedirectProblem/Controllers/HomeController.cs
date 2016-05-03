using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using CustomRoutingAndRedirectProblem.Framework;

namespace CustomRoutingAndRedirectProblem.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Page1()
        {
            return View();
        }

        public IActionResult Page2()
        {
            return View();
        }
        public IActionResult PageRedirectToActionPage1()
        {
            //not working with custom routing
            return RedirectToAction(nameof(Page1));
        }

        public IActionResult PageResponseRedirectPage1()
        {
            //always working
            Response.Redirect(nameof(Page1));
            return new JsonResult(null);
        }

        public IActionResult PageMyRedirectPage1()
        {
            //not working with custom routing
            return new MyRedirectResult(Url.Action(nameof(Page1)));
        }


        

        public IActionResult Error()
        {
            return View();
        }
    }




}
