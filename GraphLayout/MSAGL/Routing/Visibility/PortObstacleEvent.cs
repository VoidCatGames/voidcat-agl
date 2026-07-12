using VoidCat.Agl.Core.Geometry;
using VoidCat.Agl.Routing.Spline.ConeSpanner;

namespace VoidCat.Agl.Routing.Visibility {
    internal class PortObstacleEvent : SweepEvent {
        readonly Point site;

        public PortObstacleEvent(Point site) {
            this.site = site;
        }

        internal override Point Site {
            get { return site; }
        }
    }
}