using VoidCat.Agl.Core.Geometry;
using VoidCat.Agl.Core.Geometry.Curves;

namespace VoidCat.Agl.Routing.Spline.Bundling {
    internal class OrientedHubSegment {
        private ICurve segment;
        internal bool Reversed;
        internal int Index;
        internal BundleBase BundleBase;

        internal OrientedHubSegment(ICurve seg, bool reversed, int index, BundleBase bundleBase) {
            Segment = seg;
            Reversed = reversed;
            Index = index;
            BundleBase = bundleBase;
        }

        internal Point this[double t] { get { return Reversed ? Segment[Segment.ParEnd - t] : Segment[t]; } }

        internal OrientedHubSegment Other { get; set; }
        internal ICurve Segment {
            get { return segment; }
            set {
                segment = value;
            }
        }
    }
}