using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame
{
	[RequireComponent(typeof(Collider2D))]
	public class HelperCellParticle : CellParticle
	{


		protected override void OnCollisionWithTarget(Cell cell)
		{
			//cell.AddEnergy(effectToEnergy);
			//cell.AddHealth(effectToHealth);
			//DestroyParticle();

		}


	}

}