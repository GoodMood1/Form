using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcIntro.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        public ViewResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection formCollection)
        {
            string searchString = formCollection["SearchString"];

            TempData["SearchString"] = searchString;

            return RedirectToAction("Results");
        }

        public ViewResult Results()
        {
            if (TempData["SearchString"] != null)
            {
                string searchString = TempData["SearchString"].ToString();

                ViewBag.SearchString = searchString;
                ViewBag.SearchResults = "Some search results";
            }

            return View();
        }

    }
}