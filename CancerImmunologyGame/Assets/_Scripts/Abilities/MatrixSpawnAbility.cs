using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.Cancers;

namespace ImmunotherapyGame.Abilities
{

	/// <summary>
	/// Ability that spawns a game object entity on the position of the caster.
	/// The ability has effect only when a caster is targeting a cancer cell and
	/// the isntatiated object has a component of type MatrixCell.
	/// </summary>
	[CreateAssetMenu(menuName = "MyAssets/Abilities/Matrix Spawn Ability")]
	public class MatrixSpawnAbility : Ability, ISerializationCallbackReceiver
	{
		[Header ("Effect On Target Cell is always null.")]
		[SerializeField] private GameObject spawnObjectPrefab;

		/// <summary>
		/// Instantiates a game entity from the given prefab and attaches it to the target only if
		/// the target has a CancerCell component and the prefab object has a component of type MatrixCell
		/// </summary>
		/// <param name="abilityCaster"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		public override bool CastAbility(GameObject abilityCaster, GameObject target)
		{
			CancerCell cancerCell = target.GetComponent<Cancers.CancerCell>();
			if (cancerCell == null)
			{
				Debug.LogWarning("MatrixSpawnAbility Called for a not CancerCell Target");
				return false;
			}

			MatrixCell matrixCell = Instantiate(spawnObjectPrefab, target.transform).GetComponent<MatrixCell>();
			
			if (matrixCell == null)
			{
				Debug.LogWarning("MatrixSpawnAbility Spawned a not Matrix Cell prefab");
				return false;
			}

			matrixCell.AttachCancerCell(cancerCell);
			cancerCell.AttachMatrixCell(matrixCell);
			return true;
		}

		public void OnAfterDeserialize()
		{
			
		}

		public void OnBeforeSerialize()
		{
			effectOnTargetCell = null;
		}
	}
}
