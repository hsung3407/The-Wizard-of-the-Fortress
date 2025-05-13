using System.Collections;
using TMPro;
using UnityEngine;
using Utility.SingleTon;

namespace UI
{
    public class NotificationManager : SingleMono<NotificationManager>
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI error;
    
        [SerializeField] private float lifeTime;

        private Coroutine _titleCoroutine;
        private Coroutine _errorCoroutine;

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            title.alpha = 0;
            error.alpha = 0;
        }

        public void NotifyTitle(string text)
        {
            if(_titleCoroutine != null) StopCoroutine(_titleCoroutine);
            title.text = text;
            _titleCoroutine = StartCoroutine(Display(title));
        }
    
        public void NotifyError(string text)
        {
            if(_errorCoroutine != null) StopCoroutine(_errorCoroutine);
            error.text = text;
            _errorCoroutine = StartCoroutine(Display(error));
        }

        private IEnumerator Display(TextMeshProUGUI text)
        {
            for (float timer = 0; timer < lifeTime; timer += Time.deltaTime)
            {
                text.alpha = GetAlpha(timer / lifeTime);
                yield return null;
            }
        }
    
        private float GetAlpha(float alpha)
        {
            return 1 - Mathf.Pow(2 * alpha, 4);
        }
    }
}
