using Raccourcisseur_Liens.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Raccourcisseur_Liens.Services
{
    public interface IRaccoursirLiensService
    {
        IEnumerable<RaccourseurLiens> GetAll();
        IEnumerable<RaccourseurLiens> GetByIDUser(string id);
        RaccourseurLiens GetById(int id);
        RaccourseurLiens GetByPath(string path);
        RaccourseurLiens GetByOriginalUrl(string originalUrl);
        int Save(RaccourseurLiens raccourseurLiens);
        int Update(RaccourseurLiens raccourseurLiens);
        void DeleteOldLien();
        void DeleteLienDatantplus24();
        void DeleteLiensByID(int id);

        int  GetByIdUser(string id);
        int GetNbrRows();
    }
}
