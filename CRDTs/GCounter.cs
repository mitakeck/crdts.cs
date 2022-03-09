using System;
using System.Linq;
using System.Collections.Generic;
using MessagePack;

namespace CRDTs
{
    [MessagePackObject]
    public class GCounter
    {
        [Key(0)]
        public Guid Id { get; private set; }

        [Key(1)]
        public Dictionary<Guid, int> Counters { get; private set; }

        public int Value => Counters.Values.Sum();

        public GCounter(Guid id, Dictionary<Guid, int>? counters = null)
        {
            Id = id;
            Counters = counters ?? new();

            if (!Counters.ContainsKey(Id))
            {
                Counters[Id] = 0;
            }
        }

        public void Increment(int amount = 1)
        {
            if (amount <= 0)
                throw new Exception("amount value must greater than 0");

            var current = Counters[Id];

            Counters[Id] = current + amount;
        }

        public void Merge(GCounter other)
        {
            foreach (var (id, value) in other.Counters)
            {
                Counters[id] = Math.Max(value, Counters.GetValueOrDefault(id, 0));
            }
        }
    }
}
