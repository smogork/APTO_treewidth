using System;
using System.Collections.Generic;
using CommandLine;
using CommonCode;
using CommonCode.Converters;
using Randomizer.GraphRandomizers;

namespace Randomizer
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
            IGraphRandomizer randomizer;
            if (!opts.KnownTreewidth)
            {
                randomizer = new SimpleGraphRandomizer(opts.VerticesCount, opts.AdditionalParameter);
            }
            else
            {
                //RAK TODO: to nie zadziala
                randomizer = new DecompositionGenerator(opts.AdditionalParameter, opts.VerticesCount, 10);
            }
            
            PaceOutputGraph output = new PaceOutputGraph();
            output.Write(Console.OpenStandardOutput(), randomizer.Randomize());
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