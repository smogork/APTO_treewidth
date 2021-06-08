using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonCode;

namespace Coloring
{
    public class ColoringAlgorithm
    {
        DecompositionNode DecompositionRoot;
        //List<int>[] Graph;  todo jak będzie szybszy alg
        public bool[,] GraphMatrix;
        int NumberOfColors;
        public int[] ResultColoring;
        public int NumberOfGraphNodes;
        // pola pomocnicze do konwersji PACE -> C#
        private List<int>[] neighbours;
        private bool[] visited;
        private DecompositionNode[] decompositionNodes;
        public ColoringAlgorithm(string filePathToGraph, string filePathToDecomposition, int numberOfColors)
        {
            ReadGraph(filePathToGraph);
            ReadDecomposition(filePathToDecomposition);
            this.NumberOfColors = numberOfColors;
        }
        private void ReadGraph(string filePath)
        {
            string line;
            System.IO.StreamReader file =
                new System.IO.StreamReader(filePath);  
            while((line = file.ReadLine()) != null)  
            {  
                if (line[0] == 'c')
                    continue;
                string[] subs = line.Split(' ');
                if (subs[0] == "p")
                {
                    this.NumberOfGraphNodes = Convert.ToInt32(subs[2]);
                    this.GraphMatrix = new bool[this.NumberOfGraphNodes + 1, this.NumberOfGraphNodes + 1];
                }
                else
                {
                    int v1 = Convert.ToInt32(subs[0]);
                    int v2 = Convert.ToInt32(subs[1]);
                    this.GraphMatrix[v1, v2] = true;
                    this.GraphMatrix[v2, v1] = true;
                }
            }
            file.Close();
        }
        private void ReadDecomposition(string filePath)
        {
            string line;  
            int numberOfDecompositionNodes = 0;
            System.IO.StreamReader file =
                new System.IO.StreamReader(filePath);  
            while((line = file.ReadLine()) != null)  
            {  
                if (line[0] == 'c')
                    continue;
                string[] subs = line.Split(' ');
                if (subs[0] == "s")
                {
                    numberOfDecompositionNodes = Convert.ToInt32(subs[2]);
                    neighbours = new List<int>[numberOfDecompositionNodes + 1];
                    for (int i = 1; i <= numberOfDecompositionNodes; ++i)
                        neighbours[i] = new List<int>();
                    decompositionNodes = new DecompositionNode[numberOfDecompositionNodes + 1];
                }
                else if (subs[0] == "b")
                {
                    int nodeIndex = Convert.ToInt32(subs[1]);
                    List<int> vertices = new List<int>();
                    for (int i = 2; i < subs.Length; ++i)
                        vertices.Add(Convert.ToInt32(subs[i]));
                    decompositionNodes[nodeIndex] = new DecompositionNode(vertices);
                }
                else
                {
                    int v1 = Convert.ToInt32(subs[0]);
                    int v2 = Convert.ToInt32(subs[1]);
                    neighbours[v1].Add(v2);
                    neighbours[v2].Add(v1);
                }
            }
            decompositionNodes[1].Parent = new DecompositionNode(new List<DecompositionNode>(){ decompositionNodes[1] }, null, new List<int>());
            this.DecompositionRoot = decompositionNodes[1];
            visited = new bool[numberOfDecompositionNodes + 1];
            SetParenthood(1);
            file.Close();  
        }
        private void SetParenthood(int nodeIndex)
        {
            visited[nodeIndex] = true;
            foreach(int neighbour in neighbours[nodeIndex])
                if (!visited[neighbour])
                {
                    decompositionNodes[neighbour].Parent = decompositionNodes[nodeIndex];
                    decompositionNodes[nodeIndex].Children.Add(decompositionNodes[neighbour]);
                    SetParenthood(neighbour);
                }
        }
        public ColoringAlgorithm(DecompositionNode decompositionRoot, bool[,] graphMatrix, int numberOfColors)
        {
            this.DecompositionRoot = decompositionRoot;
            this.GraphMatrix = graphMatrix;
            this.NumberOfColors = numberOfColors;
            this.NumberOfGraphNodes = graphMatrix.GetUpperBound(1);
        }
        public bool FindColoring()
        {
            FindColoringInWorseTime(DecompositionRoot);
            if (GetColoring())
                return true;
            return false;
        }
        private void FindColoringInWorseTime(DecompositionNode decompositionNode)
        {
            for (int i = 0; i < decompositionNode.Children.Count; ++i)
                FindColoringInWorseTime(decompositionNode.Children[i]);
            decompositionNode.Common = new List<int>();
            for (int i = 0; i < decompositionNode.Vertices.Count; ++i)
                for (int j = 0; j < decompositionNode.Parent.Vertices.Count; ++j)
                    if (decompositionNode.Vertices[i] == decompositionNode.Parent.Vertices[j])
                        decompositionNode.Common.Add(decompositionNode.Vertices[i]);
            int combinationsCount = (int)Math.Pow(NumberOfColors, decompositionNode.Vertices.Count);
            int combinationsCommonCount = (int)Math.Pow(NumberOfColors, decompositionNode.Common.Count);
            decompositionNode.dp = Enumerable.Repeat(-1, combinationsCommonCount + 1).ToArray();
            Parallel.For(0, combinationsCount,
                number =>
                {

                    int[] colors = new int[this.NumberOfGraphNodes + 1];
                    int tmp = number;
                    int i = 0;
                    while (tmp > 0)
                    {
                        colors[decompositionNode.Vertices[i]] = tmp % NumberOfColors;
                        tmp /= NumberOfColors;
                        i++;
                    }

                    bool correctColoring = true;
                    for (i = 0; i < decompositionNode.Vertices.Count; ++i)
                    for (int j = i + 1; j < decompositionNode.Vertices.Count; ++j)
                        if (GraphMatrix[decompositionNode.Vertices[i], decompositionNode.Vertices[j]] &&
                            colors[decompositionNode.Vertices[i]] == colors[decompositionNode.Vertices[j]])
                        {
                            correctColoring = false;
                            break;
                        }

                    if (!correctColoring)
                        return;
                    bool childrenApprove = true;
                    for (i = 0; i < decompositionNode.Children.Count; ++i)
                    {
                        tmp = GetCommonColoringNumber(decompositionNode.Children[i], colors);
                        if (decompositionNode.Children[i].dp[tmp] == -1)
                        {
                            childrenApprove = false;
                            break;
                        }
                    }

                    if (childrenApprove)
                        decompositionNode.dp[GetCommonColoringNumber(decompositionNode, colors)] = number;
                });
        }
        private bool GetColoring()
        {
            if (DecompositionRoot.dp[0] != -1)
            {
                ResultColoring = new int[NumberOfGraphNodes + 1];
                GetColoringRecursive(DecompositionRoot, DecompositionRoot.dp[0]);
                return true;
            }
            return false;

        }
        private void GetColoringRecursive(DecompositionNode decompositionNode, int number)
        {
            int tmp = number;
            int i = 0;
            while (tmp > 0)
            {
                ResultColoring[decompositionNode.Vertices[i]] = tmp % NumberOfColors;
                tmp /= NumberOfColors;
                i++;
            }
            for (i = 0; i < decompositionNode.Children.Count; ++i)
                GetColoringRecursive(decompositionNode.Children[i], 
                    decompositionNode.Children[i].dp[GetCommonColoringNumber(decompositionNode.Children[i], ResultColoring)]);
        }
        private int GetCommonColoringNumber(DecompositionNode decompositionNode, int[] colors)
        {
            int tmp = 0;
            int powK = 1;
            for (int i = 0; i < decompositionNode.Common.Count; ++i)
            {
                tmp += colors[decompositionNode.Common[i]] * powK;
                powK *= NumberOfColors;
            }
            return tmp;
        }
    }
}