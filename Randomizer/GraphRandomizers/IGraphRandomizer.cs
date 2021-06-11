using CommonCode;

namespace Randomizer.GraphRandomizers
{
    /// <summary>
    /// Interfejs opisujący randomizery grafów
    /// Parametry dla każdego randomizera powinny być wystawione jako publiczne pola.
    /// </summary>
    public interface IGraphRandomizer
    {
        Graph Randomize();
    }
}