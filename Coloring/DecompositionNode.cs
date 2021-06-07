using System.Collections.Generic;
using System.Linq;

namespace Coloring
{
    public class DecompositionNode
    {
        public List<DecompositionNode> Children;
        public DecompositionNode Parent;
        public List<int> Vertices;
        public List<int> Common;
        public int[] dp;
        public DecompositionNode(List<DecompositionNode> Children, DecompositionNode Parent, List<int> Vertices)
        {
            this.Children = Children;
            this.Parent = Parent;
            this.Vertices = Vertices;
            this.dp = Enumerable.Repeat(-1, Vertices.Count).ToArray();
        }
        public DecompositionNode (List<int> Vertices)
        {
            this.Children = new List<DecompositionNode>();
            this.Vertices = Vertices;
            this.dp = Enumerable.Repeat(-1, Vertices.Count).ToArray();
        }
    }
}