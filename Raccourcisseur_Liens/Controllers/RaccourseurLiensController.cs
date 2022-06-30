using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Raccourcisseur_Liens.Controllers;
using Raccourcisseur_Liens.Helper;
using Raccourcisseur_Liens.Models;
using Raccourcisseur_Liens.Services;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Raccourcisseur_Liens.Controllers
{
    public class RaccourseurLiensController : Controller
    {
        
        private readonly IRaccoursirLiensService _service;
        private readonly IHistoriqueService _servicelog;
        private readonly IStringLocalizer<HomeController> _stringLocalizer;
        private readonly SignInManager<IdentityUser> _signInManager;
        public RaccourseurLiensController(IRaccoursirLiensService service, IStringLocalizer<HomeController> stringLocalizer, IHistoriqueService servicelog, SignInManager<IdentityUser> signInManager)
        {
            _service = service;
            _stringLocalizer = stringLocalizer;
            _servicelog = servicelog;
            _signInManager = signInManager;
        }
        
        public IActionResult Index()
        {
            return RedirectToAction(actionName: nameof(Create));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string originalUrl)
        {
            var raccourseurLiens = new RaccourseurLiens
            {
                OriginalUrl = originalUrl,
            };

            TryValidateModel(raccourseurLiens);
            if (ModelState.IsValid)
            {
                //vérifier le nbr des lignes par user (ne pas dépasser 5 liens)
                if (_service.GetByIdUser(User.FindFirst(ClaimTypes.NameIdentifier).Value) >= 5)
                {
                    return this.StatusCode(StatusCodes.Status406NotAcceptable, "dépassement du nombre de liens permis");
                }
                else
                {
                    //supprimer les liens datant de plus de 24 h
                    _service.DeleteLienDatantplus24();
                    //controler si le nbr des lignes = 20 alors supprimer le premiers
                    if (_service.GetNbrRows() == 20)
                    {
                        _service.DeleteOldLien();
                    }
                    //Recup id user connecté
                    raccourseurLiens.UserID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    raccourseurLiens.DateInsertion =DateTime.Now;
                    
                    _service.Save(raccourseurLiens);
                    raccourseurLiens.NouveauUrl = "http://localhost:1218/Raccourseurs/RedirectTo/" + RaccourseurLiensHelper.Encode(raccourseurLiens.Id);
                    _service.Update(raccourseurLiens);
                    return RedirectToAction(actionName: nameof(Show), routeValues: new { id = raccourseurLiens.Id });
                }
            }
            return View(raccourseurLiens);

        }
        public IActionResult CreateLog(string url)
        {
            string ip = RaccourseurLiensHelper.getExternalIp();
            string iduser = string.Empty;
            if (_signInManager.IsSignedIn(User))
                iduser = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var log = new Historique
            {
                UserID = iduser,
                Url = url,
                AdresseIP = ip != null ? ip.ToString():string.Empty,
                Pays = RaccourseurLiensHelper.GetCountry(ip != null ? ip.ToString() : string.Empty),
                UserAgent = HttpContext.Request.Headers["User-Agent"],
                DateAcces = DateTime.Now
            };

            TryValidateModel(log);
            if (ModelState.IsValid)
                _servicelog.Save(log);

            return Ok();

        }
        public IActionResult Show(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var raccourseurLiens = _service.GetById(id.Value);
            if (raccourseurLiens == null)
            {
                return NotFound();
            }

            ViewData["Path"] = RaccourseurLiensHelper.Encode(raccourseurLiens.Id);
            this.CreateLog(raccourseurLiens.OriginalUrl);
            return View(raccourseurLiens);
        }
        public IActionResult DeleteLiensByID(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }
            _service.DeleteLiensByID(id.Value);
            return RedirectToAction(actionName: nameof(ShowForDelete));
        }
        public IActionResult ShowAll()
        {

            var raccourseurLiensList = _service.GetAll();


            return View(raccourseurLiensList);
        }
        public IActionResult ShowForDelete(string id)
        {
            id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var raccourseurLiensList = _service.GetByIDUser(id);


            return View(raccourseurLiensList);
        }

        [HttpGet("/Raccourseurs/RedirectTo/{path:required}", Name = "Raccourseurs_RedirectTo")]
        public IActionResult RedirectTo(string path)
        {
            if (path == null)
            {
                return NotFound();
            }

            var raccourseurLiens = _service.GetByPath(path);
            if (raccourseurLiens == null)
            {
                return NotFound();
            }
            //inserer un log
            this.CreateLog(raccourseurLiens.OriginalUrl);
            return Redirect(raccourseurLiens.OriginalUrl);
        }

        
    }
}
