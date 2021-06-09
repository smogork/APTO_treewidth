using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonCode.Converters
{
    public class PaceInputDecomposition: PaceInput
    {
        private bool[] visited;
        private List<int>[] neighbours;
        private DecompositionNode[] bags;
        
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
            bags = new DecompositionNode[bagsCount];
            neighbours = new List<int>[bagsCount];
            for (int i = 0; i < bagsCount; ++i)
            {
                string[] bag = NextLine(reader);
                
                if (bag[0] != "b" || int.Parse(bag[1]) - 1 != i)
                    throw new ArgumentException($"Bag {i + 1} error: wrong format");
                
                //Zakładamy, że wierzchołki grafu są numerowane od 0 w reprezentacji wewnętrznej.
                bags[i] = new DecompositionNode(bag.Skip(2).Select(v => int.Parse(v) - 1));
                neighbours[i] = new List<int>();
            }
            
            //Wczytanie krawędzi
            for (int i = 0; i < bagsCount - 1; ++i)
            {
                string[] edge = NextLine(reader);
                
                if (edge.Length != 2)
                    throw new ArgumentException($"Edge{i + 1} error: wrong format");

                int u = int.Parse(edge[0]) - 1;
                int v = int.Parse(edge[1]) - 1;
                
                neighbours[u].Add(v);
                neighbours[v].Add(u);
            }
            
            //Ustawienie zależności ojciec-dziecko z wcześniej wczytanych krawędzi
            visited = new bool[bagsCount];
            SetParenthood(0);

            return (bags[0], treeWidth, verticesCount);
        }
        
        /// <summary>
        /// Przegladanie wszerz grafu i ustawianie relacji ojciec-dziecko na node'ach.
        /// </summary>
        /// <param name="root">Indeks worka, który ma byc rootem</param>
        private void SetParenthood(int root)
        {
            Queue<int> order = new Queue<int>();
            order.Enqueue(root);
            
            while (order.Count > 0)
            {
                int parent = order.Dequeue();
                visited[parent] = true;

                foreach (int child in neighbours[parent])
                {
                    if (!visited[child])
                    {
                        order.Enqueue(child);

                        bags[child].Parent = bags[parent];
                        bags[parent].Children.Add(bags[child]);
                    }
                }
            }
        }
    }
}