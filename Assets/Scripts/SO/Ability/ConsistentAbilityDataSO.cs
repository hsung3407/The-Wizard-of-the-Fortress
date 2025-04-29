using Ingame.Player;
using UnityEngine;

namespace SO.Ability
{
    [CreateAssetMenu(fileName = "ConsistentAbilityDataSO", menuName = "Scriptable Objects/AbilityDataSO/ConsistentAbilityDataSO")]
    public abstract class ConsistentAbilityDataSO : AbilityDataSO
    {
        [field: SerializeField] public PlayerStatsModifier Modifier {get; private set;}
    }
}