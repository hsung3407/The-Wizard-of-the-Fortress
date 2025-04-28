using UnityEngine;

namespace Level.Scenes
{
    public class Test : MonoBehaviour
    {
        public TestOb tb;
    
        private void Start()
        {
            Debug.Log(tb.test);
            tb.test = 100;
            Debug.Log(tb.test);
        }
    }

    [CreateAssetMenu(fileName = "New Test", menuName = "Test")]
    public class TestOb : ScriptableObject
    {
        public float test;
    }
}