using System;

namespace VoidCat.Agl.Routing.Rectilinear {
    /// <summary>
    /// VoidCat fork: the rectilinear path search's priority queue. The same binary heap as
    /// <see cref="GenericBinaryHeapPriorityQueue{T}"/> — the same array layout, the same comparisons and the same
    /// sift order, so entries come out in exactly the same order, ties included — but each <see cref="VertexEntry"/>
    /// is its own heap element (<see cref="VertexEntry.HeapIndex"/>, <see cref="VertexEntry.HeapPriority"/>) instead of a
    /// Dictionary mapping entries to separately allocated elements. That cost a hash, an insert, a remove and an extra
    /// object per entry (2 million per Rectilinear re-route on a 107-event graph) for one lookup in
    /// <see cref="DecreasePriority"/>.
    /// </summary>
    internal sealed class VertexEntryQueue {
        const int InitialHeapCapacity = 16;

        VertexEntry[] A = new VertexEntry[InitialHeapCapacity + 1];
        int heapSize;

        internal int Count { get { return heapSize; } }

        // The entry is in THIS queue: an entry may still carry the index of an earlier search's queue
        // (multistage source entries are enqueued again), which the dictionary of the generic queue never held.
        bool Contains(VertexEntry h) {
            return h.HeapIndex >= 1 && h.HeapIndex <= heapSize && ReferenceEquals(A[h.HeapIndex], h);
        }

        void SwapWithParent(int i) {
            var parent = A[i >> 1];
            PutAtI(i >> 1, A[i]);
            PutAtI(i, parent);
        }

        void PutAtI(int i, VertexEntry h) {
            A[i] = h;
            h.HeapIndex = i;
        }

        internal void Enqueue(VertexEntry element, double priority) {
            if (heapSize == A.Length - 1) {
                var newA = new VertexEntry[A.Length * 2];
                Array.Copy(A, 1, newA, 1, heapSize);
                A = newA;
            }
            heapSize++;
            int i = heapSize;
            element.HeapIndex = i;
            element.HeapPriority = priority;
            A[i] = element;
            while (i > 1 && A[i >> 1].HeapPriority.CompareTo(priority) > 0) {
                SwapWithParent(i);
                i >>= 1;
            }
        }

        internal VertexEntry Dequeue() {
            if (heapSize == 0)
                throw new InvalidOperationException();
            var ret = A[1];
            MoveQueueOneStepForward(ret);
            return ret;
        }

        void MoveQueueOneStepForward(VertexEntry ret) {
            ret.HeapIndex = 0;   // the generic queue's cache.Remove(ret)
            PutAtI(1, A[heapSize]);
            int i = 1;
            while (true) {
                int smallest = i;
                int l = i << 1;
                if (l <= heapSize && A[l].HeapPriority.CompareTo(A[i].HeapPriority) < 0)
                    smallest = l;
                int r = l + 1;
                if (r <= heapSize && A[r].HeapPriority.CompareTo(A[smallest].HeapPriority) < 0)
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
            var h = element;
            // ignore the element if it is not in the queue
            if (!Contains(h)) return;
            h.HeapPriority = newPriority;
            int i = h.HeapIndex;
            while (i > 1) {
                if (A[i].HeapPriority.CompareTo(A[i >> 1].HeapPriority) < 0)
                    SwapWithParent(i);
                else
                    break;
                i >>= 1;
            }
        }
    }
}
