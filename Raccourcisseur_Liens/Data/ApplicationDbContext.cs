using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Raccourcisseur_Liens.Models;

using System;
using System.Collections.Generic;
using System.Text;

namespace Raccourcisseur_Liens.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<RaccourseurLiens> Raccourseurs { get; set; }
        public DbSet<Historique> Historiques { get; set; }
    }
}
