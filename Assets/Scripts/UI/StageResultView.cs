using System;
using TMPro;
using UnityEngine;

namespace UI {
    public class StageResult
    {
        
    }
    
    public class StageResultView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI resultText;

        private void Start()
        {
            gameObject.SetActive(false);
        }

        public void Display(bool isWin, StageResult result)
        {
            gameObject.SetActive(true);
            resultText.text = isWin ? "승리" : "패배";
        }
    }
}