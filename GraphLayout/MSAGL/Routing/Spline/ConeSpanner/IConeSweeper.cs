using VoidCat.Agl.Core.Geometry;

namespace VoidCat.Agl.Routing.Spline.ConeSpanner {
    internal interface IConeSweeper {
        Point ConeRightSideDirection { get; set; }
        Point ConeLeftSideDirection { get; set; }
        Point SweepDirection { get; set; }
        double Z { get; set; }
    }
}