using System.IO;
using System;
using System.Linq;

namespace CommonCode.Converters
{
    public class PaceInputDecomposition: PaceInput
    {
        public (DecompositionNode root, int treewidth, int verticesCount) Parse(Stream stream)
        {
            var reader = new StreamReader(stream);
            string[] header = NextLine(reader);
            
            if (header.Length != 5 || header[0] != "s" || header[1] != "td")
                throw new ArgumentException($"Wrong header encountered");

            int bagsCount = int.Parse(header[2]);
            int treeWidth = int.Parse(header[3]);
            int verticesCount = int.Parse(header[4]);
            
            //Wczytanie worków
            DecompositionNode[] bags = new DecompositionNode[bagsCount];
            for (int i = 0; i < bagsCount; ++i)
            {
                string[] bag = NextLine(reader);
                
                if (bag[0] != "b" || int.Parse(bag[1]) - 1 != i)
                    throw new ArgumentException($"Bag {i + 1} error: wrong format");
                
                //Zakładamy, że wierzchołki grafu są numerowane od 0 w reprezentacji wewnętrznej.
                bags[i] = new DecompositionNode(bag.Skip(2).Select(v => int.Parse(v) - 1));
            }
            
            //Wczytanie krawędzi
            //Zakładamy, że 1 worek jest korzeniem
            //Dodatkowo krawędź skierowana jest z ojca na syna
            for (int i = 0; i < bagsCount - 1; ++i)
            {
                string[] edge = NextLine(reader);
                
                if (edge.Length != 2)
                    throw new ArgumentException($"Edge{i + 1} error: wrong format");

                int parent = int.Parse(edge[0]) - 1;
                int child = int.Parse(edge[1]) - 1;

                bags[parent].Children.Add(bags[child]);
                bags[child].Parent = bags[parent];
            }

            return (bags[0], treeWidth, verticesCount);
        }
    }
}