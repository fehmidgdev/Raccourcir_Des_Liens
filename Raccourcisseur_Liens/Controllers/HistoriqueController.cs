using Microsoft.AspNetCore.Mvc;
using Raccourcisseur_Liens.Helper;
using Raccourcisseur_Liens.Models;
using Raccourcisseur_Liens.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Raccourcisseur_Liens.Controllers
{
    public class HistoriqueController : Controller
    {
        private readonly IHistoriqueService _servicelog;
        public HistoriqueController(IHistoriqueService servicelog)
        {
            _servicelog = servicelog;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create(string url)
        {
            string ip = RaccourseurLiensHelper.getExternalIp();
            var hist = new Historique
            {
                UserID = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                Url = url,
                AdresseIP = ip.ToString(),
                Pays = RaccourseurLiensHelper.GetCountry(ip.ToString()),
                UserAgent = HttpContext.Request.Headers["User-Agent"],
                DateAcces = DateTime.Now
            };

            TryValidateModel(hist);
            if (ModelState.IsValid)
                _servicelog.Save(hist);

            return Ok();

        }
        public IActionResult ShowLog()
        {

            var logList = _servicelog.GetAll();


            return View(logList);
        }
    }
}
