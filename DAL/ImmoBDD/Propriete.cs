using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ImmoBDD
{
    public class ProprieteDAO : ITimeStamps
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public LotDAO Lot { get; set; }
        public ProprietaireDAO Proprietaire { get; set; }
        public int M2 { get; set; }
        public DateTime LastUpdate { get; set; } = DateTime.Now;
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}
