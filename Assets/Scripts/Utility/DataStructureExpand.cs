using System;
using System.Collections.Generic;

namespace Utility
{
    public static class DataStructureExpand
    {
        public static IEnumerable<LinkedListNode<T>> GetNodes<T>(this LinkedList<T> list)
        {
            for (var node = list.First; node != null; node = node.Next)
                yield return node;
        }

        public static void ForEachNodes<T>(this LinkedList<T> list, Action<LinkedListNode<T>> action)
        {
            LinkedListNode<T> nextNode;
            for (var node = list.First; node != null; node = nextNode)
            {
                nextNode = node.Next;
                action(node);
            }
        }        
    }
}