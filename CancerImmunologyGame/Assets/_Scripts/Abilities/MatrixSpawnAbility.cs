using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.Cancers;

namespace ImmunotherapyGame.Abilities
{
	[CreateAssetMenu(menuName = "Abilities/Matrix Spawn Ability")]

	public class MatrixSpawnAbility : Ability
	{
		[SerializeField] private GameObject spawnObjectPrefab;

		public override bool CastAbility(GameObject abilityCaster, GameObject target)
		{
			CancerCell cancerCell = target.GetComponent<Cancers.CancerCell>();
			if (cancerCell == null)
			{
				Debug.LogWarning("MatrixSpawnAbility Called for a not CancerCell Target");
				return false;
			}

			MatrixCell matrixCell = Instantiate(spawnObjectPrefab, target.transform.position, Quaternion.identity).GetComponent<MatrixCell>();
			
			if (matrixCell == null)
			{
				Debug.LogWarning("MatrixSpawnAbility Spawned a not Matrix Cell prefab");
				return false;
			}

			matrixCell.AttachCancerCell(cancerCell);
			cancerCell.AttachMatrixCell(matrixCell);
			return true;
		}
	}
}
