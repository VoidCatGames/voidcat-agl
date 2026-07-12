using VoidCat.Agl.Core.Geometry.Curves;
using VoidCat.Agl.Layout.Layered;

namespace VoidCat.Agl.DebugHelpers {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    /// <param name="curves"></param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "db")]
    public delegate void ShowDatabase(Database db, params ICurve[] curves);
}
