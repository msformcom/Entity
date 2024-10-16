using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ImmoBDD
{
    public class ProprieteDAO : ITimeStamps
    {
        public Guid IdProprietaire { get; set; }

        public Guid Idlot { get; set; }

        public virtual LotDAO Lot { get; set; }
        public virtual ProprietaireDAO Proprietaire { get; set; }
        public int M2 { get; set; }
        public DateTime LastUpdate { get; set; } = DateTime.Now;
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}
