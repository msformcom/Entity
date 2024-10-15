using Contrats;
using DAL.ImmoBDD;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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

            #region Configuration IDataImmo
            // Construction des options destinées au context grace au DbContextOptionsBuilder
            var optionsBuilder = new DbContextOptionsBuilder<ImmoContext>();

            // Utilisation d'une méthpde d'extension du package Microsoft.EntityFrameworkCore.SqlServer
            // Pour configurer les options avec provider SqlServer
            optionsBuilder.UseSqlServer(config.GetConnectionString("ImmoBDD"));
              


            var options = optionsBuilder.Options;
            sc.AddSingleton<DbContextOptions<ImmoContext>>(options);

            sc.AddScoped<IDataImmo,ImmoServiceBDD>();

            // Je spécifie les options de BDD qui seront prises en compte par le DbContext
            sc.AddSingleton<ModelCreation>(modelBuilder =>
            {
                // Accès aux spécificités associées à l'entite LotDAO
                modelBuilder.Entity<LotDAO>(o =>
                {
                    o.ToTable("Tbl_Lots");
                    // Spécification de la colonne associée à Reference
                    o.Property(c => c.Reference).HasColumnName("Ref").IsRequired().HasMaxLength(20);
                    o.Property(c => c.Id).HasColumnName("PK_Lot");
                });



            });



            #endregion

            // Une fois les services ajoutés à la collection
            // je génère mon Service Provider
            Injector = sc.BuildServiceProvider();

            // Vérifier que la BDD existe
            // Création de scope
            using (var scope = Injector.CreateScope())
            {
                // Les objets obtenus dans le cadre de ce scope
                // seront disposés en sortant de ce bloc using
                // Mode singleton dans ce scope
                var immo=scope.ServiceProvider.GetRequiredService<IDataImmo>();
                immo.EnsureBDDCreated();
            }


            


        }

    }
}
