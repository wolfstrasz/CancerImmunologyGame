using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Tutorials
{
	public class TMonitorCancer : TutorialEvent
	{
		[Header("Attributes")]
		[SerializeField]
		private int cancer_number = 0;

		protected override void OnEndEvent()
		{

		}

		protected override void OnStartEvent()
		{
			
		}

		protected override bool OnUpdate()
		{
			if (GlobalGameData.Cancers.Count == cancer_number)
				return true;

			return false;
		}
	}
}
