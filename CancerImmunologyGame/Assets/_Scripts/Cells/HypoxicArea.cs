using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ImmunotherapyGame
{

	[RequireComponent(typeof(Collider2D))]
	public class HypoxicArea : MonoBehaviour
	{
		[Header("Attributes")]
		[SerializeField]
		private float dmg = 0.5f;

		[Header("Debug (Read only)")]
		[SerializeField]
		List<KillerCell> killerCells = new List<KillerCell>();


		private void OnTriggerEnter2D(Collider2D collider)
		{
			KillerCell kc = collider.gameObject.GetComponent<KillerCell>();
			if (kc != null)
			{
				killerCells.Add(kc);
			}
		}

		private void OnTriggerExit2D(Collider2D collider)
		{
			KillerCell kc = collider.gameObject.GetComponent<KillerCell>();
			if (kc != null)
			{
				killerCells.Remove(kc);
			}
		}

		void Update()
		{
			if (GlobalGameData.isGameplayPaused) return;

			foreach (KillerCell cell in killerCells)
			{
				cell.HitCell(dmg);
			}
		}

	}

}