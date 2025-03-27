using System;
using SO;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class StageInfoView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI stageNameTMP;
        [SerializeField] private TextMeshProUGUI maxSpellLengthTMP;
        
        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void DisplayView(StageDataSO stageData)
        {
            stageNameTMP.text = stageData.StageName;

            maxSpellLengthTMP.text = $"Max Spell Length : {stageData.CommandCount}";
            
            gameObject.SetActive(true);
        }

        public void Exit()
        {
            gameObject.SetActive(false);
        }
    }
}