using System.Collections.Generic;
using System.Data;
using CommonCode;

namespace TwoApproxRefiner
{
    public class Refiner
    {
        #region VertexSplit
        private class VertexSplit
        {
            public const byte XCode = 0;
            public const byte C1Code = 1;
            public const byte C2Code = 2;
            public const byte C3Code = 3;
            
            public HashSet<int> C1 { get; set; }
            public HashSet<int> C2 { get; set; }
            public HashSet<int> C3 { get; set; }
            public HashSet<int> X { get; set; }

            public VertexSplit()
            {
                C1 = new HashSet<int>();
                C2 = new HashSet<int>();
                C3 = new HashSet<int>();
                X = new HashSet<int>();
            }

            public VertexSplit(Long4Number repr)
            {
                C1 = new HashSet<int>();
                C2 = new HashSet<int>();
                C3 = new HashSet<int>();
                X = new HashSet<int>();

                int i = 0;
                foreach (byte code in repr.Representation)
                {
                    switch (code)
                    {
                        case XCode:
                            X.Add(i);
                            break;
                        case C1Code:
                            C1.Add(i);
                            break;
                        case C2Code:
                            C2.Add(i);
                            break;
                        case C3Code:
                            C3.Add(i);
                            break;
                    }
                    i++;
                }
            }
        }
        #endregion
        
        
        private Graph graph;
        private DecompositionNode root;
        private int treeWidth;

        

        public Refiner(Graph graph, DecompositionNode root, int treewidth)
        {
            this.graph = graph;
            this.root = root;
            this.treeWidth = treewidth;
        }

        private IEnumerable<Long4Number> IterateSplits(int verticesNumber)
        {
            
        }

        //Spradzenie warunków na krawędzie pomiędzy Ci oraz liczności zbiorów
        private bool CheckIfSplitIsCorrect(Long4Number split)
        {
            
        }

        /// <summary>
        /// Znajduje split na worku W zgodnie z opisem w 3.1
        /// </summary>
        /// <param name="W">Worek na którym tworzymy dekompozycję</param>
        /// <returns>Zwraca gotowy minimalny split wierzchołków.</returns>
        private VertexSplit FindSplitOn(DecompositionNode W)
        {
            
        }

        public (DecompositionNode refinedDecomposition, int refinedTreewidth) RefineDecomposition()
        {
            //Tutaj dodac algorytm poprawiania
            
            return (root, treeWidth);
        }
    }
}