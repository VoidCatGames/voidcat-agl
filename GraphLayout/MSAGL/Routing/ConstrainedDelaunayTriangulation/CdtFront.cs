using VoidCat.Agl.Core.DataStructures;

namespace VoidCat.Agl.Routing.ConstrainedDelaunayTriangulation {
    internal class CdtFront  {
        RbTree<CdtSite> front = new RbTree<CdtSite>((a, b) => a.Point.X.CompareTo(b.Point.X));

        public CdtFront(CdtSite p_1, CdtSite p0, CdtSite p_2) {

        }
    }
}
