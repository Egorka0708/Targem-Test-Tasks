using System;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            ParseEquation parser = new ParseEquation();
            SolveEquation calculator = new SolveEquation(parser);
            string equation = string.Join("", args);
            //string equation = Console.ReadLine();
            try
            {
                float result = calculator.Calculate(equation);
                Console.WriteLine(result);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"ERROR: {exception.Message}");
            }
        }
    }
}
