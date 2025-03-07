using System;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ingame.Player
{
    public class CommandManager : MonoBehaviour
    {
        private readonly List<Command> _commandList = new List<Command>();
        private int _commandCount;

        [SerializeField] private CommandDisplayView commandDisplayView;

        public enum Command
        {
            Emission,
            Control
        }

        private void Start()
        {
            _commandCount = StageInfo.CommandCount;
        }

        public void AddCommand(int command) => AddCommand((Command)command);
        
        public void AddCommand(Command command)
        {
            if(_commandList.Count >= _commandCount) return;
            
            _commandList.Add(command);
            commandDisplayView.AddDisplay(command);
        }

        public void ClearCommands()
        {
            _commandList.Clear();
            commandDisplayView.ClearDisplay();
        }
    }
}