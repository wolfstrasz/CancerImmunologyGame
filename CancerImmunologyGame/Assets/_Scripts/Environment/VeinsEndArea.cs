using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bloodflow
{
	[RequireComponent(typeof(Collider2D))]
	public class VeinsEndArea : MonoBehaviour
	{
		[SerializeField]
		private GameObject arteryStart = null;
		[SerializeField]
		private BloodflowExit exit = null;

		private void OnTriggerEnter2D(Collider2D collider)
		{
			KillerCell kc = collider.gameObject.GetComponent<KillerCell>();
			if (kc != null)
			{
				kc.transform.position = arteryStart.transform.position;

				exit.OnForcedExit(kc);
			}

		}
	}
}
