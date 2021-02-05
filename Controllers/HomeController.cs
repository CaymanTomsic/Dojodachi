using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Dojodachi.Models;
using Microsoft.AspNetCore.Http;


namespace Dojodachi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            if(HttpContext.Session.GetInt32("Fullness") == null)
            {
                HttpContext.Session.SetInt32("Fullness", 20);
                HttpContext.Session.SetInt32("Happiness", 20);
                HttpContext.Session.SetInt32("Energy", 50);
                HttpContext.Session.SetInt32("Meals", 3);
            }
            ViewBag.Fullness = HttpContext.Session.GetInt32("Fullness");
            ViewBag.Happiness = HttpContext.Session.GetInt32("Happiness");
            ViewBag.Energy = HttpContext.Session.GetInt32("Energy");
            ViewBag.Meals = HttpContext.Session.GetInt32("Meals");
            ViewBag.Message = HttpContext.Session.GetString("Message");
            if(ViewBag.Fullness >= 100 && ViewBag.Happiness >= 100)
            {
                return View("Winner");
            }
            else if(ViewBag.Fullness <= 0 || ViewBag.Happiness <= 0)
            {
                return View("Loser");
            }
            return View();
        }

        [HttpGet("/feed")]
        public IActionResult Feed()
        {
            int? currentMeals = HttpContext.Session.GetInt32("Meals");
            if (currentMeals >= 1)
            {
                Random rand = new Random();
                int chance = rand.Next(4);
                if(chance == 0)
                {
                    int newMeals = (int)currentMeals - 1;
                    HttpContext.Session.SetInt32("Meals", newMeals);
                    HttpContext.Session.SetString("Message", "Dojodachi didn't like the food! +0 Fullness -1 Meals");
                }
                else
                {
                    int newMeals = (int)currentMeals - 1;
                    HttpContext.Session.SetInt32("Meals", newMeals);
                    int? fullness = HttpContext.Session.GetInt32("Fullness");
                    int change = rand.Next(5,11);
                    int newFull = (int)fullness + change;
                    HttpContext.Session.SetInt32("Fullness", newFull);
                    HttpContext.Session.SetString("Message", $"You fed Dojodachi! +{change} Fullness -1 Meals");
                }
            }
            else
            {
                HttpContext.Session.SetString("Message", "Not enough meals to feed Dojodachi!");
            }
            return RedirectToAction("Index");
        }

        [HttpGet("/play")]
        public IActionResult Play()
        {
            int? currentEnergy = HttpContext.Session.GetInt32("Energy");
            if (currentEnergy >= 5)
            {
                Random rand = new Random();
                int chance = rand.Next(4);
                if(chance == 0)
                {
                    int newEnergy = (int)currentEnergy - 5;
                    HttpContext.Session.SetInt32("Energy", newEnergy);
                    HttpContext.Session.SetString("Message", $"Dojodachi doesn't want to play! +0 Happiness -5 Energy");
                }
                else
                {
                    int newEnergy = (int)currentEnergy - 5;
                    HttpContext.Session.SetInt32("Energy", newEnergy);

                    int? happiness = HttpContext.Session.GetInt32("Happiness");
                    int change = rand.Next(5,11);
                    int newFull = (int)happiness + change;
                    HttpContext.Session.SetInt32("Happiness", newFull);
                    HttpContext.Session.SetString("Message", $"You played with Dojodachi! +{change} Happiness -5 Energy");
                }
            }
            else
            {
                HttpContext.Session.SetString("Message", "Not enough energy to play with Dojodachi!");
            }
            return RedirectToAction("Index");
        }

        [HttpGet("/work")]
        public IActionResult Work()
        {
            int? currentEnergy = HttpContext.Session.GetInt32("Energy");
            if (currentEnergy >= 5){
                int newEnergy = (int)currentEnergy - 5;
                HttpContext.Session.SetInt32("Energy", newEnergy);

                int? meals = HttpContext.Session.GetInt32("Meals");
                Random rand = new Random();
                int change = rand.Next(1,4);
                int newFull = (int)meals + change;
                HttpContext.Session.SetInt32("Meals", newFull);

                HttpContext.Session.SetString("Message", $"You worked! +{change} Meals -5 Energy");
            }
            else
            {
                HttpContext.Session.SetString("Message", "Not enough energy to work!");
            }
            return RedirectToAction("Index");
        }

        [HttpGet("/sleep")]
        public IActionResult Sleep()
        {
            int? currentEnergy = HttpContext.Session.GetInt32("Energy");
            int newEnergy = (int)currentEnergy + 15;
            HttpContext.Session.SetInt32("Energy", newEnergy);

            int? currentFullness = HttpContext.Session.GetInt32("Fullness");
            int newFullness = (int)currentFullness - 5;
            HttpContext.Session.SetInt32("Fullness", newFullness);
            
            int? currentHappiness = HttpContext.Session.GetInt32("Happiness");
            int newHappiness = (int)currentHappiness - 5;
            HttpContext.Session.SetInt32("Happiness", newHappiness);

            HttpContext.Session.SetString("Message", $"You slept! +15 Energy -5 Fullness -5 Happinesss");
            
            return RedirectToAction("Index");
        }

        [HttpGet("/reset")]
        public IActionResult Reset()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
