using Ingame.Player.Predictor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ingame.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private InputActionReference pressInput;
        [SerializeField] private InputActionReference pointInput;

        private PlayerStat _playerStat;
        private PlayerCommand _playerCommand;
        private PlayerMagic _playerMagic;

        [SerializeField] private Camera ingameCamera;
        [SerializeField] private RectTransform ingameViewRect;
        [SerializeField] private RectTransform touchAreaRect;
        private readonly Vector3[] _ingameViewCorners = new Vector3[4];
        private readonly Vector3[] _touchAreaCorners = new Vector3[4];

        [SerializeField] private PredictorManager predictorManager;

        private readonly RaycastHit[] _hits = new RaycastHit[1];
        private LayerMask _layerMask;

        private bool _interacting;

        private void Awake()
        {
            _playerCommand = GetComponent<PlayerCommand>();
            _playerMagic = GetComponent<PlayerMagic>();
        
            _layerMask = LayerMask.GetMask("Ground", "Default");
        
            pressInput.action.performed += _ =>
            {
                var point = pointInput.action.ReadValue<Vector2>();
                if(!PredictRaycast(point, _hits)) return;
            
                var command = _playerCommand.GetCommand();
                if (!_playerMagic.GetMagicDataWithCommand(command, out var magicData))
                {
                    MagicNotFound();
                    return;
                }

                _interacting = true;
                predictorManager.SetPredictor(magicData.PredictorType, magicData.PredictRange);
                predictorManager.PosUpdate(_hits[0].point);
            };

            pressInput.action.canceled += _ =>
            {
                if (!_interacting) return;
            
                var point = pointInput.action.ReadValue<Vector2>();
                if(!PredictRaycast(point, _hits)) return;
            
                _interacting = false;
                predictorManager.SetPredictor(PredictorManager.PredictorType.None);
            
                Fire(_hits[0].point);
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

        private void MagicNotFound()
        {
            _playerCommand.ClearCommands();
        }

        private void Fire(Vector3 point)
        {
            var command = _playerCommand.GetCommand();
            if (!_playerMagic.GetMagicDataWithCommand(command, out var magicData))
            {
                MagicNotFound();
                return;
            }
            _playerCommand.ClearCommands();

            if (_playerStat.UseMana(magicData.ManaCost)) return;
            if(magicData.MagicObject) Instantiate(magicData.MagicObject, point, Quaternion.identity);
        }
    }
}