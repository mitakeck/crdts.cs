using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRDTs
{
    /// <summary>
    /// 2P Set
    /// </summary>
    public class GNSet<T>
    {
        public GSet<T> Added { get; private set; }
        public GSet<T> Removed { get; private set; }

        public HashSet<T> Values => Added.Values.Except(Removed.Values).ToHashSet();

        public GNSet(GSet<T>? added = null, GSet<T>? removed = null)
        {
            Added = added ?? new();
            Removed = removed ?? new();
        }

        public void Add(T value)
        {
            Added.Add(value);
        }

        public void Remove(T value)
        {
            Removed.Add(value);
        }

        public void Merge(GNSet<T> other)
        {
            Added.Merge(other.Added);
            Removed.Merge(other.Removed);
        }
    }
}
