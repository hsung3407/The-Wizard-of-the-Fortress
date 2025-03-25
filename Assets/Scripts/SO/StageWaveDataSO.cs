using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "StageWaveDataSO", menuName = "Scriptable Objects/StageWaveeDataSO")]
    public class StageWaveDataSO : ScriptableObject
    {
        [SerializeField] private int enemyCount;
        [SerializeField] private float generatePerSec;

        public int EnemyCount => enemyCount;
        public float GeneratePerSec => generatePerSec;
    }
}
