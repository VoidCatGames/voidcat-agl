using VoidCat.Agl.Core.Geometry;
using VoidCat.Agl.Routing.Visibility;

namespace VoidCat.Agl.Routing.Rectilinear {
    /// <summary>
    /// This vertex class is used in rectilinear shortest paths
    /// </summary>
    public class VisibilityVertexRectilinear:VisibilityVertex {
        public VisibilityVertexRectilinear(Point point) : base(point) {}

        public VertexEntry[] VertexEntries { get; set; }
        
        public void SetVertexEntry(VertexEntry entry) {
            if (this.VertexEntries == null) {
                this.VertexEntries = new VertexEntry[4];
            }
            this.VertexEntries[CompassVector.ToIndex(entry.Direction)] = entry;
        }

        public void RemoveVertexEntries() {
            this.VertexEntries = null;
        }

        // VoidCat fork: per-worker vertex entries, so several path searches can run on one visibility graph at
        // once (MsmtRectilinearPath runs a stage's vertex pairs in parallel). Slot s is read and written only by the
        // search running in slot s; the slot array itself is created once, lock-free.
        internal const int MaxSearchSlots = 16;
        private VertexEntry[][] slotEntries;

        internal VertexEntry[] SlotEntries(int slot) {
            var a = this.slotEntries;
            return a == null ? null : a[slot];
        }

        internal void SetSlotEntry(int slot, VertexEntry entry) {
            var a = this.slotEntries;
            if (a == null) {
                System.Threading.Interlocked.CompareExchange(ref this.slotEntries, new VertexEntry[MaxSearchSlots][], null);
                a = this.slotEntries;
            }
            var entries = a[slot] ?? (a[slot] = new VertexEntry[4]);
            entries[CompassVector.ToIndex(entry.Direction)] = entry;
        }

        internal void RemoveSlotEntries(int slot) {
            var a = this.slotEntries;
            if (a != null) a[slot] = null;
        }

    }
}   
