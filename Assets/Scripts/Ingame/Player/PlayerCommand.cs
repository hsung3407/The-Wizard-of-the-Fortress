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
        private int _counter;
        private Command[] _commands;

        [SerializeField] private PlayerCommandView playerCommandView;

        private bool _interactable;
        public enum Command
        {
            Emission,
            Control
        }
        
        public void Init(int commandCount)
        {
            _commandCount = commandCount;
            _commands = new Command[_commandCount];
            playerCommandView.Init(_commandCount);
        }

        public void SetInteractable(bool interactable)
        {
            _interactable = interactable;
            ClearCommands();
        }

        public void AddCommand(int command) => AddCommand((Command)command);
        
        public void AddCommand(Command command)
        {
            if(!_interactable || _commands.Length <= _counter) return;
            
            _commands[_counter] = command;
            playerCommandView.AddDisplay(command, _counter++);
        }
        
        public string GetCommand()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < _counter; i++)
                sb.Append(((int)_commands[i]).ToString());

            return sb.ToString();
        }

        public void ClearCommands()
        {
            _counter = 0;
            playerCommandView.ClearDisplay();
        }
    }
}