using UnityEngine;
using UnityEngine.UI;
using Utility.SingleTon;

namespace Ingame
{
    public class HUD : SingleMono<HUD>
    {
        [SerializeField] private Image healthBar;
    
        [SerializeField] private Image manaBar;

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