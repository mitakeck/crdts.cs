using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRDTs
{
    public record ElementStatus {
        public bool Removed { get; private set; }
        public ElementStatus(bool removed)
        {
            Removed = removed;
        }

        public static ElementStatus Active = new(false);
        public static ElementStatus Disable = new(true);
    }

    public record Element<T>
    {
        public T Value { get; private set; }
        public TimeStamp TimeStamp { get; private set; }
        public ElementStatus Status { get; private set; }

        public Element(T value, ElementStatus status, TimeStamp? timestamp = null)
        {
            Value = value;
            Status = status;
            TimeStamp = timestamp ?? new();
        }

        private Element(TimeStamp? timestamp = null)
        {
            Value = default;
            Status = ElementStatus.Disable;
            TimeStamp = timestamp ?? new();
        }

        public static Element<T> EmptyElement(TimeStamp? timestamp) => new(timestamp);
    }

    public class OURSet<T>
    {
        public Dictionary<Guid, Element<T>> Sets { get; private set; }

        public HashSet<T> Values => Sets
            .Where(s => s.Value.Status == ElementStatus.Active)
            .Select(s => s.Value.Value)
            .ToHashSet();

        public OURSet(Dictionary<Guid, Element<T>>? set = null)
        {
            Sets = set ?? new();
        }

        public Guid Add(T element, TimeStamp? timestamp = null)
        {
            var current = timestamp ?? new();
            var tag = Guid.NewGuid();
            Sets[tag] = new(element, ElementStatus.Active, current);

            return tag;
        }

        public void Update(Guid tag, T element, TimeStamp? timestamp = null)
        {
            var current = timestamp ?? new();
            var target = Sets.GetValueOrDefault(tag);
            if (target == null)
            {
                Sets[tag] = new(element, ElementStatus.Active, current);
                return;
            }

            if (target.TimeStamp.Value < current.Value)
            {
                Sets[tag] = new(element, ElementStatus.Active, current);
            }
        }

        public void Remove(Guid tag, TimeStamp? timestamp = null)
        {
            var current = timestamp ?? new();
            var target = Sets.GetValueOrDefault(tag);

            if (target == null)
            {
                Sets[tag] = Element<T>.EmptyElement(current);
                return;
            }

            if (target.TimeStamp.Value <= current.Value)
            {
                Sets[tag] = new(target.Value, ElementStatus.Disable, current);
            }
        }

        public void Merge(OURSet<T> other)
        {
            Sets = Sets.Concat(other.Sets)
                .GroupBy(s => s.Key, s => s)
                .ToDictionary(
                    s => s.Key,
                    s => s.Where(a => a.Key == s.Key)
                        .Aggregate((l, r) =>
                        {
                            return (
                                l.Value.TimeStamp.CompareUfo(r.Value.TimeStamp),
                                l.Value.Status == ElementStatus.Disable
                            ) switch
                            {
                                (1, _) => l,
                                (0, true) => l,

                                _ => r,
                            };
                        })
                        .Value
                );
        }
    }
}
