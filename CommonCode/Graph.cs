using System;
using System.Collections.Generic;

namespace CommonCode
{
    /// <summary>
    /// Rakowa implementacja tablicy sąsiedztwa dla grafu nieskierowanego.
    /// Tablicy hashowanej będzie przechowywać pary połączonych wierzchołków w porządku e = (v1,v2),
    /// gdzie v1 mniejsze od v2.
    /// </summary>
    public class Graph
    {
        public int VerticesCount { get; private set; }
        /// <summary>
        /// Wierzchołki numerujemy od 0
        /// </summary>
        private HashSet<(int u, int v)> edges;

        public Graph(int verticesCount)
        {
            this.VerticesCount = verticesCount;
            edges = new HashSet<(int u, int v)>();
        }
        
        public Graph(int verticesCount, IEnumerable<(int u, int v)> edges)
        {
            this.VerticesCount = verticesCount;
            edges = new HashSet<(int u, int v)>();

            foreach ((int u, int v) in edges)
                AddEdge(u, v);
        }

        private (int u, int v) CorrectOrder(int u, int v)
        {
            return u > v ? (v, u) : (u, v);
        }

        /// <summary>
        /// Wierzchołki numerujemy od 0
        /// </summary>
        public bool AddEdge(int u, int v)
        {
            if (u == v)
                throw new ArgumentException("Vertex cannot has edge to itself");
            
            return edges.Add(CorrectOrder(u, v));
        }

        public bool IsConnected(int u, int v)
        {
            if (u == v)
                return true;

            return this.edges.Contains(CorrectOrder(u, v));
        }
    }
}