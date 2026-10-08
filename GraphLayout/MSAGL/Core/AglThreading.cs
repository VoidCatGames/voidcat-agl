using System;

namespace VoidCat.Agl.Core {
    /// <summary>
    /// VoidCat fork: worker threads the engine may use. Every parallel path gives the same result as the
    /// sequential one — the work is split so that no result depends on which thread did it or in what order
    /// (see each use). 1 (the default) runs the upstream sequential code.
    /// </summary>
    public static class AglThreading {
        private static int rectilinearPathSearchThreads = 1;

        /// <summary>The largest thread count the rectilinear path search accepts.</summary>
        public const int MaxRectilinearPathSearchThreads = 16;

        /// <summary>
        /// Threads for the rectilinear router's shortest-path search: a path's source/target vertex pairs are
        /// searched in parallel (MsmtRectilinearPath). 1 = sequential, as upstream. Clamped to 1..16.
        /// </summary>
        public static int RectilinearPathSearchThreads {
            get { return rectilinearPathSearchThreads; }
            set { rectilinearPathSearchThreads = Math.Max(1, Math.Min(MaxRectilinearPathSearchThreads, value)); }
        }
    }
}
