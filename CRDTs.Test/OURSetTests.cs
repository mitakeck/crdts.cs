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
    public class OURSetTests
    {
        [TestMethod()]
        public void LwwElementSetTest()
        {
            var set = new OURSet<string>();

            Assert.AreEqual(0, set.Values.Count);
            Assert.IsFalse(set.Values.Contains("A"));
        }

        [TestMethod()]
        public void AddTest()
        {
            var set = new OURSet<string>();

            set.Add("A");

            Assert.AreEqual(1, set.Values.Count);
            Assert.IsTrue(set.Values.Contains("A"));
        }

        [TestMethod()]
        public void AddTest2()
        {
            var set = new OURSet<string>();

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
            var set = new OURSet<string>();

            set.Add("A", new TimeStamp(100));
            var tag = set.Add("B", new TimeStamp(101));
            set.Remove(tag, new TimeStamp(102));

            Assert.AreEqual(1, set.Values.Count);
            Assert.IsTrue(set.Values.Contains("A"));
            Assert.IsFalse(set.Values.Contains("B"));
            Assert.IsFalse(set.Values.Contains("C"));
        }

        [TestMethod()]
        public void RemoveTest2()
        {
            var set = new OURSet<string>();
            var tag = Guid.NewGuid();

            set.Add("A", new TimeStamp(100));
            set.Remove(tag, new TimeStamp(100));
            set.Add("B", new TimeStamp(101));

            Assert.AreEqual(2, set.Values.Count);
            Assert.IsTrue(set.Values.Contains("A"));
            Assert.IsTrue(set.Values.Contains("B"));
            Assert.IsFalse(set.Values.Contains("C"));
        }

        [TestMethod()]
        public void RemoveTest3()
        {
            var set = new OURSet<string>();
            var tag = Guid.NewGuid();

            set.Add("A", new TimeStamp(100));
            set.Remove(tag, new TimeStamp(100));
            set.Add("C", new TimeStamp(101));

            Assert.AreEqual(2, set.Values.Count);
            Assert.IsTrue(set.Values.Contains("A"));
            Assert.IsFalse(set.Values.Contains("B"));
            Assert.IsTrue(set.Values.Contains("C"));
        }

        [TestMethod()]
        public void RemoveTest4()
        {
            var set = new OURSet<string>();
            
            var tag = set.Add("A", new TimeStamp(100));
            set.Remove(tag, new TimeStamp(101));
            set.Add("C", new TimeStamp(102));
            set.Add("A", new TimeStamp(103));

            Assert.AreEqual(2, set.Values.Count);
            Assert.IsTrue(set.Values.Contains("A"));
            Assert.IsFalse(set.Values.Contains("B"));
            Assert.IsTrue(set.Values.Contains("C"));
        }

        [TestMethod]
        public void UpdateTest()
        {
            var set = new OURSet<string>();

            var tag = set.Add("A");
            set.Update(tag, "B");

            Assert.AreEqual(1, set.Values.Count);
            Assert.IsFalse(set.Values.Contains("A"));
            Assert.IsTrue(set.Values.Contains("B"));
        }

        [TestMethod]
        public void UpdateTest2()
        {
            var set = new OURSet<string>();

            var tag = set.Add("A");
            set.Remove(tag);
            set.Update(tag, "B");

            Assert.AreEqual(1, set.Values.Count);
            Assert.IsFalse(set.Values.Contains("A"));
            Assert.IsTrue(set.Values.Contains("B"));
        }

        [TestMethod()]
        public void MergeTest()
        {
            var set1 = new OURSet<string>();
            var set2 = new OURSet<string>();

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
            var set1 = new OURSet<string>();
            var set2 = new OURSet<string>();

            var tag = set1.Add("A", new TimeStamp(100));
            set2.Remove(tag, new TimeStamp(101));

            set1.Merge(set2);

            Assert.AreEqual(0, set1.Values.Count);
            Assert.IsFalse(set1.Values.Contains("A"));
            Assert.IsFalse(set1.Values.Contains("B"));
            Assert.IsFalse(set1.Values.Contains("C"));
        }

        [TestMethod()]
        public void MergeTest3()
        {
            var set1 = new OURSet<string>();
            var set2 = new OURSet<string>();

            var tag = set1.Add("A", new TimeStamp(100));
            set2.Remove(tag, new TimeStamp(99));

            set1.Merge(set2);

            Assert.AreEqual(1, set1.Values.Count);
            Assert.IsTrue(set1.Values.Contains("A"));
            Assert.IsFalse(set1.Values.Contains("B"));
            Assert.IsFalse(set1.Values.Contains("C"));
        }

        [TestMethod()]
        public void MergeTest4()
        {
            var set1 = new OURSet<string>();
            var set2 = new OURSet<string>();

            var tagA = set1.Add("A", new TimeStamp(100));
            var tagB = set1.Add("B", new TimeStamp(100));
            var tagC = set1.Add("C", new TimeStamp(100));

            set2.Add("D", new TimeStamp(100));
            set2.Remove(tagA, new TimeStamp(200));
            set2.Remove(tagB, new TimeStamp(200));
            set2.Remove(tagC, new TimeStamp(200));

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
            var set1 = new OURSet<string>();
            var set2 = new OURSet<string>();

            var tagA = set1.Add("A", new TimeStamp(100));
            var tagB = set1.Add("B", new TimeStamp(100));
            var tagC = set1.Add("C", new TimeStamp(100));

            set2.Add("D", new TimeStamp(99));
            set2.Remove(tagA, new TimeStamp(99));
            set2.Remove(tagB, new TimeStamp(99));
            set2.Remove(tagC, new TimeStamp(99));

            set1.Merge(set2);

            Assert.AreEqual(4, set1.Values.Count);
            Assert.IsTrue(set1.Values.Contains("A"));
            Assert.IsTrue(set1.Values.Contains("B"));
            Assert.IsTrue(set1.Values.Contains("C"));
            Assert.IsTrue(set1.Values.Contains("D"));
        }

        //////////////////////////////////

        [TestMethod]
        public void BookingTest()
        {
            var set1 = new OURSet<string>();
            var set2 = new OURSet<string>();

            var time = 100;

            var tag = set1.Add("A", new TimeStamp(time));
            set2.Remove(tag, new TimeStamp(time));

            set1.Merge(set2);
            set2.Merge(set1);

            Assert.AreEqual(set1.Values.Count, set2.Values.Count);
        }

        [TestMethod]
        public void BookingTest2()
        {
            var set1 = new OURSet<string>();
            var set2 = new OURSet<string>();
            var set3 = new OURSet<string>();

            var time = 100;

            var tag = set1.Add("A", new TimeStamp(time));
            set2.Remove(tag, new TimeStamp(time));
            set3.Add("A", new TimeStamp(time + 1));

            set1.Merge(set2);
            set2.Merge(set1);

            Assert.AreEqual(set1.Values.Count, set2.Values.Count);

            set1.Merge(set3);
            set2.Merge(set3);
            set3.Merge(set1);
            set3.Merge(set2);

            Assert.AreEqual(set1.Values.Count, set2.Values.Count);
            Assert.AreEqual(set2.Values.Count, set3.Values.Count);
        }
    }
}