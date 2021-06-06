using System;
using CommandLine;

namespace Coloring
{
    public class Options
    {
        [Value(0, MetaName="inputPath", HelpText="Path to file with tree decomposition PACE2017 format.")]
        public string InputPath { get; set; }

        [Option('o', "outputPath", Required=false, HelpText="Path to output file with with coloring of original graph.")]
        public string OutputPath { get; set; }
    }
}