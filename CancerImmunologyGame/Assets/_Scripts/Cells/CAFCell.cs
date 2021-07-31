using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.Abilities;

namespace ImmunotherapyGame.Cancers
{
	public class CAFCell : Cell
	{
		[Header("CAF Cell")]
		[SerializeField] RangedAbilityCaster matrixSpawnCaster = null;
		[SerializeField] [ReadOnly] internal Cancer cancerOwner = null;

		public override void OnUpdate()
		{
			if (!matrixSpawnCaster.IsOnCooldown)
			{
				for (int i = 0; i < matrixSpawnCaster.TargetsInRange.Count; ++i)
				{
					CancerCell cell = matrixSpawnCaster.TargetsInRange[i].GetComponent<CancerCell>();
					if ( cell != null && cell.matrixCell == null)
					{
						matrixSpawnCaster.CastAbility(cell.gameObject);
						break;
					}
				}
			}
		}

		protected override void OnCellDeath()
		{
			animator.SetTrigger("Death");
		}

	}
}
