using Raccourcisseur_Liens.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Raccourcisseur_Liens.Services
{
    public interface IHistoriqueService
    {
        IEnumerable<Historique> GetAll();
        int Save(Historique historique);
    }
}
