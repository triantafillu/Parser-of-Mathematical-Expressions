
namespace Parser_of_Mathematical_Expressions
{
    //Abstract node of a node tree
    public abstract class Node
    {
        public abstract double Calculate();
    }

    //Node of number
    class NodeNumber : Node
    {
        double number;
        public NodeNumber(double N)
        {
            number = N;
        }
        public override double Calculate()
        {
            return number;
        }
    }

    //Node for unary operation (eg. -10)
    class NodeUnary : Node
    {
        Node right; //has only one side
        Operation op;

        public NodeUnary(Node R, Operation O)
        {
            right = R;
            op = O;
        }

        public override double Calculate()
        {
            double r_res = right.Calculate();
            double res = CreateOperation.EvaluateOperation(r_res, op);
            return res;
        }
    }

    //Node for binary operation
    class NodeBinary : Node
    {
        Node left; //left side of node
        Node right; //right side of node
        Operation op; //operation between sides (eg. sum, substration, etc.)

        //Constructor
        public NodeBinary(Node L, Node R, Operation O)
        {
            left = L;
            right = R;
            op = O;
        }

        //Evaluate the whole operation
        public override double Calculate()
        {
            double l_res = left.Calculate();
            double r_res = right.Calculate();
            double res = CreateOperation.EvaluateOperation(l_res, r_res, op);
            return res;
        }
    }
}
