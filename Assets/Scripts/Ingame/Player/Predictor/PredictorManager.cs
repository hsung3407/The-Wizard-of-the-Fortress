using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame.Player.Predictor
{
    public class PredictorManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> predictors;

        private GameObject _currentPredictor;

        private void Awake()
        {
            foreach (var predictor in predictors)
                predictor.SetActive(false);
        }

        public void SetPredictor(string predictorType)
        {
            _currentPredictor?.SetActive(false);
            if(predictorType == null) return;
            
            _currentPredictor = predictors.Find(p=>p.name == predictorType);
            _currentPredictor.SetActive(true);
        }

        public void Update()
        {
            if(!_currentPredictor) return;
            _currentPredictor.transform.position = transform.position;
        }
    }
}
