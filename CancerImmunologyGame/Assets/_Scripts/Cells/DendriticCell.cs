using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.Chemokines;

namespace ImmunotherapyGame
{
	public class DendriticCell : MonoBehaviour
	{ 
		[SerializeField] private List<ChemokinPath> chemokinePaths = new List<ChemokinPath>();
		[SerializeField] [ReadOnly] private bool interacted = false;

		public bool HasInteracted => interacted;

		public void Interact()
		{
			foreach (ChemokinPath path in chemokinePaths)
			{
				path.ActivateChemokines();
			}
			interacted = true;
		}
	}
}