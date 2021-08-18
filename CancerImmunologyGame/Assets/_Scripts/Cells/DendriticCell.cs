using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.Chemokines;

namespace ImmunotherapyGame
{
	public class DendriticCell : MonoBehaviour
	{ 
		[SerializeField] private List<ChemokinePath> chemokinePaths = new List<ChemokinePath>();
		[SerializeField] [ReadOnly] private bool interacted = false;

		public bool HasInteracted => interacted;

		public void Interact()
		{
			foreach (ChemokinePath path in chemokinePaths)
			{
				path.gameObject.SetActive(true);
			}
			interacted = true;
		}
	}
}