using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "StageWaveDataSO", menuName = "Scriptable Objects/StageWaveeDataSO")]
    public class StageWaveDataSO : ScriptableObject
    {
        [field: SerializeField]
        public float Health { get; private set; }
        [field: SerializeField]
        public float Damage { get; private set; }
        [field: SerializeField]
        public float Speed { get; private set; }
        [field: SerializeField]
        public int EnemyCount { get; private set; }
        [field: SerializeField]
        public float GeneratePerSec { get; private set; }
    }
}
