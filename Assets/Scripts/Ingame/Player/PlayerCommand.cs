using System;
using System.Collections.Generic;
using System.Text;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ingame.Player
{
    public class PlayerCommand : MonoBehaviour
    {
        private int _commandCount;
        private int _commandCounter;
        private Command[] _commands;

        [SerializeField] private CommandDisplayView commandDisplayView;

        public enum Command
        {
            Emission,
            Control
        }

        private void Start()
        {
            _commandCount = StageInfo.CommandCount;
            _commands = new Command[_commandCount];
            commandDisplayView.Init(_commandCount);
        }

        public void AddCommand(int command) => AddCommand((Command)command);
        
        public void AddCommand(Command command)
        {
            if(_commands.Length >= _commandCount) return;
            
            _commands[_commandCount] = command;
            commandDisplayView.AddDisplay(command, _commandCounter++);
        }
        
        public string GetCommand()
        {
            var sb = new StringBuilder();
            for (int i = 0; i <= _commandCounter; i++)
                sb.Append(((int)_commands[i]).ToString());

            return sb.ToString();
        }

        public void ClearCommands()
        {
            _commandCounter = 0;
            commandDisplayView.ClearDisplay();
        }
    }
}