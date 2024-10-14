using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public static class MesExtensions
    {
        public static string Ellipsis(this string a, int maxLength)
        {
            if (a.Length <= maxLength)
            {
                return a;
            }
            return a.Substring(0, maxLength-3)+"...";
        }


        public static IEnumerable<IEnumerable<T>> Buffer<T>(this IEnumerable<T> source, int nbElements)
        {
            var l = new List<T>();
            foreach (T item in source) {
           
                l.Add(item);
                if (l.Count == nbElements)
                {
                    yield return l;
                    l = new List<T>();
                } 
            }
            yield return l;
        }
    }
}
