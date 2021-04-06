using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cancers;

namespace ImmunotherapyGame.Tutorials
{
	public class TWaitForCancerToStartDivision : TutorialEvent
	{
		[Header("Debug")]
		[SerializeField]
		private List<Cancer> cancers = new List<Cancer>();

		protected override void OnEndEvent()
		{
		}

		protected override void OnStartEvent()
		{
			cancers.AddRange(FindObjectsOfType<Cancer>());
		}

		protected override bool OnUpdateEvent()
		{

			foreach (Cancer cancer in cancers)
			{
				// If cancer is currently dividing
				if (!cancer.CanDivide)
				{
					if (GameCamera2D.Instance.IsInCameraViewBounds(cancer.CellToDivide.transform.position))
					{
						return true;
					}
				}
			}

			return false;
		}
	}
}