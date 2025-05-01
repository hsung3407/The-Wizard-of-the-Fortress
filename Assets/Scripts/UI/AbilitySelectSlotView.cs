using System;
using SO.Ability;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class AbilitySelectSlotView : MonoBehaviour
    {
        [SerializeField] private Button button;
        
        public void Display(AbilityDataSO abilityData, Action<AbilityDataSO> callback)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => callback(abilityData));
            
            
        }
    }
}