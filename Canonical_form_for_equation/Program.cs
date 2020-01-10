using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canonical_form_for_equation
{
    class Program
    {
        static void Main(string[] args)
        {        
            while (true)
            {
                Console.Write("Enter equation or press Ctrl+C for exit: \n ");

                var line = Console.ReadLine();
                if (String.IsNullOrEmpty(line))
                    continue;

                Console.Write("Canonical form: ");
                try
                {
                    var eq = new Equation(line);
                    var equation = eq.GetEquation();
                    Console.WriteLine(equation.ToString()+"\n");
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }
        }
    }
}
