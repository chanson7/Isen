using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HexMapTools
{

    public class PriorityQueue<T>
    {

        private System.Comparison<T> comparison;
        private List<T> heap;


        public int Count
        {
            get
            {
                return heap.Count - 1;
            }
        }

        public int Capacity
        {
            get
            {
                return heap.Capacity - 1;
            }
            set
            {
                heap.Capacity = value + 1;
            }
        }


        public PriorityQueue(System.Comparison<T> comparison, int capacity = 32)
        {

            this.comparison = comparison;
            heap = new List<T>(capacity + 1);
            heap.Add(default(T));

        }

        public void Enqueue(T node)
        {
            heap.Add(node);

            UpHeap(Count);
        }

        public T Dequeue()
        {
            if (Count <= 0)
            {
                throw new System.InvalidOperationException();
            }

            T value = heap[1];
            heap[1] = heap[Count];

            heap.RemoveAt(Count);

            DownHeap(1);

            return value;
        }

        public T Peek()
        {
            if (Count <= 0)
            {
                throw new System.InvalidOperationException();
            }

            return heap[1];
        }

        public void Clear()
        {
            heap.Clear();
            heap.Add(default(T));
        }

        public void DownHeap(int i)
        {
            int l = i * 2;
            int r = i * 2 + 1;
            while (l <= Count)
            {
                int min = l;

                if (r <= Count && comparison(heap[r], heap[l]) < 0)
                {
                    min = r;
                }

                if (comparison(heap[min], heap[i]) >= 0)
                    return;

                T temp = heap[i];
                heap[i] = heap[min];
                heap[min] = temp;

                i = min;
                l = i * 2;
                r = i * 2 + 1;
            }
        }

        public void UpHeap(int i)
        {
            int parent = i / 2;

            while (parent > 0 && comparison(heap[i], heap[parent]) < 0)
            {
                T temp = heap[i];
                heap[i] = heap[parent];
                heap[parent] = temp;

                i = parent;
                parent = i / 2;
            }
        }




    }

}