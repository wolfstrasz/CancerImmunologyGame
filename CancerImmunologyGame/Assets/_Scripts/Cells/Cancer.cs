using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace Cancers
{


	public class Cancer : MonoBehaviour
	{
		[Header("Prefabs Linking")]
		[SerializeField]
		private GameObject availablePointsPrefab = null;
		[SerializeField]
		private GameObject spawnPointPrefab = null;
		[SerializeField]
		private GameObject cancerCellPrefab = null;

		[Header("Generation attributes")]
		[SerializeField]
		private bool isAlive = true;
		[SerializeField]
		private int cellsToGenerate = 0;

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

		[Header("Debug Attributes")]
		[SerializeField]
		private bool debugPlotting = false;
		[SerializeField]
		private Transform debugPlotSpawn = null;

		[Header("Debug (ReadOnly)")]
		// Spawning
		[SerializeField]
		private int nextSortOrder = 0;
		[SerializeField]
		private float timePassed = 0.0f;

		// Debug containers
		[SerializeField]
		private List<GameObject> allPlottingObjects = new List<GameObject>();
		[SerializeField]
		private List<CancerCell> cancerCells = new List<CancerCell>();

		// Division handling
		[SerializeField]
		private CancerCell cellToDivide;
		[SerializeField]
		private Vector3 locationToSpawn;
		[SerializeField]
		private bool canDivide = false;
		[SerializeField]
		private List<Vector3> availableLocations = new List<Vector3>();
		[SerializeField]
		private Dictionary<Vector3, int> availableLocationsByDensity = new Dictionary<Vector3, int>();
		[SerializeField]
		private Dictionary<Vector3, CancerCell> spawnOwners = new Dictionary<Vector3, CancerCell>();

		// Listeners
		private List<ICancerDeathListener> deathListeners = new List<ICancerDeathListener>();

		public bool CanDivide => canDivide;
		public GameObject CellToDivide => cellToDivide.gameObject;

		// Start is called before the first frame update
		void Awake()
		{
			locationToSpawn = transform.position;

			// Generates the initial cancer
			AddNewCell();
			canDivide = true;

			bool old_debug = debugPlotting;
			debugPlotting = false;

			for (int i = 0; i < cellsToGenerate; ++i)
			{
				ResetDivisionProcess();
				FindAllSpotsAvailable();
				FindSpawnSpot();
				AddNewCell();
			}

			debugPlotting = old_debug;
		}

		public void OnUpdate()
		{
			if (!isAlive) return;

			if (cancerCells.Count >= maximumCells) return;

#if CANCER_DEBUG_DIVISION_PROCESS
			if (Input.GetKeyDown(KeyCode.Alpha0))
			{
				timepassed = timeBetweenDivisions;
			}
#else
			if (canDivide)
			{
				timePassed += Time.deltaTime;
				if (timePassed > timeBetweenDivisions)
				{
					canDivide = false;
					ResetDivisionProcess();
					FindAllSpotsAvailable();
					FindSpawnSpot();
					StartDivision();
				}
			}
#endif
		}

		/// <summary>
		/// Adds new cell at the determined locationToSpawn position
		/// </summary>
		private void AddNewCell()
		{
			CancerCell newCell = Instantiate(cancerCellPrefab, locationToSpawn, Quaternion.identity, this.transform).GetComponent<CancerCell>();
			newCell.cancer = this;
			newCell.RenderSortOrder = nextSortOrder;
			++nextSortOrder;
			cancerCells.Add(newCell);
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

		internal void OnFinishDivisionPreparation()
		{
			cellToDivide.StartDivision();
		}

		internal void OnFinishDivision()
		{
			// Request new cell to spawn and make the spawning cell return to its previous
			AddNewCell();
			cellToDivide.StartReturnFromDivision();
			Debug.Log("OnFinsishDivision");
			timePassed = 0.0f;
			canDivide = true;
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

		internal void RemoveCell(CancerCell cc)
		{
			cancerCells.Remove(cc);

			if (cancerCells.Count == 0)
			{
				// notify listeners
				for (int i = 0; i < deathListeners.Count; ++i)
				{
					deathListeners[i].OnCancerDeath();
				}

				isAlive = false;
				canDivide = false;

				//GlobalGameData.Cancers.Remove(this);

				//Destroy(gameObject);
				return;
			}
		}


		// Listener functionality
		public void SubscribeListener(ICancerDeathListener listener)
		{
			deathListeners.Add(listener);
		}

		public void UnsubscribeListener(ICancerDeathListener listener)
		{
			deathListeners.Remove(listener);
		}

	}
}