using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Ingame;
using Ingame.Player;
using UnityEngine;

public class TsetClass
{
    public void TestLog()
    {
        Debug.Log("TestLog");
    }
}

public class TestTT : TsetClass
{
    public new void TestLog()
    {
        Debug.Log("TestLog Two");
    }
}

public class Test : MonoBehaviour
{
    private void Awake()
    {
        var c = new TestTT();
        c.TestLog();
    }
}
