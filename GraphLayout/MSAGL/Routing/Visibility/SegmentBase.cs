using VoidCat.Agl.Core.Geometry;

namespace VoidCat.Agl.Routing.Visibility {
    internal abstract class SegmentBase {
        abstract internal Point Start { get; }
        abstract internal Point End { get; }
        internal Point Direction { get { return End - Start; } }
        public override string ToString() {
            return Start + " " + End;
        }

    }
}