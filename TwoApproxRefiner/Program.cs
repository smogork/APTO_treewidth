using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;
using CommonCode;
using CommonCode.Converters;

namespace TwoApproxRefiner
{
    /// <summary>
    /// Ten program ma za zadanie poprawić otrzymaną wstępną dekompozycję
    /// zgodnie z pracą https://arxiv.org/pdf/2104.07463.pdf
    /// </summary>
    class Program
    {
        private static (DecompositionNode root, int treewidth, int verticesCount) ReadDecomposition(string path)
        {
            PaceInputDecomposition parser = new PaceInputDecomposition();

            using (Stream input = File.Open(path, FileMode.Open))
            {
                return parser.Parse(input);
            }
        }
        
        private static Graph ReadGraph(string path)
        {
            PaceInputGraph parser = new PaceInputGraph();

            using (Stream input = File.Open(path, FileMode.Open))
            {
                return parser.Parse(input);
            }
        }

        private static void SaveDecomposition(string path, DecompositionNode root, int tw, int verticesCount)
        {
            PaceOutputDecomposition writer = new PaceOutputDecomposition();
            Stream output;
            if (path == null)
                output = Console.OpenStandardOutput();
            else
                output = File.Open(path, FileMode.Create);
            
            writer.Write(output, root, tw, verticesCount);
            output.Flush();
        }
        
        static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunOptions)
                .WithNotParsed(HandleParseError);
        }

        static void RunOptions(Options opts)
        {
            //1. Wczytaj dane z pliku
            var decomp = ReadDecomposition(opts.InputDecompositionPath);
            Graph g = ReadGraph(opts.InputGraphPath);

            //2. Wykonaj algorytm
            Refiner refiner = new Refiner(g, decomp.treewidth - 1);
            (DecompositionNode newDecomp, int newTreeWidth) = refiner.RefineDecomposition(decomp.root);

            //3. Wypisz wyniki
            SaveDecomposition(opts.OutputPath, newDecomp, newTreeWidth + 1, decomp.verticesCount);
        }

        static void HandleParseError(IEnumerable<Error> errs)
        {
            Console.WriteLine("Errors:");
            foreach(Error e in errs)
            {
                Console.Error.WriteLine(e.ToString());
            }
        }
    }
}