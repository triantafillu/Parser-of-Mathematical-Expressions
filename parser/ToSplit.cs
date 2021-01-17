using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Parser_of_Mathematical_Expressions
{
    abstract class ToSplit
    {
        public TextReader text;
        public char current_el;
        public Sign current_sign;

        public void Move_el()
        {
            int pos = text.Read();
            if (pos < 0) //end of file
            {
                current_el = '\0';
            }
            else
            {
                //Convert current element to char
                current_el = Convert.ToChar(pos);
            }
        }

        public virtual void Move_sign()
        {
        }

    }

    //Reads the string and splits it into signs
    class MathToSplit : ToSplit
    {
        public double parsed_value;
        public string constant;

        //Assign the sign to a char element and parsing string into double
        public override void Move_sign()
        {
            //Skip all the spaces
            while (char.IsWhiteSpace(current_el))
            {
                Move_el();
            }

            //Assigning signs
            switch (current_el)
            {
                case '\0':
                    current_sign = Sign.EOF;
                    return;
                case '-':
                    Move_el();
                    current_sign = Sign.Minus;
                    return;
                case '+':
                    Move_el();
                    current_sign = Sign.Plus;
                    return;
                case '/':
                    Move_el();
                    current_sign = Sign.Divide;
                    return;
                case '*':
                    Move_el();
                    current_sign = Sign.Multiply;
                    return;
                case '(':
                    Move_el();
                    current_sign = Sign.Open;
                    return;
                case ')':
                    Move_el();
                    current_sign = Sign.Close;
                    return;
            }

            //Check whether the number is double
            if (char.IsDigit(current_el) == true || current_el == '.')
            {
                StringBuilder new_str1 = new StringBuilder();
                bool haveDecimalPoint = false;

                //To pick up the whole number
                while (char.IsDigit(current_el) == true || (haveDecimalPoint == false && current_el == '.'))
                {
                    new_str1.Append(current_el);
                    haveDecimalPoint = (current_el == '.');
                    Move_el();
                }

                //Transform the string into number
                parsed_value = double.Parse(new_str1.ToString(), CultureInfo.InvariantCulture);
                current_sign = Sign.Number;
                return;
            }

            //Check if current element is letter
            if (char.IsLetter(current_el) == true)
            {
                StringBuilder new_str2 = new StringBuilder();
                while (char.IsLetterOrDigit(current_el) == true)
                {
                    new_str2.Append(current_el);
                    Move_el();
                }

                constant = new_str2.ToString();

                //Whether it's one of the constants
                if (constant == "pi")
                {
                    parsed_value = 3.14159265359;
                    current_sign = Sign.Number;
                }

                else if (constant == "e")
                {
                    parsed_value = 2.71828182845904523536;
                    current_sign = Sign.Number;
                }

                else if (constant == "sin")
                {
                    current_sign = Sign.Sin;
                }

                else if (constant == "cos")
                {
                    current_sign = Sign.Cos;
                }

                else if (constant == "tg")
                {
                    current_sign = Sign.Tg;
                }

                else if (constant == "ctg")
                {
                    current_sign = Sign.Ctg;
                }
                else
                {
                    Console.WriteLine($"Please, input value for the variable '{constant}':");
                    parsed_value = Convert.ToDouble(Console.ReadLine());
                    current_sign = Sign.Number;
                }
                return;
            }

        }

        public MathToSplit(TextReader T)
        {
            text = T;
            Move_el();
            Move_sign();
        }
    }

    //Class to split the boolean expression
    class BooleanToSplit : ToSplit
    {
        public override void Move_sign()
        {
            //Skip all the spaces
            while (char.IsWhiteSpace(current_el))
            {
                Move_el();
            }

            //Assigning signs
            switch (current_el)
            {
                case '\0':
                    current_sign = Sign.EOF;
                    return;
                case '1':
                    Move_el();
                    current_sign = Sign.One;
                    return;
                case '0':
                    Move_el();
                    current_sign = Sign.Zero;
                    return;
                case '!':
                    Move_el();
                    current_sign = Sign.Not;
                    return;
                case '(':
                    Move_el();
                    current_sign = Sign.Open;
                    return;
                case ')':
                    Move_el();
                    current_sign = Sign.Close;
                    return;
            }

            //Check whether the sign is \/ or /\
            if (current_el == @"\"[0]) //using string literal
            {
                StringBuilder new_str1 = new StringBuilder();
                Move_el();
                if (current_el == '/')
                {
                    new_str1.Append(current_el);
                }
                else
                {
                    Console.WriteLine($"Unknown boolean sign: {current_el}");
                    Environment.Exit(0);
                }
                Move_el();
                current_sign = Sign.And;
                return;
            }

            if (current_el == '/')
            {
                StringBuilder new_str1 = new StringBuilder();
                Move_el();
                if (current_el == @"\"[0])//using string literal
                {
                    new_str1.Append(current_el);
                }
                else
                {
                    Console.WriteLine($"Unknown boolean sign: {current_el}");
                    Environment.Exit(0);
                }
                Move_el();
                current_sign = Sign.Or;
                return;
            }
        }

        public BooleanToSplit(TextReader T)
        {
            text = T;
            Move_el();
            Move_sign();
        }
    }

    //Class to split expression with matrices
    class MatrToSplit : ToSplit
    {
        public int parsed_value;

        public override void Move_sign()
        {
            //SPACES ARE IMPORTANT 

            //Assigning signs
            switch (current_el)
            {
                case '\0':
                    current_sign = Sign.EOF;
                    return;
                case ' ':
                    Move_el();
                    current_sign = Sign.Space;
                    return;
                case ';':
                    Move_el();
                    current_sign = Sign.DotComa;
                    return;
                case '[':
                    Move_el();
                    current_sign = Sign.MatrOpen;
                    return;
                case ']':
                    Move_el();
                    current_sign = Sign.MatrClose;
                    return;
                case '+':
                    Move_el();
                    current_sign = Sign.Plus;
                    return;
                case '-':
                    Move_el();
                    current_sign = Sign.Minus;
                    return;
                case '*':
                    Move_el();
                    current_sign = Sign.Multiply;
                    return;
                case '(':
                    Move_el();
                    current_sign = Sign.Open;
                    return;
                case ')':
                    Move_el();
                    current_sign = Sign.Close;
                    return;
            }

            //Check whether the number is double
            if (char.IsDigit(current_el) == true)
            {
                StringBuilder new_str1 = new StringBuilder();

                //To pick up the whole number
                while (char.IsDigit(current_el) == true)
                {
                    new_str1.Append(current_el);
                    Move_el();
                }

                //Transform the string into number
                parsed_value = int.Parse(new_str1.ToString(), CultureInfo.InvariantCulture);
                current_sign = Sign.Number;
                return;
            }

            //Check if current element is letter
            if (char.IsLetter(current_el) == true)
            {
                StringBuilder new_str2 = new StringBuilder();
                while (char.IsLetterOrDigit(current_el) == true)
                {
                    new_str2.Append(current_el);
                    Move_el();
                }

                string constant = new_str2.ToString();

                //Whether it's one of the constants
                if (constant == "inv")
                {
                    current_sign = Sign.Inv;
                }
            }
        }

        public MatrToSplit(TextReader T)
        {
            text = T;
            Move_el();
            Move_sign();
        }
    }

    //Class to split expressions with polynomials
    class PolynomialToSplit : ToSplit
    {
        public Polynomial pol;

        //Assign the sign to a char element and parsing string into double
        public override void Move_sign()
        {
            //Skip all the spaces
            while (char.IsWhiteSpace(current_el))
            {
                Move_el();
            }

            //Assigning signs
            switch (current_el)
            {
                case '\0':
                    current_sign = Sign.EOF;
                    return;
                case '-':
                    Move_el();
                    current_sign = Sign.Minus;
                    return;
                case '+':
                    Move_el();
                    current_sign = Sign.Plus;
                    return;
                case '/':
                    Move_el();
                    current_sign = Sign.Divide;
                    return;
                case '*':
                    Move_el();
                    current_sign = Sign.Multiply;
                    return;
                case '(':
                    Move_el();
                    current_sign = Sign.Open;
                    return;
                case ')':
                    Move_el();
                    current_sign = Sign.Close;
                    return;
            }


            //Check whether the number is double
            if (char.IsDigit(current_el) == true || current_el == '.')
            {
                StringBuilder new_str1 = new StringBuilder();
                bool haveDecimalPoint = false;

                //To pick up the whole number
                while (char.IsDigit(current_el) == true || (haveDecimalPoint == false && current_el == '.'))
                {
                    new_str1.Append(current_el);
                    haveDecimalPoint = (current_el == '.');
                    Move_el();
                }

                //Check whether it is a polynomial
                if (char.IsLetter(current_el) == true)
                {
                    double coef = double.Parse(new_str1.ToString(), CultureInfo.InvariantCulture);

                    StringBuilder new_str2 = new StringBuilder();
                    while (char.IsLetterOrDigit(current_el) == true)
                    {
                        new_str2.Append(current_el);
                        Move_el();
                    }

                    string constant = new_str2.ToString();
                    if (current_el == '^')
                    {
                        Move_el();
                        if (char.IsDigit(current_el) == true)
                        {
                            StringBuilder new_str3 = new StringBuilder();

                            //To pick up the whole number
                            while (char.IsDigit(current_el) == true)
                            {
                                new_str3.Append(current_el);
                                Move_el();
                            }

                            //Transform the string into number
                            int power = int.Parse(new_str1.ToString(), CultureInfo.InvariantCulture);
                            if(power>100)
                            {
                                Console.WriteLine("Impossible to perform operations with polynomials with power > 100.");
                                Environment.Exit(0);
                            }
                            double[] tmp = new double[101];
                            tmp[power] = coef;
                            pol = new Polynomial(tmp);
                            current_sign = Sign.Pol;
                            return;
                        }
                        else
                        {
                            Console.WriteLine("Error in finding power");
                            Environment.Exit(0);
                        }
                    }
                    else
                    {
                        //No power => power = 1
                        double[] tmp = new double[101];
                        tmp[1] = coef;
                        pol = new Polynomial(tmp);
                        current_sign = Sign.Pol;
                        return;
                    }
                }
                else
                {
                    //Transform the string into number
                    double coef = double.Parse(new_str1.ToString(), CultureInfo.InvariantCulture);
                    double[] tmp = new double[101];
                    tmp[0] = coef;
                    pol = new Polynomial(tmp);
                    current_sign = Sign.Pol;
                    return;
                }
            }



            //Check if current element is letter
            if (char.IsLetter(current_el) == true)
            {
                StringBuilder new_str2 = new StringBuilder();
                while (char.IsLetterOrDigit(current_el) == true)
                {
                    new_str2.Append(current_el);
                    Move_el();
                }

                string constant = new_str2.ToString();
                if (current_el == '^')
                {
                    Move_el();
                    if (char.IsDigit(current_el) == true)
                    {
                        StringBuilder new_str1 = new StringBuilder();

                        //To pick up the whole number
                        while (char.IsDigit(current_el) == true)
                        {
                            new_str1.Append(current_el);
                            Move_el();
                        }

                        //Transform the string into number
                        int power = int.Parse(new_str1.ToString(), CultureInfo.InvariantCulture);
                        if (power > 100)
                        {
                            Console.WriteLine("Impossible to perform operations with polynomials with power > 100.");
                            Environment.Exit(0);
                        }
                        //No coef => coef=1
                        double[] tmp = new double[101];
                        tmp[power] = 1;
                        pol = new Polynomial(tmp);
                        current_sign = Sign.Pol;
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Error in finding power");
                        Environment.Exit(0);
                    }
                }
                else
                {
                    //No power => power=1
                    //No coef => coef=1
                    double[] tmp = new double[101];
                    tmp[1] = 1;
                    pol = new Polynomial(tmp);
                    current_sign = Sign.Pol;
                    return;
                }
            }

        }

        public PolynomialToSplit(TextReader T)
        {
            text = T;
            Move_el();
            Move_sign();
        }
    }
}
