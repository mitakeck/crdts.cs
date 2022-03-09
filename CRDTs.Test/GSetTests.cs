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
    public class GSetTests
    {
        [TestMethod()]
        public void AddTest()
        {
            GSet<string> set = new();

            set.Add("A");

            Assert.AreEqual(1, set.Values.Count);
            Assert.IsTrue(set.Values.Contains("A"));
        }

        [TestMethod()]
        public void AddTest2()
        {
            GSet<string> set = new();

            set.Add("A");
            set.Add("A");

            Assert.AreEqual(1, set.Values.Count);
            Assert.IsTrue(set.Values.Contains("A"));
            Assert.IsFalse(set.Values.Contains("B"));
        }

        [TestMethod()]
        public void AddTest3()
        {
            GSet<string> set = new();

            set.Add("A");
            set.Add("A");
            set.Add("B");

            Assert.AreEqual(2, set.Values.Count);
            Assert.IsTrue(set.Values.Contains("A"));
            Assert.IsTrue(set.Values.Contains("B"));
            Assert.IsFalse(set.Values.Contains("C"));
        }

        [TestMethod()]
        public void MergeTest()
        {
            GSet<string> set1 = new();
            GSet<string> set2 = new();

            set1.Add("A");
            set2.Add("A");

            set1.Merge(set2);

            Assert.AreEqual(1, set1.Values.Count);
            Assert.IsTrue(set1.Values.Contains("A"));
            Assert.IsFalse(set1.Values.Contains("B"));
        }

        [TestMethod()]
        public void MergeTest2()
        {
            GSet<string> set1 = new();
            GSet<string> set2 = new();

            set1.Add("A");
            set2.Add("B");

            set1.Merge(set2);

            Assert.AreEqual(2, set1.Values.Count);
            Assert.IsTrue(set1.Values.Contains("A"));
            Assert.IsTrue(set1.Values.Contains("B"));
            Assert.IsFalse(set1.Values.Contains("C"));
        }


        [TestMethod()]
        public void MergeTest3()
        {
            GSet<string> set1 = new();
            GSet<string> set2 = new();
            GSet<string> set3 = new();
            GSet<string> set4 = new();

            set1.Add("A");
            set2.Add("B");
            set3.Add("C");
            set4.Add("D");

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