using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cancers
{


	public class Cancer : MonoBehaviour
	{

		// Links to cells
		[SerializeField]
		private List<CancerCell> cancerCells = new List<CancerCell>();

		private List<Vector3> availableLocations = new List<Vector3>();
		private Dictionary<Vector3, int> availableLocationsByDensity = new Dictionary<Vector3, int>();
		private Dictionary<Vector3, CancerCell> spawnOwners = new Dictionary<Vector3, CancerCell>();
		private int nextSortOrder = 0;


		[Header("Prefabs Linking")]
		[SerializeField]
		private GameObject availablePointsPrefab = null;
		[SerializeField]
		private GameObject spawnPointPrefab = null;
		[SerializeField]
		private GameObject cancerCellPrefab = null;

		[Header("Spawn Attributes")]
		[SerializeField]
		private int maximumCells = 10;
		[SerializeField]
		private float radius = 0.0f;
		[SerializeField]
		private float radiusThinner = 0.0f;
		[SerializeField]
		private float angleRotation = 60.0f;
		[SerializeField]
		private float offsetOfAnimation = 0.2f;
		[SerializeField]
		private bool keepCellsPacked = false;
		[SerializeField]
		[Range(5.0f, 100.0f)]
		private float timeBetweenDivisions = 10.0f;
		[Header("Generation attributes")]
		[SerializeField]
		private bool shouldGenerateCancer = false;
		[SerializeField]
		private int cellsToGenerate = 0;

		[Header("Debug Attributes")]
		[SerializeField]
		private bool debugPlotting = false;
		[SerializeField]
		private Transform debugPlotSpawn = null;

		[Header("Debug (ReadOnly)")]
		[SerializeField]
		private float timePassed = 0.0f;
		[SerializeField]
		// Debug containers
		private List<GameObject> allPlottingObjects = new List<GameObject>();
		[SerializeField]
		private bool isInitialised = false;
		// Division handling
		[SerializeField]
		private CancerCell cellToDivide;
		[SerializeField]
		private Vector3 locationToSpawn;
		[SerializeField]
		private bool canDivide = false;

		public bool CanDivide => canDivide;
		public GameObject CellToDivide => cellToDivide.gameObject;

		// Start is called before the first frame update
		void Awake()
		{
			if (cancerCells.Count == 0)
			{
				cancerCells.Add(
					Instantiate(cancerCellPrefab, this.transform)
					.GetComponent<CancerCell>()
					);
			}

			foreach (var cell in cancerCells)
			{
				cell.cancer = this;
				cell.RenderSortOrder = nextSortOrder;
				++nextSortOrder;
			}
			canDivide = true;

			if (shouldGenerateCancer)
			{
				GenerateCancer();
			}
		}

		public void OnUpdate()
		{
			if (!isInitialised) return;

			if (cancerCells.Count == 0)
			{
				canDivide = false;
				GlobalGameData.Cancers.Remove(this);
				Destroy(gameObject);
				return;
			}
			//if (Input.GetKeyDown(KeyCode.Alpha0))
			//{
			//	FullDivisionProcess();
			//}

			if (cancerCells.Count >= maximumCells) return;

			if (!GlobalGameData.isGameplayPaused && canDivide)
			{
				timePassed += Time.deltaTime;
				if (timePassed > timeBetweenDivisions)
				{
					FullDivisionProcess();
				}

			}

		}

		public void OnTriggerEnter2D(Collider2D collider)
		{
			if (collider.gameObject == GlobalGameData.player)
			{
				isInitialised = true;
			}
		}

		// Division process functionality 

		private void FindAllSpotsAvailable()
		{
			float distance = radius * 2.0f + offsetOfAnimation;
			for (int i = 0; i < cancerCells.Count; i++)
			{
				if (cancerCells[i].isDying)
				{
					continue;
				}
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
							//Debug.Log(hit.collider.gameObject.name);
							blockers.Add(hit.collider.gameObject);
						}

						if (blockers.Count >= 2)
						{
							break;
						}
					}

					if (blockers.Count <= 1)
					{
						nextSpawnLocation = cell.transform.position + (circleCastDirection * distance);
						// Clamp the floating parts to 3 decimal places
						nextSpawnLocation.x = Mathf.Round(nextSpawnLocation.x * 1000.0f) / 1000.0f;
						nextSpawnLocation.y = Mathf.Round(nextSpawnLocation.y * 1000.0f) / 1000.0f;

						if (keepCellsPacked)
						{
							if (availableLocationsByDensity.ContainsKey(nextSpawnLocation))
							{
								++availableLocationsByDensity[nextSpawnLocation];
								//Debug.Log("New location with density: " + availableLocationsByDensity[nextSpawnLocation]);
							}
							else
							{
								//Debug.Log("Adding to dictionary: " + nextSpawnLocation);
								availableLocationsByDensity.Add(nextSpawnLocation, 1);

								if (!spawnOwners.ContainsKey(nextSpawnLocation))
								{
									spawnOwners.Add(nextSpawnLocation, cell);
								}
							}
						}
						else
						{
							availableLocations.Add(nextSpawnLocation);
							if (!spawnOwners.ContainsKey(nextSpawnLocation))
							{
								spawnOwners.Add(nextSpawnLocation, cell);
							}
						}

					}
					numberOfRotations--;
				}

			}

			if (debugPlotting)
			{
				if (keepCellsPacked)
				{
					foreach (var location in availableLocationsByDensity.Keys)
					{
						allPlottingObjects.Add(Instantiate(availablePointsPrefab, location, Quaternion.identity, debugPlotSpawn));
					}
				}
				else
				{
					foreach (var location in availableLocations)
					{
						allPlottingObjects.Add(Instantiate(availablePointsPrefab, location, Quaternion.identity, debugPlotSpawn));
					}
				}
			}
		}

		private void FindSpawnSpot()
		{
			// Packed algorithm to make cancer be more packed
			if (keepCellsPacked)
			{
				int max = 0;
				List<Vector3> possibleLocationsToSpawn = new List<Vector3>();
				foreach (var key in availableLocationsByDensity.Keys)
				{
					if (availableLocationsByDensity[key] > max)
					{
						max = availableLocationsByDensity[key];
						Debug.Log("Found new max: " + max);
					}
				}

				foreach (var key in availableLocationsByDensity.Keys)
				{
					if (availableLocationsByDensity[key] == max)
					{
						Debug.Log("Found item with value(" + availableLocationsByDensity[key] + ") == " + max);
						possibleLocationsToSpawn.Add(key);
					}
				}


				int index = (int)UnityEngine.Random.Range(0.0f, possibleLocationsToSpawn.Count - 1);
				//Debug.Log(possibleLocationsToSpawn.Count);
				//Debug.Log("index = " + index);
				locationToSpawn = possibleLocationsToSpawn[index];
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

		private void StartDivision()
		{
			//Debug.Log("Division preparation");
			//Debug.Log("Spawn location: " + locationToSpawn);

			cellToDivide = spawnOwners[locationToSpawn];

			// Find the end rotation for the cancer cell division process
			Vector3 diff = locationToSpawn - cellToDivide.transform.position;
			diff.Normalize();
			float spawnRotationAngle = ((Mathf.Atan2(diff.y, diff.x)) * Mathf.Rad2Deg);


			cellToDivide.StartPrepareDivision(spawnRotationAngle);
		}

		private void ResetDivisionProcess()
		{
			availableLocations.Clear();
			availableLocationsByDensity.Clear();
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
		internal void RemoveCell(CancerCell cc)
		{
			cancerCells.Remove(cc);
		}

		internal void OnFinishDivisionPreparation()
		{
			cellToDivide.StartDivision();
		}

		internal void OnFinishDivision()
		{
			// Request new cell to spawn and make the spawning cell return to its previous
			CancerCell newCell = Instantiate(cancerCellPrefab, locationToSpawn, Quaternion.identity, this.transform).GetComponent<CancerCell>();
			newCell.cancer = this;
			newCell.RenderSortOrder = nextSortOrder;
			++nextSortOrder;
			cancerCells.Add(newCell);

			cellToDivide.StartReturnFromDivision();
			Debug.Log("OnFinsishDivision");
			timePassed = 0.0f;
			canDivide = true;
		}

		private void FullDivisionProcess()
		{
			canDivide = false;
			ResetDivisionProcess();
			FindAllSpotsAvailable();
			FindSpawnSpot();
			StartDivision();
		}

		private void GenerateCancer()
		{
			bool old_debug = debugPlotting;
			debugPlotting = false;

			for (int i = 0; i < cellsToGenerate; ++i)
			{
				ResetDivisionProcess();
				FindAllSpotsAvailable();
				FindSpawnSpot();

				// Request new cell to spawn and make the spawning cell return to its previous
				CancerCell newCell = Instantiate(cancerCellPrefab, locationToSpawn, Quaternion.identity, this.transform).GetComponent<CancerCell>();
				newCell.cancer = this;
				newCell.RenderSortOrder = nextSortOrder;
				++nextSortOrder;
				cancerCells.Add(newCell);
			}
			debugPlotting = old_debug;
		}
	}
}