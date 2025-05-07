using UnityEngine;
using Utility;
using Utility.SingleTon;

namespace UI
{
    public class FloatingTextManager : SingleMono<FloatingTextManager>
    {
        [SerializeField] private ObjectPool<FieldFloatingText> pool;
        [SerializeField] private new Camera camera;

        public void Display(Vector3 pos, string text)
        {
            var fText = pool.Get();
            fText.transform.position = pos;
            fText.transform.rotation = camera.transform.rotation;
            fText.Display(text, fT => pool.Return(fT));
        }
    }
}   