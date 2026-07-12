using System;
using System.Collections.Generic;
using VoidCat.Agl.Core.Geometry;
using VoidCat.Agl.Routing.ConstrainedDelaunayTriangulation;
using VoidCat.Agl.Routing.Visibility;

namespace VoidCat.Agl.Routing.Spline.Bundling {
    internal class SdVertex {
        internal VisibilityVertex VisibilityVertex;
        internal List<SdBoneEdge> InBoneEdges = new List<SdBoneEdge>();
        internal List<SdBoneEdge> OutBoneEdges = new List<SdBoneEdge>();

        internal SdVertex Prev {
            get {
                if (PrevEdge == null) return null;
                return PrevEdge.Source == this ? PrevEdge.Target : PrevEdge.Source;
            }
        }

        internal SdBoneEdge PrevEdge { get; set; }

        internal SdVertex(VisibilityVertex visibilityVertex) {
            VisibilityVertex = visibilityVertex;
        }

        internal CdtTriangle Triangle;

        internal bool IsSourceOfRouting { get; set; }

        internal bool IsTargetOfRouting { get; set; }

        internal Point Point { get { return VisibilityVertex.Point; } }

        double cost;

        internal double Cost {
            get {
                if (IsSourceOfRouting) return cost;
                return Prev == null ? double.PositiveInfinity : cost;
            }
            set { cost = value; }
        }

        public void SetPreviousToNull() {
            PrevEdge = null;
        }
    }
}