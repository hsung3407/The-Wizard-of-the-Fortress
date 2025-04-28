using System;
using UnityEngine;

namespace Level.Scenes
{
    public class Test : MonoBehaviour
    {
        private void Start()
        {
            Func<bool> action = null;
            Debug.Log(!action?.Invoke() ?? false);
        }
    }
}