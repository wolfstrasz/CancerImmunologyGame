using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

namespace Bloodflow
{
	[RequireComponent(typeof(Collider2D))]
	public class BloodflowEntrance : MonoBehaviour
	{
		[Header("Attributes")]
		[SerializeField]
		private BloodflowEnvironment environment = null;

		private void OnTriggerEnter2D(Collider2D collider)
		{
			KillerCell kc = collider.gameObject.GetComponent<KillerCell>();
			if (kc != null)
			{
				if (!environment.KillerCells.Contains (kc))
					environment.KillerCells.Add(kc);
			}
		}
		
	}


}