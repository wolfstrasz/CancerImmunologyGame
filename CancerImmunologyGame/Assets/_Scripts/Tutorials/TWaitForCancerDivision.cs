using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorials
{
	public class TWaitForCancerDivision : TutorialEvent
	{
		[Header("Debug")]
		[SerializeField]
		private List<Cancer> cancers = new List<Cancer>();
		[SerializeField]
		private bool isWaitingToSeeDivision = false;
		[SerializeField]
		private bool isWaitingDivisionToFinish = false;
		[SerializeField]
		private Cancer dividingCancer = null;

		protected override void OnEndEvent()
		{
		}

		protected override void OnStartEvent()
		{
			isWaitingToSeeDivision = true;
			cancers.AddRange(FindObjectsOfType<Cancer>());
		}

		protected override bool OnUpdate()
		{

			if(isWaitingToSeeDivision){
				foreach (Cancer cancer in cancers)
				{
					// If cancer is currently dividing
					if (!cancer.CanDivide)
					{
						if (SmoothCamera.Instance.IsInCameraViewBounds(cancer.CellToDivide.transform.position))
						{
							isWaitingDivisionToFinish = true;
							isWaitingToSeeDivision = false;
							dividingCancer = cancer;
							continue;
						}
					}
				}
			}

			if (isWaitingDivisionToFinish)
			{
				return dividingCancer.CanDivide;
			}

			return false;
		}
	}
}
