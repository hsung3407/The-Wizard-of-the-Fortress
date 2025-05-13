using System;
using System.Collections.Generic;
using System.Linq;
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
                NotificationManager.Instance.NotifyError("Can't Find Magic");
                Debug.Log($"해당 커맨드를 가진 정보가 없음 : {command}");
            }
            return check;
        }

        public bool Contains(string command)
        {
            return _magicDataDict.ContainsKey(command);
        }
    }
}