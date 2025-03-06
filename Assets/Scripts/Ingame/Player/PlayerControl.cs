using System;
using Ingame.Player.Predictor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private InputActionReference pressInput;
    [SerializeField] private InputActionReference pointInput;

    private bool _predicating;

    [SerializeField] private Camera ingameCamera;
    [SerializeField] private RectTransform ingameViewRect;
    private readonly Vector3[] _ingameViewCorners = new Vector3[4];

    [SerializeField] private PredictorManager predictorManager;

    private readonly RaycastHit[] _hits = new RaycastHit[1];
    
    private void Awake()
    {
        pointInput.action.performed += c =>
        {
            if (!_predicating) return;
            
            var ray = IngameViewToRay(pointInput.action.ReadValue<Vector2>());
            if (Physics.RaycastNonAlloc(ray, _hits, 30, ~LayerMask.NameToLayer("Enemy")) > 0)
            {
                if (_hits[0].transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    predictorManager.PosUpdate(_hits[0].point);
                    return;
                }
            }
            
            predictorManager.SetPredictor(PredictorManager.PredictorType.None);
            _predicating = false;
        };

        pressInput.action.performed += _ =>
        {

            var ray = IngameViewToRay(pointInput.action.ReadValue<Vector2>());
            if (Physics.RaycastNonAlloc(ray, _hits, 30, ~LayerMask.NameToLayer("Enemy")) < 1) return;
            if (_hits[0].transform.gameObject.layer != LayerMask.NameToLayer("Ground")) return;
            _predicating = true;

             //현재는 테스트용
             //TODO 마법 정보 불러와서 전달하기
             predictorManager.SetPredictor(PredictorManager.PredictorType.Square);
             predictorManager.PosUpdate(_hits[0].point);
       };

        pressInput.action.canceled += _ =>
        {
            if (!_predicating) return;
            _predicating = false;
            predictorManager.SetPredictor(PredictorManager.PredictorType.None);
            Fire();
        };
    }

    private void Start()
    {
        ingameViewRect.GetWorldCorners(_ingameViewCorners);
    }

    private Ray IngameViewToRay(Vector3 origin)
    {
        origin -= _ingameViewCorners[0];
        var size = (_ingameViewCorners[2] - _ingameViewCorners[0]);
        var x = origin.x / size.x * ingameCamera.pixelWidth;
        var y = origin.y / size.y * ingameCamera.pixelHeight;
        return ingameCamera.ScreenPointToRay(new Vector3(x, y, 0));
    }

    private void Fire()
    {
    }
    
}