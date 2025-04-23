using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Utility
{
    public static class DataStructureExpand
    {
        public static IEnumerable<LinkedListNode<T>> GetNodes<T>(this LinkedList<T> list)
        {
            for (var node = list.First; node != null; node = node.Next) yield return node;
        }

        public static LinkedListNode<T> NodeFirst<T>(this LinkedList<T> list,
            [NotNull] Func<LinkedListNode<T>, bool> predicate)
        {
            for (var node = list.First; node != null; node = node.Next)
            {
                if (predicate(node)) { return node; }
            }

            return null;
        }

        public static IEnumerable<LinkedListNode<T>> NodeWhere<T>(this LinkedList<T> list,
            [NotNull] Func<LinkedListNode<T>, bool> predicate)
        {
            for (var node = list.First; node != null; node = node.Next)
            {
                if (predicate(node)) { yield return node; }
            }
        }

        public static bool Contains<T>(this LinkedList<T> list,
            [NotNull] Func<T, bool> predicate)
        {
            bool result = false;
            for (var node = list.First; node != null; node = node.Next)
            {
                if (predicate(node.Value))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        public static int Count<T>(this LinkedList<T> list,
            [NotNull] Func<T, bool> predicate)
        {
            int result = 0;
            for (var node = list.First; node != null; node = node.Next)
            {
                if (predicate(node.Value)) { result++; }
            }

            return result;
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