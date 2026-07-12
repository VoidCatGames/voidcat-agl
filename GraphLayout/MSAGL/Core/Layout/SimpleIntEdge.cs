using VoidCat.Agl.Core.GraphAlgorithms;

namespace VoidCat.Agl.Core.Layout {
    internal class SimpleIntEdge : IEdge
    {
        public int Source { get; set; }
        public int Target { get; set; }
    }
}