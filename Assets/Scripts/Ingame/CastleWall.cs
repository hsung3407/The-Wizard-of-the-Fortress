using System;
using UnityEngine;

namespace Ingame
{
    public class CastleWall : MonoBehaviour, IHit
    {
        [SerializeField] private float maxHealth = 100f;
        private float _health;

        private void Awake()
        {
            _health = maxHealth;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Mob"))
            {
                TakeHit(other.GetComponent<Mob>().Damage);
            }
        }

        public void TakeHit(float damage)
        {
            _health -= damage;
            Debug.Log(_health);
            if(_health <= 0) Die();
        }

        private void Die()
        {
            Debug.Log("Die");
        }
    }
}
