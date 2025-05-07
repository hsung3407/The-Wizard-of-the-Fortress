using System;
using Ingame.Player.Ability;
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
        [SerializeField] private TextMeshProUGUI level;
        [SerializeField] private Button selectButton;

        public void Display(Ability ability, Action<Ability> callback)
        {
            if(ability == null) return;
            
            title.text = ability.Data.AbilityName;
            icon.sprite = ability.Data.Icon;
            description.text = ability.Data.AbilityDescription;
            level.text = $"Lv.{ability.Level} / {ability.MaxLevel}";
            
            selectButton.onClick.RemoveAllListeners();
            selectButton.onClick.AddListener(() => callback(ability));
        }
    }
}