using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chemokines {
	public class ChemokinePathEnd : MonoBehaviour
	{

		[SerializeField]
		private ChemokinPath owner;
		void OnTriggerEnter2D(Collider2D collider)
		{
			if (collider.gameObject == GlobalGameData.player)
			{
				owner.OnPlayerReachedEndOfPath();
			}
		}
		
	}
}