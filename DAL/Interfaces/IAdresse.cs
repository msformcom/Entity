using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IAdresse
    {
        public string Ligne { get; set; }
        public string CodePostal { get; set; }
        public string Ville { get; set; }
    }
}
