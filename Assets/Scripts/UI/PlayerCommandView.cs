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
    public class PlayerCommandView : MonoBehaviour
    {
        [SerializeField] private Color[] commandColors = new Color[2];
        [SerializeField] private Image commandPrefab;
        
        private Image[] _commandObjects;

        public void Init(int commandCount)
        {
            _commandObjects = new Image[commandCount];
            for (var i = 0; i < _commandObjects.Length; i++)
            {
                var image = Instantiate(commandPrefab, transform);
                image.enabled = false;
                _commandObjects[i] = image;
            }
        }

        public void AddDisplay(PlayerCommand.Command command, int commandCounter)
        {
            _commandObjects[commandCounter].enabled = true;
            _commandObjects[commandCounter].color = commandColors[(int)command];
        }

        public void ClearDisplay()
        {
            foreach (var commandObject in _commandObjects)
                commandObject.enabled = false;
        }
    }
}