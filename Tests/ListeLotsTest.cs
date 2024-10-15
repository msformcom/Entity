using Contrats;
using DAL.ImmoBDD;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Models;

namespace Tests
{
    [TestClass]
    public class ListeLotsTest
    {
        [TestMethod]
        public void CheckConfig()
        {
            // 1) Couplage fort
            // 2) Options => objet difficile à construire
            // 3) Cycle de vie de ImmoServiceBDD difficile à gérer
            // 4) réutilisation de ImmoServiceBDD
            // var service = new ImmoServiceBDD();
            // => utiliser une service s'injection de dépendance

            // Obtient la config à partir de l'injection de dépednace
            var config = DI.Injector.GetRequiredService<IConfiguration>();
            var title = config.GetSection("title").Value;
            Assert.IsNotNull(title);
        }
        [TestMethod]
        public async Task CheckListLots()
        {
             var service=DI.Injector.GetRequiredService<IDataImmo>();
            var resultats = await service.GetLotsAsync(new SearchLotModel() { CodePostal = "86000" });
            Assert.IsNotNull(resultats);
            Assert.IsTrue(resultats.Count() > 0);
        
        }

        }
}
