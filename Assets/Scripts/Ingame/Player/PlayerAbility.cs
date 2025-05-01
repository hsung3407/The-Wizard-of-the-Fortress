using System;
using System.Collections.Generic;
using System.Linq;
using SO.Ability;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Ingame.Player
{
    public class PlayerAbility : MonoBehaviour
    {
        private Player _player;
        private List<AbilityDataSO> _selectableAbilities = new();
        private readonly Dictionary<string, Ability.Ability> _abilities = new Dictionary<string, Ability.Ability>();

        [SerializeField] private AbilitySelectView abilitySelectView;

        private void Awake()
        {
            _player = GetComponent<Player>();
            Init(null);
        }

        public void Init(HashSet<string> selectableAbilities)
        {
            var all = Resources.LoadAll<AbilityDataSO>("AbilityData");

            if (selectableAbilities == null) { _selectableAbilities = all.ToList(); }
            else
            {
                _selectableAbilities = all.Where(ability => selectableAbilities.Contains(ability.AbilityName))
                    .ToList();
            }
        }

        public void Select(int count, Action<AbilityDataSO> onSelected)
        {
            List<AbilityDataSO> abilities = new List<AbilityDataSO>(count);
            for (int i = 0; i < count; i++) { abilities.Add(Rand()); }

            abilitySelectView.Display(abilities, onSelected);
        }

        public void UpdateAbilities()
        {
            _selectableAbilities = _selectableAbilities.Where(abilityData =>
                    !_abilities.TryGetValue(abilityData.AbilityName, out Ability.Ability ability)
                    || ability.IsSelectable())
                .ToList();
        }

        public AbilityDataSO Rand()
        {
            return _selectableAbilities[Random.Range(0, _selectableAbilities.Count)];
        }

        public void AddAbility(AbilityDataSO abilityData)
        {
            if (_abilities.TryGetValue(abilityData.AbilityName, out Ability.Ability ability)) { ability.Upgrade(); }
            else
                _abilities.Add(abilityData.AbilityName, abilityData.CreateAbility(_player));
        }
    }
}