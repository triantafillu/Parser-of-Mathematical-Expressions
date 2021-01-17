using System;
using System.Collections.Generic;
using System.Text;

namespace Parser_of_Mathematical_Expressions
{
    //Abstract node of a node tree
    public abstract class Node_P
    {
        public abstract Polynomial Calculate();
    }

    //Node of number
    class NodePolynomial : Node_P
    {
        Polynomial pol;
        public NodePolynomial(Polynomial P)
        {
            pol = P;
        }
        public override Polynomial Calculate()
        {
            return pol;
        }
    }

    //Node for unary operation (eg. -10)
    class NodePolUnary : Node_P
    {
        Node_P right; //has only one side
        Operation op;

        public NodePolUnary(Node_P R, Operation O)
        {
            right = R;
            op = O;
        }

        public override Polynomial Calculate()
        {
            Polynomial r_res = right.Calculate();
            Polynomial res = CreateOperation.EvaluateOperation(r_res, op);
            return res;
        }
    }

    //Node for binary operation
    class NodePolBinary : Node_P
    {
        Node_P left; //left side of node
        Node_P right; //right side of node
        Operation op; //operation between sides (eg. sum, substration, etc.)

        //Constructor
        public NodePolBinary(Node_P L, Node_P R, Operation O)
        {
            left = L;
            right = R;
            op = O;
        }

        //Evaluate the whole operation
        public override Polynomial Calculate()
        {
            Polynomial l_res = left.Calculate();
            Polynomial r_res = right.Calculate();
            Polynomial res = CreateOperation.EvaluateOperation(l_res, r_res, op);
            return res;
        }
    }
}
