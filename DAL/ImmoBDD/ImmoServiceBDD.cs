using Contrats;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ImmoBDD
{
    public class ImmoServiceBDD : IDataImmo, IDisposable
    {
        private readonly DbContextOptions<ImmoContext> options;
        private ImmoContext context;

        public ImmoServiceBDD(DbContextOptions<ImmoContext> options,
                ModelCreation? modelCreation
                )
        {
            this.options = options;
            this.context = new ImmoContext(options, modelCreation);
        }

        public void EnsureBDDCreated()
        {
            if (this.context.Database.EnsureCreated())
            {
                // Lors de la créationde la BDD
                var L1 = new LotDAO() {     Reference = "12B", 
                                            Adresse = new AdresseDAO() { CodePostal = "86000", Ligne = "Rue des Lilas", Ville = "Poitiers" }, 
                                            Prix = 187987978M, 
                                            RezDeChaussee = true };
                var L2 = new LotVipDAO() {  Reference = "13B", 
                                            Adresse = new AdresseDAO() { CodePostal = "86100", Ligne = "Rue des Orangers", Ville = "Poitiers" }, 
                                            Prix = 1987978M, RezDeChaussee = true, 
                                            PisteAtterissageJet = true };
                this.context.Lots.Add(L1);
                this.context.Lots.Add(L2);
                this.context.SaveChanges();
            }

        }




        public Task<IEnumerable<ISearchResult<string, ILot>>> GetLotsAsync(ISearchLotModel? search)
        {

            // Sortie du bloc using
            // Methode dispose appelée
            // 1) Demande à l'objet de fermer les resource externes (net,file,connection) ouverte
            // 2) SuppressFinalize


            // Je dois accéder aux données
            IQueryable<LotDAO> query = context.LotVips;  // SELECT * FROM Lots
            if (search != null)
            {
                query = query.Where(c => c.Adresse.CodePostal == search.CodePostal);

            }
            var resultat = query.Select(c => new SearchResult<string, ILot>()
            {
                Id = c.Reference,
                Libelle = $"Bien immo à  {c.Adresse.Ville}",
                Description = $"Prix : {c.Prix: C}"
            } as ISearchResult<string, ILot>).AsEnumerable();
            return Task.FromResult(resultat);


        }

        #region Dispose implementation
        private bool _disposedValue;


        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            // Appel de la méthode de nettoyage
            Dispose(true);
            // information au GC => inutile de nettoyer cet objet au moment du garbage
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    this.context.Dispose();
                }

                _disposedValue = true;
            }
        }

        // Destructeur => méthode appelée par le garbage collector
        // lors de la destruction de l'espace méméoire associée à l'objet
        ~ImmoServiceBDD()
        {
            Dispose(false);
        }



        #endregion
    }
}
