using Canonical_form_for_equation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canonical_form_for_equation_test
{
    [TestClass]
    public class Equation_calculation
    {
        [TestMethod]
        public void Calculation1()
        {
            string line = "x ^ 2 + 3.5xy + y = y ^ 2 - xy + y";
            string answer = "x^2 - y^2 + 4.5xy = 0";

            Equation eq = new Equation(line);
            Assert.AreEqual(eq.GetEquation(), answer);
        }

        [TestMethod]
        public void Calculation2()
        {
            string line = "x = 1";
            string answer = "x - 1 = 0";

            Equation eq = new Equation(line);
            Assert.AreEqual(eq.GetEquation(), answer);
        }

        [TestMethod]
        public void Calculation3()
        {
            string line = "x - (y^2 - x) = 0";
            string answer = " - y^2 + 2x = 0";

            Equation eq = new Equation(line);
            Assert.AreEqual(eq.GetEquation(), answer);
        }

        [TestMethod]
        public void Calculation4()
        {
            string line = "x - (0 - (0 - x)) = 0";
            string answer = "0 = 0";

            Equation eq = new Equation(line);
            Assert.AreEqual(eq.GetEquation(), answer);
        }

        [TestMethod]
        public void Calculation5()
        {
            string line = "21x^x+5x^3+3y*4+(x^4)/x=2^4+25-100+10x";
            string answer = "6x^3 + 21x^x + 12y + 59 - 10x = 0";

            Equation eq = new Equation(line);
            Assert.AreEqual(eq.GetEquation(), answer);
        }

        [TestMethod]
        public void Calculation6()
        {
            string line = "4 * (x + xy) - 4x - 4y = 10 + 5 - 2xy";
            string answer = " - 15 + 6xy - 4y = 0";

            Equation eq = new Equation(line);
             Assert.AreEqual(eq.GetEquation(), answer);
        }

        [TestMethod]
        public void Calculation7()
        {
            string line = "x - x + 10 + y = 0";
            string answer = "10 + y = 0";

            Equation eq = new Equation(line);
            Assert.AreEqual(eq.GetEquation(), answer);
        }

        [TestMethod]
        public void Calculation8()
        {
            string line = "- x = 0";
            string answer = " - x = 0";

            Equation eq = new Equation(line);
            Assert.AreEqual(eq.GetEquation(), answer);
        }

        [TestMethod]
        public void Calculation9()
        {
            string line = "- x + 1 = 0";
            string answer = " - x + 1 = 0";

            Equation eq = new Equation(line);
            Assert.AreEqual(eq.GetEquation(), answer);
        }

        [TestMethod]
        public void Calculation10()
        {
            string line = "(x) = 0";
            string answer = "x = 0";

            Equation eq = new Equation(line);
            Assert.AreEqual(eq.GetEquation(), answer);
        }

        [TestMethod]
        public void Calculation11()
        {
            string line = "(x + 1) = 0";
            string answer = "x + 1 = 0";

            Equation eq = new Equation(line);
            Assert.AreEqual(eq.GetEquation(), answer);
        }

        [TestMethod]
        public void Calculation12()
        {
            string line = "(x^2) = 0";
            string answer = "x^2 = 0";

            Equation eq = new Equation(line);
            Assert.AreEqual(eq.GetEquation(), answer);
        }

        [TestMethod]
        public void Calculation13()
        {
            string line = "(- x) = 0";
            string answer = " - x = 0";

            Equation eq = new Equation(line);
            Assert.AreEqual(eq.GetEquation(), answer);
        }

        [TestMethod]
        public void Calculation14()
        {
            string line = "(x + 1) - y = 0";
            string answer = "x + 1 - y = 0";

            Equation eq = new Equation(line);
            Assert.AreEqual(eq.GetEquation(), answer);
        }

        [TestMethod]
        public void Calculation15()
        {
            string line = "2xy - yx = 0";
            string answer = "xy = 0";

            Equation eq = new Equation(line);
            Assert.AreEqual(eq.GetEquation(), answer);
        }
    }
}
