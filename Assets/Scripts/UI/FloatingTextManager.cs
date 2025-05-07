using UnityEngine;
using Utility.SingleTon;

namespace UI
{
    public class FloatingTextManager : SingleMono<FloatingTextManager>
    {
        [SerializeField] private new Camera camera;
        [SerializeField] private FieldFloatingText prefab;

        public void Display(Vector3 pos, string text)
        {
            var fText = Instantiate(prefab, pos, camera.transform.rotation);
            fText.Display(text);
        }
    }
}   