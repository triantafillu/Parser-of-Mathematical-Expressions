using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Parser_of_Mathematical_Expressions
{
    //Parse the expresion on the correct order
    class MathParser
    {
        MathToSplit splitter; //used to read the text and split it into defined signs

        //Constructor
        public MathParser(MathToSplit S)
        {
            splitter = S;
        }

        //Find and create the leaf with number
        //(is called every time the number is found)
        Node Part()
        {
            //Number
            if (splitter.current_sign == Sign.Number)
            {
                Node part = new NodeNumber(splitter.parsed_value); //create the node with found value
                splitter.Move_sign();//move to the next sign
                return part;
            }

            //Sin
            if (splitter.current_sign == Sign.Sin)
            {
                splitter.Move_sign(); //move to the next sign
                if (splitter.current_sign == Sign.Open)
                {
                    splitter.Move_sign();
                    //Parse sequentially all the expression after the (
                    Node inside = PlusMinus();

                    // Find the )
                    if (splitter.current_sign != Sign.Close) //if there's no )
                    {
                        Console.WriteLine("Missing closing parenthesis");
                        Environment.Exit(0);
                    }
                    else if (splitter.current_sign == Sign.Close)
                    {
                        double res = Math.Sin(inside.Calculate());
                        Node part = new NodeNumber(res); //create the node with value of sin
                        splitter.Move_sign();//move to the next sign
                        return part;
                    }
                }
                else
                {
                    Console.WriteLine("Missing opening parenthesis");
                    Environment.Exit(0);
                }
            }

            //Cos
            if (splitter.current_sign == Sign.Cos)
            {
                splitter.Move_sign(); //move to the next sign
                if (splitter.current_sign == Sign.Open)
                {
                    splitter.Move_sign();
                    //Parse sequentially all the expression after the (
                    Node inside = PlusMinus();

                    // Find the )
                    if (splitter.current_sign != Sign.Close) //if there's no )
                    {
                        Console.WriteLine("Missing closing parenthesis");
                        Environment.Exit(0);
                    }
                    else if (splitter.current_sign == Sign.Close)
                    {
                        double res = Math.Cos(inside.Calculate());
                        Node part = new NodeNumber(res); //create the node with value of sin
                        splitter.Move_sign();//move to the next sign
                        return part;
                    }
                }
                else
                {
                    Console.WriteLine("Missing opening parenthesis");
                    Environment.Exit(0);
                }
            }

            //Tg
            if (splitter.current_sign == Sign.Sin)
            {
                splitter.Move_sign(); //move to the next sign
                if (splitter.current_sign == Sign.Open)
                {
                    splitter.Move_sign();
                    //Parse sequentially all the expression after the (
                    Node inside = PlusMinus();

                    // Find the )
                    if (splitter.current_sign != Sign.Close) //if there's no )
                    {
                        Console.WriteLine("Missing closing parenthesis");
                        Environment.Exit(0);
                    }
                    else if (splitter.current_sign == Sign.Close)
                    {
                        double res = Math.Tan(inside.Calculate());
                        Node part = new NodeNumber(res); //create the node with value of sin
                        splitter.Move_sign();//move to the next sign
                        return part;
                    }
                }
                else
                {
                    Console.WriteLine("Missing opening parenthesis");
                    Environment.Exit(0);
                }
            }

            //Ctg
            if (splitter.current_sign == Sign.Sin)
            {
                splitter.Move_sign(); //move to the next sign
                if (splitter.current_sign == Sign.Open)
                {
                    splitter.Move_sign();
                    //Parse sequentially all the expression after the (
                    Node inside = PlusMinus();

                    // Find the )
                    if (splitter.current_sign != Sign.Close) //if there's no )
                    {
                        Console.WriteLine("Missing closing parenthesis");
                        Environment.Exit(0);
                    }
                    else if (splitter.current_sign == Sign.Close)
                    {
                        double res = 1 / Math.Tan(inside.Calculate());
                        Node part = new NodeNumber(res); //create the node with value of sin
                        splitter.Move_sign();//move to the next sign
                        return part;
                    }
                }
                else
                {
                    Console.WriteLine("Missing opening parenthesis");
                    Environment.Exit(0);
                }
            }

            //Parenthesis
            if (splitter.current_sign == Sign.Open)
            {
                splitter.Move_sign(); //move to the next sign

                //Parse sequentially all the expression after the (
                Node part = PlusMinus();

                // Find the )
                if (splitter.current_sign != Sign.Close) //if there's no )
                {
                    Console.WriteLine("Missing close parenthesis");
                }
                splitter.Move_sign(); //move to the next sign

                return part;
            }

            //If the sign is not classified
            Console.WriteLine($"Unknown sign: {splitter.current_sign}");
            throw new SystemException($"Unknown sign: {splitter.current_sign}");
        }

        Node Unary()
        {
            while (true)
            {
                //In case of unary plus
                //Unary plus doesn't need any operation
                if (splitter.current_sign == Sign.Plus)
                {
                    splitter.Move_sign(); //Move to the next sign
                    continue;
                }

                //In case of unary minus
                else if (splitter.current_sign == Sign.Minus)
                {
                    splitter.Move_sign(); //Move to the next sign

                    //Recursive call to find the sequence of unary operators (if exists)
                    //eg -+--10 = -10
                    Node a = Unary();

                    //Node for unary operator
                    return new NodeUnary(a, Operation.UnaryMinus);
                }

                //Just return number if no unary operator is found
                return Part();
            }
        }

        Node MultDiv()
        {
            //Check whether the current node is a unary operator
            Node left = Unary();

            while (true)
            {
                //Identify the operation
                Operation op = Operation.Null;
                if (splitter.current_sign == Sign.Divide)
                {
                    op = Operation.Division;
                }
                else if (splitter.current_sign == Sign.Multiply)
                {
                    op = Operation.Multiplication;
                }

                //If no multiply or divide sign is found
                if (op == Operation.Null)
                {
                    return left;
                }

                splitter.Move_sign(); //move to the next sign

                //Check whether the current node is a unary operator
                Node right = Unary();

                //Create a node with a correspondent binary operation
                left = new NodeBinary(left, right, op);
            }
        }

        Node PlusMinus()
        {
            Node left = MultDiv();
            while (true)
            {
                //Identify the operation
                Operation op = Operation.Null;
                if (splitter.current_sign == Sign.Plus)
                {
                    op = Operation.Addition;
                }
                else if (splitter.current_sign == Sign.Minus)
                {
                    op = Operation.Substraction;
                }

                //If no plus or minus sign is found
                if (op == Operation.Null)
                {
                    return left;
                }

                splitter.Move_sign(); //move to the next sign

                Node right = MultDiv();

                //Create a node with a correspondent binary operation
                left = new NodeBinary(left, right, op);
            }
        }

        //Method that parses the whole expression, which calls other methods one by one
        //in order to build a correct binary tree of expression
        public Node CheckExpression()
        {
            Node node = PlusMinus();
            if (splitter.current_sign != Sign.EOF)
            {
                Console.WriteLine("Unexpected characters at end of expression");
                Environment.Exit(0);
            }

            return node;
        }

        //Method to simplify calling the parser from main
        public static void StringParse(string s)
        {
            var node = new MathParser(new MathToSplit(new StringReader(s)));
            double res = node.CheckExpression().Calculate();
            Console.WriteLine();
            Console.WriteLine("Result: " + res);
        }
    }

    //Parse Boolean expression
    class BooleanParser
    {
        BooleanToSplit splitter; //used to read the text and split it into defined signs

        //Constructor
        public BooleanParser(BooleanToSplit S)
        {
            splitter = S;
        }

        Node Part()
        {
            //1
            if (splitter.current_sign == Sign.One)
            {
                Node part = new NodeNumber(1); //create the node with found value
                splitter.Move_sign();//move to the next sign
                return part;
            }

            //0
            if (splitter.current_sign == Sign.Zero)
            {
                Node part = new NodeNumber(0); //create the node with found value
                splitter.Move_sign();//move to the next sign
                return part;
            }

            //!
            if (splitter.current_sign == Sign.Not)
            {
                splitter.Move_sign(); //move to the next sign
                if (splitter.current_sign == Sign.One)
                {
                    Node part = new NodeNumber(0); //Invert one to zero
                    splitter.Move_sign();//move to the next sign
                    return part;
                }
                else if (splitter.current_sign == Sign.Zero)
                {
                    Node part = new NodeNumber(1); //Invert to one
                    splitter.Move_sign();//move to the next sign
                    return part;
                }
                else if (splitter.current_sign == Sign.Open)
                {
                    splitter.Move_sign();
                    //Parse sequentially all the expression after the (
                    Node inside = Disj();

                    // Find the )
                    if (splitter.current_sign != Sign.Close) //if there's no )
                    {
                        Console.WriteLine("Missing closing parenthesis");
                        Environment.Exit(0);
                    }
                    else if (splitter.current_sign == Sign.Close)
                    {
                        if (inside.Calculate() == 1)
                        {
                            double res = 0;
                            Node part = new NodeNumber(res);
                            //create the node with value of !
                            splitter.Move_sign();//move to the next sign
                            return part;
                        }
                        else if (inside.Calculate() == 0)
                        {
                            double res = 1;
                            Node part = new NodeNumber(res);
                            //create the node with value of !
                            splitter.Move_sign();//move to the next sign
                            return part;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Missing opening parenthesis");
                    Environment.Exit(0);
                }
            }

            //Parenthesis
            if (splitter.current_sign == Sign.Open)
            {
                splitter.Move_sign(); //move to the next sign

                //Parse sequentially all the expression after the (
                Node part = Disj();

                // Find the )
                if (splitter.current_sign != Sign.Close) //if there's no )
                {
                    Console.WriteLine("Missing close parenthesis");
                    Environment.Exit(0);
                }
                splitter.Move_sign(); //move to the next sign

                return part;
            }

            //If the sign is not classified
            Console.WriteLine($"Unknown sign: {splitter.current_sign}");
            throw new SystemException($"Unknown sign: {splitter.current_sign}");
        }

        Node Conj()
        {
            Node left = Part();
            while (true)
            {
                //Identify the operation
                Operation op = Operation.Null;
                if (splitter.current_sign == Sign.And)
                {
                    op = Operation.And;
                }

                //If no multiply or divide sign is found
                if (op == Operation.Null)
                {
                    return left;
                }

                splitter.Move_sign(); //move to the next sign

                Node right = Part();

                //Create a node with a correspondent binary operation
                left = new NodeBinary(left, right, op);
            }
        }

        Node Disj()
        {
            Node left = Conj();
            while (true)
            {
                //Identify the operation
                Operation op = Operation.Null;
                if (splitter.current_sign == Sign.Or)
                {
                    op = Operation.Or;
                }

                //If no plus or minus sign is found
                if (op == Operation.Null)
                {
                    return left;
                }

                splitter.Move_sign(); //move to the next sign

                Node right = Conj();

                //Create a node with a correspondent binary operation
                left = new NodeBinary(left, right, op);
            }
        }

        //Method that parses the whole expression, which calls other methods one by one
        //in order to build a correct binary tree of expression
        public Node CheckExpression()
        {
            Node node = Disj();
            if (splitter.current_sign != Sign.EOF)
            {
                Console.WriteLine("Unexpected characters at end of expression");
                Environment.Exit(0);
            }

            return node;
        }

        //Method to simplify calling the parser from main
        public static void StringParse(string s)
        {
            var node = new BooleanParser(new BooleanToSplit(new StringReader(s)));
            double res = node.CheckExpression().Calculate();
            Console.WriteLine();
            Console.WriteLine("Result: " + res);
        }
    }

    //Parse an expression with matrices
    class MatrParser
    {
        MatrToSplit splitter; //used to read the text and split it into defined signs

        //Constructor
        public MatrParser(MatrToSplit S)
        {
            splitter = S;
        }

        //To create minor
        void MatrMinors(double[,] A, double[,] tmp, int cur_row, int cur_col, int n)
        {
            int i = 0, j = 0;

            for (int r = 0; r < n; r++)
            {
                for (int c = 0; c < n; c++)
                {
                    //Create minor from r and c which are not current ones
                    if (r != cur_row && c != cur_col)
                    {
                        tmp[i, j++] = A[r, c];

                        //Current row if filled
                        if (j == n - 1)
                        {
                            j = 0;
                            i++;
                        }
                    }
                }
            }
        }

        //To calculate determinant
        double MatrDet(double[,] A, int n)
        {
            double det = 0;

            //Only one element
            if (n == 1)
            {
                return A[0, 0];
            }

            double[,] tmp = new double[n, n]; //will be used for minor

            int sign = 1;
            for (int i = 0; i < n; i++)
            {
                //Minor for [0,i]
                MatrMinors(A, tmp, 0, i, n);
                det += sign * A[0, i] * MatrDet(tmp, n - 1);
                sign = -sign;
            }
            return det;
        }

        void MatrAdj(double[,] A, double[,] adj, int n)
        {
            if (n == 1)
            {
                adj[0, 0] = 1;
                return;
            }

            int sign = 1;
            double[,] tmp = new double[n, n]; //for minors

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    //Minor of A[i,j] 
                    MatrMinors(A, tmp, i, j, n);
                    if ((i + j) % 2 == 0)
                    {
                        sign = 1;
                    }
                    else
                    {
                        sign = -1;
                    }
                    adj[j, i] = (sign) * (MatrDet(tmp, n - 1));
                }
            }
        }

        bool MatrInv(double[,] A, double[,] MatrInv, int n)
        {
            double det = MatrDet(A, n);
            if (det == 0)
            {
                Console.Write("Impossible to use inv() for singular matrix");
                return false;
            }

            double[,] adj = new double[n, n];
            MatrAdj(A, adj, n);

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    MatrInv[i, j] = adj[i, j] / det;

            return true;
        }

        Node_M Part()
        {
            /*[ 2 0 ; -1 3 ] * [ 2 0 ; -1 3 ]*/
            //Matr
            if (splitter.current_sign == Sign.MatrOpen)
            {
                splitter.Move_sign();
                List<List<int>> matr = new List<List<int>>();
                int c = 0;
                while (splitter.current_sign != Sign.MatrClose)
                {
                    matr.Add(new List<int>());
                    if (splitter.current_sign == Sign.Space)
                    {
                        splitter.Move_sign();//skip space
                    }
                    else if (splitter.current_sign == Sign.DotComa)
                    {
                        splitter.Move_sign();
                    }
                    while (splitter.current_sign != Sign.DotComa)
                    {
                        if (splitter.current_sign == Sign.Minus)
                        {
                            splitter.Move_sign();
                            if (splitter.current_sign == Sign.Space)
                            {
                                splitter.Move_sign();//skip space
                            }
                            if (splitter.current_sign == Sign.Number)
                            {
                                matr[c].Add(-splitter.parsed_value);
                                splitter.Move_sign();//move to the next element
                            }
                            else
                            {
                                Console.WriteLine("Matrix element error");
                                Environment.Exit(0);
                            }
                        }
                        if (splitter.current_sign == Sign.Number)
                        {
                            matr[c].Add(splitter.parsed_value);
                            splitter.Move_sign();//move to the next element
                        }
                        else if (splitter.current_sign == Sign.Space)
                        {
                            splitter.Move_sign();//skip space
                        }
                        else if (splitter.current_sign == Sign.MatrClose)
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Matrix element error");
                            Environment.Exit(0);
                        }
                    }
                    c++;
                }



                //Check whether each row has an even number of elements
                int columns = matr.Count;
                List<int> curr_row = matr[0];
                for (int i = 1; i < columns; i++)
                {
                    if (curr_row.Count == matr[i].Count)
                    {
                        curr_row = matr[i];
                    }
                    else
                    {
                        Console.WriteLine("The number of colums is uneven");
                        Environment.Exit(0);
                    }
                }

                double[,] matr1 = new double[columns, matr[0].Count];
                int c1 = 0;
                int c2 = 0;
                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < matr[0].Count; j++)
                    {
                        matr1[i, j] = matr[i][j];
                    }
                }
                Node_M part = new NodeMatr(matr1);
                splitter.Move_sign();//move to the next sign
                if (splitter.current_sign == Sign.Space)
                {
                    splitter.Move_sign();//skip space
                }
                return part;
            }

            //inv
            if (splitter.current_sign == Sign.Inv)
            {
                //inv([ 2 5 7 ; 6 3 4 ; 5 -2 -3])
                splitter.Move_sign(); //move to the next sign
                if (splitter.current_sign == Sign.Space)
                {
                    splitter.Move_sign();//skip space
                }
                if (splitter.current_sign == Sign.Open)
                {
                    splitter.Move_sign();
                    if (splitter.current_sign == Sign.Space)
                    {
                        splitter.Move_sign();//skip space
                    }
                    //Parse sequentially all the expression after the (
                    Node_M inside = PlusMinus();

                    // Find the )
                    if (splitter.current_sign != Sign.Close) //if there's no )
                    {
                        Console.WriteLine("Missing closing parenthesis");
                        Environment.Exit(0);
                    }
                    else if (splitter.current_sign == Sign.Close)
                    {
                        int r = inside.Calculate().GetLength(0);
                        int c = inside.Calculate().GetLength(1);
                        double[,] matr = new double[r, c];
                        for (int i = 0; i < r; i++)
                        {
                            for (int j = 0; j < c; j++)
                            {
                                matr[i, j] = inside.Calculate()[i, j];
                            }
                        }
                        if (r == c)
                        {
                            int n = r;
                            int t = 0;
                            double[,] A = new double[r, c * 2];

                            //Create a copy of array 
                            for (int i = 0; i < n; i++)
                            {
                                for (int j = 0; j < n; j++)
                                {
                                    A[i, j] = matr[i, j];
                                }
                            }

                            double[,] inv = new double[n, n];
                            if (MatrInv(A, inv, n))
                            {
                                Node_M part = new NodeMatr(inv); //create the node with value of matrix
                                splitter.Move_sign();//move to the next sign
                                if (splitter.current_sign == Sign.Space)
                                {
                                    splitter.Move_sign();
                                }
                                return part;
                            }
                            else
                            {
                                Console.WriteLine("Impossible to MatrInv");
                                Environment.Exit(0);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Impossible to MatrInv a non-square matrix");
                            Environment.Exit(0);
                        }
                    }
                }

                else
                {
                    Console.WriteLine("Missing opening parenthesis");
                    Environment.Exit(0);
                }
            }

            //Parenthesis
            if (splitter.current_sign == Sign.Open)
            {
                splitter.Move_sign(); //move to the next sign
                if (splitter.current_sign == Sign.Space)
                {
                    splitter.Move_sign();
                }
                //Parse sequentially all the expression after the (
                Node_M part = PlusMinus();

                // Find the )
                if (splitter.current_sign != Sign.Close) //if there's no )
                {
                    Console.WriteLine("Missing close parenthesis");
                    Environment.Exit(0);
                }

                splitter.Move_sign(); //move to the next sign
                if (splitter.current_sign == Sign.Space)
                {
                    splitter.Move_sign();
                }

                return part;
            }

            //If the sign is not classified

            Console.WriteLine($"Unknown sign: {splitter.current_sign}");
            throw new SystemException($"Unknown sign: {splitter.current_sign}");
        }

        Node_M Unary()
        {
            while (true)
            {
                //In case of unary plus
                //Unary plus doesn't need any operation
                if (splitter.current_sign == Sign.Plus)
                {
                    splitter.Move_sign(); //Move to the next sign
                    if (splitter.current_sign == Sign.Space)
                    {
                        splitter.Move_sign();
                    }
                    continue;
                }

                //In case of unary minus
                else if (splitter.current_sign == Sign.Minus)
                {
                    splitter.Move_sign(); //Move to the next sign
                    if (splitter.current_sign == Sign.Space)
                    {
                        splitter.Move_sign();
                    }

                    //Recursive call to find the sequence of unary operators (if exists)
                    //eg -+--10 = -10
                    Node_M a = Unary();

                    //Node for unary operator
                    return new NodeMatrUnary(a, Operation.UnaryMinus);
                }

                //Just return number if no unary operator is found
                return Part();
            }
        }

        Node_M Mult()
        {
            //Check whether the current node is a unary operator
            Node_M left = Unary();

            while (true)
            {
                //Identify the operation
                Operation op = Operation.Null;
                if (splitter.current_sign == Sign.Multiply)
                {
                    op = Operation.Multiplication;
                }

                //If no multiply or divide sign is found
                if (op == Operation.Null)
                {
                    return left;
                }

                splitter.Move_sign(); //move to the next sign
                if (splitter.current_sign == Sign.Space)
                {
                    splitter.Move_sign();
                }

                //Check whether the current node is a unary operator
                Node_M right = Unary();

                //Create a node with a correspondent binary operation
                left = new NodeMatrBinary(left, right, op);
            }
        }

        Node_M PlusMinus()
        {
            Node_M left = Mult();
            while (true)
            {
                //Identify the operation
                Operation op = Operation.Null;
                if (splitter.current_sign == Sign.Plus)
                {
                    op = Operation.Addition;
                }
                else if (splitter.current_sign == Sign.Minus)
                {
                    op = Operation.Substraction;
                }

                //If no plus or minus sign is found
                if (op == Operation.Null)
                {
                    return left;
                }

                splitter.Move_sign(); //move to the next sign
                if (splitter.current_sign == Sign.Space)
                {
                    splitter.Move_sign();
                }

                Node_M right = Mult();

                //Create a node with a correspondent binary operation
                left = new NodeMatrBinary(left, right, op);
            }
        }

        //Method that parses the whole expression, which calls other methods one by one
        //in order to build a correct binary tree of expression
        public Node_M CheckExpression()
        {
            Node_M node = PlusMinus();
            if (splitter.current_sign != Sign.EOF)
            {
                Console.WriteLine("Unexpected characters at end of expression");
                Environment.Exit(0);
            }

            return node;
        }

        //Method to simplify calling the parser from main
        public static void StringParse(string s)
        {
            var node = new MatrParser(new MatrToSplit(new StringReader(s)));
            double[,] res = node.CheckExpression().Calculate();
            Console.WriteLine();
            Console.Write("Result: [ ");
            for (int i = 0; i < res.GetLength(0); i++)
            {
                for (int j = 0; j < res.GetLength(1); j++)
                {
                    Console.Write(res[i, j] + " ");
                }
                if (i != (res.GetLength(0) - 1))
                {
                    Console.Write("; ");
                }
            }
            Console.Write("]");
        }
    }

    //Parse an expression with polynomials
    class PolynomialParser
    {
        PolynomialToSplit splitter; //used to read the text and split it into defined signs

        //Constructor
        public PolynomialParser(PolynomialToSplit S)
        {
            splitter = S;
        }

        //Find and create the leaf with polynomial
        Node_P Part()
        {
            //Polynomial
            if (splitter.current_sign == Sign.Pol)
            {
                Node_P part = new NodePolynomial(splitter.pol); //create the node with found polynomial
                splitter.Move_sign();//move to the next sign
                return part;
            }

            //Parenthesis
            if (splitter.current_sign == Sign.Open)
            {
                splitter.Move_sign(); //move to the next sign

                //Parse sequentially all the expression after the (
                Node_P part = PlusMinus();

                // Find the )
                if (splitter.current_sign != Sign.Close) //if there's no )
                {
                    Console.WriteLine("Missing close parenthesis");
                }
                splitter.Move_sign(); //move to the next sign

                return part;
            }

            //If the sign is not classified
            Console.WriteLine($"Unknown sign: {splitter.current_sign}");
            throw new SystemException($"Unknown sign: {splitter.current_sign}");
        }

        Node_P Unary()
        {
            while (true)
            {
                //In case of unary plus
                //Unary plus doesn't need any operation
                if (splitter.current_sign == Sign.Plus)
                {
                    splitter.Move_sign(); //Move to the next sign
                    continue;
                }

                //In case of unary minus
                else if (splitter.current_sign == Sign.Minus)
                {
                    splitter.Move_sign(); //Move to the next sign

                    //Recursive call to find the sequence of unary operators (if exists)
                    //eg -+--10 = -10
                    Node_P a = Unary();

                    //Node for unary operator
                    return new NodePolUnary(a, Operation.UnaryMinus);
                }

                //Just return number if no unary operator is found
                return Part();
            }
        }

        Node_P MultDiv()
        {
            //Check whether the current node is a unary operator
            Node_P left = Unary();

            while (true)
            {
                //Identify the operation
                Operation op = Operation.Null;
                if (splitter.current_sign == Sign.Divide)
                {
                    op = Operation.Division;
                }
                else if (splitter.current_sign == Sign.Multiply)
                {
                    op = Operation.Multiplication;
                }

                //If no multiply or divide sign is found
                if (op == Operation.Null)
                {
                    return left;
                }

                splitter.Move_sign(); //move to the next sign

                //Check whether the current node is a unary operator
                Node_P right = Unary();

                //Create a node with a correspondent binary operation
                left = new NodePolBinary(left, right, op);
            }
        }

        Node_P PlusMinus()
        {
            Node_P left = MultDiv();
            while (true)
            {
                //Identify the operation
                Operation op = Operation.Null;
                if (splitter.current_sign == Sign.Plus)
                {
                    op = Operation.Addition;
                }
                else if (splitter.current_sign == Sign.Minus)
                {
                    op = Operation.Substraction;
                }

                //If no plus or minus sign is found
                if (op == Operation.Null)
                {
                    return left;
                }

                splitter.Move_sign(); //move to the next sign

                Node_P right = MultDiv();

                //Create a node with a correspondent binary operation
                left = new NodePolBinary(left, right, op);
            }
        }

        //Method that parses the whole expression, which calls other methods one by one
        //in order to build a correct binary tree of expression
        public Node_P CheckExpression()
        {
            Node_P node = PlusMinus();
            if (splitter.current_sign != Sign.EOF)
            {
                Console.WriteLine("Unexpected characters at end of expression");
                Environment.Exit(0);
            }

            return node;
        }

        //Method to simplify calling the parser from main
        public static void StringParse(string s)
        {
            var node = new PolynomialParser(new PolynomialToSplit(new StringReader(s)));
            Polynomial res = node.CheckExpression().Calculate();
            Console.WriteLine();
            Console.WriteLine("Result: ");
            int counter = 0;
            for (int i = 0; i < 101; i++)
            {
                if (res.pol_array[i] != 0)
                {
                    if (i == 0)
                    {
                        if (counter == 0)
                        {
                            Console.Write(res.pol_array[i]);
                            counter++;
                        }
                        else
                        {
                            Console.Write(res.pol_array[i]);
                        }
                    }
                    else
                    {
                        //Coef = 1
                        if (res.pol_array[i] == 1)
                        {
                            if (counter == 0)
                            {
                                if (i == 1)
                                {
                                    Console.Write("x");
                                    counter++;
                                }
                                else
                                {
                                    Console.Write($"x^{i}");
                                    counter++;
                                }
                            }
                            else
                            {
                                if (i == 1)
                                {
                                    Console.Write("+x");
                                }
                                else
                                {
                                    Console.Write($"+x^{i}");
                                }
                            }
                        }
                        //Coef = -1
                        else if (res.pol_array[i] == -1)
                        {
                            if (i == 1)
                            {
                                Console.Write("-x");
                            }
                            else
                            {
                                Console.Write($"-x^{i}");
                            }
                        }
                        else
                        {
                            //Coef > 0
                            if (res.pol_array[i] > 0)
                            {
                                if (counter == 0)
                                {
                                    if (i == 1)
                                    {
                                        Console.Write($"{res.pol_array[i]}x");
                                    }
                                    else
                                    {
                                        Console.Write($"{res.pol_array[i]}x^{i}");
                                    }
                                    counter++;
                                }
                                else
                                {
                                    if (i == 1)
                                    {
                                        Console.Write($"+{res.pol_array[i]}x");
                                    }
                                    else
                                    {
                                        Console.Write($"+{res.pol_array[i]}x^{i}");
                                    }
                                }
                            }
                            //Coef < 0
                            else
                            {
                                if (i == 1)
                                {
                                    Console.Write($"{res.pol_array[i]}x");
                                }
                                else
                                {
                                    Console.Write($"{res.pol_array[i]}x^{i}");
                                }
                            }
                        }
                    }
                }
            }

        }
    }
}
