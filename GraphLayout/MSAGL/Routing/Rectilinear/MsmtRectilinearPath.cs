using System;
using System.Collections.Generic;
using System.Linq;

using VoidCat.Agl.Core.Geometry;
using VoidCat.Agl.Routing.Visibility;

namespace VoidCat.Agl.Routing.Rectilinear {
    public class MsmtRectilinearPath {
        private readonly double bendPenaltyAsAPercentageOfDistance = SsstRectilinearPath.DefaultBendPenaltyAsAPercentageOfDistance;

    
        // Temporary for accumulating target entries.
        private readonly VertexEntry[] currentPassTargetEntries = new VertexEntry[4];

        public MsmtRectilinearPath(double bendPenalty) {
            this.bendPenaltyAsAPercentageOfDistance = bendPenalty;
        }

        /// <summary>
        /// Get the lowest-cost path from one of one or more sources to one of one or more targets, without waypoints.
        /// </summary>
        /// <param name="sources">One or more source vertices</param>
        /// <param name="targets">One or more target vertices</param>
        /// <returns>A single enumeration of path points.</returns>
        internal IEnumerable<Point> GetPath(IEnumerable<VisibilityVertex> sources, IEnumerable<VisibilityVertex> targets) {
            var entry = GetPathStage(null, sources, null, targets);
            return SsstRectilinearPath.RestorePath(entry);
        }

/// <summary>
        /// Route a single stage of a possibly multi-stage (due to waypoints) path.
        /// </summary>
        /// <param name="sourceVertexEntries">The VertexEntry array that was in the source vertex if it was the target of a prior stage.</param>
        /// <param name="sources">The enumeration of source vertices; must be only one if sourceVertexEntries is non-null.</param>
        /// <param name="targets">The enumeration of target vertex entries; must be only one if targetVertexEntries is non-null.</param>
        /// <param name="targetVertexEntries">The VertexEntry array that is in the target at the end of the stage.</param>
        private VertexEntry GetPathStage(VertexEntry[] sourceVertexEntries, IEnumerable<VisibilityVertex> sources,
                                                VertexEntry[] targetVertexEntries, IEnumerable<VisibilityVertex> targets) {
            var ssstCalculator = new SsstRectilinearPath();
            VertexEntry bestEntry = null;

            // This contains the best (lowest) path cost after normalizing origins to the center of the sources
            // and targets.  This is used to avoid selecting a vertex pair whose path has more bends than another pair of
            // vertices, but the bend penalty didn't total enough to offset the additional length between the "better" pair.
            // This also plays the role of an upper bound on the path length; if a path cost is greater than adjustedMinCost 
            // then we stop exploring it, which saves considerable time after low-cost paths have been found.
            double bestCost = double.MaxValue / ScanSegment.OverlappedWeight;
            double bestPathCostRatio = double.PositiveInfinity;

            // Calculate the bend penalty multiplier.  This is a percentage of the distance between the source and target,
            // so that we have the same relative importance if we have objects of about size 20 that are about 100 apart
            // as for objects of about size 200 that are about 1000 apart.
            Point sourceCenter = Barycenter(sources);
            Point targetCenter = Barycenter(targets);
            var distance = SsstRectilinearPath.ManhattanDistance(sourceCenter, targetCenter);
            ssstCalculator.BendsImportance = Math.Max(0.001, distance * (this.bendPenaltyAsAPercentageOfDistance * 0.01));

            // We'll normalize by adding (a proportion of) the distance (only; not bends) from the current endpoints to
            // their centers. This is similar to routeToCenter, but routing multiple paths like this means we'll always
            // get at least a tie for the best vertex pair, whereas routeToCenter can introduce extraneous bends
            // if the sources/targets are not collinear with the center (such as an E-R diagram).
            // interiorLengthAdjustment is a way to decrease the cost adjustment slightly to allow a bend if it saves moving
            // a certain proportion of the distance parallel to the object before turning to it.
            var interiorLengthAdjustment = ssstCalculator.LengthImportance;

            // VertexEntries for the current pass of the current stage, if multistage.
            var tempTargetEntries = (targetVertexEntries != null) ? this.currentPassTargetEntries : null;

            // VoidCat fork: a single-stage path (no waypoints — every path the CodeCompass tools ask for) runs its
            // vertex pairs in parallel. See GetSingleStagePathInParallel for why the result is the sequential one.
            if (PairSearchThreads >= 2 && sourceVertexEntries == null && targetVertexEntries == null) {
                return GetSingleStagePathInParallel(sources, targets, sourceCenter, targetCenter, interiorLengthAdjustment,
                                                    ssstCalculator.BendsImportance);
            }

            // Process closest pairs first, so we can skip longer ones (jump out of SsstRectilinear sooner, often immediately).
            // This means that we'll be consistent on tiebreaking for equal scores with differing bend counts (the shorter
            // path will win).  In overlapped graphs the shortest path may have more higher-weight edges. 
            foreach (var pair in
                    from VisibilityVertexRectilinear source in sources
                    from VisibilityVertexRectilinear target in targets
                    orderby SsstRectilinearPath.ManhattanDistance(source.Point, target.Point)
                    select new { sourceV = source, targetV = target }) {
                var source = pair.sourceV;
                var target = pair.targetV;
                if (PointComparer.Equal(source.Point, target.Point)) {
                    continue;
                }
                var sourceCostAdjustment = SsstRectilinearPath.ManhattanDistance(source.Point, sourceCenter) * interiorLengthAdjustment;
                var targetCostAdjustment = SsstRectilinearPath.ManhattanDistance(target.Point, targetCenter) * interiorLengthAdjustment;

                var adjustedBestCost = bestCost;
                if (targetVertexEntries != null) {
                    Array.Clear(tempTargetEntries, 0, tempTargetEntries.Length);
                    adjustedBestCost = ssstCalculator.MultistageAdjustedCostBound(bestCost);
                }
                VertexEntry lastEntry = ssstCalculator.GetPathWithCost(sourceVertexEntries, source, sourceCostAdjustment,
                                                                        tempTargetEntries, target, targetCostAdjustment, 
                                                                        adjustedBestCost);
                if (tempTargetEntries != null) {
                    UpdateTargetEntriesForEachDirection(targetVertexEntries, tempTargetEntries, ref bestCost, ref bestEntry);
                    continue;
                }

                // This is the final (or only) stage. Break ties by picking the lowest ratio of cost to ManhattanDistance between the endpoints.
                if (lastEntry == null) {
                    continue;
                }
                var costRatio = lastEntry.Cost / SsstRectilinearPath.ManhattanDistance(source.Point, target.Point);
                if ((lastEntry.Cost < bestCost) || ApproximateComparer.Close(lastEntry.Cost, bestCost) && (costRatio < bestPathCostRatio)) {
                    bestCost = lastEntry.Cost;
                    bestEntry = lastEntry;
                    bestPathCostRatio = lastEntry.Cost / SsstRectilinearPath.ManhattanDistance(source.Point, target.Point);
                }
            }
            return bestEntry;
        }

        // VoidCat fork: worker threads for GetSingleStagePathInParallel (VoidCat.Agl.Core.AglThreading); below 2, the
        // upstream loop. At most VisibilityVertexRectilinear.MaxSearchSlots.
        private static int PairSearchThreads {
            get { return Math.Min(VisibilityVertexRectilinear.MaxSearchSlots, VoidCat.Agl.Core.AglThreading.RectilinearPathSearchThreads); }
        }

        // Below this many pairs left, the hand-off to worker threads costs more than it saves.
        private const int MinPairsForParallel = 3;

        /// <summary>
        /// VoidCat fork: the single-stage loop of GetPathStage with its vertex pairs searched in parallel, and the
        /// same result. Upstream, pair k is searched with the best cost found by pairs 0..k-1 as its bound, and a pair
        /// is kept when it beats the best so far. Here: the pairs are searched, in parallel, all with the bound the
        /// fold currently has; then the results are folded IN PAIR ORDER, exactly as upstream. While no pair has
        /// improved the best, every pair's bound WAS that bound — its search is the sequential search, result for
        /// result. When a pair does improve it, the pairs after it are searched again (another round) with the new
        /// bound, so no pair's result is ever taken from a search with a bound the sequential loop would not have
        /// used. Each search keeps its vertex entries in its own slot on the vertices (SsstRectilinearPath.Slot), so
        /// concurrent searches on the shared visibility graph never see each other; the graph is not modified.
        /// Measured on a 244-event graph: 16 pairs per edge, the closest pair the best in ~80 % of edges.
        /// </summary>
        private VertexEntry GetSingleStagePathInParallel(IEnumerable<VisibilityVertex> sources, IEnumerable<VisibilityVertex> targets,
                Point sourceCenter, Point targetCenter, double interiorLengthAdjustment, double bendsImportance) {
            var pairs = (from VisibilityVertexRectilinear source in sources
                         from VisibilityVertexRectilinear target in targets
                         orderby SsstRectilinearPath.ManhattanDistance(source.Point, target.Point)
                         select new { sourceV = source, targetV = target })
                        .Where(p => !PointComparer.Equal(p.sourceV.Point, p.targetV.Point))
                        .Select(p => new SearchPair(p.sourceV, p.targetV,
                            SsstRectilinearPath.ManhattanDistance(p.sourceV.Point, sourceCenter) * interiorLengthAdjustment,
                            SsstRectilinearPath.ManhattanDistance(p.targetV.Point, targetCenter) * interiorLengthAdjustment))
                        .ToList();

            VertexEntry bestEntry = null;
            double bestCost = double.MaxValue / ScanSegment.OverlappedWeight;
            double bestPathCostRatio = double.PositiveInfinity;
            var results = new VertexEntry[pairs.Count];

            // The fold step of the upstream loop, for pair k's result.
            void Fold(int k, VertexEntry lastEntry) {
                if (lastEntry == null) {
                    return;
                }
                var p = pairs[k];
                var costRatio = lastEntry.Cost / SsstRectilinearPath.ManhattanDistance(p.Source.Point, p.Target.Point);
                if ((lastEntry.Cost < bestCost) || ApproximateComparer.Close(lastEntry.Cost, bestCost) && (costRatio < bestPathCostRatio)) {
                    bestCost = lastEntry.Cost;
                    bestEntry = lastEntry;
                    bestPathCostRatio = lastEntry.Cost / SsstRectilinearPath.ManhattanDistance(p.Source.Point, p.Target.Point);
                }
            }

            // One pair at a time until a path is found: until then there is no bound, and a pair searched without one
            // explores the whole graph — searching the others alongside would only search them unbounded and again.
            // In most edges the closest pair finds the path that stays best.
            var next = 0;
            var first = NewCalculator(bendsImportance, -1);
            for (; next < pairs.Count && bestEntry == null; next++) {
                var p = pairs[next];
                Fold(next, first.GetPathWithCost(null, p.Source, p.SourceCostAdjustment, null, p.Target, p.TargetCostAdjustment, bestCost));
            }
            while (next < pairs.Count) {
                var bound = bestCost;
                var count = pairs.Count - next;
                if (count < MinPairsForParallel) {
                    // Few left: the upstream loop, one pair at a time.
                    var calc = NewCalculator(bendsImportance, -1);
                    for (; next < pairs.Count; next++) {
                        var p = pairs[next];
                        Fold(next, calc.GetPathWithCost(null, p.Source, p.SourceCostAdjustment, null, p.Target, p.TargetCostAdjustment, bestCost));
                    }
                    break;
                }
                // A round is one pair per thread: a pair that improves the best invalidates at most the rest of its round.
                var end = Math.Min(pairs.Count, next + PairSearchThreads);
                SearchRound(pairs, results, next, end, bound, bendsImportance);
                // Fold in pair order while every searched pair's bound is the bound the sequential loop would have
                // given it; at the first pair after an improvement, the round's remaining results are stale.
                var k = next;
                for (; k < end; k++) {
                    if (bestCost != bound) {
                        break;   // pair k's sequential bound is the improved cost: search it (and the rest) again
                    }
                    Fold(k, results[k]);
                }
                next = k;
            }
            return bestEntry;
        }

        private sealed class SearchPair {
            internal readonly VisibilityVertexRectilinear Source, Target;
            internal readonly double SourceCostAdjustment, TargetCostAdjustment;
            internal SearchPair(VisibilityVertexRectilinear source, VisibilityVertexRectilinear target, double sourceCostAdjustment, double targetCostAdjustment) {
                Source = source; Target = target; SourceCostAdjustment = sourceCostAdjustment; TargetCostAdjustment = targetCostAdjustment;
            }
        }

        private static SsstRectilinearPath NewCalculator(double bendsImportance, int slot) {
            return new SsstRectilinearPath { BendsImportance = bendsImportance, Slot = slot };
        }

        // Searches pairs[from..end) with the same bound on worker threads, each search in its own entry slot.
        private static void SearchRound(List<SearchPair> pairs, VertexEntry[] results, int from, int end, double bound, double bendsImportance) {
            var threads = Math.Min(PairSearchThreads, end - from);
            var nextPair = from - 1;
            var tasks = new System.Threading.Tasks.Task[threads];
            for (var t = 0; t < threads; t++) {
                var slot = t;
                tasks[t] = System.Threading.Tasks.Task.Factory.StartNew(() => {
                    var calc = NewCalculator(bendsImportance, slot);
                    while (true) {
                        var k = System.Threading.Interlocked.Increment(ref nextPair);
                        if (k >= end) {
                            return;
                        }
                        var p = pairs[k];
                        results[k] = calc.GetPathWithCost(null, p.Source, p.SourceCostAdjustment, null, p.Target, p.TargetCostAdjustment, bound);
                    }
                }, System.Threading.CancellationToken.None, System.Threading.Tasks.TaskCreationOptions.DenyChildAttach,
                   System.Threading.Tasks.TaskScheduler.Default);
            }
            try {
                System.Threading.Tasks.Task.WaitAll(tasks);
            } catch (AggregateException e) when (e.InnerExceptions.Count > 0) {
                System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(e.InnerExceptions[0]).Throw();
            }
        }

        private static void UpdateTargetEntriesForEachDirection(VertexEntry[] targetVertexEntries, VertexEntry[] tempTargetEntries,
                            ref double bestCost, ref VertexEntry bestEntry) {
            for (int ii = 0; ii < tempTargetEntries.Length; ++ii) {
                var tempEntry = tempTargetEntries[ii];
                if (tempEntry == null) {
                    continue;
                }
                if ((targetVertexEntries[ii] == null) || (tempEntry.Cost < targetVertexEntries[ii].Cost)) {
                    targetVertexEntries[ii] = tempEntry;
                    if (tempEntry.Cost < bestCost) {
                        // This does not have the ratio tiebreaker because the individual stage path is only used as a success indicator.
                        bestCost = tempEntry.Cost;
                        bestEntry = tempEntry;
                    }
                }
            }
            return;
        }

        private static Point Barycenter(IEnumerable<VisibilityVertex> vertices) {
            var center = new Point();
            foreach (var vertex in vertices) {
                center += vertex.Point;
            }
            return center / vertices.Count();
        }
    }
}