using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Ingame;
using Ingame.Player;
using UnityEngine;



public class Test : MonoBehaviour
{
    private LinkedList<int> list = new();

    private void Awake()
    {
        for (var i = 0; i < 10; i++)
        {
            list.AddLast(i);
        }
        
        LinkedListNode<int> nextNode = null;
        for (var node = list.First; node != null; node = nextNode)
        {
            nextNode = node.Next;
            Debug.Log(node.Value);
            list.Remove(node);
        }
    }
}
