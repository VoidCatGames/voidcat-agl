using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using VoidCat.Agl.Core.Geometry;
using VoidCat.Agl.Core.Geometry.Curves;
using VoidCat.Agl.Core.Layout;
using VoidCat.Agl.DebugHelpers;

namespace VoidCat.Agl.Routing.Spline.Bundling {
    /// <summary>
    /// 
    /// </summary>
    interface IMetroMapOrderingAlgorithm {
        IEnumerable<Metroline> GetOrder(Station u, Station v);

        /// <summary>
        /// Get the index of line on the edge (u->v) and node u
        /// </summary>
        int GetLineIndexInOrder(Station u, Station v, Metroline metroLine);
    }



}
