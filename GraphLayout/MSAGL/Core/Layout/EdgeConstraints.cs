using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VoidCat.Agl.Core.Geometry;

namespace VoidCat.Agl.Core.Layout
{
    /// <summary>
    /// Settings controlling how ideal edge lengths will be calculated for layouts that consider it.
    /// </summary>
    /// <remarks>
    /// This is a struct so we can do a shallow copy without having to do a MemberwiseClone.
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")]
    public struct EdgeConstraints
    {
        
        /// <summary>
        /// If not None, then direction separation constraints will be applied to all edges on InitializeLayout
        /// </summary>
        public Direction Direction { get; set; }

        /// <summary>
        /// Controls the separation used in Edge Constraints
        /// </summary>
        public double Separation { get; set; }
        
        
    }
}
