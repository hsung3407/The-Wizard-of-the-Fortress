using System;
using Ingame.Player.Predictor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Ingame.Player
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private InputActionReference pressInput;
        [SerializeField] private InputActionReference pointInput;

        [SerializeField] private Camera ingameCamera;
        [SerializeField] private RectTransform ingameViewRect;
        [SerializeField] private RectTransform touchAreaRect;
        private readonly Vector3[] _ingameViewCorners = new Vector3[4];
        private readonly Vector3[] _touchAreaCorners = new Vector3[4];

        private readonly RaycastHit[] _hits = new RaycastHit[1];
        private LayerMask _layerMask;

        private bool _interacting;

        public event Func<bool> CheckInteractable;
        public event Action<Vector3> OnInteractStart;
        public event Action<Vector3> OnInteract;
        public event Action OnInteractEnd;
        public event Action<Vector3> OnApply;

        private void Awake()
        {
            CheckInteractable = null;
            OnInteractStart = null;
            OnInteract = null;
            OnInteractEnd = null;
            OnApply = null;

            _layerMask = LayerMask.GetMask("Ground", "Default");

            pressInput.action.performed += _ =>
            {
                var point = pointInput.action.ReadValue<Vector2>();
                if ((!CheckInteractable?.Invoke() ?? true) || !PredictRaycast(point, _hits)) { return; }

                _interacting = true;
                OnInteractStart?.Invoke(_hits[0].point);
            };

            pressInput.action.canceled += _ =>
            {
                if (!_interacting) return;
                _interacting = false;

                OnInteractEnd?.Invoke();

                var point = pointInput.action.ReadValue<Vector2>();
                if (!PredictRaycast(point, _hits)) return;

                OnApply?.Invoke(_hits[0].point);
            };

            pointInput.action.performed += c =>
            {
                if (!_interacting) return;

                var point = c.ReadValue<Vector2>();

                if (PredictRaycast(point, _hits)) { OnInteract?.Invoke(_hits[0].point); }
                else
                {
                    _interacting = false;
                    OnInteractEnd?.Invoke();
                }
            };
        }

        private void Start()
        {
            ingameViewRect.GetWorldCorners(_ingameViewCorners);
            touchAreaRect.GetWorldCorners(_touchAreaCorners);
        }

        public void SetInteractable(bool interactable)
        {
            _interacting = false;
            if (interactable)
            {
                pressInput.action.Enable();
                pointInput.action.Enable();
            }
            else
            {
                pressInput.action.Disable();
                pointInput.action.Disable();
            }
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
    }
}