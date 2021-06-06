using Microsoft.VisualStudio.TestTools.UnitTesting;
using Coloring;
using System.Collections.Generic;
using System;

namespace ColoringTests
{
    [TestClass]
    public class UnitTest1
    {
        private bool CheckColoring(int[] colors, bool[,] graph)
        {
            if (colors == null)
                return false;
            bool correctColoring = true;
            for (int i = 0; i < colors.Length; ++i)
                for (int j = i + 1; j < colors.Length; ++j)
                    if (graph[i, j] && colors[i] == colors[j])
                    {
                        correctColoring = false;
                        break;
                    }
            return correctColoring;
        }
        private bool CheckColoring(int[] colors, List<int> graph)
        {
            if (colors == null)
                return false;
            bool correctColoring = true;
            for (int i = 0; i < colors.Length; ++i)
                foreach (int j in graph)
                    if (colors[i] == colors[j])
                    {
                        correctColoring = false;
                        break;
                    }
            return correctColoring;
        }
        [TestMethod]
        public void TestMethod1()
        {
            var underRoot = new DecompositionNode(new List<DecompositionNode>(), null, new List<int>());
            var root = new DecompositionNode(new List<DecompositionNode>(), underRoot, new List<int>(){1, 2});
            var child1 = new DecompositionNode(new List<DecompositionNode>(), root, new List<int>(){1, 3});
            root.Children.Add(child1);
            var graphMatrix = new bool[4, 4];
            graphMatrix[1, 2] = true;
            graphMatrix[2, 1] = true;
            var coloringAlgorithm = new ColoringAlgorithm(root, graphMatrix, 2);
            coloringAlgorithm.FindColoring();
            for (int i = 1; i < coloringAlgorithm.ResultColoring.Length; ++i)
                Console.WriteLine("{0} {1}", i, coloringAlgorithm.ResultColoring[i]);
            Assert.IsTrue(CheckColoring(coloringAlgorithm.ResultColoring, graphMatrix));
        }
        [TestMethod]
        public void TestMethod2()
        {
            var coloringAlgorithm = new ColoringAlgorithm("/home/oskar/APTO/APTO_treewidth/flow-cutter-pace17/flow-cutter-pace17/output.gr");
            coloringAlgorithm.FindColoring();
            for (int i = 1; i < coloringAlgorithm.ResultColoring.Length; ++i)
                Console.WriteLine("{0} {1}", i, coloringAlgorithm.ResultColoring[i]);
            Assert.IsTrue(CheckColoring(coloringAlgorithm.ResultColoring, graphMatrix));
        }
    }
}
