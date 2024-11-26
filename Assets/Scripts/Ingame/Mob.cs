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

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        
        
    }
}
