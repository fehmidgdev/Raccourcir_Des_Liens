using Raccourcisseur_Liens.Data;
using Raccourcisseur_Liens.Helper;
using Raccourcisseur_Liens.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Raccourcisseur_Liens.Services
{
    public class RaccoursirLiensService : IRaccoursirLiensService
    {
        private readonly ApplicationDbContext _context;

        public RaccoursirLiensService(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<RaccourseurLiens> GetAll()
        {
            return _context.Raccourseurs;
        }
        public IEnumerable<RaccourseurLiens> GetByIDUser(string id)
        {
            return _context.Raccourseurs.Where(x=>x.UserID==id);
        }

        public RaccourseurLiens GetById(int id)
        {
            return _context.Raccourseurs.Find(id);
        }

        public RaccourseurLiens GetByPath(string path)
        {
            return _context.Raccourseurs.Find(RaccourseurLiensHelper.Decode((path)));
        }

        public RaccourseurLiens GetByOriginalUrl(string originalUrl)
        {
            foreach (var raccourseurLiens in _context.Raccourseurs)
            {
                if (raccourseurLiens.OriginalUrl == originalUrl)
                {
                    return raccourseurLiens;
                }
            }

            return null;
        }

        public int Save(RaccourseurLiens raccourseurLiens)
        {
            _context.Raccourseurs.Add(raccourseurLiens);
            _context.SaveChanges();

            return raccourseurLiens.Id;
        }

        public int Update(RaccourseurLiens raccourseurLiens)
        {
            _context.Raccourseurs.Update(raccourseurLiens);
            _context.SaveChanges();

            return raccourseurLiens.Id;
        }

        public int GetByIdUser(string id)
        {
            return _context.Raccourseurs.Where(lien => lien.UserID == id).Count();
        }

        public int GetNbrRows()
        {
            return _context.Raccourseurs.Count();
        }

        public void DeleteOldLien()
        {
            RaccourseurLiens raccourseurLiens = _context.Raccourseurs.OrderBy(x => x.Id).FirstOrDefault();
            _context.Remove(raccourseurLiens);
            _context.SaveChanges();

        }
        public void DeleteLienDatantplus24()
        {
            IEnumerable<RaccourseurLiens> raccourseurLiens = _context.Raccourseurs.Where(x => x.DateInsertion < DateTime.Now.AddDays(-1));
            bool trouve = false;
            foreach(var rl in raccourseurLiens)
            {
                _context.Remove(rl);
                trouve = true;
            }

            if (trouve)
                _context.SaveChanges();


        }

        public void DeleteLiensByID(int id)
        {
            RaccourseurLiens raccourseurLiens = _context.Raccourseurs.Find(id);
            _context.Remove(raccourseurLiens);
            _context.SaveChanges();
        }
    }
}
