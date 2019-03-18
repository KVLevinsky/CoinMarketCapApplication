using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoinMarketCapApplication.Models;
using CoinMarketCapApplication.Services;

namespace CoinMarketCapApplication.Controllers {
    public class HomeController : Controller {
        private ICoinMarketCapCache _cache;

        public HomeController(ICoinMarketCapCache cache) {
            _cache = cache;
        }
        public IActionResult Index() {
            return View();
        }

        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
