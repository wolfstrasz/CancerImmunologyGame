using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame.Cancers
{
	public class Cancer : MonoBehaviour
	{
		[Header("Spawning")]
		[SerializeField] private GameObject cancerCellPrefab = null;
		[SerializeField] private GameObject cafCellPrefab = null;

		[SerializeField] private bool isAlive = true;
		[SerializeField] private bool canSpawnCells = true;
		[SerializeField] private bool canSpawnCafCells = false;
		[SerializeField] private bool shouldKeepCancerCellsPacked = false;

		[SerializeField] private int maximumCells = 10;
		[SerializeField] [Range(5.0f, 100.0f)] private float timeBetweenDivisions = 10.0f;
		[SerializeField] private int spawnsBeforeCafSpawn = 5;
		[SerializeField] private int balanceRatioCellToCaf = 3;

		[Header ("Spawning to division animation fixes")]
		[SerializeField] private float radius = 0.0f;
		[SerializeField] private float radiusThinner = 0.0f;
		[SerializeField] private float angleRotation = 60.0f;
		[SerializeField] private float offsetOfAnimation = 0.2f;

		[Header("Initial Generation")]
		[SerializeField] private int cellsToGenerate = 0;

		[Header("Debug Visualisation")]
		[SerializeField] private bool debugPlotting = false;
		[SerializeField] private GameObject availablePointsPrefab = null;
		[SerializeField] private GameObject spawnPointPrefab = null;
		[SerializeField] private Transform debugPlotSpawn = null;

		[Header("Debug")]
		[SerializeField] [ReadOnly] private int nextSortOrder = 0;
		[SerializeField] [ReadOnly] private float timePassed = 0.0f;
		[SerializeField] [ReadOnly] private int cancerCellContinuousSpawns = 0;

		// Debug containers
		[SerializeField] [ReadOnly] private List<CAFCell> cafCells = new List<CAFCell>();
		[SerializeField] [ReadOnly] private List<CancerCell> cancerCells = new List<CancerCell>();
		[SerializeField] [ReadOnly] private List<GameObject> allPlottingObjects = new List<GameObject>();

		// Division handling
		[SerializeField] [ReadOnly] private CancerCell cellToDivide;
		[SerializeField] [ReadOnly] private Vector3 locationToSpawn;
		[SerializeField] [ReadOnly] private bool canGoInDivision = false;
		[SerializeField] [ReadOnly] private List<Vector3> availableLocations = new List<Vector3>();

		private Dictionary<Vector3, int> availableLocationsByDensity = new Dictionary<Vector3, int>();
		private Dictionary<Vector3, CancerCell> spawnOwners = new Dictionary<Vector3, CancerCell>();

		// Listeners
		private List<ICancerDeathObserver> onDeathObservers = new List<ICancerDeathObserver>();
		private List<ICancerDivisionObserver> onDivisionObservers = new List<ICancerDivisionObserver>();

		// Attributes
		public bool IsAlive => isAlive;
		public bool CanGoInDivision => canGoInDivision;
		public GameObject CellToDivide => cellToDivide.gameObject;

		// Start is called before the first frame update
		void Start()
		{
			if (!isAlive) return;

			// Generates the first cancer cell
			locationToSpawn = transform.position;
			AddNewCancerCell();
			canGoInDivision = true;

			// Cancel any debug plotting for initial generation
			bool old_debug = debugPlotting;
			debugPlotting = false;

			GenerateInitialCells();
						
			// Return previous value
			debugPlotting = old_debug;

			// Check if any other divison is allowed
			canGoInDivision = canSpawnCells;
			timePassed = 0.0f;
		}


		public void GenerateInitialCells()
		{
			for (int generatedCells = 1; generatedCells < cellsToGenerate; ++generatedCells)
			{
				ResetDivisionProcess();
				Spawn(true);
			}
		}

		public void OnUpdate()
		{
			if (!isAlive)
			{
				return;
			}

			for (int i = 0; i < cafCells.Count; ++i)
			{
				cafCells[i].OnUpdate();
			}

			if (cancerCells.Count + cafCells.Count >= maximumCells)
			{
				return;
			}

			if (canGoInDivision)
			{
				timePassed += Time.deltaTime;
				if (timePassed > timeBetweenDivisions)
				{
					timePassed = 0.0f;
					canGoInDivision = false;
					Spawn(false);
				}
			}

		}

		private void Spawn(bool shouldQuickSpawn)
		{
			ResetDivisionProcess();

			if (canSpawnCafCells && cancerCellContinuousSpawns == spawnsBeforeCafSpawn)
			{
				// CAF spawning is a quick Spawn so definitely can go in division
				// doesn't matter if successful spawn or not
				canGoInDivision = true;

				// Caff Cell spawning should always be packed!
				if (!FindSpawnSpot(true))
				{
					return;
				}

				AddNewCAFCell();

				// Reset counter for Cancer Cells
				cancerCellContinuousSpawns = 0;
			}
			else
			{
				if (!FindSpawnSpot(shouldKeepCancerCellsPacked))
				{
					canGoInDivision = true;
					return;
				}
				else if (shouldQuickSpawn) // Cancer Cell without division animation
				{
					AddNewCancerCell();
					// Quick spawn => can go instantly into another division
					canGoInDivision = true;
				}
				else // CancerCell with division animation
				{
					StartCancerCellDivision();
				}

				// If successfully spawned / started spawning increase counter
				cancerCellContinuousSpawns++;
			}
			
		}


	
		/// <summary>
		/// Adds new cell at the determined locationToSpawn position
		/// </summary>
		private void AddNewCancerCell()
		{
			CancerCell newCancerCell = Instantiate(cancerCellPrefab, locationToSpawn, Quaternion.identity, this.transform).GetComponent<CancerCell>();
			newCancerCell.onDeathEvent += OnCancerCellDeath;
			newCancerCell.RenderSortOrder = nextSortOrder;
			newCancerCell.cancerOwner = this;
			++nextSortOrder;
			cancerCells.Add(newCancerCell);
		}



		private void AddNewCAFCell()
		{
			CAFCell newCAFCell = Instantiate(cafCellPrefab, locationToSpawn, Quaternion.identity, this.transform).GetComponent<CAFCell>();
			newCAFCell.onDeathEvent += OnCAFCellDeath;
			newCAFCell.RenderSortOrder = nextSortOrder;
			newCAFCell.cancerOwner = this;
			++nextSortOrder;
			cafCells.Add(newCAFCell);
		}


		// Division process functionality 
		private bool FindAllSpotsAvailable(bool shouldKeepPacked)
		{
			bool hasFoundAtLeastOneAvailableSpot = false;
			float distance = radius * 2.0f + offsetOfAnimation;
			for (int i = 0; i < cancerCells.Count; i++)
			{
				// Skip non-idle cancer cells
				if (cancerCells[i].isImmune)
				{
					continue;
				}
				CancerCell cell = cancerCells[i];

				Vector3 circleCastDirection = Vector3.right;
				float numberOfRotations = 360.0f / angleRotation;
				while (numberOfRotations > 0)
				{

					// Cast in next direction
					circleCastDirection = Quaternion.Euler(0.0f, 0.0f, angleRotation) * circleCastDirection;
					Vector3 nextSpawnLocation = cell.transform.position + (circleCastDirection * distance);
					var hits = Physics2D.CircleCastAll(cell.transform.position, radius - radiusThinner, circleCastDirection, distance);

					// Check for blockers
					List<GameObject> blockers = new List<GameObject>();
					foreach (var hit in hits)
					{
						if (hit.collider.gameObject.tag == "BlocksCancerSpawns")
						{
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


						if (shouldKeepPacked)
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

						hasFoundAtLeastOneAvailableSpot = true;
					}
					numberOfRotations--;
				}

			}

			if (debugPlotting)
			{
				if (shouldKeepPacked)
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

			return hasFoundAtLeastOneAvailableSpot;
		}

		private bool FindSpawnSpot(bool shouldKeepPacked)
		{
			// Find all available spots and if non exists return skip search
			if (!FindAllSpotsAvailable(shouldKeepPacked))
			{
				return false;
			}

			// Packed algorithm to make cancer be more packed
			if (shouldKeepPacked)
			{
				int max = 0;
				List<Vector3> possibleLocationsToSpawn = new List<Vector3>();
				foreach (var key in availableLocationsByDensity.Keys)
				{
					if (availableLocationsByDensity[key] > max)
					{
						max = availableLocationsByDensity[key];
						//Debug.Log("Found new max: " + max);
					}
				}

				foreach (var key in availableLocationsByDensity.Keys)
				{
					if (availableLocationsByDensity[key] == max)
					{
						//Debug.Log("Found item with value(" + availableLocationsByDensity[key] + ") == " + max);
						possibleLocationsToSpawn.Add(key);
					}
				}

				if (possibleLocationsToSpawn.Count > 0)
				{
					int index = (int)UnityEngine.Random.Range(0, possibleLocationsToSpawn.Count);
					locationToSpawn = possibleLocationsToSpawn[index];

					// Instantiate plot choice dot
					if (debugPlotting)
						allPlottingObjects.Add(Instantiate(spawnPointPrefab, locationToSpawn, Quaternion.identity, debugPlotSpawn));

					return true;
				} else
				{
					locationToSpawn = Vector3.zero;
					return false;
				}

				//Debug.Log(possibleLocationsToSpawn.Count);
				//Debug.Log("index = " + index);
			}
			else // Choose a random point to spawn next cell
			{

				if (availableLocations.Count > 0)
				{
					int index = (int)UnityEngine.Random.Range(0, availableLocations.Count);
					locationToSpawn = availableLocations[index];

					// Instantiate plot choice dot
					if (debugPlotting)
						allPlottingObjects.Add(Instantiate(spawnPointPrefab, locationToSpawn, Quaternion.identity, debugPlotSpawn));

					return true;
				}
				else
				{
					locationToSpawn = Vector3.zero;
					return false;
				}

				//Debug.Log("Location to spawn: " + "( " + index + " ) " + locationToSpawn);

			}

	
		}

		private void StartCancerCellDivision()
		{
			//Debug.Log("Division preparation");
			//Debug.Log("Spawn location: " + locationToSpawn);

			cellToDivide = spawnOwners[locationToSpawn];

			// Find the end rotation for the cancer cell division process
			Vector3 diff = locationToSpawn - cellToDivide.transform.position;
			diff.Normalize();
			float spawnRotationAngle = ((Mathf.Atan2(diff.y, diff.x)) * Mathf.Rad2Deg);


			cellToDivide.StartPrepareDivision(spawnRotationAngle);

			foreach (ICancerDivisionObserver observer in onDivisionObservers)
			{
				observer.OnDivisionStart(this);
			}
		}

		internal void OnFinishDivisionPreparation()
		{
			cellToDivide.StartDivision();
		}

		internal void OnFinishDivision()
		{
			// Request new cell to spawn and make the spawning cell return to its previous
			AddNewCancerCell();
			cellToDivide.StartReturnFromDivision();
			Debug.Log("OnFinsishDivision");
			timePassed = 0.0f;
			canGoInDivision = true;

			foreach (ICancerDivisionObserver observer in onDivisionObservers)
			{
				observer.OnDivisionEnd(this);
			}
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

		// Listener functionality
		public void SubscribeDeathObserver(ICancerDeathObserver observer)
		{
			if (!onDeathObservers.Contains(observer))
				onDeathObservers.Add(observer);
		}

		public void UnsubscribeDeathObserver(ICancerDeathObserver observer)
		{
			if (onDeathObservers.Contains(observer))
				onDeathObservers.Remove(observer);
		}

		public void SubscribeDivisionObserver(ICancerDivisionObserver observer)
		{
			if (!onDivisionObservers.Contains(observer))
				onDivisionObservers.Add(observer);
		}

		public void UnsubscribeDivisionObserver(ICancerDivisionObserver observer)
		{
			if (onDivisionObservers.Contains(observer))
				onDivisionObservers.Remove(observer);
		}

		public void OnCancerCellDeath(Cell cell)
		{
			CancerCell cancerCell = cell.GetComponent<CancerCell>();
			cancerCells.Remove(cancerCell);

			if (cancerCells.Count == 0)
			{
				// notify listeners
				for (int i = 0; i < onDeathObservers.Count; ++i)
				{
					onDeathObservers[i].OnCancerDeath(this);
				}

				isAlive = false;
				canGoInDivision = false;
				gameObject.SetActive(false);
			}
		}

		public void OnCAFCellDeath(Cell cell)
		{
			CAFCell cafCell = cell.GetComponent<CAFCell>();
			cafCells.Remove(cafCell);
		}
	}
}