using System;
using System.Collections.Generic;
using Ingame.Player.Ability;
using SO.Ability;
using UnityEngine;

namespace UI
{
    public class AbilitySelectView : MonoBehaviour
    {
        [SerializeField] private AbilitySelectSlotView prefab;
        [SerializeField] private Transform selectSlotParent;
        private readonly List<GameObject> slots = new List<GameObject>();

        private void Start()
        {
            gameObject.SetActive(false);
        }

        public void Display(List<Ability> abilities, Action<Ability> onSelected)
        {
            gameObject.SetActive(true);

            foreach (var ability in abilities)
            {
                var slot = Instantiate(prefab, selectSlotParent);
                slots.Add(slot.gameObject);
                slot.Display(ability, onSelected += _ => { Clear(); });
            }
        }

        private void Clear()
        {
            foreach (var slot in slots) { Destroy(slot); }

            slots.Clear();
            gameObject.SetActive(false);
        }
    }
}