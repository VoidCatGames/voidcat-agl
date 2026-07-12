using VoidCat.Agl.Core.Geometry;
using VoidCat.Agl.Routing.Spline.ConeSpanner;

namespace VoidCat.Agl.Routing.Rectilinear.Nudging {
    internal class AxisEdgeLowPointEvent : SweepEvent {
        Point site;
        
        internal AxisEdge AxisEdge { get; set; }

        public AxisEdgeLowPointEvent(AxisEdge  edge, Point point) {
            site = point;
            AxisEdge = edge;
        }

        internal override Point Site {
            get { return site; }
        }

       
    }
}