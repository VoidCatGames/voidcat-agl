# NOTICE — VoidCat AGL

**VoidCat AGL** is a fork of **Microsoft Automatic Graph Layout (MSAGL)**
by VoidCat Studios LLC.

- Upstream project: https://github.com/microsoft/automatic-graph-layout
- Upstream copyright: © Microsoft Corporation
- Upstream license: MIT (see `LICENSE` at the root of this repository —
  unchanged from upstream and applying to this fork in full)
- Fork point: upstream `master` @ commit `d3ae8e8` (2026-07-12)
- Fork maintained at: https://github.com/VoidCatGames/voidcat-agl

Only the core layout engine (`GraphLayout/MSAGL`, built as
`VoidCat.Agl.dll`) is built and distributed by VoidCat Studios. The
remaining projects in this repository (viewers, samples, tests) are
retained as-is from upstream and are not built or shipped.

## Modifications by VoidCat Studios LLC

All VoidCat changes are marked in-source with `// VoidCat fork:` comments
and live on the `voidcat` branch; `master` remains a clean mirror of
upstream.

1. **External package dependencies removed**
   (`GraphLayout/MSAGL/AutomaticGraphLayout.csproj`): dropped the
   `DotNet.ReproducibleBuilds` (build tooling) and `System.Text.Json`
   package references and NuGet packing configuration, making the core
   build fully self-contained.
2. **`System.Text.Json` usage removed**
   (`GraphLayout/MSAGL/DebugHelpers/DebugCurveCollection.cs`): the
   debug-curve file dump/read helpers are disabled no-ops. This was the
   sole use of the dependency; no layout functionality is affected.
3. **Assembly and namespace rename**: `Microsoft.Msagl` →
   `VoidCat.Agl` throughout the core project (scripted:
   `tools/voidcat-rename.ps1`; assembly name `VoidCat.Agl.dll`). The
   rename exists to (a) avoid duplicate-assembly collisions in Unity
   projects that also contain the original `Microsoft.Msagl.dll`, and
   (b) avoid distributing a modified library under Microsoft's name.
   It does not assert any change of copyright: the renamed code remains
   © Microsoft Corporation under the MIT license, with VoidCat
   modifications © VoidCat Studios LLC under the same license.
4. **Rectilinear path search: in-edges walked by index**
   (`Routing/Rectilinear/SsstRectilinearPath.cs`, `Routing/Visibility/VisibilityVertex.cs`): the A* search
   iterated each vertex's in-edges through `IEnumerable`, boxing an enumerator per vertex expansion (4 million on a
   107-event graph's Rectilinear re-route); it now walks the list by index, as it already walked out-edges. Same
   edges, same order.
5. **Rectilinear path search: priority queue without a dictionary**
   (`Routing/Rectilinear/VertexEntryQueue.cs`, new; `SsstRectilinearPath.cs`, `VertexEntry.cs`): the search's queue
   is the same binary heap as `GenericBinaryHeapPriorityQueue<T>` — same layout, comparisons and sift order, so
   entries leave in the same order, ties included — but each entry carries its own heap element instead of a
   `Dictionary` lookup (a hash, an insert and a remove per entry).
6. **Rectilinear path search: inner loop trimmed** (`SsstRectilinearPath.cs`, `VertexEntry.cs`): cost weights
   read from fields, the target point read once per search, the direction to a neighbour computed once and passed
   down instead of recomputed, neighbour slots cleared inline. Every arithmetic expression is unchanged.

"Microsoft" is a trademark of Microsoft Corporation. This fork is not
endorsed by, affiliated with, or supported by Microsoft.

_This NOTICE ships alongside `VoidCat.Agl.dll` (together with the MIT
license text as `LICENSE-MSAGL.txt`) in VoidCat Studios products, including
the CodeCompass Unity package._
