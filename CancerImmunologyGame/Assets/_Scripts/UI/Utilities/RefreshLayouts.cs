using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ImmunotherapyGame.UI
{
	/// <summary>
	/// Refreshes layouts in order. 
	/// </summary>
    public class RefreshLayouts : MonoBehaviour
    {
        public List<RectTransform> layoutsToRefresh = new List<RectTransform>();
		[SerializeField][ReadOnly] private bool refreshNotFinished = false;
		[SerializeField][ReadOnly] private bool shouldRefreshFromStart = false;
		[SerializeField][ReadOnly] public int count = 0;

		private void Update()
		{
			if (shouldRefreshFromStart)
			{
				shouldRefreshFromStart = false;
				refreshNotFinished = true;
				count = 0;
				return;
			}

			if (refreshNotFinished)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(layoutsToRefresh[count++]);
				if (count >= layoutsToRefresh.Count)
				{
					count = 0;
					refreshNotFinished = false;
				}
			}
		}

		private void OnEnable()
		{
			shouldRefreshFromStart = true;
		}

		private void OnDisable()
		{
			shouldRefreshFromStart = false;
			refreshNotFinished = false;
			count = 0;
		}

		public void ForceRefresh()
		{
			shouldRefreshFromStart = true;
		}
	}
}
