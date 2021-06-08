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
        private static (DecompositionNode root, int treewidth, int verticesCount) InsertDecomposition(string path)
        {
            PaceInputDecomposition parser = new PaceInputDecomposition();

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
            var decomp = InsertDecomposition(opts.InputPath);

            //2. Wykonaj algorytm

            //3. Wypisz wyniki
            SaveDecomposition(opts.OutputPath, decomp.root, decomp.treewidth, decomp.verticesCount);
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