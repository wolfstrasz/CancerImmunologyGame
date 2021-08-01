using UnityEngine;
using ImmunotherapyGame.Player;

namespace ImmunotherapyGame
{
	public class DendriticCellInteractionArea : MonoBehaviour
	{
		[SerializeField] DendriticCell owner = null;

		private void OnTriggerEnter2D(Collider2D collider)
		{
			PlayerController pc = collider.gameObject.GetComponent<PlayerController>();
			if (pc != null)
			{
				owner.Interact();
			}
		}

	}
}