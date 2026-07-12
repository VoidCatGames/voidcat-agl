using VoidCat.Agl.Core.Geometry;
using VoidCat.Agl.Core.Geometry.Curves;

namespace VoidCat.Agl.Routing.Spline.ConeSpanner
{
    internal class RightObstacleSide : ObstacleSide
    {
        Point end;
        internal RightObstacleSide(PolylinePoint startVertex)
            : base(startVertex)
        {
            this.end = startVertex.PrevOnPolyline.Point;
        }
        internal override Point End
        {
            get { return end; }
        }

        internal override PolylinePoint EndVertex
        {
            get { return StartVertex.PrevOnPolyline; }
        }

    }
}