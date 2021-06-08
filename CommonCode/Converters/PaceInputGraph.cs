using System;
using System.IO;

namespace CommonCode.Converters
{
    /// <summary>
    /// KLasa odpowiada za wczytywanie danych podanych w formacie dla PACE17
    /// </summary>
    public class PaceInputGraph: PaceInput
    {
        public Graph Parse(Stream stream)
        {
            var reader = new StreamReader(stream);
            string[] header = NextLine(reader);
            
            if (header.Length != 4 || header[0] != "p" || header[1] != "tw")
                throw new ArgumentException($"Wrong header encountered");
            
            int verticesCount = int.Parse(header[2]);
            int edgesCount = int.Parse(header[3]);

            Graph result = new Graph(verticesCount);
            for (int i = 0; i < edgesCount; ++i)
            {
                string[] edge = NextLine(reader);
                
                int u = int.Parse(edge[0]) - 1;
                int v = int.Parse(edge[1]) - 1;
                result.AddEdge(u, v);
            }
            
            return result;
        }
    }
}