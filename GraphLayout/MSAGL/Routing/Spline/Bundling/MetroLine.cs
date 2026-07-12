using System;
using System.Collections.Generic;
using System.Linq;
using VoidCat.Agl.Core.DataStructures;
using VoidCat.Agl.Core.Geometry;
using VoidCat.Agl.Core.Geometry.Curves;

namespace VoidCat.Agl.Routing.Spline.Bundling {
    /// <summary>
    /// holds the data of a path
    /// </summary>

    internal class Metroline {
        internal double Width;
        internal double Length { get; set; }

        internal double IdealLength { get; set; }

        internal Polyline Polyline { get; set; }
        public int Index { get; set; }

        internal Metroline(Polyline polyline, double width, Func<Tuple<Polyline, Polyline>> sourceAndTargetLoosePolys, int index) {
            Width = width;
            Polyline = polyline;
            this.sourceAndTargetLoosePolylines = sourceAndTargetLoosePolys;
            this.Index = index;
        }

        internal void UpdateLengths() {
            var l = 0.0;
            for (var p = Polyline.StartPoint; p.Next != null; p = p.Next) {
                l += (p.Next.Point - p.Point).Length;
            }
            Length = l;
            IdealLength = (Polyline.End - Polyline.Start).Length;
        }

        internal Func<Tuple<Polyline, Polyline>> sourceAndTargetLoosePolylines;
    }
}