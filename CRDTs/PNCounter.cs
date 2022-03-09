using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRDTs
{
    public class PNCounter
    {
        public Guid Id { get; private set; }
        public GCounter P { get; private set; }
        public GCounter N { get; private set; }

        public int Value => P.Value - N.Value;

        public PNCounter(Guid id, GCounter? p, GCounter? n)
        {
            Id = id;

            P = new(id, p?.Counters);
            N = new(id, n?.Counters);
        }

        public void Increment(int amount = 1)
        {
            P.Increment(amount);
        }

        public void Decrement(int amount = 1)
        {
            N.Increment(amount);
        }

        public void Merge(PNCounter other)
        {
            P.Merge(other.P);
            N.Merge(other.N);
        }
    }
}
