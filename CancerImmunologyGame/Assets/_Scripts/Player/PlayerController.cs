using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cells.Cancers;

namespace ImmunotherapyGame.Player
{
	public class PlayerController : Singleton<PlayerController> , ICellController, ICancerDeathObserver
	{
		[SerializeField]
		private KillerCell kc = null;

		[Header("Range functionality")]
		[SerializeField]
		private PlayerRangeDisplay rangeDisplay = null;
		private bool canAttack = false;

		[Header("Debug (Read Only)")]
		[SerializeField]
		private Vector2 movementVector = Vector2.zero;
		[SerializeField]
		private bool areControlsEnabled = true;
		[SerializeField]
		private List<IPlayerObserver> observers = new List<IPlayerObserver>();

		[SerializeField]
		private List<Cancer> cancersNearby = new List<Cancer>();

		public KillerCell KC => kc;

		public void Initialise()
		{
			PlayerUI.Instance.Initialise();
			PlayerUI.Instance.SetPlayerInfo(kc);
			kc.Sense.controller = this;
			kc.controller = this;
			transform.position = kc.transform.position;
			rangeDisplay.Initialise(kc);
		}


		void OnTriggerEnter2D(Collider2D collider)
		{
			Cancer cancer = collider.GetComponent<Cancer>();
			if (cancer != null && !cancersNearby.Contains(cancer))
			{
				cancersNearby.Add(cancer);
				BackgroundMusic.Instance.PlayBattleMusic();
				cancer.SubscribeDeathObserver(this);
			}
		}

		void OnTriggerExit2D(Collider2D collider)
		{
			Cancer cancer = collider.GetComponent<Cancer>();
			if (cancer != null && cancersNearby.Contains(cancer))
			{
				cancersNearby.Remove(cancer);
				cancer.UnsubscribeDeathObserver(this);

				if (cancersNearby.Count <= 0)
				{
					BackgroundMusic.Instance.PlayNormalMusic();
				}
			}
		}

		// input
		public void OnUpdate()
		{
			// TODO: manage it better
			transform.position = kc.transform.position;
			transform.rotation = kc.transform.rotation;

			PlayerUI.Instance.OnUpdate();
			rangeDisplay.OnUpdate();

#if PLAYER_DEBUG
			DebugInput();
#endif

			kc.SpriteOrientation = rangeDisplay.orientation;

			if (canAttack)
			{
				if (Input.GetKey(KeyCode.Mouse0))
				{
					kc.Attack(rangeDisplay.centre.position);
				}
			}
			else
			{
				kc.StopAttack();
			}
		}

		public void OnFixedUpdate()
		{
			if (!areControlsEnabled)
			{
				kc.MovementVector = Vector2.zero;
				return;
			}

			// Collect input 
			movementVector.x = Input.GetAxisRaw("Horizontal");
			movementVector.y = Input.GetAxisRaw("Vertical");

			// Damping if both axis are pressed. sqare root of 2.
			if (Mathf.Abs(movementVector.x) == 1 && Mathf.Abs(movementVector.y) == 1)
			{
				movementVector = movementVector * 0.74f;
			}

			kc.MovementVector = movementVector;
		}

		// Subscribtions
		public void OnEnemiesInRange()
		{
			rangeDisplay.gameObject.SetActive(true);
			canAttack = true;
		}

		public void OnEnemiesOutOfRange()
		{
			rangeDisplay.gameObject.SetActive(false);
			canAttack = false;
		}

		public void OnCancerDeath(Cancer cancer)
		{

			if (cancersNearby.Contains(cancer))
			{
				cancersNearby.Remove(cancer);

				if (cancersNearby.Count <= 0)
					BackgroundMusic.Instance.PlayNormalMusic();

			}
		}

		public void OnCellDeath()
		{ 
			// Find closest spawn location
			List < PlayerRespawnArea > respawnLocations = GlobalGameData.RespawnAreas;
			Vector3 closestRespawnLocation = respawnLocations[0].transform.position;
			float minDistance = Vector3.Distance(transform.position, respawnLocations[0].Location);

			foreach (var area in respawnLocations)
			{
				float distance = Vector3.Distance(transform.position, area.Location);
				if (distance <= minDistance)
				{
					minDistance = distance;
					closestRespawnLocation = area.Location;
				}
			}

			kc.AddHealth(kc.maxHealth);
			kc.AddEnergy(kc.maxEnergy);
			kc.gameObject.transform.position = closestRespawnLocation;
			gameObject.transform.position = closestRespawnLocation;

			for (int i = 0; i < observers.Count; ++i)
			{
				observers[i].OnPlayerDeath();
			}
		}

		// Observers
		public void SubscribeObserver(IPlayerObserver observer)
		{
			observers.Add(observer);
		}

		public void UnsubscribeObserver(IPlayerObserver observer)
		{
			observers.Remove(observer);
		}

#if PLAYER_DEBUG
		private void DebugInput()
		{
			if (Input.GetKeyDown(KeyCode.Keypad7))
			{
				kc.AddHealth(-20.0f);
			}
			if (Input.GetKeyDown(KeyCode.Keypad9))
			{
				kc.AddHealth(+20.0f);
			}
			if (Input.GetKeyDown(KeyCode.Keypad4))
			{
				kc.AddEnergy(-20.0f);
			}
			if (Input.GetKeyDown(KeyCode.Keypad6))
			{
				kc.AddEnergy(+20.0f);
			}
		}
#endif
	}
}