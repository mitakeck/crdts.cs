using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRDTs
{
    public class GSet<T>
    {
        public HashSet<T> Values { get; private set; }

        public GSet(HashSet<T>? hashSets = null)
        {
            Values = hashSets ?? new();
        }

        public void Add(T value)
        {
            Values.Add(value);
        }

        public void Merge(GSet<T> other)
        {
            Values.UnionWith(other.Values);
        }
    }
}
