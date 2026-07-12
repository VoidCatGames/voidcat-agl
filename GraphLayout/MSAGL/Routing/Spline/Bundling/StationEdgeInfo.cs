using System.Collections.Generic;
using VoidCat.Agl.Core.Geometry;
using VoidCat.Agl.Core.Geometry.Curves;

namespace VoidCat.Agl.Routing.Spline.Bundling {
    internal class StationEdgeInfo {

        internal int Count {get {return this.Metrolines.Count;} }
        internal double Width;

        internal List<Metroline> Metrolines = new List<Metroline>();

        #region cache

        internal double cachedBundleCost;

        //internal Polyline cachedBoundary;

        #endregion
    }
}