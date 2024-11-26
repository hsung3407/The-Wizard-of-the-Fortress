using UnityEngine;
using UnityEngine.UI;
using Utility.SingleTon;

namespace Ingame
{
    public class HUD : SingleMono<HUD>
    {
        [SerializeField] private Image healthBar;
    
        [SerializeField] private Image manaBar;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            healthBar.fillAmount = 1;
            manaBar.fillAmount = 1;
        }

        public void SetHealth(float health, float maxHealth)
        {
            healthBar.fillAmount = health / maxHealth;
        }
        
        public void SetMana(float mana, float maxMana)
        {
            
        }
    }
}