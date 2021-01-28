using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bloodflow
{
	[RequireComponent(typeof(Collider2D))]
	public class BloodflowExit : MonoBehaviour
	{
		[Header("Attributes")]
		[SerializeField]
		private BloodflowEnvironment environment = null;


		private void OnTriggerEnter2D(Collider2D collider)
		{
			KillerCell kc = collider.gameObject.GetComponent<KillerCell>();
			if (kc != null && environment.KillerCells.Contains(kc))
			{
				environment.KillerCells.Remove(kc);
				kc.FlowVector = Vector2.zero;

#if BLOODFLOW_ROTATION
				kc.CorrectRotation = Quaternion.identity;
#endif
			}
		}

		internal void OnForcedExit(KillerCell kc)
		{
			if (environment.KillerCells.Contains(kc))
				environment.KillerCells.Remove(kc);
		}
	}
}

