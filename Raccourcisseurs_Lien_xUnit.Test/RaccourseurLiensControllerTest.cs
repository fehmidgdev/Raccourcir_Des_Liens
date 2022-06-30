using Microsoft.EntityFrameworkCore;
using Raccourcisseur_Liens.Data;
using Raccourcisseur_Liens.Helper;
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
    public class RaccourseurLiensControllerTest
    {

        public RaccourseurLiensControllerTest()
        {
            InitContext();
        }

        private ApplicationDbContext _dbContext;
        [Fact]
        public void InitContext()
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());

            var context = new ApplicationDbContext(builder.Options);
            var lienslist = Enumerable.Range(1, 10)
                .Select(i => new RaccourseurLiens { Id = i, OriginalUrl = $"OriginalUrl{i}", NouveauUrl = $"NouveauUrl{i}", UserID = "1", DateInsertion = DateTime.Now });
            context.Raccourseurs.AddRange(lienslist);
            int changed = context.SaveChanges();
            _dbContext = context;
        }

        [Fact]
        public void TestGetAll()
        {
            var controller = new RaccoursirLiensService(_dbContext);
            IEnumerable<RaccourseurLiens> result = controller.GetAll();
            Assert.Equal(10, result.Count());
        }

        [Fact]
        public void TestGetLienById()
        {
            string nouveaulien = "NouveauUrl2";
            var controller = new RaccoursirLiensService(_dbContext);
            RaccourseurLiens result = controller.GetById(2);
            Assert.Equal(nouveaulien, result.NouveauUrl);
        }

        [Fact]
        public void TestAddShortenURL()
        {
            var controller = new RaccoursirLiensService(_dbContext);
            //nb de ligne avant l'ajout
            int nb_before = controller.GetAll().Count();

            var raccourseurLiens = new RaccourseurLiens
            {
                Id = nb_before + 1,
                OriginalUrl = "OriginalUrl11",
                NouveauUrl = "NouveauUrl11",
                UserID = "1",
                DateInsertion = DateTime.Now
            };
            int result = controller.Save(raccourseurLiens);
            //nb de ligne apres l'ajout
            int nb_after = controller.GetAll().Count();
            Assert.Equal(nb_after, result);
        }

        [Fact]
        public void TestDeleteFirstURL()
        {
            var controller = new RaccoursirLiensService(_dbContext);
            //nb de ligne avant la suppression
            int nb_before = controller.GetAll().Count();

            controller.DeleteOldLien();
            //nb de ligne apres l'ajout
            int nb_after = controller.GetAll().Count();
            Assert.Equal(nb_after, nb_before - 1);
        }

        [Fact]
        public void TestEncodeURL()
        {
            int id = 30;
            string path = RaccourseurLiensHelper.Encode(id);

            Assert.Equal("D", path);
        }
        [Fact]
        public void TestDecodeURL()
        {
            string str = "F";
            int indice = RaccourseurLiensHelper.Decode(str);

            Assert.Equal(31, indice);
        }
    }
}
