using TMPro;
using UnityEngine;

namespace UI {
    public class StageResult
    {
        
    }
    
    public class StageResultView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI resultText;
        
        public void Display(bool isWin, StageResult result)
        {
            resultText.text = isWin ? "승리" : "패배";
        }
    }
}