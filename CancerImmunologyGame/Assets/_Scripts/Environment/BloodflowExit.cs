using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ImmunotherapyGame.Bloodflow
{
	[RequireComponent(typeof(Collider2D))]
	public class BloodflowExit : MonoBehaviour
	{
		[Header("Attributes")]
		[SerializeField] private BloodflowEnvironment environment = null;

		private void OnTriggerEnter2D(Collider2D collider)
		{
			IControllerMovementOverridable overridable = collider.gameObject.GetComponent<IControllerMovementOverridable>();
			if (overridable != null)
			{
				overridable.UnsubscribeMovementOverride(environment);
			}
		}
	}
}

