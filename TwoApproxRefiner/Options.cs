using CommandLine;

namespace TwoApproxRefiner
{
    public class Options
    {
        [Value(0, MetaName="inputDecompositionPath", HelpText="Path to file with tree decomposition PACE2017 format.")]
        public string InputDecompositionPath { get; set; }
        
        [Value(1, MetaName="inputGraphPath", HelpText="Path to file with graph PACE2017 format.")]
        public string InputGraphPath { get; set; }

        [Option('o', "outputPath", Required=false, HelpText="Path to output file with refined decomposition.")]
        public string OutputPath { get; set; }
    }
}