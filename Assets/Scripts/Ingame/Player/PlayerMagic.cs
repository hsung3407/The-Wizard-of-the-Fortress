using System;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;

namespace Ingame.Player
{
    public class PlayerMagic : MonoBehaviour
    {
        private Dictionary<string, MagicDataSO> _magicDataDict = new Dictionary<string, MagicDataSO>();

        private void Start()
        {
            var data = Resources.LoadAll<MagicDataSO>("MagicDataSO");
            _magicDataDict = data.ToDictionary(datum=>datum.Command, datum=>datum);
        }

        public bool GetMagicDataWithCommand(string command, out MagicDataSO data)
        {
            var check =  _magicDataDict.TryGetValue(command, out data);
            if (!check)
            {
                NotificationManager.Instance.NotifyError("완성된 마법이 없습니다");
            }
            return check;
        }

        public bool Contains(string command)
        {
            return _magicDataDict.ContainsKey(command);
        }
    }
}