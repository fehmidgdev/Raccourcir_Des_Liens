using Raccourcisseur_Liens.Data;
using Raccourcisseur_Liens.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Raccourcisseur_Liens.Services
{
    public class HistoriqueService : IHistoriqueService
    {
        private readonly ApplicationDbContext _context;
        public HistoriqueService(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Historique> GetAll()
        {
            return _context.Historiques;
        }

        public int Save(Historique historique)
        {
            _context.Historiques.Add(historique);
            _context.SaveChanges();

            return historique.Id;
        }
    }
}
