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
        [SerializeField] private string magicName;
        [SerializeField] private string command;
        
        [Header("Description")]
        [SerializeField] private Sprite icon;
        [SerializeField] private string description;

        [Header("Data")] 
        [SerializeField] private PredictorManager.PredictorType predictorType;
        [SerializeField] private float predictRange;
        
        [FormerlySerializedAs("useMana")] [SerializeField] private float manaCost;
        
        [SerializeField] private MagicController magicObject;
    
        public string MagicName => magicName;
        public string Command => command;
        
        public Sprite Icon => icon;
        public string Description => description;
        
        public PredictorManager.PredictorType PredictorType => predictorType;
        public float PredictRange => predictRange;
        public float ManaCost => manaCost;
        public MagicController MagicObject => magicObject;
    }
}
