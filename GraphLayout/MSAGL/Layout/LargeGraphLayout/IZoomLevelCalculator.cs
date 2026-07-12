using System.Collections.Generic;
using VoidCat.Agl.Core.Layout;

namespace VoidCat.Agl.Layout.LargeGraphLayout {
    internal interface IZoomLevelCalculator {
        /// <summary>
        /// returns the top node
        /// </summary>
        /// <returns></returns>
        void Run();

        /// <summary>
        /// the graph under layout
        /// </summary>
        GeometryGraph Graph { get; set; }

        /// <summary>
        /// layout settings
        /// </summary>
        LgLayoutSettings Settings { get; set; }

        List<LgNodeInfo> SortedLgNodeInfos { get; }

        List<int> LevelNodeCounts { get; }

    }
}