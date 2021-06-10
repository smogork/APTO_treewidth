using System.Collections.Generic;
using System.Linq;

namespace CommonCode
{
    public class DecompositionNode
    {
        /// <summary>
        /// Lista dzieci w strukturze drzewiastej.
        /// </summary>
        public List<DecompositionNode> Children { get; set; }
        /// <summary>
        /// Rodzic w strukturze drzewiastej.
        /// </summary>
        public DecompositionNode Parent { get; set; }
        /// <summary>
        /// Zbiór wierzchołków zawierający się w wybranym worku.
        /// </summary>
        public List<int> Vertices { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public List<int> Common { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public int[] dp { get; set; }
        public int Distance { get; set; }
        public DecompositionNode(List<DecompositionNode> Children, DecompositionNode Parent, IEnumerable<int> Vertices)
        {
            this.Children = Children;
            this.Parent = Parent;
            this.Vertices = new List<int>(Vertices);
            this.dp = Enumerable.Repeat(-1, Vertices.Count()).ToArray();
        }
        public DecompositionNode (IEnumerable<int> Vertices)
        {
            this.Children = new List<DecompositionNode>();
            this.Vertices = new List<int>(Vertices);
            this.dp = Enumerable.Repeat(-1, Vertices.Count()).ToArray();
        }
    }
}