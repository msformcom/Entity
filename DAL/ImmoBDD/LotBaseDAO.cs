using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ImmoBDD
{
    public class LotBaseDAO
    {
        [Column("PK_Lot")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("Ref")]
        public string Reference { get; set; }

        public decimal Prix { get; set; }
        public bool RezDeChaussee { get; set; }

        public DateTime LastUpdate { get; set; } = DateTime.Now;
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}
