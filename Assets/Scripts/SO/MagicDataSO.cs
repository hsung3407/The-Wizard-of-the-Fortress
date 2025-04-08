using Ingame.Player.Predictor;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ingame.Player
{
    [CreateAssetMenu(fileName = "MagicDataSO", menuName = "Scriptable Objects/MagicDataSO")]
    public class MagicDataSO : ScriptableObject
    {
        [Header("Identify")]
        [field: SerializeField] public string MagicName { get; private set; }
        [field: SerializeField] public string Command { get; private set; }
        
        [Header("Description")]
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        
        [Header("Data")] 
        [field: SerializeField] public PredictorManager.PredictorType PredictorType { get; private set; }
        [field: SerializeField] public float PredictRange { get; private set; }
        [field: SerializeField] public float ManaCost { get; private set; }
        [field: SerializeField] public MagicController MagicObject { get; private set; }
    }
}
