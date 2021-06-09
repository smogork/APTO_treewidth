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
        private DecompositionNode root;
        private int treeWidth;
        
        public Refiner(Graph graph, DecompositionNode root, int treewidth)
        {
            this.graph = graph;
            this.root = root;
            this.treeWidth = treewidth;
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
                if (split.Representation[itA] == A)
                {
                    for (int itB = itA + 1; itB < split.Length; ++itB)
                    {
                        if (graph.IsConnected(itA, itB))
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
                        bestSplit = split;
                    }
                }
            }
            
            //Nie udało się znaleźć splitu => szerokość drzewowa większa niż oczekiwana
            if (bestSplit == null)
                return null;
            
            return new VertexSplit(bestSplit);
        }
        #endregion

        public (DecompositionNode refinedDecomposition, int refinedTreewidth) RefineDecomposition()
        {
            //Tutaj dodac algorytm poprawiania
            
            return (root, treeWidth);
        }
    }
}