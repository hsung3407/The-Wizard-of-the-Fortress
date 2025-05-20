using System;
using System.Collections.Generic;
using System.Linq;
using Main;
using SO;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    private StageDataSO[] stages;
    [SerializeField] private StageTitleView stageTitleView;

    private void Awake()
    {
        stages = Resources.LoadAll<StageDataSO>("StageData");
    }

    private void Start()
    {
        stageTitleView.SetView(stages[UserInfo.MaxStage].StageName, stages[UserInfo.MaxStage].StageImage);
    }
}