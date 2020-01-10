using Microsoft.VisualStudio.TestTools.UnitTesting;
using Canonical_form_for_equation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canonical_form_for_equation_test
{
    [TestClass]
    public class Equation_sort
    {
        Equation e;
        public Equation_sort()
        {
            e = new Equation();
        }

        [TestMethod]
        public void Sort1()
        {
            string line = "x + x^2 + x^99";
            string answer = "x^99 + x^2 + x";

            Assert.AreEqual(e.SortMembers(line), answer);
        }

        [TestMethod]
        public void Sort2()
        {
            string line = "x - x^2 - x^99";
            string answer = " - x^99 - x^2 + x";

            Assert.AreEqual(e.SortMembers(line), answer);
        }


        [TestMethod]
        public void Sort3()
        {
            string line = "x^10 + x^1 + x^2";
            string answer = "x^10 + x^2 + x";

            Assert.AreEqual(e.SortMembers(line), answer);
        }

        [TestMethod]
        public void Sort4()
        {
            string line = "x^y^2 + x^y^1 + x^2";
            string answer = "x^y^2 + x^y^1 + x^2";

            Assert.AreEqual(e.SortMembers(line), answer);
        }

        [TestMethod]
        public void Sort5()
        {
            string line = "x^2^y^1 + x^y^1 + x^2 + x^2^y^2";
            string answer = "x^2^y^2 + x^2^y^1 + x^y^1 + x^2";

            Assert.AreEqual(e.SortMembers(line), answer);
        }
    }
}
