using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ingame
{
    public class EnemyHitEffect : MonoBehaviour
    {
        private Renderer _renderer;

        private static readonly int BaseColorID = Shader.PropertyToID("_BaseColor");
        private Color _originColor;
        [SerializeField] private Color flashColor;

        [SerializeField] private float playTime;

        //이전 이펙트가 끝나지 않았을 때 캔슬이 가능한 시간
        [SerializeField] private float effectCancelActiveTime;
        private float _timer;

        private Coroutine _coroutine;

        private void Awake()
        {
            _renderer = GetComponentInChildren<Renderer>();
            _originColor = _renderer.material.GetColor(BaseColorID);
            Init();
        }

        private void OnDestroy()
        {
            Init();
        }

        private void Init()
        {
            _timer = 0;
            _renderer.material.SetColor(BaseColorID, _originColor);
        }

        public void Play()
        {
            if (_timer > 0 && _timer < effectCancelActiveTime) { return; }
            
            if (_coroutine != null) { StopCoroutine(_coroutine); }

            _coroutine = StartCoroutine(PlayFlow());
        }

        private IEnumerator PlayFlow()
        {
            Init();
            
            for (; _timer < playTime; _timer += Time.deltaTime)
            {
                var alpha = _timer / playTime;
                var color = Color.Lerp(_originColor, flashColor, GetPlayValue(alpha));
                _renderer.material.SetColor(BaseColorID, color);

                yield return null;
            }

            Init();
        }

        //TODO: 특정 함수나 animation curve(=베지어)를 활용한 값 변환
        private float GetPlayValue(float alpha)
        {
            return Mathf.Clamp01(Mathf.Pow(alpha - 1.2f, 6));
        }
    }
}