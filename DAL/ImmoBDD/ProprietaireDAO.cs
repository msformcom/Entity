using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ImmoBDD
{
    public class ProprietaireDAO : ITimeStamps
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nom { get; set; }

       // public AdresseDAO Adresse { get; set; }
        public DateTime LastUpdate { get; set; } = DateTime.Now;
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public  ICollection<ProprieteDAO> Proprietes { get; set; }
    }
}
