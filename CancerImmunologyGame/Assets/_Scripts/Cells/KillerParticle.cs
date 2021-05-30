using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.Cancers;

namespace ImmunotherapyGame
{  
	public class KillerParticle : CellParticle
	{
		protected override void OnCollisionWithTarget(Cell cell)
		{
			//cancerCell.HitCell(effectToHealth);
			//Destroy(gameObject);
		}

	}
}