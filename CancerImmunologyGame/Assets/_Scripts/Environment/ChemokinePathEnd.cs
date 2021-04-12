using UnityEngine;
using ImmunotherapyGame.Player;

namespace Chemokines {
	public class ChemokinePathEnd : MonoBehaviour
	{

		[SerializeField]
		private ChemokinPath owner = null;

		void OnTriggerEnter2D(Collider2D collider)
		{
			if (collider.gameObject == PlayerController.Instance.gameObject)
			{

				owner.OnPlayerReachedEndOfPath();
			}
		}
		
	}
}