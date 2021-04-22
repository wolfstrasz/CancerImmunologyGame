using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using PathCreation.Examples;
using ImmunotherapyGame;

namespace ImmunotherapyGame.Bloodflow
{
	public class BloodcellSpawner : Singleton<BloodcellSpawner>
	{ 
		[SerializeField] private GameObject bloodCellPrefab = null;
		[SerializeField] private List<Sprite> bloodCellSprites = new List<Sprite>();
		[SerializeField] private List<BloodcellPathAttributes> pathAttributes = new List<BloodcellPathAttributes>();
		[SerializeField][ReadOnly] private List<BloodCell> bloodcells = new List<BloodCell>();

		void Start()
		{
			Initialise();
		}

		void FixedUpdate()
		{
			OnFixedUpdate();
		}

		void Initialise()
		{
			int count = 0;
			for(int i = 0; i < pathAttributes.Count; ++i)
			{
				count += Mathf.FloorToInt(pathAttributes[i].pathCreator.path.length / pathAttributes[i].spawnGap);
			}

			bloodcells = new List<BloodCell>(count);

			foreach (BloodcellPathAttributes attribute in pathAttributes)
			{
				// Get the path and its length
				VertexPath path = attribute.pathCreator.path;

				float spawnDistance = attribute.spawnOffset;
				float maxSpawnDistance = path.length - (attribute.spawnGap - attribute.spawnOffset);
				int numberOfCells = (int)(path.length / attribute.spawnGap);
				float endDistance = numberOfCells * attribute.spawnGap;

				while (spawnDistance < maxSpawnDistance)
				{
					GameObject bcObj = Instantiate(bloodCellPrefab, gameObject.transform);
					BloodCell bc = bcObj.GetComponent<BloodCell>();

					bc.Initialise(path, spawnDistance, endDistance, bloodCellSprites[Random.Range(0, bloodCellSprites.Count)]);
					bloodcells.Add(bc);
					spawnDistance += attribute.spawnGap;
				}

			}
		}

		void OnFixedUpdate()
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