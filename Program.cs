using System;

namespace Parser_of_Mathematical_Expressions
{
    class Program
    {
        static void Main(string[] args)
        {
            bool showMenu = true;
            while (showMenu)
            {
                showMenu = MainMenu();
            }
        }
        private static bool MainMenu()
        {
            Console.Clear();
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1) Algebraic expression");
            Console.WriteLine("2) Boolean expression");
            Console.WriteLine("3) Matrices");
            Console.WriteLine("4) Polynomials");
            Console.WriteLine("5) Exit");
            Console.Write("\r\nSelect an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine("Enter the expression like in the example. Trigonometric functions, constants 'pi' and 'e', variables are allowed.");
                    Console.WriteLine("Example: (1.5 + sin(2.0 + cos(3.5)))*3");
                    Console.WriteLine();
                    string s = Console.ReadLine();
                    MathParser.StringParse(s);
                    Console.WriteLine();
                    Console.WriteLine("If you want to continue, press enter. Press 0 to exit.");
                    string exit = Console.ReadLine();
                    if (exit == "0")
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                case "2":
                    Console.WriteLine("Enter the expression like in the example. Constants '1' and '0' are allowed.");
                    Console.WriteLine("Example: (0 \\/ ((1))) /\\ 1");
                    Console.WriteLine();
                    string s1 = Console.ReadLine();
                    BooleanParser.StringParse(s1);
                    Console.WriteLine();
                    Console.WriteLine("If you want to continue, press enter. Press 0 to exit.");
                    string exit1 = Console.ReadLine();
                    if (exit1 == "0")
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                case "3":
                    Console.WriteLine("Enter the expression like in the example. Inversion (inv()) is allowed.");
                    Console.WriteLine("Example: [ 1 2 ; 3 4 ] + ([ 5 6 ; 7 8] + [0 0 ; 0 0])");
                    Console.WriteLine();
                    string s2 = Console.ReadLine();
                    MatrParser.StringParse(s2);
                    Console.WriteLine();
                    Console.WriteLine("If you want to continue, press enter. Press 0 to exit.");
                    string exit2 = Console.ReadLine();
                    if (exit2 == "0")
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                case "4":
                    Console.WriteLine("Enter the expression like in the example. Only the powers < 100 allowed.");
                    Console.WriteLine("Example: x^100 + (5 + x^1) * x^3");
                    Console.WriteLine();
                    string s3 = Console.ReadLine();
                    PolynomialParser.StringParse(s3);
                    Console.WriteLine();
                    Console.WriteLine("If you want to continue, press enter. Press 0 to exit.");
                    string exit3 = Console.ReadLine();
                    if (exit3 == "0")
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                default:
                    return false;
            }
        }
    }
}

