using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cancer : MonoBehaviour
{

	// Links to cells
	[SerializeField]
	private List<CancerCell> cancerCells = new List<CancerCell>();

	private List<Vector3> availableLocations = new List<Vector3>();
	private Dictionary<Vector3, int> availableLocationDensity = new Dictionary<Vector3, int>();
	private Dictionary<Vector3, CancerCell> spawnOwners = new Dictionary<Vector3, CancerCell>();
	private int nextSortOrder = 0;

	public CancerCell cellToDivide;
	public Vector3 locationToSpawn;

	[Header("Spawn Attributes")]
	[SerializeField]
	private float radius;
	[SerializeField]
	private float radiusThinner;
	[SerializeField]
	private float angleRotation = 60.0f;
	[SerializeField]
	private bool packed = false;
	[SerializeField]
	private float offsetOfAnimation = 0.2f;
	[Header("Debug Attributes")]
	[SerializeField]
	private bool debugPlotting = false;
	[SerializeField]
	private Transform debugPlotSpawn = null;

	[Header("Prefabs")]
	[SerializeField]
	private GameObject availablePointsPrefab = null;
	[SerializeField]
	private GameObject spawnPointPrefab = null;
	[SerializeField]
	private GameObject cancerCellPrefab = null;


	// Debug containers
	private List<GameObject> allPlottingObjects = new List<GameObject>();


	// Start is called before the first frame update
	void Awake()
	{
		foreach (var cell in cancerCells)
		{
			cell.cancer = this;
			cell.SetSortOrder(nextSortOrder);
			++nextSortOrder;
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha0))
		{
			FullDivisionProcess();
		}
	}
	void FindAllSpotsAvailable()
	{
		float distance = radius * 2.0f + offsetOfAnimation;
		for (int i = 0; i < cancerCells.Count; i++)
		{
			CancerCell cell = cancerCells[i];
			Vector3 circleCastDirection = Vector3.right;

			float numberOfRotations = 360.0f / angleRotation;
			//Debug.Log(numberOfRotations + " Checking for: " + cell.gameObject.name);
			while (numberOfRotations > 0)
			{

				// Cast in next direction
				circleCastDirection = Quaternion.Euler(0.0f, 0.0f, angleRotation) * circleCastDirection;
				Vector3 nextSpawnLocation = cell.transform.position + (circleCastDirection * distance);
				var hits = Physics2D.CircleCastAll(cell.transform.position, radius - radiusThinner, circleCastDirection, distance);
				//Debug.Log(circleCastDirection);

				// Check for blockers
				List<GameObject> blockers = new List<GameObject>();
				foreach (var hit in hits)
				{
					if (hit.collider.gameObject.tag == "BlocksCancerSpawns")
					{
						Debug.Log(hit.collider.gameObject.name);
						blockers.Add(hit.collider.gameObject);
					}
				}

				if (blockers.Count <= 1)
				{
					nextSpawnLocation = cell.transform.position + (circleCastDirection * distance);
					availableLocations.Add(nextSpawnLocation);
					if (!spawnOwners.ContainsKey(nextSpawnLocation))
					{
						spawnOwners.Add(nextSpawnLocation, cell);
					}

				}
				numberOfRotations--;
			}

		}

		if (debugPlotting)
			for (int i = 0; i < availableLocations.Count; i++)
			{
				allPlottingObjects.Add(Instantiate(availablePointsPrefab, availableLocations[i], Quaternion.identity, debugPlotSpawn));
			}
	}

	void FindSpawnSpot()
	{
		// Packed algorithm to make cancer be more packed
		if (packed)
		{
			int max = 0;
			foreach (var key in availableLocationDensity.Keys)
			{
				if (availableLocationDensity[key] > max)
				{
					max = availableLocationDensity[key];
					locationToSpawn = key;
				}
			}
		}
		else // Choose a random point to spawn next cell
		{
			int index = (int)UnityEngine.Random.Range(0.0f, availableLocations.Count - 1);
			locationToSpawn = availableLocations[index];
			//Debug.Log("Location to spawn: " + "( " + index + " ) " + locationToSpawn);

		}

		// Instantiate plot choice dot
		if (debugPlotting)
			allPlottingObjects.Add(Instantiate(spawnPointPrefab, locationToSpawn, Quaternion.identity, debugPlotSpawn));
	}

	void StartDivision()
	{
		Debug.Log("Division preparation");
		Debug.Log("Spawn location: " + locationToSpawn);

		cellToDivide = spawnOwners[locationToSpawn];

		// Find the end rotation for the cancer cell division process
		Vector3 diff = locationToSpawn - cellToDivide.transform.position;
		diff.Normalize();
		float spawnRotationAngle = ((Mathf.Atan2(diff.y, diff.x)) * Mathf.Rad2Deg);


		cellToDivide.StartPrepareDivision(locationToSpawn, spawnRotationAngle);
	}

	void ResetDivisionProcess()
	{
		availableLocations.Clear();
		availableLocationDensity.Clear();
		while (allPlottingObjects.Count > 0)
		{
			var plotOBj = allPlottingObjects[0];
			allPlottingObjects.RemoveAt(0);
			Destroy(plotOBj);
		}
		allPlottingObjects.Clear();
		spawnOwners.Clear();
	}

	// CALLBACKS FROM THE CELL
	internal void OnFinishDivisionPreparation()
	{
		cellToDivide.StartDivision();
	}

	internal void OnFinishDivision()
	{
		// Request new cell to spawn and make the spawning cell return to its previous
		CancerCell newCell = Instantiate(cancerCellPrefab, locationToSpawn, Quaternion.identity, this.transform).GetComponent<CancerCell>();
		newCell.cancer = this;
		newCell.SetSortOrder(nextSortOrder);
		++nextSortOrder;
		cancerCells.Add(newCell);

		cellToDivide.StartReturnFromDivision();
	}

	public void FullDivisionProcess()
	{
		ResetDivisionProcess();
		FindAllSpotsAvailable();
		FindSpawnSpot();
		StartDivision();

	}
}
