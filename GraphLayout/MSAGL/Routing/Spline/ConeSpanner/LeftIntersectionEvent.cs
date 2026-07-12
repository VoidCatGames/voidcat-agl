using VoidCat.Agl.Core.Geometry;
using VoidCat.Agl.Core.Geometry.Curves;

namespace VoidCat.Agl.Routing.Spline.ConeSpanner {
    /// <summary>
    /// left here means an intersection of a left cone side with an obstacle edge
    /// </summary>
    internal class LeftIntersectionEvent : SweepEvent {
        internal ConeLeftSide coneLeftSide;
        Point intersectionPoint;
        PolylinePoint endVertex;

        internal PolylinePoint EndVertex {
            get { return endVertex; }
        }

        internal LeftIntersectionEvent(ConeLeftSide coneLeftSide,
            Point intersectionPoint,
            PolylinePoint endVertex) {
            this.coneLeftSide = coneLeftSide;
            this.intersectionPoint = intersectionPoint;
            this.endVertex = endVertex;
        }

        internal override Point Site {
            get { return intersectionPoint; }
        }

        public override string ToString() {
            return "LeftIntersectionEvent " + intersectionPoint;
        }
    }
}
