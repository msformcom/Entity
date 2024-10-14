using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public  interface IDataImmo
    {
        Task<IEnumerable<ISearchResult<string,ILot>>> GetLotsAsync(ISearchLotModel? search);
    }
}
