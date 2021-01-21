using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using PathCreation.Examples;

namespace BloodcellAnimation
{
	public class BloodcellSpawner : MonoBehaviour
	{
		[Header("Functional Linking")]
		[SerializeField]
		private GameObject bloodCellPrefab;
		[SerializeField]
		private List<BloodcellPathAttributes> pathAttributes = new List<BloodcellPathAttributes>();

		private List<BloodCell> bloodcells = new List<BloodCell>();
		int index = 0;


		void Awake()
		{
			foreach (BloodCell cell in bloodcells)
			{
				Destroy(cell.gameObject);
			}

			bloodcells = new List<BloodCell>();

			foreach (BloodcellPathAttributes attribute in pathAttributes)
			{
				// Get the path and its length
				VertexPath path = attribute.pathCreator.path;

				float spawnDistance = attribute.spawnOffset;
				Debug.Log("For path: ------------------");
				float maxSpawnDistance = path.length - (attribute.spawnGap - attribute.spawnOffset);
				Debug.Log("Path lenght = " + path.length);
				int numberOfCells = (int)(path.length / attribute.spawnGap);
				Debug.Log("Cells = " + numberOfCells);

				float endDistance = numberOfCells * attribute.spawnGap;
				Debug.Log("End Distance = " + endDistance);

				while (spawnDistance < maxSpawnDistance)
				{
					GameObject bcObj = Instantiate(bloodCellPrefab, gameObject.transform);
					BloodCell bc = bcObj.GetComponent<BloodCell>();

					Debug.Log("Spawn position: " + spawnDistance);
					bc.SetData(path, spawnDistance, endDistance);
					spawnDistance += attribute.spawnGap;
				}

				// Start spawning bloodcells on given distances


			}
		}

		[System.Serializable]
		struct BloodcellPathAttributes
		{
			public PathCreator pathCreator;
			public float spawnGap;
			public float spawnOffset;
		}
	}
}