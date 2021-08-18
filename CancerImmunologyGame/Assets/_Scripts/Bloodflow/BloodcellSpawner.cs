using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using ImmunotherapyGame.Core;

namespace ImmunotherapyGame.Bloodflow
{
	public class BloodcellSpawner : Singleton<BloodcellSpawner>
	{ 
		[SerializeField] private GameObject bloodCellPrefab = null;
		[SerializeField] private List<Sprite> bloodCellSprites = new List<Sprite>();
		[SerializeField] private List<BloodcellPathAttributes> pathAttributes = new List<BloodcellPathAttributes>();
		[SerializeField][ReadOnly] private List<BloodCell> bloodcells = new List<BloodCell>();


		private void OnEnable()
		{
			GenerateBloodCells();
			GlobalLevelData.BloodCellSpawners.Add(this);
		}

		private void OnDisable()
		{
			GlobalLevelData.BloodCellSpawners.Remove(this);
		}

		public void GenerateBloodCells()
		{
			// Calculate cell counts
			int count = 0;
			for(int i = 0; i < pathAttributes.Count; ++i)
			{
				count += Mathf.FloorToInt(pathAttributes[i].pathCreator.path.length / pathAttributes[i].spawnGap);
			}
			bloodcells = new List<BloodCell>(count);

			// Spawn cells
			foreach (BloodcellPathAttributes pathAttribute in pathAttributes)
			{
				if (pathAttribute.pathCreator == null)
				{
					Debug.LogWarning("Unassigned path creator at index for Bloodcell Spawner (" + this + ")");
				}
				else
				{
					SpawnCellsForPath(pathAttribute);
				}
			}
		}

		private void SpawnCellsForPath(BloodcellPathAttributes pathAttribute)
		{
			// Get the path and its length
			VertexPath path = pathAttribute.pathCreator.path;

			float spawnDistance = pathAttribute.spawnOffset;
			float maxSpawnDistance = path.length - (pathAttribute.spawnGap - pathAttribute.spawnOffset);
			int numberOfCells = (int)(path.length / pathAttribute.spawnGap);
			float endDistance = numberOfCells * pathAttribute.spawnGap;

			while (spawnDistance < maxSpawnDistance)
			{
				BloodCell bc = Instantiate(bloodCellPrefab, gameObject.transform).GetComponent<BloodCell>();
				bc.Initialise(path, spawnDistance, endDistance, bloodCellSprites[Random.Range(0, bloodCellSprites.Count)]);
				bloodcells.Add(bc);
				spawnDistance += pathAttribute.spawnGap;
			}
		}

		public void OnFixedUpdate()
		{
			foreach (BloodCell cell in bloodcells)
			{
				cell.OnFixedUpdate();
			}
		}


		[System.Serializable]
		class BloodcellPathAttributes
		{
			public PathCreator pathCreator = null;
			public float spawnGap = 0.0f;
			public float spawnOffset = 0.0f;
		}
	}
}