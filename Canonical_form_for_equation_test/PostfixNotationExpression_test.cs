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
    public class PostfixNotationExpression_test
    {
        [TestMethod]
        public void Parse_Test()
        {
            PostfixNotationExpression p = new PostfixNotationExpression();

            string line = "y ^ 2 - xy + y".Replace(" ", "");
            var answer = "y 2 ^ xy - y +".Split(' ');
            CollectionAssert.AreEqual(p.ConvertToPostfixNotation(line), answer);

            line = "x ^ 2 + 3.5xy + y".Replace(" ","");
            answer = "x 2 ^ 3.5xy + y +".Split(' ');
            CollectionAssert.AreEqual(p.ConvertToPostfixNotation(line), answer);

            line = "x - (0 - (0 - x))".Replace(" ", "");
            answer = "x 0 0 x - - -".Split(' ');
            CollectionAssert.AreEqual(p.ConvertToPostfixNotation(line), answer);
        }
    }
}
