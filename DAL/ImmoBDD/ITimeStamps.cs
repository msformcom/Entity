using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ImmoBDD
{
    internal interface ITimeStamps
    {
        public DateTime LastUpdate { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
