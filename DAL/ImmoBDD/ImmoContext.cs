
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ImmoBDD
{
    public class ImmoContext : DbContext
    {
        // le constructeur de cette classe reçoit les options spécifiques à lui-même
        // provenant normalement du système injection de dépendance
        // provider, chaine de connection, authentification, journalisation, etc
        // et les fait passer au contructeur de la base
        internal ImmoContext(DbContextOptions<ImmoContext> options):base(options)
        {
            
        }
        // Cette méthode permet de spécifier des options supplémentaires
        // par rapport à celles reçues dans l'objet DbContextOptions
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.EnableDetailedErrors();
        }

        public DbSet<LotDAO>   Lots { get; set; }
    }
}
