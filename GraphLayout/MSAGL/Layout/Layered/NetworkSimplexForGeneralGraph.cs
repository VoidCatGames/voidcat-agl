using System.Collections.Generic;
using System.Linq;
using VoidCat.Agl.Core;
using VoidCat.Agl.Core.GraphAlgorithms;
using VoidCat.Agl.Core.Layout;

namespace VoidCat.Agl.Layout.Layered {
    internal class NetworkSimplexForGeneralGraph : LayerCalculator {
        BasicGraphOnEdges<PolyIntEdge> graph;
        /// <summary>
        /// a place holder for the cancel flag
        /// </summary>
        internal CancelToken Cancel { get; set; }

        public int[] GetLayers() {
            NetworkSimplex ns = new NetworkSimplex(graph, this.Cancel);
            return ns.GetLayers();
        }

        
        internal NetworkSimplexForGeneralGraph(BasicGraph<Node, PolyIntEdge> graph, CancelToken cancelObject) {
            this.graph = graph;
            this.Cancel = cancelObject;
        }

    }
}
