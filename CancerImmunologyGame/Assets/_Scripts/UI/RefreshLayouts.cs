using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ImmunotherapyGame.UI
{
    public class RefreshLayouts : MonoBehaviour
    {
        public List<RectTransform> layoutsToRefresh = new List<RectTransform>();
		public bool shouldRefresh = false;
		public bool shouldRefreshFromStart = false;
		public int count = 0;

		private void Update()
		{
			if (shouldRefreshFromStart)
			{
				shouldRefreshFromStart = false;
				shouldRefresh = true;
				count = 0;
				return;
			}

			if (shouldRefresh)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(layoutsToRefresh[count++]);
				if (count >= layoutsToRefresh.Count)
				{
					count = 0;
					shouldRefresh = false;
				}
			}
		}

		private void OnEnable()
		{
			shouldRefreshFromStart = true;
		}
	}
}
