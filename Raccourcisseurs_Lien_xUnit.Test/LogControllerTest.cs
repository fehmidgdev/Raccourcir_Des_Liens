using Microsoft.EntityFrameworkCore;
using Raccourcisseur_Liens.Data;
using Raccourcisseur_Liens.Models;
using Raccourcisseur_Liens.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Raccourcisseurs_Lien_xUnit.Test
{
    public class LogControllerTest
    {

        public LogControllerTest()
        {
            InitContextLog();
        }
        private ApplicationDbContext _dbContext;
        [Fact]
        public void InitContextLog()
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());

            var context = new ApplicationDbContext(builder.Options);
            var logs = new List<Historique>
            {
                new Historique() { Id=1, Url="https://www.google.com/",UserID="1",AdresseIP="192.168.1.77",Pays="Tunisia",UserAgent = "Chrome",DateAcces=DateTime.Now},
                new Historique() { Id=2, Url="https://www.facebook.com/",UserID="18",AdresseIP="172.168.1.11",Pays="Almagne",UserAgent = "Mozila",DateAcces=DateTime.Now},
                new Historique() { Id=3, Url="https://www.facebook.com/",UserID="120",AdresseIP="203.4.8.77",Pays="Sued",UserAgent = "Chrome",DateAcces=DateTime.Now},
                new Historique() { Id=4, Url="https://docs.microsoft.com/",UserID="530",AdresseIP="50.28.1.23",Pays="Belgique",UserAgent = "Mozila",DateAcces=DateTime.Now},
            };
            context.Historiques.AddRange(logs);
            int changed = context.SaveChanges();
            _dbContext = context;
        }
        [Fact]
        public void TestGetAll()
        {
            var controller = new HistoriqueService(_dbContext);
            IEnumerable<Historique> result = controller.GetAll();
            Assert.Equal(4, result.Count());
        }

        [Fact]
        public void TestAddLog()
        {
            var controller = new HistoriqueService(_dbContext);
            //nb de ligne avant l'ajout
            int nb_before = controller.GetAll().Count();

            var hist = new Historique
            {
                Id = nb_before + 1,
                Url = "https://translate.google.fr/",
                AdresseIP = "100.2.3.7",
                Pays = "France",
                UserAgent = "Chrome",
                DateAcces = DateTime.Now
            };
            int result = controller.Save(hist);
            //nb de ligne apres l'ajout
            int nb_after = controller.GetAll().Count();
            Assert.Equal(nb_after, result);
        }
    }
}
