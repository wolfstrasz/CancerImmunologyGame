using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame
{
	[RequireComponent(typeof(Collider2D))]
	public class CellTeleporter : MonoBehaviour
	{
		[SerializeField] private Transform teleportToTransform = null;

		private void OnTriggerEnter2D(Collider2D collider)
		{
			Cell cell = collider.gameObject.GetComponent<Cell>();
			if (cell != null)
			{
				cell.gameObject.transform.position = teleportToTransform.position;
			}

		}
	}
}
