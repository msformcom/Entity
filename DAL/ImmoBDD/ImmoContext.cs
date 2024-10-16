
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ImmoBDD
{

    public delegate void ModelCreation(ModelBuilder model);

    public class ImmoContext : DbContext
    {
        private readonly ModelCreation? modelCreation;

        // le constructeur de cette classe reçoit les options spécifiques à lui-même
        // provenant normalement du système injection de dépendance
        // provider, chaine de connection, authentification, journalisation, etc
        // et les fait passer au contructeur de la base
        public ImmoContext(
                DbContextOptions<ImmoContext> options,
                ModelCreation? modelCreation


            ) :base(options)
        {
            this.modelCreation = modelCreation;
        }
        // Cette méthode permet de spécifier des options supplémentaires
        // par rapport à celles reçues dans l'objet DbContextOptions
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.EnableDetailedErrors();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            // Configuration des spécificités de la BDD
            // par rapport aux DAO
            base.OnModelCreating(modelBuilder);
            if (modelCreation != null) {
                modelCreation(modelBuilder);
            }
      

        }

        public DbSet<LotDAO>   Lots { get; set; }
        public DbSet<LotVipDAO> LotVips { get; set; }

        public DbSet<ProprieteDAO> Proprietes { get; set; }
        public DbSet<ProprietaireDAO> Proprietaire { get; set; }


        public void DeleteProprietaire(Guid id)
        {
       
            this.Database.ExecuteSqlRaw("EXEC DeleteProprietaire @a ", 
                new SqlParameter[]
            {
                 new SqlParameter("@a",id)
            });
        }

        public IQueryable<LotBaseDAO> GetLotsBasPrix(decimal prixmax)
        {
           return  this.Database.SqlQueryRaw<LotBaseDAO>("SELECT * FROM GetLotsBasPrix(@prix)", new SqlParameter[]
            {
                new SqlParameter("@prix",prixmax)
            });
            
        }
    }
}
