using CommandLine;

namespace Randomizer
{
    public class Options
    {
        [Value(0, MetaName="verticesCount", Required=true, HelpText="Number of vertices in generated graph")]
        public int VerticesCount { get; set; }
    
        [Value(1, MetaName="additionalParameter", Required=true, HelpText="Additional parameter for chosen algoritm. Default states for number of edges.")]
        public int AdditionalParameter { get; set; }

        [Option('t', "outputPath", Required=false, HelpText="Generate graph of known treewidth. Then AdditionalParameter states for treewidth")]
        public int KnownTreewidth { get; set; }
    }
}