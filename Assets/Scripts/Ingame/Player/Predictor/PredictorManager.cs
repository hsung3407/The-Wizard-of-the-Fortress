using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame.Player.Predictor
{
    public class PredictorManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> predictors;

        private GameObject _currentPredictor;

        public enum PredictorType
        {
            Square,
            Circle,
            None
        }

        private void Awake()
        {
            foreach (var predictor in predictors)
                predictor.SetActive(false);
        }

        public void SetPredictor(PredictorType predictorType, float range = 1)
        {
            _currentPredictor?.SetActive(false);

            if(predictorType == PredictorType.None) return;
            
            _currentPredictor = predictors[(int)predictorType];
            _currentPredictor.SetActive(true);
        }

        public void PosUpdate(Vector3 position)
        {
            if(!_currentPredictor || !_currentPredictor.activeSelf) return;
            transform.position = position;
        }
    }
}
