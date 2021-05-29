using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame.Cancers
{
	public class Cancer : MonoBehaviour, IEvilCellObserver
	{
		[Header("Prefabs Linking")]
		[SerializeField]
		private GameObject availablePointsPrefab = null;
		[SerializeField]
		private GameObject spawnPointPrefab = null;
		[SerializeField]
		private GameObject cancerCellPrefab = null;
		[SerializeField]
		private GameObject cafCellPrefab = null;

		[Header("Generation attributes")]
		[SerializeField]
		private bool isAlive = true;
		[SerializeField]
		private bool canSpawnCafs = false;
		[SerializeField]
		private int cellsToGenerate = 0;
		[SerializeField]
		private int spawnsBeforeCafSpawn = 5;
		[SerializeField]
		private int CAFBalanceRatio = 3;

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
		[SerializeField]
		private int spawns = 0;

		// Debug containers
		[SerializeField]
		private List<GameObject> allPlottingObjects = new List<GameObject>();
		[SerializeField]
		private List<CancerCell> cancerCells = new List<CancerCell>();
		[SerializeField]
		private List<CAFCell> cafCells = new List<CAFCell>();
		[SerializeField]
		public List<EvilCell> AllCells = new List<EvilCell>();

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
		private List<ICancerDeathObserver> onDeathObservers = new List<ICancerDeathObserver>();
		private List<ICancerDivisionObserver> onDivisionObservers = new List<ICancerDivisionObserver>();

		public bool IsAlive => isAlive;
		public bool CanDivide => canDivide;
		public GameObject CellToDivide => cellToDivide.gameObject;

		// Start is called before the first frame update
		void Start()
		{
			locationToSpawn = transform.position;

			// Generates the initial cancer
			AddNewCancerCell();
			canDivide = true;

			bool old_debug = debugPlotting;
			debugPlotting = false;

			while (cancerCells.Count < cellsToGenerate)
			{
				//Debug.Log(cancerCells.Count + " " + cellsToGenerate);

				if (canSpawnCafs && spawns == spawnsBeforeCafSpawn) // Cav cell
				{
					bool previousSetup = keepCellsPacked;
					keepCellsPacked = true;

					ResetDivisionProcess();
					FindAllSpotsAvailable();
					if (availableLocationsByDensity.Keys.Count < 0 && keepCellsPacked)
					{
						Debug.LogWarning("Update start setup prevents cancer from spawning cells");
						timePassed = 0.0f;
						canDivide = true;
						keepCellsPacked = previousSetup;
						cellsToGenerate--;
						continue;
					}
					else
					{
						if (!FindSpawnSpot()) continue;

						AddNewCAFCell();
						spawns = 0;
						timePassed = 0.0f;
						canDivide = true;
						keepCellsPacked = previousSetup;
						continue;
					}
				}
				else // CancerCell
				{
					ResetDivisionProcess();
					FindAllSpotsAvailable();
					if ((availableLocations.Count < 0 && !keepCellsPacked) || (availableLocationsByDensity.Keys.Count < 0 && keepCellsPacked))
					{
						Debug.LogWarning("Update start setup prevents cancer from spawning cells");
						timePassed = 0.0f;
						canDivide = true;
						cellsToGenerate--;

						continue;
					}
					else
					{
						if (!FindSpawnSpot()) continue;

						AddNewCancerCell();
						canDivide = true;
						if (CAFBalanceRatio * cafCells.Count <= cancerCells.Count)
							spawns++;


						continue;
					}

				}
			}

			debugPlotting = old_debug;
		}

		public void OnUpdate()
		{
	
			if (!isAlive) return;

			for (int i = 0; i < cafCells.Count; ++i)
			{
				cafCells[i].OnUpdate();
			}

			if (cancerCells.Count >= maximumCells) return;

#if CANCER_DEBUG_DIVISION_PROCESS
			
#else
#endif
			//if (Input.GetKeyDown(KeyCode.Alpha0))
			//{
			//	timePassed = timeBetweenDivisions;
			//}

			if (canDivide)
			{
				timePassed += Time.deltaTime;
				if (timePassed > timeBetweenDivisions)
				{

					canDivide = false;

					if (canSpawnCafs && spawns == spawnsBeforeCafSpawn) // Cav cell
					{
						bool previousSetup = keepCellsPacked;
						keepCellsPacked = true;

						ResetDivisionProcess();
						FindAllSpotsAvailable();
						if (availableLocationsByDensity.Keys.Count < 0 && keepCellsPacked)
						{
							Debug.Log("Update frame setup prevents cancer from spawning cells");
							timePassed = 0.0f;
							canDivide = true;
							keepCellsPacked = previousSetup;
							return;
						} else
						{
							if (!FindSpawnSpot()) return;
							spawns = 0;
							AddNewCAFCell();
							timePassed = 0.0f;
							canDivide = true;
							keepCellsPacked = previousSetup;
							return;
						}
					} 
					else // CancerCell
					{
						ResetDivisionProcess();
						FindAllSpotsAvailable();
						if ((availableLocations.Count < 0 && !keepCellsPacked) || (availableLocationsByDensity.Keys.Count < 0 && keepCellsPacked))
						{
							Debug.Log("Update frame setup prevents cancer from spawning cells");
							timePassed = 0.0f;
							canDivide = true;
							return;
						}
						else
						{
							if (!FindSpawnSpot()) return;
							StartDivision();
							if (CAFBalanceRatio * cafCells.Count <= cancerCells.Count)
								spawns++;
							return;
						}

					}
				}
			}

		
		}

		/// <summary>
		/// Adds new cell at the determined locationToSpawn position
		/// </summary>
		private void AddNewCancerCell()
		{
			CancerCell newCell = Instantiate(cancerCellPrefab, locationToSpawn, Quaternion.identity, this.transform).GetComponent<CancerCell>();
			newCell.AddObserver(this);
			newCell.RenderSortOrder = nextSortOrder;
			newCell.cancerOwner = this;
			++nextSortOrder;
			cancerCells.Add(newCell);
			AllCells.Add(newCell);
		}



		private void AddNewCAFCell()
		{
			CAFCell newCell = Instantiate(cafCellPrefab, locationToSpawn, Quaternion.identity, this.transform).GetComponent<CAFCell>();
			newCell.AddObserver(this);
			newCell.RenderSortOrder = nextSortOrder;
			newCell.cancerOwner = this;
			++nextSortOrder;
			cafCells.Add(newCell);
			AllCells.Add(newCell);
		}


		// Division process functionality 
		private void FindAllSpotsAvailable()
		{
			float distance = radius * 2.0f + offsetOfAnimation;
			for (int i = 0; i < cancerCells.Count; i++)
			{
				if (cancerCells[i].isImmune)
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

		private bool FindSpawnSpot()
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
			canDivide = true;

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

		// Subscriber Evil cells
		public void NotifyOfDeath(EvilCell evilCell)
		{

			AllCells.Remove(evilCell);

			CancerCell cell = evilCell.gameObject.GetComponent<CancerCell>();
			if (cell != null)
			{
				cancerCells.Remove(cell);
			}

			CAFCell cafCell = evilCell.gameObject.GetComponent<CAFCell>();

			if (cafCell != null)
			{
				cafCells.Remove(cafCell);
			}

			if (cancerCells.Count == 0)
			{
				// notify listeners
				for (int i = 0; i < onDeathObservers.Count; ++i)
				{
					onDeathObservers[i].OnCancerDeath(this);
				}

				isAlive = false;
				canDivide = false;
				gameObject.SetActive(false);
				//GlobalGameData.Cancers.Remove(this);
				//Destroy(gameObject);
				return;
			}
		}
	}
}