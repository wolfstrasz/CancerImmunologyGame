using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ImmunotherapyGame.LevelManagement
{
    public class LevelSelectItem : MonoBehaviour
    {
        [SerializeField]
        private GameObject completedOverlay = null;
        [SerializeField]
        private GameObject lockedOverlay = null;
        [SerializeField]
        private TMP_Text text = null;

        internal void UpdateData(LevelData data)
		{
            Debug.Log(gameObject);
            Debug.Log(completedOverlay);
            Debug.Log(data);
            completedOverlay.SetActive(data.isCompleted);
            lockedOverlay.SetActive(data.isLocked);
            text.text = data.levelIndex.ToString();
		}
    }
}
