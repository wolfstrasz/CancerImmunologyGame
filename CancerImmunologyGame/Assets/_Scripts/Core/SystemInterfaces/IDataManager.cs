using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame.Core.SystemInterfaces
{
    public interface IDataManager
    {
        public void LoadData();
        public void SaveData();
        public void ResetData();
    }
}
