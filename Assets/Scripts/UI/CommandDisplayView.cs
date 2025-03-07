using System;
using System.Collections.Generic;
using System.Text;
using Ingame;
using Ingame.Player;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class CommandDisplayView : MonoBehaviour
    {
        [SerializeField] private Color[] commandColors = new Color[2];
        [SerializeField] private Image commandPrefab;

        private CommandManager.Command[] _commands;
        private Image[] _commandObjects;
        private int _count;

        private void Start()
        {
            var commandCount = StageInfo.CommandCount;
            
            _commands = new CommandManager.Command[commandCount];
            
            _commandObjects = new Image[commandCount];
            for (var i = 0; i < _commandObjects.Length; i++)
            {
                var image = Instantiate(commandPrefab, transform);
                image.enabled = false;
                _commandObjects[i] = image;
            }

            _count = 0;
        }

        public void AddDisplay(CommandManager.Command command)
        {
            if (_count >= _commandObjects.Length) return;
            
            _commands[_count] = command;
            _commandObjects[_count].enabled = true;
            _commandObjects[_count].color = commandColors[(int)command];
            
            Debug.Log(GetCommand());

            _count++;
        }

        public string GetCommand()
        {
            var sb = new StringBuilder();
            for (int i = 0; i <= _count; i++)
                sb.Append(((int)_commands[i]).ToString());

            return sb.ToString();
        }

        public void ClearDisplay()
        {
            foreach (var commandObject in _commandObjects)
                commandObject.enabled = false;

            _count = 0;
        }
    }
}