using Microsoft.VisualStudio.TestTools.UnitTesting;
using Coloring;
using System.Collections.Generic;

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
            var coloringAlgorithm = new ColoringAlgorithm(root, new bool[4, 4], 3);
            coloringAlgorithm.FindColoring();
            
        }
    }
}
