using VoidCat.Agl.Core.Layout;

namespace VoidCat.Agl.Prototype.Phylo {
    /// <summary>
    /// Phylogenetic edge: an edge with a specified length
    /// </summary>
    public class PhyloEdge:Edge {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="sourceP"></param>
        /// <param name="targetP"></param>
        public PhyloEdge(Node sourceP, Node targetP) : base(sourceP, targetP) { }
    }
}
