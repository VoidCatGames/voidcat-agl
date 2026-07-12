using VoidCat.Agl.Core.Geometry;
using VoidCat.Agl.Core.Geometry.Curves;
using VoidCat.Agl.Routing;

namespace VoidCat.Agl.Prototype.LayoutEditing {
    /// <summary>
    /// node restore data
    /// </summary>
    public class NodeRestoreData:RestoreData {
        
        internal NodeRestoreData(ICurve boundaryCurve) {
            this.boundaryCurve = boundaryCurve;
        }

        private ICurve boundaryCurve;

        /// <summary>
        /// node boundary curve
        /// </summary>
        public ICurve BoundaryCurve {
            get { return boundaryCurve; }
            set { boundaryCurve = value; }
        }
    }
}
