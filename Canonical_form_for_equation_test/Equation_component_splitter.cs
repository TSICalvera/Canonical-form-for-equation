using Microsoft.VisualStudio.TestTools.UnitTesting;
using Canonical_form_for_equation;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Canonical_form_for_equation_test
{
    [TestClass]
    public class Equation_component_splitter
    {
        [TestMethod]
        public void Split()
        {
            PostfixNotationExpression p = new PostfixNotationExpression();

            var expected = new string[]
            {
                 "+2x",
                 "+2y",
                 "-2z"
            };
            var actual = p.SplitEqualation("2x+2y-2z").ToArray();
            CollectionAssert.AreEqual(expected, actual);

            expected = new string[]
            {
                 "-2x",
                 "-2y",
                 "+2z"
            };
            actual = p.SplitEqualation("-2x-2y+2z").ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
