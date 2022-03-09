using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRDTs
{
    public record TimeStamp
    {
        public long Value { get; private set; }

        public TimeStamp(long? ts = null)
        {
            Value = ts ?? DateTime.UtcNow.Ticks;
        }
    }

    public class LwwElementSet<T> where T: notnull
    {
        public Dictionary<T, TimeStamp> Added { get; private set; }
        public Dictionary<T, TimeStamp> Removed { get; private set; }

        public LwwElementSet(Dictionary<T, TimeStamp>? added = null, Dictionary<T, TimeStamp>? removed = null)
        {
            Added = added ?? new();
            Removed = removed ?? new();
        }


    }
}
