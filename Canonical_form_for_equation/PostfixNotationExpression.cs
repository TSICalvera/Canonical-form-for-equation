using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Canonical_form_for_equation
{
    public class PostfixNotationExpression
    {
        public PostfixNotationExpression()
        {
            operators = new List<string>(new string[] { "(", ")", "+", "-", "*", "/", "^" });
        }
        private List<string> operators;

        private IEnumerable<string> Separate(string input)
        {
            int pos = 0;
            while (pos < input.Length)
            {
                string s = string.Empty + input[pos];
                if (s == "(" && (input[pos + 1] == '-' || input[pos + 1] == '+'))
                {
                    s = "";
                    if (input[pos + 1] == '+')
                        pos += 2;
                    else pos++;
                    while (input[pos] != ')') // ('vars')
                    {
                        s += input[pos++];
                    }
                    pos++;
                    yield return s;
                    continue;
                }

                if (!operators.Contains(input[pos].ToString()))
                {
                    if (Char.IsDigit(input[pos]) || Char.IsLetter(input[pos]))
                        for (int i = pos + 1; i < input.Length &&
                            (Char.IsDigit(input[i]) || (Char.IsLetter(input[i])) || input[i] == ',' || input[i] == '.'); i++)
                            s += input[i];
                    else throw new Exception($"Char \'{input[pos]}\' is not correct");
                }
                yield return s;
                pos += s.Length;
            }
        }
        public IEnumerable<string> SplitEqualation(string input)
        {
            if (input == "")
                yield return "";

            string temp = input[0] == '-' ? "-" : "+";
            for (int i = 0; i != input.Length; i++)
            {
                if (!operators.Where(x => x != "^").ToList().Contains(input[i].ToString()))
                {
                    temp += input[i];
                    if (i != input.Length - 1)
                        continue;
                }
                else if (temp == "-" || temp == "+") continue;

                yield return temp;
                temp = input[i] == '-' ? "-" : "+";
            }
        }

        private byte GetPriority(string s)
        {
            switch (s)
            {
                case "(":
                case ")":
                    return 0;
                case "+":
                case "-":
                    return 1;
                case "*":
                case "/":
                    return 2;
                case "^":
                    return 3;
                default:
                    return 4;
            }
        }

        private (double, string) GetPairCoeffAndVar(string str)
        {
            string temp = "";
            double coefficient = 1.0;

            Regex regex = new Regex(@"^[\+,\-]?\d+[\.|\,]?\d*");
            if (regex.IsMatch(str))
            {
                string match = regex.Match(str).Value;
                coefficient = Convert.ToDouble(match);
                temp = str.Substring(match.Length, str.Length - match.Length);
            }
            else
            {
                coefficient = str[0] == '-' ? -1 : 1;
                temp = str[0] == '-' || str[0] == '+' ? str.Substring(1, str.Length - 1) : str;
            }

            if (temp.Length > 1)// replace yx => xy
            {
                foreach (var s in temp.Split('^'))
                {
                    Regex reg = new Regex(@"\D+");
                    if (reg.IsMatch(s))
                    {
                        char[] tmpArr = reg.Match(s).ToString().ToCharArray();
                        Array.Sort(tmpArr);
                        temp = temp.Replace(reg.Match(s).ToString(), new String(tmpArr));
                    }
                }
            }

            #region Checking Is input string equalation
            string strCheck = "";
            if (temp.Contains("^"))
                strCheck = temp.Substring(0, temp.IndexOf("^"));
            else strCheck = temp;

            if (SplitEqualation(str).Count() > 1) //2+x
                return (1, str);
            #endregion

            return (coefficient, temp);
        }
        public (double, string, string) GetPairCoeffAndVarAndLevel(string str)
        {
            var temp = GetPairCoeffAndVar(str);

            double coefficient = temp.Item1;
            int indexLevel = temp.Item2.IndexOf("^");

            string v = "", level = "1";
            if (indexLevel != -1)
            {
                v = temp.Item2.Substring(0, indexLevel);
                level = temp.Item2.Substring(indexLevel + 1, temp.Item2.Length - (indexLevel + 1));
            }
            else v = temp.Item2;

            return (coefficient, v, level);
        }

        public string[] ConvertToPostfixNotation(string input)
        {
            List<string> outputSeparated = new List<string>();
            Stack<string> stack = new Stack<string>();
            foreach (string c in Separate(input))
            {
                if (operators.Contains(c))
                {
                    if (stack.Count > 0 && !c.Equals("("))
                    {
                        if (c.Equals(")"))
                        {
                            string s = stack.Pop();
                            while (s != "(")
                            {
                                outputSeparated.Add(s);
                                s = stack.Pop();
                            }
                        }
                        else if (GetPriority(c) > GetPriority(stack.Peek()))
                            stack.Push(c);
                        else
                        {
                            while (stack.Count > 0 && GetPriority(c) <= GetPriority(stack.Peek()))
                                outputSeparated.Add(stack.Pop());
                            stack.Push(c);
                        }
                    }
                    else
                        stack.Push(c);
                }
                else
                    outputSeparated.Add(c);
            }
            if (stack.Count > 0)
                foreach (string c in stack)
                    outputSeparated.Add(c);

            return outputSeparated.ToArray();
        }
        public string result(string input)
        {
            Stack<string> stack = new Stack<string>();
            Queue<string> queue = new Queue<string>(ConvertToPostfixNotation(input));

            if (queue.Count() <= 2)
            {
                if (input.StartsWith("("))
                    return input.Substring(1, input.Length - 2);
                else return input;
            }

            string str = queue.Dequeue();
            while (queue.Count >= 0)
            {
                if (!operators.Contains(str))
                {
                    stack.Push(str);
                    str = queue.Dequeue();
                }
                else
                {
                    string summ = "";
                    try
                    {
                        switch (str)
                        {
                            case "+":
                                {
                                    string a = stack.Pop();
                                    string b = stack.Pop();

                                    var A = GetPairCoeffAndVar(a);
                                    var B = GetPairCoeffAndVar(b);

                                    if (A.Item2 == B.Item2)
                                    {
                                        double tmp = B.Item1 + A.Item1;

                                        if (tmp == 0)
                                            summ = "0";
                                        else if (Math.Abs(tmp) == 1 && (A.Item2 != "" || B.Item2 != ""))
                                            summ = tmp >= 0 ? $"{A.Item2}" : $"-{A.Item2}";
                                        else summ = $"{tmp}{A.Item2}";
                                    }
                                    else if (SplitEqualation(b).Count() > 1 || SplitEqualation(a).Count() > 1)  //string a or b contains +-/^* ?
                                    {
                                        if (SplitEqualation(a).Count() > 1)
                                        {
                                            summ = result($"({a}+{b})");
                                            break;
                                        }

                                        bool flagIsContains = false;
                                        var pairA = GetPairCoeffAndVarAndLevel(a);

                                        foreach (var member in SplitEqualation(b))
                                        {
                                            var pairB = GetPairCoeffAndVarAndLevel(member);
                                            if (!flagIsContains && pairA.Item2 == pairB.Item2 && pairA.Item3 == pairB.Item3)
                                            {
                                                flagIsContains = true;

                                                double tmp = pairB.Item1 + pairA.Item1;

                                                if (pairA.Item3 != "1")
                                                {
                                                    if (tmp == 0)
                                                        summ += summ == "" ? "0" : "+0";
                                                    else if (Math.Abs(tmp) == 1)
                                                    {
                                                        if (summ == "")
                                                            summ += tmp >= 0 ? $"{pairA.Item2}^{pairA.Item3}" : $"-{pairA.Item2}^{pairA.Item3}";
                                                        else summ += tmp >= 0 ? $"+{pairA.Item2}^{pairA.Item3}" : $"-{pairA.Item2}^{pairA.Item3}";
                                                    }
                                                    else if (summ == "" || tmp <= 0)
                                                        summ += $"{tmp}{pairA.Item2}^{pairA.Item3}";
                                                    else summ += $"+{tmp}{pairA.Item2}^{pairA.Item3}";
                                                }
                                                else
                                                {
                                                    if (tmp == 0)
                                                        summ += summ == "" ? "0" : "+0";
                                                    else if (Math.Abs(tmp) == 1)
                                                    {
                                                        if (summ == "")
                                                            summ += tmp >= 0 ? $"{pairA.Item2}" : $"-{pairA.Item2}";
                                                        else summ += tmp >= 0 ? $"+{pairA.Item2}" : $"-{pairA.Item2}";
                                                    }
                                                    else if (summ == "" || tmp <= 0)
                                                        summ += $"{tmp}{pairA.Item2}";
                                                    else summ += $"+{tmp}{pairA.Item2}";
                                                }
                                                continue;
                                            }

                                            summ += summ == "" && member[0] != '-' ? member.Substring(1, member.Length - 1) : $"{member}";
                                        }

                                        if (!flagIsContains)
                                            summ = $"{b}+{a}";
                                    }
                                    else summ = $"{b}+{a}";

                                    break;
                                }
                            case "-":
                                {
                                    string a = stack.Pop();
                                    string b = stack.Pop();

                                    var A = GetPairCoeffAndVar(a);
                                    var B = GetPairCoeffAndVar(b);

                                    if (A.Item2 == B.Item2)
                                    {
                                        double tmp = B.Item1 - A.Item1;
                                        if (tmp == 0)
                                            summ = "0";
                                        else if (Math.Abs(tmp) == 1 && (A.Item2 != "" || B.Item2 != ""))
                                            summ = tmp >= 0 ? $"{A.Item2}" : $"-{A.Item2}";
                                        else summ = $"{tmp}{A.Item2}";
                                    }
                                    else if (SplitEqualation(b).Count() > 1 || SplitEqualation(a).Count() > 1)  //string a or b contains +-/^* ?
                                    {
                                        if (SplitEqualation(a).Count() > 1)
                                        {
                                            summ = result($"(-1)*({a}-{b})");
                                            break;
                                        }

                                        bool flagIsContains = false;
                                        var pairA = GetPairCoeffAndVarAndLevel(a);

                                        foreach (var member in SplitEqualation(b))
                                        {
                                            var pairB = GetPairCoeffAndVarAndLevel(member);
                                            if (!flagIsContains && pairA.Item2 == pairB.Item2 && pairA.Item3 == pairB.Item3)
                                            {
                                                flagIsContains = true;

                                                double tmp = pairB.Item1 - pairA.Item1;

                                                if (pairA.Item3 != "1")
                                                {
                                                    if (tmp == 0)
                                                        summ += summ == "" ? "0" : "+0";
                                                    else if (Math.Abs(tmp) == 1)
                                                    {
                                                        if (summ == "")
                                                            summ += tmp >= 0 ? $"{pairA.Item2}^{pairA.Item3}" : $"-{pairA.Item2}^{pairA.Item3}";
                                                        else summ += tmp >= 0 ? $"+{pairA.Item2}^{pairA.Item3}" : $"-{pairA.Item2}^{pairA.Item3}";
                                                    }
                                                    else if (summ == "" || tmp <= 0)
                                                        summ += $"{tmp}{pairA.Item2}^{pairA.Item3}";
                                                    else summ += $"+{tmp}{pairA.Item2}^{pairA.Item3}";
                                                }
                                                else
                                                {
                                                    if (tmp == 0)
                                                        summ += summ == "" ? "0" : "+0";
                                                    else if (Math.Abs(tmp) == 1)
                                                    {
                                                        if(summ == "")
                                                            summ += tmp >= 0 ? $"{pairA.Item2}" : $"-{pairA.Item2}";
                                                        else summ += tmp >= 0 ? $"+{pairA.Item2}" : $"-{pairA.Item2}";
                                                    }
                                                    else if (summ == "" || tmp <= 0)
                                                        summ += $"{tmp}{pairA.Item2}";
                                                    else summ += $"+{tmp}{pairA.Item2}";
                                                }
                                                continue;
                                            }

                                            summ += summ == "" ? member.Substring(1, member.Length - 1) : $"{member}";
                                        }

                                        if (!flagIsContains)
                                            summ = $"{b}-{a}";
                                    }
                                    else if (SplitEqualation(a).Count() > 1 || SplitEqualation(b).Count() > 1)  //string a or b contains +-/^* ?
                                    {
                                        bool flagIsContains = false;
                                        var pairA = GetPairCoeffAndVarAndLevel(a);

                                        foreach (var member in SplitEqualation(b))
                                        {
                                            var pairB = GetPairCoeffAndVarAndLevel(member);
                                            if (!flagIsContains && pairA.Item2 == pairB.Item2 && pairA.Item3 == pairB.Item3)
                                            {
                                                flagIsContains = true;

                                                double tmp = pairB.Item1 - pairA.Item1;

                                                if (pairA.Item3 != "1")
                                                {
                                                    if (tmp == 0)
                                                        summ += summ == "" ? "0" : "+0";
                                                    else
                                                    {
                                                        if (summ != "" && tmp > 0 && tmp != 1)
                                                            summ += $"+{tmp}{pairA.Item2}^{pairA.Item3}";
                                                        if (summ != "" && tmp == 1)
                                                            summ += $"+{pairA.Item2}^{pairA.Item3}";
                                                        else summ += $"{tmp}{pairA.Item2}^{pairA.Item3}";
                                                    }
                                                }
                                                else
                                                {
                                                    if (tmp == 0)
                                                        summ += summ == "" ? "0" : "+0";
                                                    else if (summ == "" || tmp < 0 && tmp != 1)
                                                        summ += $"{tmp}{pairA.Item2}";
                                                    else if (summ == "" || tmp == 1)
                                                        summ += $"{pairA.Item2}";
                                                    else
                                                        summ += tmp == 0 ? "+0" : $"+{tmp}{pairA.Item2}";
                                                }
                                                continue;
                                            }

                                            summ += summ == "" ? member.Substring(1, member.Length - 1) : $"{member}";
                                        }

                                        if (!flagIsContains)
                                            summ = $"{b}-{a}";
                                    }
                                    else
                                    {
                                        if (a[0] == '-')
                                        {
                                            a = a.Substring(1, a.Length - 1);
                                            summ = $"{b}+{a}";
                                        }
                                        else summ = $"{b}-{a}";
                                    }

                                    break;
                                }
                            case "*":
                                {
                                    string a = stack.Pop();
                                    string b = stack.Pop();

                                    if (SplitEqualation(b).Count() > 1) // d * ax+by+cz
                                    {
                                        foreach (var member in SplitEqualation(b))
                                        {
                                            if (summ != "" && a[0] != '-')
                                                summ += member[0].ToString();

                                            if (a[0] == '-')
                                            {
                                                var temp = result($"({a})*({member})");
                                                if (summ == "" || !(temp[0] != '-' && temp[0] != '+'))
                                                    summ += temp;
                                                else summ += $"+{temp}";

                                            }
                                            else summ += $"{result($"{a}*{member.Substring(1, member.Length - 1)}")}";
                                        }
                                    }
                                    else if (SplitEqualation(a).Count() > 1) // ax+by+cz * d
                                    {
                                        foreach (var member in SplitEqualation(a))
                                        {
                                            if (summ != "" && b[0] != '-')
                                                summ += member[0].ToString();

                                            if (b[0] == '-')
                                            {
                                                var temp = result($"({b})*({member})");
                                                if (summ == "" || !(temp[0] != '-' && temp[0] != '+'))
                                                    summ += temp;
                                                else summ += $"+{temp}";

                                            }
                                            else summ += $"{result($"{b}*{member.Substring(1, member.Length - 1)}")}";
                                        }
                                    }
                                    else if (a.Contains("^") || b.Contains("^"))
                                    {
                                        var A = GetPairCoeffAndVarAndLevel(a);
                                        var B = GetPairCoeffAndVarAndLevel(b);

                                        double tmp = B.Item1 * A.Item1;
                                        if (tmp == 0)
                                        {
                                            summ = "0";
                                            break;
                                        }
                                        else if (Math.Abs(tmp) == 1)
                                            summ = tmp >= 0 ? "+" : "-";
                                        else summ = tmp.ToString();

                                        if (A.Item2 == B.Item2)
                                        {
                                            if (A.Item3.Contains("-"))
                                            {
                                                string temp = result($"{B.Item3}{A.Item3}");
                                                summ += SplitEqualation(temp).Count() > 1 ? $"{A.Item2}^({temp})" : $"{A.Item2}^{temp}";
                                            }
                                            else
                                            {
                                                string temp = result($"{B.Item3}+{A.Item3}");
                                                summ += SplitEqualation(temp).Count() > 1 ? $"{A.Item2}^({temp})" : $"{A.Item2}^{temp}";
                                            }
                                        }
                                        else if (A.Item2 == "")
                                            summ += $"{B.Item2}^{B.Item3}";
                                        else if (B.Item2 == "")
                                            summ += $"{A.Item2}^{A.Item3}";
                                        else summ = $"{b}*{a}";
                                    }
                                    else
                                    {
                                        var A = GetPairCoeffAndVar(a);
                                        var B = GetPairCoeffAndVar(b);

                                        double tmp = B.Item1 * A.Item1;
                                        if (tmp == 0)
                                        {
                                            summ = "0";
                                            break;
                                        }
                                        else if (Math.Abs(tmp) == 1 && (A.Item2 != "" || B.Item2 != ""))
                                            summ = tmp >= 0 ? "" : "-";
                                        else summ = tmp.ToString();

                                        if (A.Item2 == B.Item2 && A.Item2 != "")
                                            summ += $"{A.Item2}^2";
                                        else if (A.Item2 == "")
                                            summ += $"{B.Item2}";
                                        else if (B.Item2 == "")
                                            summ += $"{A.Item2}";
                                        else
                                        {
                                            var coef = A.Item1 * B.Item1;
                                            string pref = "";
                                            if (coef < 0)
                                                pref = "-";
                                            
                                            if(Math.Abs(coef) == 1)
                                            {
                                                if (B.Item2 == "" && A.Item2 == "")
                                                    summ = $"{pref}{coef}";
                                                else summ = $"{pref}{B.Item2}{A.Item2}";
                                            }
                                            else summ = $"{pref}{coef}{B.Item2}{A.Item2}";
                                        };

                                    }
                                    break;
                                }
                            case "/":
                                {
                                    string a = stack.Pop();
                                    string b = stack.Pop();

                                    var A = GetPairCoeffAndVarAndLevel(a);
                                    var B = GetPairCoeffAndVarAndLevel(b);

                                    if (A.Item2 == B.Item2)
                                    {
                                        summ = $"{B.Item1 * A.Item1}{A.Item2}^{result($"(-1)*({A.Item3})+{B.Item3}")}";
                                    }
                                    else summ = result($"{b}*{a}^(-1)");

                                    break;
                                }
                            case "^":
                                {
                                    string a = stack.Pop();
                                    string b = stack.Pop();

                                    var A = GetPairCoeffAndVar(a);
                                    var B = GetPairCoeffAndVar(b);

                                    if (A.Item2 == "" && B.Item2 == "")
                                    {
                                        summ = Math.Pow(Convert.ToDouble(B.Item1), Convert.ToDouble(A.Item1)).ToString();
                                    }
                                    else if (Separate(a).Where(x => operators.Contains(x)).Count() > 0)  //string a contains +-*/^ ?
                                    {
                                        summ = $"{b}^({a})";
                                    }
                                    else summ = $"{b}^{a}";

                                    break;
                                }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    stack.Push(summ.ToString());
                    if (queue.Count > 0)
                        str = queue.Dequeue();
                    else
                        break;
                }

            }

            return stack.Pop().Replace("+0", "").Replace("-0", "");
        }
    }
}
