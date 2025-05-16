using System;
using UnityEngine;
using UnityEngine.Events;
using Utility;
using Utility.Sound;

namespace Ingame
{
    public class CastleWall : MonoBehaviour
    {
        [SerializeField] private UnityEvent<float> onHit; 
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                var enemy = other.GetComponent<Enemy>();
                enemy.TakeDamage(float.MaxValue);
                onHit?.Invoke(enemy.Damage);
                SoundManager.Instance.PlaySFX(SFXType.WallCrash, 0.5f);
            }
        }
    }
}
