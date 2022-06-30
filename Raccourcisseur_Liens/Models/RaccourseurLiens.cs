using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Raccourcisseur_Liens.Models
{
    public class RaccourseurLiens
    {
        public int Id { get; set; }

        [Required]
        public string OriginalUrl { get; set; }

        public string NouveauUrl { get; set; }

        public string UserID { get; set; }

        public DateTime DateInsertion { get; set; }


    }
}
