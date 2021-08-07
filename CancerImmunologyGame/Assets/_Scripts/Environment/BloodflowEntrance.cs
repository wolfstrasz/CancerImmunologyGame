using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

namespace ImmunotherapyGame.Bloodflow
{
	[RequireComponent(typeof(Collider2D))]
	public class BloodflowEntrance : MonoBehaviour
	{
		private void OnTriggerEnter2D(Collider2D collider)
		{
			IControllerMovementOverridable overridable = collider.gameObject.GetComponent<IControllerMovementOverridable>();
			if (overridable != null)
			{
				BloodflowEnvironment.Instance.AddOverridable(collider.gameObject);
			}
		}

		private void OnTriggerExit2D(Collider2D collider)
		{
			IControllerMovementOverridable overridable = collider.gameObject.GetComponent<IControllerMovementOverridable>();
			if (overridable != null)
			{
				BloodflowEnvironment.Instance.RemoveOverridable(collider.gameObject);
			}
		}

	}


}