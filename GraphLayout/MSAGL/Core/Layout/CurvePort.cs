using VoidCat.Agl.Core.Geometry;
using VoidCat.Agl.Core.Geometry.Curves;

namespace VoidCat.Agl.Core.Layout {
    /// <summary>
    /// 
    /// </summary>
    public class CurvePort:Port  {
        double parameter;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="parameter"></param>
        public CurvePort(ICurve curve, double parameter) {
            this.curve = curve;
            this.parameter = parameter;
        }

       
        /// <summary>
        /// empty constructor
        /// </summary>
        public CurvePort() { }
        /// <summary>
        /// 
        /// </summary>
        public double Parameter {
            get { return parameter; }
            set { parameter = value; }
        }
        ICurve curve;
        /// <summary>
        /// 
        /// </summary>
        override public ICurve Curve {
            get { return curve; }
            set { curve = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override Point Location {
#if SHARPKIT
            get { return Curve[parameter].Clone(); }
#else
            get { return Curve[parameter]; }
#endif
        }
    }
}
