using CommonCode;

namespace TwoApproxRefiner
{
    public class Refiner
    {
        private Graph graph;
        private DecompositionNode root;
        private int treeWidth;

        public Refiner(Graph graph, DecompositionNode root, int treewidth)
        {
            this.graph = graph;
            this.root = root;
            this.treeWidth = treewidth;
        }

        public (DecompositionNode refinedDecomposition, int refinedTreewidth) RefineDecomposition()
        {
            //Tutaj dodac algorytm poprawiania
            
            return (root, treeWidth);
        }
    }
}