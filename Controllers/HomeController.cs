using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DnDemo.Models;
// Don't forget to bring in session!
using Microsoft.AspNetCore.Http;

namespace DnDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public List<String> MaleNames = new List<String>() {"Lydan","Syrin","Ptorik","Joz","Varog","Gethrod","Hezra","Feron","Ophni","Colborn","Fintis","Gatlin","Jinto","Hagalbar","Krinn","Lenox","Revvyn","Hodus","Dimian","Paskel","Kontas","Weston","Azamarr","Jather","Tekren","Jareth","Adon","Zaden","Eune","Graff","Tez","Jessop","Gunnar","Pike","Domnhar","Baske","Jerrick","Mavrek","Riordan","Wulfe","Straus","Tyvrik","Henndar","Favroe","Whit","Jaris","Renham","Kagran","Lassrin","Vadim","Arlo","Quintis","Vale","Caelan","Jakrin","Fangar","Baxar","Hawke","Nazim","Kadric","Moki","Rankar","Lothe","Ryven","Cassian","Verssek","Rourke","Blaiz","Zagaroth","Baashar"};

        public List<String> FemaleNames = new List<String>(){"Syrana","Resha","Varin","Wren","Yuni","Talis","Kessa","Magaltie","Aeris","Desmina","Krynna","Asralyn","Herra","Pret","Kory","Afia","Tessel","Rhiannon","Zara","Jesi","Belen","Rei","Ciscra","Temy","Renalee","Estyn","Maarika","Lynorr","Tiv","Annihya","Semet","Tamrin","Antia","Reslyn","Basak","Vixra","Pekka","Xavia","Beatha","Yarri","Liris","Sonali","Razra","Maeve","Yelina","Palra","Elysa","Ketra","Agama","Thesra","Tezani","Naima","Rydna","Baakshi","Ibera","Dessa","Silene","Phressa","Anika","Rasy","Harper","Vita","Drusila","Minha","Merula","Kye","Lyla","Turi","Beela","Leska","Vemery","Lunex","Tisette","Partha"};

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        // DISCLAIMER: Using session to store all of our data is a bad idea! But it's all we have right now, so we're rolling with it. But once you know how to store data in a database you can basically replace everything I'm about to do with models instead of session
        [HttpPost("addFighter")]
        public IActionResult addFighter(Fighter You)
        {
            // If this triggers, we did it right
            if(ModelState.IsValid)
            {
                // Store our name
                HttpContext.Session.SetString("MyName", You.Name);

                // Generate our character stats
                Random rand = new Random();
                // generate self
                HttpContext.Session.SetInt32("MyHealth", 100);
                HttpContext.Session.SetInt32("MyAttack", rand.Next(20,36));
                HttpContext.Session.SetInt32("MyNum", rand.Next(1,25));

                GenerateEnemy();
                // Console.WriteLine(You.Name);
                return RedirectToAction("Arena");
            } else {
                return View("Index");
            }
        }

        public void GenerateEnemy()
        {
            Random rand = new Random();
            // generate opponent
            int OpNum = rand.Next(1,25);
            HttpContext.Session.SetInt32("OpNum", OpNum);
            HttpContext.Session.SetInt32("OpHealth", 100);
            HttpContext.Session.SetInt32("OpAttack", rand.Next(15,36));
            if(OpNum % 2 == 0)
            {
                // This means it's a female character
                HttpContext.Session.SetString("OpName", FemaleNames[rand.Next(0, FemaleNames.Count)]);
            } else {
                HttpContext.Session.SetString("OpName", MaleNames[rand.Next(0, MaleNames.Count)]);
            }
        }


        [HttpGet("Arena")]
        public IActionResult Arena()
        {
            // Set up for Self
            ViewBag.Name = HttpContext.Session.GetString("MyName");
            ViewBag.MyHealth = HttpContext.Session.GetInt32("MyHealth");
            int MyNum = (int)HttpContext.Session.GetInt32("MyNum");
            ViewBag.MyChar = Url.Content($"~/images/Fighter_{MyNum}.jpg");
            
            // Set up for Opponent
            int OpNum = (int)HttpContext.Session.GetInt32("OpNum");
            ViewBag.OpHealth = HttpContext.Session.GetInt32("OpHealth");
            ViewBag.OpChar = Url.Content($"~/images/Fighter_{OpNum}.jpg");
            ViewBag.OpName = HttpContext.Session.GetString("OpName");
            return View();
        }

        [HttpGet("Attack")]
        public IActionResult Attack()
        {
            // This is where they hit each other
            int newOpHealth = (int)HttpContext.Session.GetInt32("OpHealth") - (int)HttpContext.Session.GetInt32("MyAttack");
            HttpContext.Session.SetInt32("OpHealth", newOpHealth);

            int newMyHealth = (int)HttpContext.Session.GetInt32("MyHealth") - (int)HttpContext.Session.GetInt32("OpAttack");
            HttpContext.Session.SetInt32("MyHealth", newMyHealth);
            return RedirectToAction("Arena");
        }

        [HttpGet("Reset")]
        public IActionResult Reset()
        {
            // We need a full reset of our game, meaning we need to go back to the index page where we filled out the name form
            return RedirectToAction("Index");
        }

        [HttpGet("NewMatch")]
        public IActionResult NewMatch()
        {
            GenerateEnemy();
            HttpContext.Session.SetInt32("MyHealth", 100);
            return RedirectToAction("Arena");
        }
    }
}
