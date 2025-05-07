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
        private readonly List<GameObject> slots = new List<GameObject>();

        public void Display(List<Ability> abilities, Action<Ability> onSelected)
        {
            foreach (var ability in abilities)
            {
                var slot = Instantiate(prefab, transform);
                slots.Add(slot.gameObject);
                slot.Display(ability, onSelected += _ => { Clear(); });
            }
        }

        private void Clear()
        {
            foreach (var slot in slots) { Destroy(slot); }

            slots.Clear();
        }
    }
}