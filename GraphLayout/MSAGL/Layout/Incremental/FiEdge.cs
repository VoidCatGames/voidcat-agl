using VoidCat.Agl.Core.Geometry;
using VoidCat.Agl.Core.GraphAlgorithms;
using VoidCat.Agl.Core.Layout;

namespace VoidCat.Agl.Layout.Incremental {
    /// <summary>
    /// An edge is a pair of nodes and an ideal length required between them
    /// </summary>
    internal class FiEdge : IEdge {
        internal Edge mEdge;
        public FiNode source;
        public FiNode target;

        public FiEdge(Edge mEdge) {
            this.mEdge = mEdge;
            source = (FiNode) mEdge.Source.AlgorithmData;
            target = (FiNode) mEdge.Target.AlgorithmData;
        }
        #region IEdge Members

        public int Source {
            get { return source.index; }
            set { }
        }

        public int Target {
            get { return target.index; }
            set { }
        }

        #endregion

        internal Point vector() {
            return source.mNode.Center - target.mNode.Center;
        }
    }
}