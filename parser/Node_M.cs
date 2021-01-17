namespace Parser_of_Mathematical_Expressions
{
    public abstract class Node_M
    {
        public abstract double[,] Calculate();
    }
    class NodeMatr:Node_M
    {
        double[,] matr;

        public NodeMatr(double[,] M)
        {
            matr =M;
        }

        public override double[,] Calculate()
        {
            return matr;
        }
    }

    //Node for unary operation (eg. -10)
    class NodeMatrUnary : Node_M
    {
        Node_M right; //has only one side
        Operation op;

        public NodeMatrUnary(Node_M R, Operation O)
        {
            right = R;
            op = O;
        }

        public override double[,] Calculate()
        {
            double[,] r_res = right.Calculate();
            double[,] res = CreateOperation.EvaluateOperation(r_res, op);
            return res;
        }
    }

    //Node for binary operation
    class NodeMatrBinary : Node_M
    {
        Node_M left; //left side of node
        Node_M right; //right side of node
        Operation op; //operation between sides (eg. sum, substration, etc.)

        //Constructor
        public NodeMatrBinary(Node_M L, Node_M R, Operation O)
        {
            left = L;
            right = R;
            op = O;
        }

        //Evaluate the whole operation
        public override double[,] Calculate()
        {
            double[,] l_res = left.Calculate();
            double[,] r_res = right.Calculate();
            double[,] res = CreateOperation.EvaluateOperation(l_res, r_res, op);
            return res;
        }
    }
}
