using UnityEngine;

namespace Utility.Sound
{
    [CreateAssetMenu(fileName = "SFXDataSO", menuName = "Scriptable Objects/SFXDataSO")]
    public class SFXDataSO : ScriptableObject
    {
        [field: SerializeField]public SFXType Type { get; private set; }
        [field: SerializeField]public AudioClip Clip { get; private set; }
    }
}