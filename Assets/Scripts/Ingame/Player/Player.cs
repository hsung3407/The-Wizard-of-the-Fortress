using System;
using Ingame.Player.Predictor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ingame.Player
{
    public class Player : MonoBehaviour
    {
        public PlayerInput PlayerInput { get; private set; }
        public PlayerStats PlayerStats { get; private set; }
        public PlayerAbility PlayerAbility { get; private set; }
        public PlayerCommand PlayerCommand { get; private set; }
        public PlayerMagic PlayerMagic { get; private set; }

        [SerializeField] private PredictorManager predictorManager;

        public event Action<MagicDataSO, MagicStatsModifier> OnFire;

        private bool _interactable;

        private void Awake()
        {
            PlayerInput = GetComponent<PlayerInput>();
            PlayerStats = GetComponent<PlayerStats>();
            PlayerCommand = GetComponent<PlayerCommand>();
            PlayerMagic = GetComponent<PlayerMagic>();
            PlayerAbility = GetComponent<PlayerAbility>();
        }

        private void Start()
        {
            PlayerCommand.Init(PlayerStats.CommandCount);

            PlayerInput.CheckInteractable += () =>
            {
                if (!_interactable || PlayerMagic.Contains(PlayerCommand.GetCommand())) { return true; }

                //TODO: 선택된 마법 없음 표시
                PlayerCommand.ClearCommands();
                return false;
            };

            PlayerInput.OnInteractStart += pos =>
            {
                PlayerMagic.GetMagicDataWithCommand(PlayerCommand.GetCommand(), out var magicData);
                predictorManager.SetPredictor(magicData.PredictorType, magicData.PredictRange);
                predictorManager.PosUpdate(pos);
            };

            PlayerInput.OnInteract += pos => predictorManager.PosUpdate(pos);

            PlayerInput.OnInteractEnd += () => { predictorManager.SetPredictor(PredictorManager.PredictorType.None); };

            PlayerInput.OnApply += Fire;
        }

        public void SetInteractable(bool interactable, bool init = false)
        {
            _interactable = interactable;
            PlayerInput.SetInteractable(interactable);
            PlayerCommand.SetInteractable(interactable);
            predictorManager.SetPredictor(PredictorManager.PredictorType.None);
            
            if (init)
            {
                PlayerCommand.ClearCommands();
            }
        }

        private void Fire(Vector3 point)
        {
            var command = PlayerCommand.GetCommand();
            PlayerMagic.GetMagicDataWithCommand(command, out var magicData);
            PlayerCommand.ClearCommands();

            var magicObject = magicData.MagicObject;
            var magicStatsModifier = new MagicStatsModifier();
            OnFire?.Invoke(magicData, magicStatsModifier);
            var modifiedMagicStats = magicStatsModifier.Modify(magicData.MagicStats);

            if (!PlayerStats.UseMana(modifiedMagicStats.ManaCost)) { return; }

            var magic = Instantiate(magicObject, point, Quaternion.identity);
            magic.InitMagic(magicData, modifiedMagicStats, PlayerStats.ModifiedStats);
        }
    }
}