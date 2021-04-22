using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame.Cancers
{
	public class CAFCell : EvilCell
	{

		[Header("CAF Cell")]
		[SerializeField]
		private CAFSense sense = null;
		[SerializeField]
		private GameObject matrixPrefab = null;
		[SerializeField]
		private float timeBetweenMatrixSpawns = 15f;
		[SerializeField]
		private float timePassedForMatrixSpawns = 0f;

		[Header("Debug (Read Only)")]
		[SerializeField]
		internal Cancer cancerOwner = null;

		public override bool isImmune => isDying;

		void Awake()
		{
			healthBar.MaxHealth = maxHealth;
			healthBar.Health = maxHealth;
		}

		public void OnUpdate()
		{
			timePassedForMatrixSpawns += Time.deltaTime;
			if (timePassedForMatrixSpawns >= timeBetweenMatrixSpawns)
			{
				timePassedForMatrixSpawns = 0f;

				CancerCell cellToSpawn = FindACancerCellToSpawnMatrix();

				if (cellToSpawn != null)
				{
					MatrixCell matrix = Instantiate(matrixPrefab, transform.position, Quaternion.identity).GetComponent<MatrixCell>();
					matrix.SetMatrixData(cellToSpawn.transform, this.transform);
					cellToSpawn.matrix = matrix;
				}

			}
		}


		private CancerCell FindACancerCellToSpawnMatrix()
		{
			List<CancerCell> possibleCells = new List<CancerCell>(sense.cancerCells.Count);

			for (int i = 0; i < sense.cancerCells.Count; ++i)
			{
				if (!sense.cancerCells[i].isImmune && sense.cancerCells[i].matrix == null)
				{
					possibleCells.Add(sense.cancerCells[i]);
				}
			}

			if (possibleCells.Count != 0)
			{
				int index = Random.Range(0, possibleCells.Count);
				return possibleCells[index];
			}

			return null;
		}


		protected override void OnDeath()
		{
			animator.SetTrigger("Death");
		}
	}
}
