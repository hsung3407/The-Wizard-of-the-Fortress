using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace UI
{
    public class FieldFloatingText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI tmp;
        [SerializeField] private float lifeTime = 1;
        
        private void Awake()
        {
            var color = tmp.color;
            color.a = 0;
            tmp.color = color;
        }

        public void Display(string text, float size = 0.5f)
        {
            tmp.text = text;
            tmp.fontSize *= size;
            StartCoroutine(DisplayCoroutine());
        }

        private IEnumerator DisplayCoroutine()
        {
            for (float timer = 0; timer < lifeTime; timer += Time.deltaTime)
            {
                var color = tmp.color;
                color.a = GetAlpha(timer / lifeTime);
                tmp.color = color;
                yield return null;
            }
        }
        
        private float GetAlpha(float alpha)
        {
            return 1 - Mathf.Pow(2*alpha, 4);
        }
    }
}
