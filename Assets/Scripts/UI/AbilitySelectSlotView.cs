using System;
using SO.Ability;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class AbilitySelectSlotView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private Button selectButton;

        public void Display(AbilityDataSO abilityData, Action<AbilityDataSO> callback)
        {
            if(!abilityData) return;
            
            title.text = abilityData.AbilityName;
            icon.sprite = abilityData.Icon;
            description.text = abilityData.AbilityDescription;
            
            selectButton.onClick.RemoveAllListeners();
            selectButton.onClick.AddListener(() => callback(abilityData));
        }
    }
}