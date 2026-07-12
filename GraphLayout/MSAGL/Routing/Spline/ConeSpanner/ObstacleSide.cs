using VoidCat.Agl.Core.Geometry;
using VoidCat.Agl.Core.Geometry.Curves;
using VoidCat.Agl.Routing.Visibility;

namespace VoidCat.Agl.Routing.Spline.ConeSpanner
{
         abstract class ObstacleSide : SegmentBase
    {
        internal PolylinePoint StartVertex { get; private set; }
        internal ObstacleSide(PolylinePoint startVertex)
        {
            StartVertex = startVertex;
        }

        internal abstract PolylinePoint EndVertex { get; }

        internal Polyline Polyline { get { return StartVertex.Polyline; } }

        internal override Point Start
        {
            get { return StartVertex.Point; }
        }

        internal override Point End
        {
            get { return EndVertex.Point; }
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            string typeString = this.GetType().ToString();
            int lastDotLoc = typeString.LastIndexOf('.');
            if (lastDotLoc >= 0) {
                typeString = typeString.Substring(lastDotLoc + 1);
            }
            return typeString + " [" + this.Start.ToString() + " -> " + this.End.ToString() + "]";
        }
    }
}
