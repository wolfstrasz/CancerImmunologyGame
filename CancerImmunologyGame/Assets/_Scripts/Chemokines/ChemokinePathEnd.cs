using UnityEngine;
using ImmunotherapyGame.Player;

namespace ImmunotherapyGame.Chemokines {
	public class ChemokinePathEnd : MonoBehaviour
	{
		[SerializeField] private ChemokinePath path = null;

		void OnTriggerEnter2D(Collider2D collider)
		{
			if (collider.gameObject.GetComponent<PlayerController>() != null)
			{
				path.HideChemokines();
				gameObject.SetActive(false);
			}
		}
		
	}
}