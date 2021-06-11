using System;
using CommonCode;
using CommonCode.Converters;

namespace Randomizer
{
    class Program
    {
        static void Main(string[] args)
        {
            PaceOutputGraph output = new PaceOutputGraph();
            Graph path = new Graph(4);
            path.AddEdge(0, 1);
            path.AddEdge(2, 1);
            path.AddEdge(3, 2);
            
            output.Write(Console.OpenStandardOutput(), path);
        }
    }
}