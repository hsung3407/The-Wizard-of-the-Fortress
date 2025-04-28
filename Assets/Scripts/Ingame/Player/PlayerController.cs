using System;
using Ingame.Player.Predictor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ingame.Player
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerInput _playerInput;
        private PlayerStatsManager playerStatsManager;
        private PlayerCommand _playerCommand;
        private PlayerMagic _playerMagic;

        [SerializeField] private PredictorManager predictorManager;

        public event Action<MagicDataSO, MagicStatsModifier> OnFire;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            _playerCommand = GetComponent<PlayerCommand>();
            _playerMagic = GetComponent<PlayerMagic>();
        }

        private void Start()
        {
            _playerCommand.Init(playerStatsManager.CommandCount);

            _playerInput.CheckInteractable += () =>
            {
                if (_playerMagic.Contains(_playerCommand.GetCommand())) { return true; }

                //TODO: 선택된 마법 없음 표시
                _playerCommand.ClearCommands();
                return false;
            };

            _playerInput.OnInteractStart += pos =>
            {
                _playerMagic.GetMagicDataWithCommand(_playerCommand.GetCommand(), out var magicData);
                predictorManager.SetPredictor(magicData.PredictorType, magicData.PredictRange);
                predictorManager.PosUpdate(pos);
            };

            _playerInput.OnInteract += pos => predictorManager.PosUpdate(pos);

            _playerInput.OnInteractEnd += () => { predictorManager.SetPredictor(PredictorManager.PredictorType.None); };

            _playerInput.OnApply += Fire;
        }

        private void Fire(Vector3 point)
        {
            var command = _playerCommand.GetCommand();
            _playerMagic.GetMagicDataWithCommand(command, out var magicData);
            _playerCommand.ClearCommands();

            var magicObject = magicData.MagicObject;
            var magicStatsModifier = new MagicStatsModifier();
            OnFire?.Invoke(magicData, magicStatsModifier);
            var modifiedMagicStats = magicStatsModifier.Modify(magicData.MagicStats);

            if (!playerStatsManager.UseMana(modifiedMagicStats.ManaCost)) { return; }

            var magic = Instantiate(magicObject, point, Quaternion.identity);
            magic.InitMagic(magicData, modifiedMagicStats);
        }
    }
}