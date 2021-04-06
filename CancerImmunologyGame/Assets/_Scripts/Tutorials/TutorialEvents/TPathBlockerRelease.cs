using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace ImmunotherapyGame.Tutorials
{
	public class TPathBlockerRelease : TutorialEvent
	{
		List<TutorialPathBlocker> pathBlockers = new List<TutorialPathBlocker>();

		protected override void OnEndEvent()
		{
		}

		protected override void OnStartEvent()
		{
			pathBlockers.AddRange(FindObjectsOfType<TutorialPathBlocker>());

			if (pathBlockers.Count == 0)
			{
				return;
			}
			if (pathBlockers.Count == 1)
			{
				pathBlockers[0].gameObject.SetActive(false);
				return;
			}
			Vector3 playerPos = PlayerController.Instance.gameObject.transform.position;
			float minDist = (pathBlockers[0].transform.position - playerPos).sqrMagnitude;
			TutorialPathBlocker closestPB = pathBlockers[0];

			for (int i = 1; i < pathBlockers.Count; ++i)
			{
				float dist = (pathBlockers[i].transform.position - playerPos).sqrMagnitude;
				if (dist < minDist)
				{
					minDist = dist;
					closestPB = pathBlockers[i];
				}
			}

			closestPB.gameObject.SetActive(false);

		}

		protected override bool OnUpdateEvent()
		{
			return true;
		}

	}

}

