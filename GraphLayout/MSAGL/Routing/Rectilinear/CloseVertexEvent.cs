//
// CloseVertexEvent.cs
// MSAGL Obstacle class for top-most (obstacle-closing) vertex events for Rectilinear Edge Routing.
//
// Copyright Microsoft Corporation.

using VoidCat.Agl.Core.Geometry.Curves;

namespace VoidCat.Agl.Routing.Rectilinear {
    internal class CloseVertexEvent: BasicVertexEvent {
        internal CloseVertexEvent(Obstacle obstacle, PolylinePoint p) : base(obstacle, p) { }
    }
}
