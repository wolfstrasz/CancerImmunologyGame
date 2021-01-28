using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bloodflow
{
	public class BloodflowController : Singleton<BloodflowController>
	{
		[Header("Debug (Read Only)")]
		[SerializeField]
		private List<BloodflowEnvironment> bloodflows = new List<BloodflowEnvironment>();

		public void Initialise()
		{
			bloodflows.AddRange(FindObjectsOfType<BloodflowEnvironment>()); 
			foreach (var bloodflow in bloodflows)
			{
				bloodflow.Initialise();
			}
		}

		public void OnFixedUpdate()
		{
			foreach(var bloodflow in bloodflows)
			{
				bloodflow.OnFixedUpdate();
			}
		}
	}

}