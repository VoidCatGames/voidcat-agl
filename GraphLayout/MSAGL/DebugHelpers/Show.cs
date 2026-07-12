using VoidCat.Agl.Core.Geometry.Curves;
using VoidCat.Agl.Core.Layout;

namespace VoidCat.Agl.DebugHelpers {
    /// <summary>
    /// shows curves
    /// </summary>
    /// <param name="curves"></param>
    public delegate void Show(params ICurve[] curves);

    ///<summary>
    ///</summary>
    ///<param name="graph"></param>
    public delegate void ShowGraph(GeometryGraph graph);
}
