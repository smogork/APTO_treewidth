using System.Collections.Generic;
using System;

namespace Coloring
{
    public class ColoringAlgorithm
    {
        DecompositionNode DecompositionRoot;
        List<int>[] Graph;
        bool[,] GraphMatrix;
        int NumberOfColors;
        public int[] ResultColoring;
        int DecompositionNodesCount;
        public ColoringAlgorithm(string filename)
        {
            throw new NotImplementedException();
        }
        public ColoringAlgorithm(DecompositionNode decompositionRoot, bool[,] graphMatrix, int numberOfColors)
        {
            this.DecompositionRoot = decompositionRoot;
            this.GraphMatrix = graphMatrix;
            this.NumberOfColors = numberOfColors;
            this.DecompositionNodesCount = graphMatrix.GetUpperBound(1);
            Console.WriteLine(this.DecompositionNodesCount);
        }
        public void FindColoring()
        {
            if (Graph == null)
                FindColoringInWorseTime(DecompositionRoot);
            if (GetColoring())
                Console.WriteLine("Coloring found");
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
            decompositionNode.dp = new int[combinationsCount];
            for (int number = 0; number < combinationsCount; ++number)
            {
                int[] colors = new int[DecompositionNodesCount + 1];
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
                    continue;
                bool childrenApprove = true;
                int powK;
                for (i = 0; i < decompositionNode.Children.Count; ++i)
                {
                    tmp = 0;
                    powK = 1;
                    for (int j = 0; j < decompositionNode.Children[i].Common.Count; ++j)
                    {
                        tmp += colors[decompositionNode.Children[i].Common[i]] * powK;
                        powK *= NumberOfColors;
                    }
                    if (decompositionNode.Children[i].dp[tmp] == -1)
                    {
                        childrenApprove = false;
                        break;
                    }
                }
                if (!childrenApprove)
                    continue;
                decompositionNode.dp[GetCommonColoringNumber(decompositionNode, colors)] = number;
            }
        }
        private bool GetColoring()
        {
            for (int i = 0; i < DecompositionRoot.dp.Length; ++i)
                if (DecompositionRoot.dp[i] != -1)
                {
                    ResultColoring = new int[DecompositionNodesCount + 1];
                    GetColoringRecursive(DecompositionRoot, DecompositionRoot.dp[i]);
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
                GetColoringRecursive(decompositionNode.Children[i], GetCommonColoringNumber(decompositionNode, ResultColoring));
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