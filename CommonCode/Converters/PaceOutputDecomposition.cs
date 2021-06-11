using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace CommonCode.Converters
{
    /// <summary>
    /// Klasa odpowiada za wyksportowanie wyniku dekompozycji do formatu zgodnego z PACE17
    /// </summary>
    public class PaceOutputDecomposition
    {
        private List<List<int>> bags;
        private List<(int u, int v)> edges;
        private int node_counter;
        
        public void Write(Stream stream, DecompositionNode root, int tw, int verticesCount)
        {
            var writer = new StreamWriter(stream);

            bags = new List<List<int>>();
            edges = new List<(int u, int v)>();
            node_counter = 1;

            //Przeszukaj wszerz drzewo dekompozycji
            BreadthSearch(root);
            
            writer.WriteLine("s td {0} {1} {2}", bags.Count, tw, verticesCount);

            int i = 1;
            foreach (List<int> vertices in bags)
            {
                StringBuilder str = new StringBuilder();
                str.AppendFormat("b {0}", i++);
                foreach (int vertex in vertices)
                    str.AppendFormat(" {0}", vertex + 1);
                writer.WriteLine(str.ToString());
            }

            foreach ((int u, int v) in edges)
                writer.WriteLine("{0} {1}", u, v);
            writer.Flush();
        }

        private void BreadthSearch(DecompositionNode root)
        {
            Queue<(int, DecompositionNode)> order = new Queue<(int, DecompositionNode)>();
            order.Enqueue((this.node_counter++, root));

            while (order.Count > 0)
            {
                (int number, DecompositionNode node) = order.Dequeue();
                bags.Add(node.Vertices);

                foreach (DecompositionNode child in node.Children)
                {
                    order.Enqueue((this.node_counter, child));
                    edges.Add((number, this.node_counter++));
                }
            }
        }
    }
}