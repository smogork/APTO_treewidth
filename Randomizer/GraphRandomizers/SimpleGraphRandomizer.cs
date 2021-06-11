using System;
using CommonCode;

namespace Randomizer.GraphRandomizers
{
    public class SimpleGraphRandomizer: IGraphRandomizer
    {
        private int edgesCount;
        private int verticesCount;
        private UniqueRandom rand;
        
        public SimpleGraphRandomizer(int verticesCount, int edgesCount)
        {
            this.verticesCount = verticesCount;
            this.edgesCount = edgesCount;
            rand = new UniqueRandom((verticesCount - 1) * (verticesCount - 2));
        }

        private (int u, int v) TranslateRandomToEdge(int n)
        {
            int row = (int)Math.Floor((-1.0 + Math.Sqrt(1 + 8 * n)) / 2.0);
            int col = n - ((row * (row + 1)) / 2);

            return (row + 1, col);
        }

        public Graph Randomize()
        {
            Graph result = new Graph(this.verticesCount);
            rand.NonrepetableReset();

            for (int i = 0; i < this.edgesCount; ++i)
            {
                int n = rand.Next();
                var edge = TranslateRandomToEdge(n);
                result.AddEdge(edge);
            }

            return result;
        }
    }
}