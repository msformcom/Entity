using DAL.ImmoBDD;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class ListeLotsTest
    {
        [TestMethod]
        public void GetResults()
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
    }
}
