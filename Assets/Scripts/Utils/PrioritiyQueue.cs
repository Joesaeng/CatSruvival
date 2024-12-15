using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T> where T : IComparable<T>
{
    private List<T> heap = new List<T>();

    // 큐에 요소 추가
    public void Enqueue(T item)
    {
        heap.Add(item);
        int childIndex = heap.Count - 1;
        int parentIndex = (childIndex - 1) / 2;

        // 최소 힙 유지: 부모 노드가 자식 노드보다 작거나 같게
        while (childIndex > 0 && heap[childIndex].CompareTo(heap[parentIndex]) < 0)
        {
            Swap(childIndex, parentIndex);
            childIndex = parentIndex;
            parentIndex = (childIndex - 1) / 2;
        }
    }

    // 최우선순위 요소 제거 및 반환
    public T Dequeue()
    {
        if (heap.Count == 0)
            throw new InvalidOperationException("Priority queue is empty.");

        T root = heap[0];
        int lastIndex = heap.Count - 1;
        heap[0] = heap[lastIndex];
        heap.RemoveAt(lastIndex);

        int parentIndex = 0;

        // 최소 힙 유지: 부모 노드가 자식 노드보다 작거나 같게
        while (true)
        {
            int leftChildIndex = 2 * parentIndex + 1;
            int rightChildIndex = 2 * parentIndex + 2;
            int smallestIndex = parentIndex;

            if (leftChildIndex < heap.Count && heap[leftChildIndex].CompareTo(heap[smallestIndex]) < 0)
            {
                smallestIndex = leftChildIndex;
            }

            if (rightChildIndex < heap.Count && heap[rightChildIndex].CompareTo(heap[smallestIndex]) < 0)
            {
                smallestIndex = rightChildIndex;
            }

            if (smallestIndex == parentIndex)
                break;

            Swap(parentIndex, smallestIndex);
            parentIndex = smallestIndex;
        }

        return root;
    }

    // 큐의 최우선순위 요소 반환 (제거하지 않음)
    public T Peek()
    {
        if (heap.Count == 0)
            throw new InvalidOperationException("Priority queue is empty.");
        return heap[0];
    }

    // 큐의 요소 개수
    public int Count => heap.Count;

    // 두 인덱스의 요소를 교체
    private void Swap(int indexA, int indexB)
    {
        T temp = heap[indexA];
        heap[indexA] = heap[indexB];
        heap[indexB] = temp;
    }
}
