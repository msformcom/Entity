using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contrats
{
    public interface ILot 
    {
        public string Reference { get; set; }
        public IAdresse? Adresse { get; set; }

        public decimal Prix { get; set; }
        public bool RezDeChaussee { get; set; }
    }
}
