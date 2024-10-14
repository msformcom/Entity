using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public static class DI
    {
        public static IServiceProvider Injector;

        static DI()
        {
            var sc = new ServiceCollection();
            #region Ajout de la config
            // Pour créer l'objet IConfiguration de mon application
            // J'utilise un builder
            var configBuilder = new ConfigurationBuilder();

            // L'objet créé tiendra compte des informations contenues dans appsettings.json
            configBuilder.AddJsonFile("appsettings.json");
            // Construction d'un objet IConfiguration
            var config = configBuilder.Build();
            // Ajout à l'injection de dépendance
            sc.AddSingleton<IConfiguration>(config);
            #endregion

            // Une fois les services ajoutés à la collection
            // je génère mon Service Provider
            Injector = sc.BuildServiceProvider();
        }

    }
}
