using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.SaveSystem;

namespace ImmunotherapyGame.Tutorials
{
    [CreateAssetMenu(menuName = "MyAssets/Tutorial Log Data")]
    public class TutorialLogsData : ScriptableObject
    {
		public bool isSystemUnlocked;
        public List<TutorialLog> allLogs;


        internal void ResetData()
        {
            for (int i = 0; i < allLogs.Count; ++i)
            {
                allLogs[i].isUnlocked = false;
            }

        }
    }

	[System.Serializable]
    public class SerializableTutorialLogsData : SaveableObject
    {
        public List<bool> unlocked = new List<bool>();
		public bool isSystemUnlocked = false;

		public SerializableTutorialLogsData(TutorialLogsData data)
		{
			unlocked.Clear();
			for (int i = 0; i < data.allLogs.Count; ++i)
			{
				unlocked.Add(data.allLogs[i].isUnlocked);
			}
			isSystemUnlocked = data.isSystemUnlocked;
		}

		public SerializableTutorialLogsData()
		{
			unlocked.Clear();
		}

		public void CopyTo(TutorialLogsData data)
		{
			int minIndex = Mathf.Min(data.allLogs.Count, unlocked.Count);

			data.isSystemUnlocked = isSystemUnlocked;
			for (int i = 0; i < minIndex; ++i)
			{
				data.allLogs[i].isUnlocked = unlocked[i];
			}
		}
	}
}
