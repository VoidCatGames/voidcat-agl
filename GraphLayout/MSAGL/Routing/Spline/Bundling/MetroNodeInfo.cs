using VoidCat.Agl.Core.Geometry.Curves;

namespace VoidCat.Agl.Routing.Spline.Bundling {
    internal class MetroNodeInfo {
        Metroline metroline;
        Station station;
        PolylinePoint polyPoint;

        internal MetroNodeInfo(Metroline metroline, Station station, PolylinePoint polyPoint) {
            this.metroline = metroline;
            this.station = station;
            this.polyPoint = polyPoint;
        }

        internal Metroline Metroline {
            get { return metroline; }
        }

        internal PolylinePoint PolyPoint {
            get { return polyPoint; }
        }
    }
}