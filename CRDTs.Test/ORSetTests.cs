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
    public class ORSetTests
    {

        [TestMethod()]
        public void AddTest()
        {
            var set = new ORSet<string>();
            var tag = Guid.NewGuid();

            set.Add("A", tag);

            Assert.AreEqual(1, set.Values.Count);
            Assert.IsTrue(set.Values.Contains("A"));
        }

        [TestMethod()]
        public void AddTest2()
        {
            var set = new ORSet<string>();

            set.Add("A");
            set.Add("A");

            Assert.AreEqual(1, set.Values.Count);
            Assert.IsTrue(set.Values.Contains("A"));
        }

        [TestMethod()]
        public void AddTest3()
        {
            var set = new ORSet<string>();

            set.Add("A");
            set.Add("A");
            set.Add("B");

            Assert.AreEqual(2, set.Values.Count);
            Assert.IsTrue(set.Values.Contains("A"));
            Assert.IsTrue(set.Values.Contains("B"));
            Assert.IsFalse(set.Values.Contains("C"));
        }

        [TestMethod()]
        public void RemoveTest()
        {
            var set = new ORSet<string>();

            set.Add("A");
            var tag = set.Add("B");
            set.Remove("B", tag);

            Assert.AreEqual(1, set.Values.Count);
            Assert.IsTrue(set.Values.Contains("A"));
        }

        [TestMethod()]
        public void RemoveTest2()
        {
            var set = new ORSet<string>();

            set.Add("A");
            set.Remove("B", Guid.NewGuid());

            Assert.AreEqual(1, set.Values.Count);
            Assert.IsTrue(set.Values.Contains("A"));
        }

        [TestMethod()]
        public void RemoveTest3()
        {
            var set = new ORSet<string>();

            var tag = Guid.NewGuid();
            set.Add("A", tag);
            set.Remove("B", tag);
            set.Add("B", tag);

            Assert.AreEqual(1, set.Values.Count);
            Assert.IsTrue(set.Values.Contains("A"));
        }

        [TestMethod()]
        public void MergeTest()
        {
            var set1 = new ORSet<string>();
            var set2 = new ORSet<string>();

            set1.Add("A");
            set2.Add("B");

            set1.Merge(set2);

            Assert.AreEqual(2, set1.Values.Count);
            Assert.IsTrue(set1.Values.Contains("A"));
            Assert.IsTrue(set1.Values.Contains("B"));
        }

        [TestMethod()]
        public void MergeTest2()
        {
            var set1 = new ORSet<string>();
            var set2 = new ORSet<string>();

            var tag = Guid.NewGuid();

            set1.Add("A", tag);
            set1.Add("B");
            set2.Remove("A", tag);

            set1.Merge(set2);
            set2.Merge(set1);

            Assert.AreEqual(1, set1.Values.Count);
            Assert.AreEqual(set1.Values.Count, set2.Values.Count);
            Assert.IsFalse(set1.Values.Contains("A"));
            Assert.IsFalse(set2.Values.Contains("A"));
            Assert.IsTrue(set1.Values.Contains("B"));
            Assert.IsTrue(set2.Values.Contains("B"));
        }

        [TestMethod()]
        public void MergeTest3()
        {
            var set1 = new ORSet<string>();
            var set2 = new ORSet<string>();
            var set3 = new ORSet<string>();
            var set4 = new ORSet<string>();

            var tag1 = Guid.NewGuid();
            var tag2 = Guid.NewGuid();
            var tag3 = Guid.NewGuid();
            var tag4 = Guid.NewGuid();

            set1.Add("A", tag1);

            set2.Remove("A", tag2);
            set2.Add("B", tag2);

            set3.Remove("A", tag3);
            set3.Remove("B", tag3);
            set3.Add("C", tag3);

            set4.Remove("A", tag4);
            set4.Remove("B", tag4);
            set4.Remove("C", tag4);
            set4.Add("D", tag4);

            set1.Merge(set2);
            set1.Merge(set3);
            set1.Merge(set4);

            Assert.AreEqual(4, set1.Values.Count);
            Assert.IsTrue(set1.Values.Contains("A"));
            Assert.IsTrue(set1.Values.Contains("B"));
            Assert.IsTrue(set1.Values.Contains("C"));
            Assert.IsTrue(set1.Values.Contains("D"));
        }
    }
}