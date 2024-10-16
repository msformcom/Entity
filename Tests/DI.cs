using Contrats;
using DAL.ImmoBDD;
using DAL.ImmoBDD.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            //sc.AddLogging(options =>
            //{
            //    options.AddDebug();
            //});


            #region Ajout du logging
            var logFactory = LoggerFactory.Create(builder =>
            {
                builder.AddDebug();
            });
            sc.AddSingleton<ILoggerFactory>(logFactory);
            #endregion




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
            sc.AddTransient<ImmoContext>();
            #endregion

            #region Configuration IDataImmo
            // Construction des options destinées au context grace au DbContextOptionsBuilder



            //  cette entrée dans les services permet de generer un DbContextOptionsBuilder général
            sc.AddTransient<DbContextOptionsBuilder<ImmoContext>>(s =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<ImmoContext>();

                optionsBuilder.AddInterceptors(new TimeStampInterceptor());

                // Utilisation d'une méthpde d'extension du package Microsoft.EntityFrameworkCore.SqlServer
                // Pour configurer les options avec provider SqlServer
                optionsBuilder.UseSqlServer(config.GetConnectionString("ImmoBDD"));
                //optionsBuilder.UseLazyLoadingProxies();
                optionsBuilder.UseLoggerFactory(logFactory);
                return optionsBuilder;
            });
            // demande des options générées à partir du DbContextOptionsBuilder<ImmoContext>
            sc.AddTransient<DbContextOptions<ImmoContext>>(s => s.GetRequiredService<DbContextOptionsBuilder<ImmoContext>>().Options);

            sc.AddScoped<IDataImmo, ImmoServiceBDD>();

            // Je spécifie les options de BDD qui seront prises en compte par le DbContext
            sc.AddSingleton<ModelCreation>(modelBuilder =>
            {

                #region Ajout d'un mapping entre une fonction du C# et une fonction SQL
                var methodeInfo = typeof(FonctionsSQL)
                                        .GetMethod(nameof(FonctionsSQL.GetFrancs))!;
                modelBuilder.HasDbFunction(methodeInfo)
                    .HasSchema("dbo").HasName("GetFrancs");

                #endregion

                #region Ajout d'un mapping entre une fonction du C# et une expression SQL
                var cosh = typeof(Math)
                                        .GetMethod(nameof(Math.Cosh))!;


            modelBuilder.HasDbFunction(cosh)
                // Je definis ici la traduction de cosh en SQL  ()
                .HasTranslation(args =>
                new SqlBinaryExpression(ExpressionType.Add,
                    new SqlFunctionExpression("exp", new SqlExpression[] { args[0] }, false,new bool[] { true },typeof(double),null)
                    ,
                    new SqlFunctionExpression("exp", new SqlExpression[] { args[0] }, false, new bool[] { true },typeof(double), null),
                    typeof(double),null)
                    
       
                );

                #endregion


                // Accès aux spécificités associées à l'entite LotDAO
                modelBuilder.Entity<LotDAO>(o =>
                {
                    o.HasKey(c => c.Id);
                    o.Property(c => c.Prix).IsConcurrencyToken();
                    o.ToTable("Tbl_Lots"); //.HasDiscriminator("Disc",typeof(int)).HasValue(1);
                    o.OwnsOne(c => c.Adresse).ToTable("Tbl_Adresses");
                    // Spécification de la colonne associée à Reference
                    o.Property(c => c.Reference).HasColumnName("Ref").IsRequired().HasMaxLength(20);
                    o.Property(c => c.Id).HasColumnName("PK_Lot");
                });



                modelBuilder.Entity<LotVipDAO>(o =>
                {

                    o.ToTable("Tbl_LotsVips")
                    .HasBaseType<LotDAO>();//.HasDiscriminator("Disc", typeof(int)).HasValue(2);
                    // Spécification de la colonne associée à Reference
                    o.Property(c => c.Reference).HasColumnName("Ref").IsRequired().HasMaxLength(20);
                    o.Property(c => c.Id).HasColumnName("PK_Lot");
                });
                modelBuilder.Entity<ProprietaireDAO>(o =>
                {
                    o.HasIndex(o => new { o.Nom, o.CreationDate });
                    o.HasKey(c => c.Id);
                    o.ToTable("Tbl_Proprietaires"); //.HasDiscriminator("Disc",typeof(int)).HasValue(1);
                    //o.OwnsOne(c => c.Adresse).ToTable("Tbl_Adresses");
                });
                modelBuilder.Entity<ProprieteDAO>(o =>
                {
                    o.HasKey(c => new { c.IdProprietaire, c.Idlot });
                    o.HasOne(c => c.Proprietaire)
                        .WithMany(c => c.Proprietes)
                        .HasForeignKey(c => c.IdProprietaire).OnDelete(DeleteBehavior.Restrict);
                    o.HasOne(c => c.Lot)
                        .WithMany(c => c.Proprietes)
                        .HasForeignKey(c => c.Idlot).OnDelete(DeleteBehavior.Restrict);
                    //o.HasKey(c => new {LotId= c.Lot.Id, ProprietaireId=c.Proprietaire.Id });
                    o.ToTable("Tbl_Proprietes");
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
                var immo = scope.ServiceProvider.GetRequiredService<IDataImmo>();
                immo.EnsureBDDCreated();
            }





        }

    }
}
