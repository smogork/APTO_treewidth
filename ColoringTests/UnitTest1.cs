using Microsoft.VisualStudio.TestTools.UnitTesting;
using Coloring;
using System.Collections.Generic;
using System;

namespace ColoringTests
{
    [TestClass]
    public class UnitTest1
    {
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
            for (int i = 1; i <= coloringAlgorithm.ResultColoring.Length; ++i)
                Console.WriteLine("{0} {1}", i, coloringAlgorithm.ResultColoring[i]);
        }
    }
}
