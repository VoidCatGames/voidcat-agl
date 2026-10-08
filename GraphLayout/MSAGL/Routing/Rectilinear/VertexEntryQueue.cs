using System;
using VoidCat.Agl.Core.DataStructures;

namespace VoidCat.Agl.Routing.Rectilinear {
    /// <summary>
    /// VoidCat fork: the rectilinear path search's priority queue. The same binary heap as
    /// <see cref="GenericBinaryHeapPriorityQueue{T}"/> — the same array layout, the same comparisons and the same
    /// sift order, so entries come out in exactly the same order, ties included — but each <see cref="VertexEntry"/>
    /// carries its own heap element instead of a Dictionary mapping entries to elements. That dictionary cost a hash,
    /// an insert and a remove per entry (2 million per Rectilinear re-route on a 107-event graph) for one lookup in
    /// <see cref="DecreasePriority"/>.
    /// </summary>
    internal sealed class VertexEntryQueue {
        const int InitialHeapCapacity = 16;

        GenericHeapElement<VertexEntry>[] A = new GenericHeapElement<VertexEntry>[InitialHeapCapacity + 1];
        int heapSize;

        internal int Count { get { return heapSize; } }

        // The element is in THIS queue: an entry may still hold the element of an earlier search's queue
        // (multistage source entries are enqueued again), which the dictionary of the generic queue never held.
        bool Contains(GenericHeapElement<VertexEntry> h) {
            return h != null && h.indexToA >= 1 && h.indexToA <= heapSize && ReferenceEquals(A[h.indexToA], h);
        }

        void SwapWithParent(int i) {
            var parent = A[i >> 1];
            PutAtI(i >> 1, A[i]);
            PutAtI(i, parent);
        }

        void PutAtI(int i, GenericHeapElement<VertexEntry> h) {
            A[i] = h;
            h.indexToA = i;
        }

        internal void Enqueue(VertexEntry element, double priority) {
            if (heapSize == A.Length - 1) {
                var newA = new GenericHeapElement<VertexEntry>[A.Length * 2];
                Array.Copy(A, 1, newA, 1, heapSize);
                A = newA;
            }
            heapSize++;
            int i = heapSize;
            A[i] = element.QueueElement = new GenericHeapElement<VertexEntry>(i, priority, element);
            while (i > 1 && A[i >> 1].priority.CompareTo(priority) > 0) {
                SwapWithParent(i);
                i >>= 1;
            }
        }

        internal VertexEntry Dequeue() {
            if (heapSize == 0)
                throw new InvalidOperationException();
            var ret = A[1].v;
            MoveQueueOneStepForward(ret);
            return ret;
        }

        void MoveQueueOneStepForward(VertexEntry ret) {
            ret.QueueElement = null;   // the generic queue's cache.Remove(ret)
            PutAtI(1, A[heapSize]);
            int i = 1;
            while (true) {
                int smallest = i;
                int l = i << 1;
                if (l <= heapSize && A[l].priority.CompareTo(A[i].priority) < 0)
                    smallest = l;
                int r = l + 1;
                if (r <= heapSize && A[r].priority.CompareTo(A[smallest].priority) < 0)
                    smallest = r;
                if (smallest != i)
                    SwapWithParent(smallest);
                else
                    break;
                i = smallest;
            }
            heapSize--;
        }

        internal void DecreasePriority(VertexEntry element, double newPriority) {
            var h = element.QueueElement;
            // ignore the element if it is not in the queue
            if (!Contains(h)) return;
            h.priority = newPriority;
            int i = h.indexToA;
            while (i > 1) {
                if (A[i].priority.CompareTo(A[i >> 1].priority) < 0)
                    SwapWithParent(i);
                else
                    break;
                i >>= 1;
            }
        }
    }
}
