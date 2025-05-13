using System;
using Ingame.Player.Magic.Detector;
using Ingame.Player.Modifier;
using Ingame.Player.Predictor;
using UnityEngine;
using Utility.Sound;

namespace Ingame.Player
{
    public class MagicController : MonoBehaviour
    {
        [SerializeField] private AudioClip sfx;
        [SerializeField] private float volume = .2f;
        [SerializeField] private float lifeTime = 5f;
        
        private DetectorBase detector;
        private ModifierBase modifier;

        private void Awake()
        {
            detector = GetComponentInParent<DetectorBase>();
            modifier = GetComponentInParent<ModifierBase>();
        }

        private void Start()
        {
            if (!detector || !modifier) return;
            detector.OnDetect += modifier.Modify;
            detector.OnRelease += modifier.UnModify;
            SoundManager.Instance.PlaySFX(sfx, volume);
            
            Destroy(gameObject, lifeTime);
        }

        public void InitMagic(MagicDataSO data, MagicStats modifiedStats, PlayerFlatStats playerStats)
        {
            modifier.Init(data, modifiedStats, playerStats);
        }
    }
}