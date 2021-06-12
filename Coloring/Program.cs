using System;
using System.Collections.Generic;
using CommandLine;

namespace Coloring
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunOptions)
                .WithNotParsed(HandleParseError);
        }

        static void RunOptions(Options opts)
        {
            //1. Wczytaj dane z pliku
            var coloringAlgorithm = new ColoringAlgorithm(opts.InputGraphPath, opts.InputDecompositionPath, opts.ColorCount);
            
            //2. Wykonaj algorytm
            coloringAlgorithm.FindColoring();

            //3. Wypisz wyniki
        }

        static void HandleParseError(IEnumerable<Error> errs)
        {
            Console.WriteLine("Errors:");
            foreach(Error e in errs)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
