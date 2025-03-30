using Ingame;
using SO;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Main
{
    public class StageManager : MonoBehaviour
    {
        [SerializeField] private StageInfoView stageInfoView;
    
        private void Awake()
        {
            var stageData = Resources.LoadAll<StageDataSO>("StageData");
        
            var buttons = GetComponentsInChildren<Button>();

            if(stageData.Length != buttons.Length) Debug.LogError("스테이지 데이터 & 버튼 개수 불일치");
        
            for (var i = 0; i < stageData.Length; i++)
            {
                var button = buttons[i];
                var data = stageData[i];
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() =>
                {
                    StaticStageInfo.StageIndex = data.StageIndex;
                    stageInfoView.DisplayView(data);
                });
            }
        }

        public void StartStage()
        {
            SceneManager.LoadScene("Ingame");
        }
    }
}
