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
            if (ts <= 0)
                throw new Exception();

            Value = ts ?? DateTime.UtcNow.Ticks;
        }

        public int CompareUfo(TimeStamp other)
        {
            return (
                Value < other.Value,
                Value == other.Value
            ) switch
            {
                (true, _) => -1,
                (_, true) => 0,
                _ => 1,
            };
        }
    }

    public class LwwElementSet<T> where T: notnull
    {
        public Dictionary<T, TimeStamp> Added { get; private set; }
        public Dictionary<T, TimeStamp> Removed { get; private set; }

        public HashSet<T> Values => Added.Keys.ToHashSet();

        public LwwElementSet(Dictionary<T, TimeStamp>? added = null, Dictionary<T, TimeStamp>? removed = null)
        {
            Added = added ?? new();
            Removed = removed ?? new();
        }

        public TimeStamp? Lookup(T element)
        {
            TimeStamp? addedTime = Added.GetValueOrDefault(element);
            TimeStamp? removedTime = Removed.GetValueOrDefault(element);

            if (addedTime != null && removedTime != null)
            {
                return new(Math.Max(addedTime.Value, removedTime.Value));
            }

            return addedTime ?? removedTime ?? null;
        }

        public void Add(T element, TimeStamp? timestamp = null)
        {
            TimeStamp target = timestamp ?? new();
            TimeStamp? current = Lookup(element);

            if (current == null)
            {
                Added.Add(element, target);
                return;
            }

            if (current.Value > target.Value)
            {
                return;
            }

            if (current.Value < target.Value)
            {
                Added.Remove(element);
                Removed.Remove(element);

                Added.Add(element, target);
                return;
            }

            if (current.Value == target.Value)
            {
                Added.Remove(element);
                Removed.Remove(element);

                Added.Add(element, target);
                return;
            }

        }

        public void Remove(T element, TimeStamp? timestamp = null)
        {
            TimeStamp target = timestamp ?? new();
            TimeStamp? current = Lookup(element);

            if (current == null)
            {
                Removed.Add(element, target);
                return;
            }

            if (current.Value > target.Value)
            {
                return;
            }

            if (current.Value < target.Value)
            {
                Added.Remove(element);
                Removed.Remove(element);

                Removed.Add(element, target);
                return;
            }

            if (current.Value == target.Value)
            {
                Added.Remove(element);
                Removed.Remove(element);

                Added.Add(element, target);
                return;
            }
        }

        public void Merge(LwwElementSet<T> other)
        {
            foreach (var (item, timestamp) in other.Added)
            {
                Add(item, timestamp);
            }
            foreach (var (item, timestamp) in other.Removed)
            {
                Remove(item, timestamp);
            }
        }
    }
}
