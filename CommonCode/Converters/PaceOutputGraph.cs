using System.IO;

namespace CommonCode.Converters
{
    public class PaceOutputGraph
    {
        public void Write(Stream stream, Graph graph)
        {
            var writer = new StreamWriter(stream);
            
            writer.WriteLine("p tw {0} {1}", graph.VerticesCount, graph.EdgesCount);
            
            foreach ((int u, int v) in graph.GetAllEdges())
                writer.WriteLine("{0} {1}", u + 1, v + 1);
            writer.Flush();
        }
    }
}