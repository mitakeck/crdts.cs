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
    public class GNSetTests
    {
        [TestMethod()]
        public void AddTest()
        {
            var gnset = new GNSet<string>();

            Assert.AreEqual(0, gnset.Values.Count);
        }

        [TestMethod()]
        public void AddTest2()
        {
            var gnset = new GNSet<string>();

            gnset.Add("A");
            gnset.Remove("A");

            Assert.AreEqual(0, gnset.Values.Count);
        }

        [TestMethod()]
        public void AddTest3()
        {
            var gnset = new GNSet<string>();

            gnset.Add("A");
            gnset.Remove("B");

            Assert.AreEqual(1, gnset.Values.Count);
            Assert.IsTrue(gnset.Values.Contains("A"));
            Assert.IsFalse(gnset.Values.Contains("B"));
        }

        [TestMethod()]
        public void MergeTest()
        {
            var gnset1 = new GNSet<string>();
            var gnset2 = new GNSet<string>();

            gnset1.Add("A");
            gnset2.Add("A");

            gnset1.Merge(gnset2);

            Assert.AreEqual(1, gnset1.Values.Count);
            Assert.IsTrue(gnset1.Values.Contains("A"));
            Assert.IsFalse(gnset1.Values.Contains("B"));
        }

        [TestMethod()]
        public void MergeTest2()
        {
            var gnset1 = new GNSet<string>();
            var gnset2 = new GNSet<string>();

            gnset1.Add("A");
            gnset2.Add("A");

            gnset1.Merge(gnset2);

            Assert.AreEqual(1, gnset1.Values.Count);
            Assert.IsTrue(gnset1.Values.Contains("A"));
            Assert.IsFalse(gnset1.Values.Contains("B"));

            gnset1.Add("B");
            gnset1.Merge(gnset2);

            Assert.AreEqual(2, gnset1.Values.Count);
            Assert.IsTrue(gnset1.Values.Contains("A"));
            Assert.IsTrue(gnset1.Values.Contains("B"));
        }

        [TestMethod()]
        public void MergeTest3()
        {
            var gnset1 = new GNSet<string>();
            var gnset2 = new GNSet<string>();

            gnset1.Add("A");
            gnset2.Add("A");
            gnset2.Remove("B");

            gnset1.Merge(gnset2);

            Assert.AreEqual(1, gnset1.Values.Count);
            Assert.IsTrue(gnset1.Values.Contains("A"));
            Assert.IsFalse(gnset1.Values.Contains("B"));

            gnset1.Add("B");
            gnset1.Merge(gnset2);

            Assert.AreEqual(1, gnset1.Values.Count);
            Assert.IsTrue(gnset1.Values.Contains("A"));
            Assert.IsFalse(gnset1.Values.Contains("B"));
        }
    }
}
