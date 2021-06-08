using System.IO;

namespace CommonCode.Converters
{
    public class PaceInput
    {
        protected string[] NextLine(StreamReader stream)
        {
            string[] splitted;
            string line;
            do
            {
                line = stream.ReadLine();
                if (line == null)
                    return null;
                splitted = line.Split(' ');
            } while (splitted[0] == "c");

            return splitted;
        }
    }
}