using System;
using System.Collections.Generic;
using System.Linq;

namespace Canonical_form_for_equation
{
    public class Equation
    {
        PostfixNotationExpression p;
        private string equation = "";

        private string calcEquation(string input)
        {
            if (input.StartsWith("-"))
                input = $"(-1)*{input.Substring(1, input.Length - 1)}";

            return p.result(input).Replace("+", " + ").Replace("-", " - ").Replace("*", " * ");
        }

        public Equation()
        {
            p = new PostfixNotationExpression();
        }

        public Equation(string str)
        {
            p = new PostfixNotationExpression();

            var tmp = str.Replace(" ", "").Split('=');
            if (tmp.Length != 2)
                throw new Exception("Equation is not correct");
            else if (tmp[0].Count(x => x == '(') != tmp[0].Count(x => x == ')'))
                throw new Exception("Left part equaion is not correct");
            else if (tmp[1].Count(x => x == '(') != tmp[1].Count(x => x == ')'))
                throw new Exception("Right part equaion is not correct");

            if (tmp[0] == "0")
                equation = $"{calcEquation(tmp[1])} = 0";
            else if (tmp[1] == "0")
                equation = $"{calcEquation(tmp[0])} = 0";
            else
            {
                var eqLeft = calcEquation(tmp[0]).Replace(" ", "");
                var eqRight = calcEquation($"(-1)*({tmp[1]})").Replace(" ", "");

                var eqCommon = calcEquation(eqLeft + (eqRight[0] == '-' ? eqRight : $"+{eqRight}"));

                equation = $"{eqCommon} = 0";
            }

            equation = SortMembers(equation);
        }

        public string SortMembers(string line)
        {
            if (line == "")
                return "";
            else if (line.Contains("="))
            {
                var temp = line.Split('=');
                return $"{SortMembers(temp[0])} = {SortMembers(temp[1])}";
            }

            List<(double, string, string)> members = new List<(double, string, string)>();
            foreach (var str in p.SplitEqualation(line.Replace(" ", "")))
            {
                members.Add(p.GetPairCoeffAndVarAndLevel(str));
            }

            string result = "";

            foreach (var v in members.OrderByDescending(x => x.Item3.Split('^').Length).ThenByDescending(y => p.GetPairCoeffAndVarAndLevel(y.Item3).Item1).ThenByDescending(y => y.Item3))
            {
                string prefix = result == "" || v.Item1 < 0 ? "" : "+";
                string level = v.Item3 == "1" ? "" : $"^{v.Item3}";
                string varification = v.Item2;
                string coefficient = Math.Abs(v.Item1) == 1 && (varification != "" || level != "") ?
                    v.Item1 == -1 ? "-" : "" :
                    v.Item1.ToString();

                result += $"{prefix}{coefficient}{varification}{level}";
            }

            return result.Replace("+", " + ").Replace("-", " - ").Replace("*", " * ");
        }

        public string GetEquation() => equation;
    }
}
