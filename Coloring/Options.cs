using System;
using CommandLine;

namespace Coloring
{
    public class Options
    {
        [Value(0, MetaName="inputDecompositionPath", Required=true, HelpText="Path to file with tree decomposition PACE2017 format.")]
        public string InputDecompositionPath { get; set; }
        
        [Value(1, MetaName="inputGraphPath", Required=true, HelpText="Path to file with graph PACE2017 format.")]
        public string InputGraphPath { get; set; }

        [Option('c', "colorCount", Required=true, HelpText="Number of colors.")]
        public int ColorCount { get; set; }
    }
}