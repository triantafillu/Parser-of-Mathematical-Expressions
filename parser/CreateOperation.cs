using System;
using System.Collections.Generic;

namespace Parser_of_Mathematical_Expressions
{
    //Class contains static method to calculate the operation
    class CreateOperation
    {
        NodeNumber a;
        NodeNumber b;
        Operation op;

        public CreateOperation(NodeNumber A, NodeNumber B, Operation O)
        {
            a = A;
            b = B;
            op = O;
        }


        //Method reads the operation and performs it
        public static double EvaluateOperation(double A, double B, Operation O)
        {
            double res = 0;
            switch (O)
            {
                case Operation.Addition:
                    res = A + B;
                    break;
                case Operation.Substraction:
                    res = A - B;
                    break;
                case Operation.Multiplication:
                    res = A * B;
                    break;
                case Operation.Division:
                    res = A / B;
                    break;
                case Operation.And:
                    if (A==1)
                    {
                        if (B == 1) res = 1;
                        else if (B == 0) res = 1;
                    }
                    else if (A == 0)
                    {
                        if (B == 1) res = 1;
                        else if (B == 0) res = 0;
                    }
                    break;
                case Operation.Or:
                    if (A == 1)
                    {
                        if (B == 1) res = 1;
                        else if (B == 0) res = 0;
                    }
                    else if (A == 0)
                    {
                        if (B == 1) res = 0;
                        else if (B == 0) res = 0;
                    }
                    break;
            }
            return res;
        }

        //Method for Unary
        public static double EvaluateOperation(double A, Operation O)
        {
            double res = 0;
            if (O == Operation.UnaryMinus)
            {
                res = -A;
            }
            return res;
        }

        public static bool CheckMatrixSizes(double[,] A, double[,] B)
        {
            int r = A.GetLength(0);
            int c = A.GetLength(1);
            int r1 = B.GetLength(0);
            int c1 = B.GetLength(1);
            if (r == r1 && c == c1) return true;
            else
            {
                Console.WriteLine("Matrices are not of equal size");
                Environment.Exit(0);
                return false;
            }
        }
        //Method reads the operation and performs it (Matrix)
        public static double[,] EvaluateOperation(double[,] A, double[,] B, Operation O)
        {
            int r = A.GetLength(0);
            int c = A.GetLength(1);
            int r1 = B.GetLength(0);
            int c1 = B.GetLength(1);
            double[,] res = new double[r, c];
            if (O == Operation.Addition & CheckMatrixSizes(A, B))
            {
                for (int i = 0; i < r; i++)
                {
                   for (int j = 0; j < c; j++)
                   {
                       res[i, j] = A[i, j] + B[i, j];
                   }
                }
            }
            else if (O == Operation.Substraction & CheckMatrixSizes(A, B))
            {
                for (int i = 0; i < r; i++)
                {
                    for (int j = 0; j < c; j++)
                    {
                        res[i, j] = A[i, j] - B[i, j];
                    }
                }
            }
            else if (O == Operation.Multiplication)
            {
                if (c == r1)
                {
                    for (int row = 0; row < r; row++)
                    {
                        for (int col = 0; col < c; col++)
                        {
                            for (int inner = 0; inner < A.GetLength(1); inner++)
                            {
                                res[row, col] += A[row, inner] * B[inner, col];
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Impossible to multiply matrices");
                    Environment.Exit(0);
                }

            }
            return res;
        }

        //Method for Unary (Matrix)
        public static double[,] EvaluateOperation(double[,] A, Operation O)
        {
            int r = A.GetLength(0);
            int c = A.GetLength(1);
            double[,] res = new double[r, c];
            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < c; j++)
                {
                    res[i, j] = -res[i,j];
                }
            }
            return res;
        }

        //Method for polynomials (unary)
        public static Polynomial EvaluateOperation(Polynomial A, Operation O)
        {
            double[] tmp = new double[100];
            for(int i=0; i<100; i++)
            {
                if (A.pol_array[i] != 0)
                {
                    tmp[i] = -A.pol_array[i];
                }
                else
                {
                    tmp[i] = 0;
                }
            }
            Polynomial res = new Polynomial(tmp);
            return res;
        }

        public static double[] DividePolynomials(Polynomial A, Polynomial B)
        {
                int powA = 0;
                int powB = 0;
                for (int i=0; i<101; i++)
                {
                    if(A.pol_array[i]!=0)
                    {
                        powA = i;
                    }
                    if (B.pol_array[i] != 0)
                    {
                        powB = i;
                    }
                }
                
                if(powA<powB)
                {
                    Console.WriteLine("Operations with negative powers are not supported");
                    Environment.Exit(0);
                }

                List<int> powers_A = new List<int>();
                List<double> coefs_A = new List<double>();
                for (int i=0; i<A.pol_array.Length; i++)
                {
                    if (A.pol_array[i]!=0)
                    {
                        powers_A.Add(i);
                        coefs_A.Add(A.pol_array[i]);
                    }
                }

                List<int> powers_B = new List<int>();
                List<double> coefs_B = new List<double>();
                for (int i = 0; i < B.pol_array.Length; i++)
                {
                    if (B.pol_array[i] != 0)
                    {
                        powers_B.Add(i);
                        coefs_B.Add(B.pol_array[i]);
                    }
                }

                double[] tmp = new double[A.pol_array.Length+B.pol_array.Length];

                if (powers_B.Count == 1)
                {
                    for (int i = 0; i < powers_A.Count; i++)
                    {
                        for (int j = 0; j < powers_B.Count; j++)
                        {
                            int tmp_pow = powers_A[i] - powers_B[j];
                            double tmp_coef = coefs_A[i] / coefs_B[j];
                            tmp[tmp_pow] = tmp_coef;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("The division on multiple polynomials is not supported");
                }

                return tmp;
        }
        
        //Method for polynomials
        public static Polynomial EvaluateOperation(Polynomial A, Polynomial B, Operation O)
        {
            if (O == Operation.Addition)
            {
                double[] tmp = new double[A.pol_array.Length];
                for (int i = 0; i < 101; i++)
                {
                    tmp[i] = A.pol_array[i] + B.pol_array[i];
                }
                Polynomial res = new Polynomial(tmp);
                return res;
            }
            else if (O==Operation.Substraction)
            {
                double[] tmp = new double[A.pol_array.Length];
                for (int i = 0; i < 101; i++)
                {
                    tmp[i] = A.pol_array[i] - B.pol_array[i];
                }
                Polynomial res = new Polynomial(tmp);
                return res;
            }
            else if (O == Operation.Multiplication)
            {
                double[] tmp = new double[A.pol_array.Length+B.pol_array.Length];
                for (int i = 0; i < 101; i++)
                {
                    for (int j=0; j<101; j++)
                    {
                        tmp[i + j] += A.pol_array[i] * B.pol_array[j];
                    }
                }
                Polynomial res = new Polynomial(tmp);
                return res;
            }
            else if (O == Operation.Division)
            {
                double[] tmp = DividePolynomials(A, B);
                Polynomial res = new Polynomial(tmp);
                return res;
            }
            double[] tmp1 = new double[A.pol_array.Length];
            Polynomial res1 = new Polynomial(tmp1);
            return res1;
        }
    }
}
