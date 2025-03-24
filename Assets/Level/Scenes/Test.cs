    using System.Collections.Generic;
    using UnityEngine;

public class Test : MonoBehaviour
{
    Dictionary<string, int> dict = new Dictionary<string, int>();
    void Start()
    {
        dict.Add("Test", 0);
        
        dict["Test"]++;
        
        Debug.Log($"Value : {dict["Test"]}");
    }
}
