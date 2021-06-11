using System;
using System.Collections.Generic;
using CommonCode;
using CommonCode.Converters;

namespace Randomizer.GraphRandomizers
{
    public class DecompositionGenerator: IGraphRandomizer
    {
        private int tw, bagsCount, nodesCount;
        private DecompositionNode root = new DecompositionNode(new List<int>());

        public DecompositionGenerator(int tw, int bagsCount, int nodesCount)
        {
            this.tw = tw;
            this.bagsCount = bagsCount;
            this.nodesCount = nodesCount;
        }

        public Graph Randomize()
        {
            Graph graph;
            Random random = new Random();
            int numberOfBags = bagsCount - 1;
            int maxKidsCount = bagsCount - 1;
            Queue<DecompositionNode> queue = new Queue<DecompositionNode>();
            List<DecompositionNode> bags = new List<DecompositionNode>();
            queue.Enqueue(this.root);
            bags.Add(this.root);
            // tworzenie struktury pustych worków z połączeniami
            while (numberOfBags > 0)
            {
                var bag = queue.Dequeue();
                int kidsCount = random.Next(1, maxKidsCount);
                if (numberOfBags - kidsCount < 0)
                    kidsCount = numberOfBags;
                numberOfBags -= kidsCount;
                for (int i = 0; i < kidsCount; ++i)
                {
                    var kid = new DecompositionNode(new List<int>());
                    kid.Parent = bag;
                    bag.Children.Add(kid);
                    bags.Add(kid);
                    queue.Enqueue(kid);
                }
            }
            for (int i = 0; i < nodesCount; ++i)
            {
                //wylosuj bag dopóki nie znajdziesz wolnego
                int bagNumber;
                while (true)
                {
                    bagNumber = random.Next(0, bagsCount);
                    if (bags[bagNumber].Vertices.Count < tw)
                        break;
                }
                bags[bagNumber].Vertices.Add(i);
            }
            // tworzenie grafu odpowiadającego dekompozycji
            graph = new Graph(nodesCount);
            int treewidth = 0;
            foreach (var bag in bags)
            {
                treewidth = Math.Max(treewidth, bag.Vertices.Count);
                
                for (int i = 0; i < bag.Vertices.Count; ++i)
                for (int j = i + 1; j < bag.Vertices.Count; ++j)
                    graph.AddEdge(bag.Vertices[i], bag.Vertices[j]);
            }

            //PaceOutputDecomposition outputDecomposition = new PaceOutputDecomposition();
            //outputDecomposition.Write(Console.OpenStandardOutput(), root, treewidth, nodesCount);
            return graph;
        }
    }
}