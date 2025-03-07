using System;
using System.Collections.Generic;
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

        private Image[] _commandObjects;
        private int _count;

        private void Start()
        {
            _commandObjects = new Image[StageInfo.CommandCount];
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
            
            _commandObjects[_count].enabled = true;
            _commandObjects[_count].color = commandColors[(int)command];

            _count++;
        }

        public void ClearDisplay()
        {
            foreach (var commandObject in _commandObjects)
                commandObject.enabled = false;

            _count = 0;
        }
    }
}