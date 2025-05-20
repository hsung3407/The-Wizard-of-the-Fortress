using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "StageDataSO", menuName = "Scriptable Objects/StageDataSO")]
    public class StageDataSO : ScriptableObject
    {
        [field: SerializeField] public string StageName { get; private set; }
        [field: SerializeField] public int StageIndex { get; private set; }
        [field: SerializeField] public Sprite StageImage { get; private set; }
    }
}