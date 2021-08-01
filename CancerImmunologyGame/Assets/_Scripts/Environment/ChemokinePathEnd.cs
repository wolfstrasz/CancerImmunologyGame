using UnityEngine;
using ImmunotherapyGame.Player;

namespace ImmunotherapyGame.Chemokines {
	public class ChemokinePathEnd : MonoBehaviour
	{
		[SerializeField] private ChemokinPath owner = null;

		void OnTriggerEnter2D(Collider2D collider)
		{
			if (collider.gameObject.GetComponent<PlayerController>() != null)
			{
				owner.OnPlayerReachedEndOfPath();
			}
		}
		
	}
}