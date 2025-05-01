using System;
using System.Collections.Generic;
using SO.Ability;
using UnityEngine;

namespace UI
{
    public class AbilitySelectView : MonoBehaviour
    {
        [SerializeField] private AbilitySelectSlotView prefab;
        private readonly List<GameObject> slots = new List<GameObject>();

        public void Display(List<AbilityDataSO> abilities, Action<AbilityDataSO> onSelected)
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