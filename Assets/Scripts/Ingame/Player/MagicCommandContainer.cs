using UnityEngine;

namespace Ingame.Player
{
    [CreateAssetMenu(fileName = "MagicCommandContainer", menuName = "Scriptable Objects/MagicCommandContainer")]
    public class MagicCommandContainer : ScriptableObject
    {
        [SerializeField] private string magicName;
        [SerializeField] private string command;
        [SerializeField] private MagicBase magicObject;
    
        public string MagicName => magicName;
        public string Command => command;
        public MagicBase MagicObject => magicObject;
    }
}
