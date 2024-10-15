using Contrats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ImmoBDD
{
    public class SearchResult<TKey,TItem> : ISearchResult<TKey, TItem>
    {
        public TKey Id { get; set; }
        public string Libelle { get; set; }
        public string Description { get; set; }
    }
}
