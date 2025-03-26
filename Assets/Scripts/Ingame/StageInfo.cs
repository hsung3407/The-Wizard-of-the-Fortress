using System.Collections.Generic;
using UnityEngine;

namespace Ingame
{
    public class StageInfo : MonoBehaviour
    {
        public static int StageIndex = 0;
        public static int CommandCount = 3;
        public static string MobType = "Slime";

        [SerializeField] private int stageIndex;
        [SerializeField] private int commandCount;
    }
}