using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorials
{
	public class TForceCancerGrowth : TutorialEvent
	{
		[Header("Attributes")]
		[SerializeField]
		private float timeToPassBeforeForceGrowth = 5.0f;

		[Header("Debug")]
		[SerializeField]
		private Cancer closestCancer = null;
		[SerializeField]
		private bool isWaitingToEnterCameraFrustum = false;
		[SerializeField]
		private bool isWaitingForCancerToGrow = false;


		protected override void OnEndEvent()
		{
		}

		protected override void OnStartEvent()
		{
			isWaitingToEnterCameraFrustum = true;
		}

		protected override bool OnUpdate()
		{

			if (isWaitingToEnterCameraFrustum)
			{
				if (SmoothCamera.Instance.IsInCameraViewBounds(gameObject.transform.position))
					ForceGrowth();
			}
	
			if (isWaitingForCancerToGrow)
			{
				if (closestCancer.TimePassed < (closestCancer.TimeBetweenDivisions - timeToPassBeforeForceGrowth))
					return true;
			}

			return false;
		}

		private void ForceGrowth()
		{
			isWaitingToEnterCameraFrustum = false;
			Cancer[] Cancers = FindObjectsOfType<Cancer>();
			float closestDist = Vector3.SqrMagnitude(Cancers[0].gameObject.transform.position - gameObject.transform.position);
			closestCancer = Cancers[0];

			foreach (Cancer cancer in Cancers)
			{
				float distanceSquered = Vector3.SqrMagnitude(cancer.gameObject.transform.position - gameObject.transform.position);
				if (closestDist < distanceSquered)
				{
					closestDist = distanceSquered;
					closestCancer = cancer;
				}
			}
			closestCancer.ForceGrowthAfterTime(timeToPassBeforeForceGrowth);
			isWaitingForCancerToGrow = true;

		}
	}
}
