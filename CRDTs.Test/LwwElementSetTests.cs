using Microsoft.VisualStudio.TestTools.UnitTesting;
using CRDTs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRDTs.Tests
{
    [TestClass()]
    public class LwwElementSetTests
    {
        [TestMethod()]
        public void LwwElementSetTest()
        {
            var set = new LwwElementSet<string>();

            Assert.AreEqual(0, set.Values.Count);
            Assert.IsFalse(set.Values.Contains("A"));
        }

        [TestMethod()]
        public void AddTest()
        {
            var set = new LwwElementSet<string>();

            set.Add("A");

            Assert.AreEqual(1, set.Values.Count);
            Assert.IsTrue(set.Values.Contains("A"));
        }

        [TestMethod()]
        public void AddTest2()
        {
            var set = new LwwElementSet<string>();

            set.Add("A", new TimeStamp(100));
            set.Add("B", new TimeStamp(101));

            Assert.AreEqual(2, set.Values.Count);
            Assert.IsTrue(set.Values.Contains("A"));
            Assert.IsTrue(set.Values.Contains("B"));
            Assert.IsFalse(set.Values.Contains("C"));
        }

        [TestMethod()]
        public void RemoveTest()
        {
            var set = new LwwElementSet<string>();

            set.Add("A", new TimeStamp(100));
            set.Add("B", new TimeStamp(101));
            set.Remove("B", new TimeStamp(102));

            Assert.AreEqual(1, set.Values.Count);
            Assert.IsTrue(set.Values.Contains("A"));
            Assert.IsFalse(set.Values.Contains("B"));
            Assert.IsFalse(set.Values.Contains("C"));
        }

        [TestMethod()]
        public void RemoveTest2()
        {
            var set = new LwwElementSet<string>();

            set.Add("A", new TimeStamp(100));
            set.Remove("B", new TimeStamp(100));
            set.Add("B", new TimeStamp(101));

            Assert.AreEqual(2, set.Values.Count);
            Assert.IsTrue(set.Values.Contains("A"));
            Assert.IsTrue(set.Values.Contains("B"));
            Assert.IsFalse(set.Values.Contains("C"));
        }

        [TestMethod()]
        public void RemoveTest3()
        {
            var set = new LwwElementSet<string>();

            set.Add("A", new TimeStamp(100));
            set.Remove("B", new TimeStamp(100));
            set.Add("C", new TimeStamp(101));

            Assert.AreEqual(2, set.Values.Count);
            Assert.IsTrue(set.Values.Contains("A"));
            Assert.IsFalse(set.Values.Contains("B"));
            Assert.IsTrue(set.Values.Contains("C"));
        }

        [TestMethod()]
        public void MergeTest()
        {
            var set1 = new LwwElementSet<string>();
            var set2 = new LwwElementSet<string>();

            set1.Add("A");
            set2.Add("B");

            set1.Merge(set2);

            Assert.AreEqual(2, set1.Values.Count);
            Assert.IsTrue(set1.Values.Contains("A"));
            Assert.IsTrue(set1.Values.Contains("B"));
            Assert.IsFalse(set1.Values.Contains("C"));
        }

        [TestMethod()]
        public void MergeTest2()
        {
            var set1 = new LwwElementSet<string>();
            var set2 = new LwwElementSet<string>();

            set1.Add("A", new TimeStamp(100));
            set2.Remove("A", new TimeStamp(101));

            set1.Merge(set2);

            Assert.AreEqual(0, set1.Values.Count);
            Assert.IsFalse(set1.Values.Contains("A"));
            Assert.IsFalse(set1.Values.Contains("B"));
            Assert.IsFalse(set1.Values.Contains("C"));
        }

        [TestMethod()]
        public void MergeTest3()
        {
            var set1 = new LwwElementSet<string>();
            var set2 = new LwwElementSet<string>();

            set1.Add("A", new TimeStamp(100));
            set2.Remove("A", new TimeStamp(99));

            set1.Merge(set2);

            Assert.AreEqual(1, set1.Values.Count);
            Assert.IsTrue(set1.Values.Contains("A"));
            Assert.IsFalse(set1.Values.Contains("B"));
            Assert.IsFalse(set1.Values.Contains("C"));
        }

        [TestMethod()]
        public void MergeTest4()
        {
            var set1 = new LwwElementSet<string>();
            var set2 = new LwwElementSet<string>();

            set1.Add("A", new TimeStamp(100));
            set1.Add("B", new TimeStamp(100));
            set1.Add("C", new TimeStamp(100));

            set2.Add("D", new TimeStamp(100));
            set2.Remove("A", new TimeStamp(200));
            set2.Remove("B", new TimeStamp(200));
            set2.Remove("C", new TimeStamp(200));

            set1.Merge(set2);

            Assert.AreEqual(1, set1.Values.Count);
            Assert.IsFalse(set1.Values.Contains("A"));
            Assert.IsFalse(set1.Values.Contains("B"));
            Assert.IsFalse(set1.Values.Contains("C"));
            Assert.IsTrue(set1.Values.Contains("D"));
        }

        [TestMethod()]
        public void MergeTest5()
        {
            var set1 = new LwwElementSet<string>();
            var set2 = new LwwElementSet<string>();

            set1.Add("A", new TimeStamp(100));
            set1.Add("B", new TimeStamp(100));
            set1.Add("C", new TimeStamp(100));

            set2.Add("D", new TimeStamp(99));
            set2.Remove("A", new TimeStamp(99));
            set2.Remove("B", new TimeStamp(99));
            set2.Remove("C", new TimeStamp(99));

            set1.Merge(set2);

            Assert.AreEqual(4, set1.Values.Count);
            Assert.IsTrue(set1.Values.Contains("A"));
            Assert.IsTrue(set1.Values.Contains("B"));
            Assert.IsTrue(set1.Values.Contains("C"));
            Assert.IsTrue(set1.Values.Contains("D"));
        }
    }
}