using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Ingame.Player.Predictor
{
    public class PredictorManager : MonoBehaviour
    {
        private List<DecalProjector> predictors;

        private GameObject _currentPredictor;

        public enum PredictorType
        {
            Square,
            Circle,
            None
        }

        private void Awake()
        {
            predictors = GetComponentsInChildren<DecalProjector>().ToList();
            
            foreach (var predictor in predictors)
                predictor.gameObject.SetActive(false);
        }

        public void SetPredictor(PredictorType predictorType, float range = 1)
        {
            _currentPredictor?.SetActive(false);

            if(predictorType == PredictorType.None) return;
            
            var projector = predictors[(int)predictorType];
            projector.size = new Vector2(range, range);
            _currentPredictor = projector.gameObject;
            _currentPredictor.SetActive(true);
        }

        public void PosUpdate(Vector3 position)
        {
            if(!_currentPredictor || !_currentPredictor.activeSelf) return;
            transform.position = position;
        }
    }
}
