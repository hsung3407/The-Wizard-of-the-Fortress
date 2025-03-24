using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ingame.Player
{
    public class PlayerStat : MonoBehaviour
    {
        [SerializeField] private float maxMana = 100;
        private float _mana;

        private void Start()
        {
            _mana = maxMana;
        }

        public bool CheckMana(float mana) => mana <= _mana;

        public void UseMana(float mana)
        {
            _mana -= mana;
            HUD.Instance.SetMana(_mana, maxMana);
        }
    }
}