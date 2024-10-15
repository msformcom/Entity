using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contrats
{
    public  interface IDataImmo
    {
        void EnsureBDDCreated();
        Task<IEnumerable<ISearchResult<string,ILot>>> GetLotsAsync(ISearchLotModel? search);
      
    }
}
