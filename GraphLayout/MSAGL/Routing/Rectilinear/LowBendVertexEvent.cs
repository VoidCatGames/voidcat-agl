//
// LowBendVertexEvent.cs
// MSAGL Obstacle class for vertex events on a LowObstacleSide for Rectilinear Edge Routing.
//
// Copyright Microsoft Corporation.

using VoidCat.Agl.Core.Geometry.Curves;

namespace VoidCat.Agl.Routing.Rectilinear {
    internal class LowBendVertexEvent: BasicVertexEvent {
        internal LowBendVertexEvent(Obstacle obstacle, PolylinePoint p) : base(obstacle, p) { }
    }
}
