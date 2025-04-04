using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ingame.Player
{
    public class PlayerStat : MonoBehaviour
    {
        [SerializeField] private float maxMana = 100;
        [SerializeField] private float manaRegenerationAmountPerSec = 10;
        private float _mana;

        //TODO 세이브 파일에서 로드
        public int CommandCount { get; private set; } = 1;

        private void Start()
        {
            _mana = maxMana;
        }

        private void Update()
        {
            _mana = Mathf.Clamp(_mana + manaRegenerationAmountPerSec * Time.deltaTime, 0, maxMana);
            HUD.Instance.SetMana(_mana, maxMana);
        }

        public bool UseMana(float mana)
        {
            if (mana > _mana)
            {
                Debug.Log("마나 부족");
                return false;
            }

            _mana -= mana;
            HUD.Instance.SetMana(_mana, maxMana);
            return true;
        }
    }
}