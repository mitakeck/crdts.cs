using CRDTs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CRDTs.Tests
{
    [TestClass]
    public class GCounterTest
    {
        [TestMethod]
        public void Initialize()
        {
            var id = Guid.NewGuid();
            var counter = new GCounter(id);

            Assert.AreEqual(0, counter.Value);
            Assert.AreEqual(0, counter.Counters[id]);
        }

        [TestMethod]
        public void Increment()
        {
            var id = Guid.NewGuid();
            var counter = new GCounter(id);

            counter.Increment();

            Assert.AreEqual(1, counter.Value);
            Assert.AreEqual(1, counter.Counters[id]);
        }

        [TestMethod]
        public void Increment5()
        {
            var id = Guid.NewGuid();
            var counter = new GCounter(id);

            counter.Increment(5);

            Assert.AreEqual(5, counter.Value);
            Assert.AreEqual(5, counter.Counters[id]);
        }

        [TestMethod]
        public void Increment5x2()
        {
            var id = Guid.NewGuid();
            var counter = new GCounter(id);

            counter.Increment(5);
            counter.Increment(5);

            Assert.AreEqual(10, counter.Value);
            Assert.AreEqual(10, counter.Counters[id]);
        }

        [TestMethod]
        public void Merge()
        {
            var id = Guid.NewGuid();
            var counter1 = new GCounter(id);
            var counter2 = new GCounter(id);

            counter1.Increment();
            counter2.Increment();

            counter1.Merge(counter2);

            Assert.AreEqual(1, counter1.Value);
            Assert.AreEqual(1, counter1.Counters[id]);
        }

        [TestMethod]
        public void Merge2()
        {
            var id1 = Guid.NewGuid();
            var counter1 = new GCounter(id1);

            var id2 = Guid.NewGuid();
            var counter2 = new GCounter(id2);

            counter1.Increment();
            counter2.Increment();

            counter1.Merge(counter2);

            Assert.AreEqual(2, counter1.Value);
            Assert.AreEqual(1, counter1.Counters[id1]);
            Assert.AreEqual(1, counter1.Counters[id2]);
        }
    }
}
