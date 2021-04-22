using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame
{
	[RequireComponent(typeof(Collider2D))]
	public class KillerCellTeleporter : MonoBehaviour
	{
		[SerializeField]
		private Transform teleportToTransform = null;

		private void OnTriggerEnter2D(Collider2D collider)
		{
			KillerCell kc = collider.gameObject.GetComponent<KillerCell>();
			if (kc != null)
			{
				kc.transform.position = teleportToTransform.position;
			}

		}
	}
}
