using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "StageDataSO", menuName = "Scriptable Objects/StageDataSO")]
    public class StageDataSO : ScriptableObject
    {
        [SerializeField] private string stageName;
        [SerializeField] private int stageIndex;
        
        public string StageName => stageName;
        public int StageIndex => stageIndex;
    }
}
