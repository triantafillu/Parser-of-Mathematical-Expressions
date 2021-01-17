using System;
using System.Collections.Generic;
using System.Text;

namespace Parser_of_Mathematical_Expressions
{
    public class Polynomial
    {
        /*string variable;
        double power;
        double coef;

        public Polynomial(string V, double P, double C)
        {
            variable = V;
            power = P;
            coef = C;
        }*/

        public double[] pol_array;
        public Polynomial(double [] A)
        {
            pol_array = A;
        }

    }
}
