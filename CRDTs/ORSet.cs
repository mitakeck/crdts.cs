using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRDTs
{
    public class MultiHashSet<Tk, Tv> where Tk: notnull
    { 
        public Dictionary<Tk, List<Tv>> Values { get; private set; }

        public MultiHashSet(Dictionary<Tk, List<Tv>>? values = null)
        {
            Values = values ?? new();
        }

        public void Put(Tk key, Tv value)
        {
            var current = Values.GetValueOrDefault(key, new());
            current.Add(value);

            Values[key] = current;
        }

        public void PutAll(Tk key, List<Tv> values)
        {
            var current = Values.GetValueOrDefault(key, new());
            current.AddRange(values);

            Values[key] = current;
        }

        public bool Remove(Tk key, Tv value)
        {
            var current = Values.GetValueOrDefault(key, new());
            var result = current.Remove(value);

            Values[key] = current;

            return result;
        }

        public List<Tv> RemoveAll(Tk key)
        {
            var current = Values.GetValueOrDefault(key, new());
            Values.Remove(key);

            return current;
        }

        public void ReplaceValues(Tk key, List<Tv> values)
        {
            Values[key] = values;
        }

        public void Replace(Tk key, Tv value)
        {
            Values[key] = new();
            Values[key].Add(value);
        }

        public void Merge(MultiHashSet<Tk, Tv> other)
        {
            foreach (var (key, value) in other.Values)
            {
                PutAll(key, value);
            }
        }

        public HashSet<Tk> Keys()
        {
            return Values.Keys.ToHashSet();
        }

        public IEnumerable<(Tk, Tv)> Entiries()
        {
            foreach (var (key, values) in Values)
            {
                foreach (var value in values)
                {
                    yield return (key, value);
                }
            }
        }

        public bool ContainsEntry(Tk key, Tv value)
        {
            var current = Values.GetValueOrDefault(key, new());

            return current.Contains(value);
        }

        public bool ContainsValue(Tv value)
        {
            return Keys().Any(key => ContainsEntry(key, value));
        }
    }

    public class ORSet<T> where T: notnull
    {
        public MultiHashSet<T, Guid> Observed { get; private set; }
        public MultiHashSet<T, Guid> Removed { get; private set; }

        public HashSet<T> Values => Observed.Values
            .Where(kv =>
               {
                   return kv.Value.Except(Removed.Values.GetValueOrDefault(kv.Key, new())).Any();
               })
            .Select(kv => kv.Key)
            .ToHashSet();

        public ORSet(MultiHashSet<T, Guid>? observed = null, MultiHashSet<T, Guid>? removed = null)
        {
            Observed = observed ?? new();
            Removed = removed ?? new();
        }

        public Guid Add(T element, Guid? tag = null)
        {
            var tagid = tag ?? Guid.NewGuid();
            Observed.Put(element, tagid);

            return tagid;
        }

        public void Remove(T element, Guid tag)
        {
            Removed.Put(element, tag);
        }

        public void Merge(ORSet<T> other)
        {
            Observed.Merge(other.Observed);
            Removed.Merge(other.Removed);
        }
    }
}
