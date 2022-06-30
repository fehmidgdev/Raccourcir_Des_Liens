using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Raccourcisseur_Liens.Models
{
    public class Historique
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public string UserID { get; set; }
        public string AdresseIP { get; set; }

        public string Pays { get; set; }
        public string UserAgent { get; set; }

        public DateTime DateAcces { get; set; }
    }
}
