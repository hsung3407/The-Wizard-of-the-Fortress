using System;
using UnityEngine;
using Utility;

namespace Ingame
{
    public class CastleWall : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 100f;
        private float _health;

        private void Awake()
        {
            _health = maxHealth;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                var enemy = other.GetComponent<Enemy>();
                enemy.TakeDamage(float.MaxValue);
                TakeHit(enemy.Damage);
            }
        }

        public void TakeHit(float damage)
        {
            _health -= damage;
            HUD.Instance.SetHealth(_health, maxHealth);
            // Debug.Log(_health);
            if(_health <= 0) Die();
        }

        private void Die()
        {
            Debug.Log("Die");
            Time.timeScale = 0;
        }
    }
}
