using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Canonical_form_for_equation;

namespace Canonical_form_for_equation_test
{
    [TestClass]
    public class Equation_operation
    {
        [TestMethod]
        public void Equation_sum()
        {
            PostfixNotationExpression p = new PostfixNotationExpression();

            Assert.AreEqual(p.result("2x+3x"), "5x");
            Assert.AreEqual(p.result("2x+1+3x"), "5x+1");
            Assert.AreEqual(p.result("2x+3x+1"), "5x+1");
            Assert.AreEqual(p.result("2x^2+3x"), "2x^2+3x");
            Assert.AreEqual(p.result("2x^(2^2)+3x^4"), "5x^4");
            Assert.AreEqual(p.result("2x+5+3x+1"), "5x+6");
            Assert.AreEqual(p.result("2x+3y"), "2x+3y");
            Assert.AreEqual(p.result("2x+y+3y+1"), "2x+4y+1");
            Assert.AreEqual(p.result("y^4x+2x+y^4+x"), "y^4x+3x+y^4");
        }

        [TestMethod]
        public void Equation_diff()
        {
            PostfixNotationExpression p = new PostfixNotationExpression();

            Assert.AreEqual(p.result("2x-3x"), "-x");
            Assert.AreEqual(p.result("3x-3x+1"), "1");
            Assert.AreEqual(p.result("3x-2x+1"), "x+1");
            Assert.AreEqual(p.result("2x^2-3x"), "2x^2-3x");
            Assert.AreEqual(p.result("3x^(2^2)-2x^4"), "x^4");
            Assert.AreEqual(p.result("2x+5-x-1"), "x+4");
            Assert.AreEqual(p.result("2x-y+3y-1"), "2x+2y-1");
            Assert.AreEqual(p.result("2x-5y+3y-1"), "2x-2y-1");
            Assert.AreEqual(p.result("2y^4x+2x-y^4x-x"), "y^4x+x");
        }

        [TestMethod]
        public void Equation_mul()
        {
            PostfixNotationExpression p = new PostfixNotationExpression();

            Assert.AreEqual(p.result("2x*3x"), "6x^2");
            Assert.AreEqual(p.result("2x*3x*0"), "0");
            Assert.AreEqual(p.result("(-1)*(2x*3x)"), "-6x^2");
            Assert.AreEqual(p.result("(-1)*(2x+3y)"), "-2x-3y");
            Assert.AreEqual(p.result("2x^2*3x"), "6x^3");
            Assert.AreEqual(p.result("2x*(1-3x)"), "2x-6x^2");
            Assert.AreEqual(p.result("(3x-2x)*1"), "x");
            Assert.AreEqual(p.result("(3x-2y)*1"), "3x-2y");
            Assert.AreEqual(p.result("(3x^2-2y)*1"), "3x^2-2y");
            Assert.AreEqual(p.result("2x^2*3x"), "6x^3");
            Assert.AreEqual(p.result("3x^(2^2)*2x^4"), "6x^8");
            Assert.AreEqual(p.result("x*(x+y-1)"), "x^2+xy-x");
        }

    }
}