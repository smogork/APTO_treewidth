using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
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
            
            //Liczność zbioru X decyduje o byciu minimalnym splitem.
            public int Value => X.Count;

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
        //private DecompositionNode root;
        private int treeWidth;
        private int k;
        
        public Refiner(Graph graph, int treewidth)
        {
            this.graph = graph;
            this.treeWidth = treewidth;
            this.k = Math.Max(treewidth / 4, 1);
        }

        #region Split
        /// <summary>
        /// Metoda iteruje po liczbie w sytemie czwórkowym.
        /// Każda taka liczba reprezentuje podział zbioru na 4 rozłączne podzbiory.
        /// </summary>
        /// <param name="verticesNumber">Liczba wierzchołków w grafie.</param>
        /// <returns>Zwraca kolekcję podziałów zbioru wierzchołków.</returns>
        private IEnumerable<Long4Number> IterateSplits(int verticesNumber)
        {
            Long4Number split = new Long4Number(verticesNumber);

            while (split.Increment())
                yield return split;
        }
        
        /// <summary>
        /// Przeszukuje każdą parę wierzchołków w zbiorach A i B.
        /// Dla każdej pary sprawdza, czy istnieje pomiędzy nimi krawędź.
        /// </summary>
        /// <param name="split">Podział zbioru wierzchołków.</param>
        /// <param name="A">Numer zbioru A.</param>
        /// <param name="B">Numer zbioru B.</param>
        /// <returns>true - istnieje krawędź dla pewnej pary; false - wpw.</returns>
        private bool AreEdgesBeetwenSets(Long4Number split, byte A, byte B)
        {
            for (int itA = 0; itA < split.Length; ++itA)
            {
                if (split.Representation[itA] == A || split.Representation[itA] == B)
                {
                    for (int itB = itA + 1; itB < split.Length; ++itB)
                    {
                        if (graph.IsEdge(itA, itB))
                            return true;
                    }
                }
            }
            return false;
        }
        
        /// <summary>
        /// Metoda sprawdza warunek na liczność przecięcia się zbioru A z W oraz sumy z X
        /// opisanym w rozdziale 3.1.
        /// </summary>
        /// <param name="split">Rozważany podział wierzchołków.</param>
        /// <param name="W">Rozważany worek dekompozycji.</param>
        /// <param name="A">Zbiór, dla którego sprawdzamy warunek.</param>
        /// <returns>true - sbiór spełnia warunek; false - wpw.</returns>
        private bool CheckSetWCondition(Long4Number split, DecompositionNode W, byte A)
        {
            int count = 0;
            
            //Zlicz przecięcie zbioru A oraz W
            foreach (int vertex in W.Vertices)
                if (split.Representation[vertex] == A)
                    count++;
            
            //Później dodaj do tego liczbę elementów z X
            //Wystarczy zwykla suma, bo A i X są rozłączne
            for (int i = 0; i < split.Length; ++i)
                if (split.Representation[i] == VertexSplit.XCode)
                    count++;

            return count < W.Vertices.Count;
        }

        /// <summary>
        /// Spradzenie warunków na krawędzie pomiędzy Ci oraz liczności zbiorów
        /// </summary>
        /// <returns>true - split poprawny; wpw. false</returns>
        private bool CheckIfSplitIsCorrect(Long4Number split, DecompositionNode W)
        {
            //Edges
            if (AreEdgesBeetwenSets(split, VertexSplit.C1Code, VertexSplit.C2Code))
                return false;
            if (AreEdgesBeetwenSets(split, VertexSplit.C1Code, VertexSplit.C3Code))
                return false;
            if (AreEdgesBeetwenSets(split, VertexSplit.C2Code, VertexSplit.C3Code))
                return false;
            
            //Warunke na przecięcie z W
            if (!CheckSetWCondition(split, W, VertexSplit.C1Code))
                return false;
            if (!CheckSetWCondition(split, W, VertexSplit.C2Code))
                return false;
            if (!CheckSetWCondition(split, W, VertexSplit.C3Code))
                return false;
            
            return true;
        }

        private int ValueOfSplit(Long4Number split)
        {
            int value = 0;
            for (int i = 0; i < split.Length; ++i)
                if (split.Representation[i] == VertexSplit.XCode)
                    value++;
            return value;
        }

        /// <summary>
        /// Znajduje split na worku W zgodnie z opisem w 3.1
        /// </summary>
        /// <param name="W">Worek na którym tworzymy dekompozycję</param>
        /// <returns>Zwraca gotowy minimalny split wierzchołków.</returns>
        private VertexSplit FindSplitOn(DecompositionNode W)
        {
            Long4Number bestSplit = null;
            int splitValue = graph.VerticesCount;
            
            foreach (Long4Number split in IterateSplits(graph.VerticesCount))
            {
                if (CheckIfSplitIsCorrect(split, W))
                {
                    int val = ValueOfSplit(split);
                    if (val < splitValue)
                    {
                        splitValue = val;
                        bestSplit = new Long4Number(split);
                    }
                }
            }
            
            //Nie udało się znaleźć splitu => szerokość drzewowa większa niż oczekiwana
            if (bestSplit == null)
                return null;
            
            return new VertexSplit(bestSplit);
        }
        #endregion

        public (DecompositionNode refinedDecomposition, int refinedTreewidth) RefineDecomposition(DecompositionNode root)
        {
            //Tutaj dodac algorytm poprawiania
            while (true)
            {
                var wBag = FindLargestBag(root);
                if (wBag.Vertices.Count <= 2 * k + 2)
                    return (root, CountTreewidth(root));
                var split = FindSplitOn(wBag);
                if (split == null)
                    return (root, CountTreewidth(root));
                var t1Bag = BuildDecomposition(wBag, split.C1, split.X);
                var t2Bag = BuildDecomposition(wBag, split.C2, split.X);
                var t3Bag = BuildDecomposition(wBag, split.C3, split.X);
                root = Merge(t1Bag, t2Bag, t3Bag, split.X);
            }
        }
        private DecompositionNode FindLargestBag(DecompositionNode decompositionNode)
        {
            var largestBag = decompositionNode;
            for (int i = 0; i < decompositionNode.Children.Count; ++i)
                if (FindLargestBag(decompositionNode.Children[i]).Vertices.Count > largestBag.Vertices.Count)
                    largestBag = decompositionNode.Children[i];
            return largestBag;
        }

        private int CountTreewidth(DecompositionNode root)
        {
            Queue<DecompositionNode> order = new Queue<DecompositionNode>();
            order.Enqueue(root);

            int treewidth = 0;
            while (order.Count > 0)
            {
                DecompositionNode node = order.Dequeue();

                int tw = node.Vertices.Count - 1;
                if (tw > treewidth)
                    treewidth = tw;

                foreach (DecompositionNode child in node.Children)
                    order.Enqueue(child);
            }

            return treewidth;
        }
        
        #region Merge
        private DecompositionNode BuildDecomposition(DecompositionNode wBag, 
             HashSet<int> cSet, HashSet<int> xSet)
        {
            var homeBag = new DecompositionNode[graph.VerticesCount];
            var vertices = new List<int>();
            for (int i = 0; i < wBag.Vertices.Count; ++i) 
                if (cSet.Contains(wBag.Vertices[i]) || xSet.Contains(wBag.Vertices[i]))
                    vertices.Add(wBag.Vertices[i]);
            var t = new DecompositionNode(vertices);
            t.Distance = 0;
            var result = t;
            var queue = new Queue<(DecompositionNode originalNode, DecompositionNode newNode)>();
            queue.Enqueue((wBag, t));
            while (queue.Count > 0)
            {
                (wBag, t) = queue.Dequeue();
                for (int i = 0; i < wBag.Children.Count; ++i)
                {
                    vertices = new List<int>();
                    for (int j = 0; j < wBag.Children[i].Vertices.Count; ++j)
                        if (cSet.Contains(wBag.Children[i].Vertices[j]) ||
                            xSet.Contains(wBag.Children[i].Vertices[j]))
                            vertices.Add(wBag.Children[i].Vertices[j]);
                    var kid = new DecompositionNode(vertices);
                    kid.Distance = t.Distance + 1;
                    kid.Parent = t;
                    t.Children.Add(kid);
                    queue.Enqueue((wBag.Children[i], kid));
                }
                for (int i = 0; i < t.Vertices.Count; ++i)
                    if (homeBag[t.Vertices[i]] != null)
                        homeBag[t.Vertices[i]] = t;
            }
            foreach (var x in xSet)
                FillPath(wBag, homeBag[x], x);
            return result;
        }
        private void FillPath(DecompositionNode wBag, DecompositionNode homeBag, int x)
        {
            if (homeBag == null)
                return;
            if (!homeBag.Vertices.Contains(x)) 
                homeBag.Vertices.Add(x);
            if (homeBag == wBag)
                return;
            var nextBag = homeBag.Parent;
            for (int i = 0; i < homeBag.Children.Count; ++i)
                if (homeBag.Children[i].Distance == homeBag.Distance - 1)
                {
                    nextBag = homeBag.Children[i];
                    break;
                }
            FillPath(wBag, nextBag, x);
        }
        private DecompositionNode Merge(DecompositionNode t1, DecompositionNode t2, DecompositionNode t3,
            HashSet<int> x)
        {
            var xBag = new DecompositionNode(x);
            xBag.Children.Add(t1);
            xBag.Children.Add(t2);
            xBag.Children.Add(t3);
            t1.Parent = xBag;
            t2.Parent = xBag;
            t2.Parent = xBag;
            return xBag;
        }
        #endregion
    }
}