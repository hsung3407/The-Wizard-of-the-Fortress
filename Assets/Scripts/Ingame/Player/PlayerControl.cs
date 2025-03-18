using System;
using Ingame.Player.Predictor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private InputActionReference pressInput;
    [SerializeField] private InputActionReference pointInput;

    private bool _interacting;

    [SerializeField] private Camera ingameCamera;
    [SerializeField] private RectTransform ingameViewRect;
    [SerializeField] private RectTransform touchAreaRect;
    private readonly Vector3[] _ingameViewCorners = new Vector3[4];
    private readonly Vector3[] _touchAreaCorners = new Vector3[4];

    [SerializeField] private PredictorManager predictorManager;

    private readonly RaycastHit[] _hits = new RaycastHit[1];
    private LayerMask _layerMask;

    private void Awake()
    {
        _layerMask = LayerMask.GetMask("Ground", "Default");
        
        pressInput.action.performed += _ =>
        {
            var point = pointInput.action.ReadValue<Vector2>();
            if(!PredictRaycast(point, _hits)) return;

            _interacting = true;
            //현재는 테스트용
            //TODO 마법 정보 불러와서 전달하기
            predictorManager.SetPredictor(PredictorManager.PredictorType.Square);
            predictorManager.PosUpdate(_hits[0].point);
        };

        pressInput.action.canceled += _ =>
        {
            if (!_interacting) return;
            
            var point = pointInput.action.ReadValue<Vector2>();
            if(!PredictRaycast(point, _hits)) return;
            
            _interacting = false;
            predictorManager.SetPredictor(PredictorManager.PredictorType.None);
            
            Fire();
        };

        pointInput.action.performed += c =>
        {
            if (!_interacting) return;

            var point = c.ReadValue<Vector2>();
            if (PredictRaycast(point, _hits))
            {
                predictorManager.PosUpdate(_hits[0].point);
            }
            else
            {
                predictorManager.SetPredictor(PredictorManager.PredictorType.None);
                _interacting = false;
            }
        };
    }

    private void Start()
    {
        ingameViewRect.GetWorldCorners(_ingameViewCorners);
        touchAreaRect.GetWorldCorners(_touchAreaCorners);
    }

    private bool PredictRaycast(Vector2 point, RaycastHit[] hits)
    {
        if (!IsInTouchArea(point)) return false;
        var ray = IngameViewToRay(point);
        return Physics.RaycastNonAlloc(ray, hits, 30, _layerMask) > 0 &&
               hits[0].transform.gameObject.layer == LayerMask.NameToLayer("Ground");
    }

    private bool IsInTouchArea(Vector3 origin)
    {
        return origin.x > _touchAreaCorners[0].x && origin.y > _touchAreaCorners[0].y &&
               origin.x < _touchAreaCorners[2].x && origin.y < _touchAreaCorners[2].y;
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