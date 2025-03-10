using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ingame
{
    public class Mob : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 100f;
        private float _health;
        
        [SerializeField] private float damage = 1f;
        public float Damage => damage;

        [SerializeField] private float speed = 1;

        private void OnEnable()
        {
            _health = maxHealth;
        }

        private void Update()
        {
            transform.position += transform.forward * (speed * Time.deltaTime);
        }
    }
}
