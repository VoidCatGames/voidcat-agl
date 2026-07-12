//
// HighBendVertexEvent.cs
// MSAGL Obstacle class for vertex events on a HighObstacleSide for Rectilinear Edge Routing.
//
// Copyright Microsoft Corporation.

using VoidCat.Agl.Core.Geometry.Curves;

namespace VoidCat.Agl.Routing.Rectilinear {
    internal class HighBendVertexEvent: BasicVertexEvent {
        internal HighBendVertexEvent(Obstacle obstacle, PolylinePoint p) : base(obstacle, p) { }
    }
}
