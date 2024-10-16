using DAL.ImmoBDD;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Proxies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Tests
{
    [TestClass]
    public class TestDAL
    {

        [TestMethod]
        public void ConcurrencyAndTimeStamps()
        {
            // lasy loading de la propriété de navigation Propriete
            var optionBuilder = DI.Injector.GetRequiredService<DbContextOptionsBuilder<ImmoContext>>();
            var model = DI.Injector.GetRequiredService<ModelCreation>();

       
            using (var context = new ImmoContext(optionBuilder.Options, model))
            {
                var lot1 = context.Lots.TagWith("NORECOMPILE").AsTracking().First();
                lot1.Prix += 1;
                lot1.RezDeChaussee = !lot1.RezDeChaussee;

                var changes = context.ChangeTracker.Entries().ToList();
                context.
                context.SaveChanges();
            }


        }

        [TestMethod]
        public void CreateExpression()
        {
            // cosh(x)= (exp(x)+exp(-x))/2;

            // Where(c=>c.Math.Cosh(c.prix)>10)
            // Transcription SQL
            // Where((EXP(Prix)+Exp(-PRIX))/2>10)


            // lasy loading de la propriété de navigation Propriete
            var optionBuilder = DI.Injector.GetRequiredService<DbContextOptionsBuilder<ImmoContext>>();
            var model = DI.Injector.GetRequiredService<ModelCreation>();

           

            using (var context = new ImmoContext(optionBuilder.Options, model))
            {
                var query = context.Lots.Where(c => Math.Cosh((double)c.Prix)>1);
                var resultats=query.ToList();
            }
        }

        [TestMethod]
        public void UseScalarSQLFunction()
        {
            // lasy loading de la propriété de navigation Propriete
            var optionBuilder = DI.Injector.GetRequiredService<DbContextOptionsBuilder<ImmoContext>>();
            var model = DI.Injector.GetRequiredService<ModelCreation>();

            // Creation d'un ModelCreation à partir de celui qui existe dans DI
            ModelCreation modelModifie = (builder) =>
            {
                //model(builder);
                //// Ajout d'un mapping entre une fonction du C#
                //var methodeInfo = typeof(FonctionsSQL)
                //                        .GetMethod(nameof(FonctionsSQL.GetFrancs))!;
                //builder.HasDbFunction(methodeInfo)
                //    .HasSchema("dbo").HasName("GetFrancs");

            };

        

            using (var context = new ImmoContext(optionBuilder.Options, modelModifie))
            {
                context.Database.ExecuteSqlRaw($"CREATE OR ALTER FUNCTION GetFrancs(@prixEuro DECIMAL(18,2))\r\nRETURNS DECIMAL(18,2)\r\nAS BEGIN\r\n\r\n\tRETURN @PrixEuro * 6.549\r\nEND");

                var query=context.Lots.Where(c => FonctionsSQL.GetFrancs(c.Prix) > 10);
                var resultat=query.ToList();
            }
        }

        [TestMethod]
        public void StoreExpressions()
        {
            // lasy loading de la propriété de navigation Propriete
            var optionBuilder = DI.Injector.GetRequiredService<DbContextOptionsBuilder<ImmoContext>>();
            var model = DI.Injector.GetRequiredService<ModelCreation>();

            optionBuilder.UseLazyLoadingProxies();

            using (var context = new ImmoContext(optionBuilder.Options, model))
            {
                var query = context.Lots
                    .Where(c=>Math.Cos((double)c.Prix)<1)
                    .Where(c=>c.RezDeChaussee==true)
                    // Execution en tant que IQueryable jusque là 
                    // car cosh n'est pas traductible en SQL
                    .AsEnumerable()
                    // le reste des traitements se faite en IEnumerable
                    .Where(c => Math.Cosh((double)c.Prix) > 0);

                var resultats= query.ToList();
            }
        }

        [TestMethod]
        public void TestFunctionTable()
        {
            // lasy loading de la propriété de navigation Propriete
            var optionBuilder = DI.Injector.GetRequiredService<DbContextOptionsBuilder<ImmoContext>>();
            var model = DI.Injector.GetRequiredService<ModelCreation>();

            using (var context = new ImmoContext(optionBuilder.Options, model))
            {
                context.Database.ExecuteSql($"CREATE OR ALTER FUNCTION GetLotsBasPrix (@Prix DECIMAL(18,2)) RETURNS TABLE RETURN     SELECT *  FROM [dbo].Tbl_Lots WHERE Prix <@Prix");

                var query = context.GetLotsBasPrix(1000).OrderBy(c => c.Reference);
                var resultats = query.ToList();
            }
        }


        [TestMethod]
        public void UseStoredProcedure()
        {
            // lasy loading de la propriété de navigation Propriete
            var optionBuilder = DI.Injector.GetRequiredService<DbContextOptionsBuilder<ImmoContext>>();
            var model = DI.Injector.GetRequiredService<ModelCreation>();

           

            using (var context = new ImmoContext(optionBuilder.Options, model))
            {
                context.Database.ExecuteSql($"CREATE OR ALTER PROCEDURE DeleteProprietaire(@id UniqueIdentifier) AS BEGIN Print 'OK' END");

                context.DeleteProprietaire(Guid.NewGuid());
            }
        }

        [TestMethod]
        public void TestLasyLoading()
        {
            // lasy loading de la propriété de navigation Propriete
            var optionBuilder = DI.Injector.GetRequiredService<DbContextOptionsBuilder<ImmoContext>>();
            var model = DI.Injector.GetRequiredService<ModelCreation>();

            optionBuilder.UseLazyLoadingProxies();

            using (var context = new ImmoContext(optionBuilder.Options, model))
            {
                // avec options.UseLasyLoadingProxies()
                LotDAO lot1 = context.Lots.First();
                Assert.IsNotNull(lot1.Proprietes);

                var prop1 = lot1.Proprietes.First();
                var proprio1 = prop1.Proprietaire;
                Assert.IsNotNull(proprio1);
            }
        }







        [TestMethod]
        public void ChargementProprietesDeNavigation()
        {
            using (var context = DI.Injector.GetRequiredService<ImmoContext>())
            {
                
                // Pas de chargement de propriété
                var lot1 =context.Lots.First();
                Assert.IsNull(lot1.Proprietes);
            }
            // Eager loading de la propriété de navigation Propriete
            using (var context = DI.Injector.GetRequiredService<ImmoContext>())
            {
                var lot1 = context.Lots
                        .Include(c => c.Proprietes)
                        .ThenInclude(c => c.Proprietaire).First();
                Assert.IsNotNull(lot1.Proprietes);
                Assert.IsNotNull(lot1.Proprietes.First().Proprietaire);
            }

            // explicit loading de la propriété de navigation Propriete
            using (var context = DI.Injector.GetRequiredService<ImmoContext>())
            {
                var lot1 = context.Lots.First();
                Assert.IsNull(lot1.Proprietes);

                // Chargement des infos lot1.Proprietes
                context.Entry(lot1).Collection(c => c.Proprietes).Load();
                Assert.IsNotNull(lot1.Proprietes);

            }






        }
    }
}
