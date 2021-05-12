using UnityEngine;
using UnityEngine.InputSystem;

using System.Collections.Generic;
using ImmunotherapyGame.Cancers;
using ImmunotherapyGame.Core;
using ImmunotherapyGame.Audio;

namespace ImmunotherapyGame.Player
{
	public class PlayerController : Singleton<PlayerController> , ICellController, ICancerDeathObserver, IControllerMovementOverridable
	{
		[SerializeField]
		private KillerCell kc = null;

		[Header("Range functionality")]
		[SerializeField]
		private PlayerRangeDisplay rangeDisplay = null;

		[Header("Debug (Read Only)")]
		[SerializeField]
		private List<IPlayerObserver> observers = new List<IPlayerObserver>();
		[SerializeField]
		private List<IControllerMovementOverride> movementOverrides = new List<IControllerMovementOverride>();

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

			//kc.SpriteOrientation = rangeDisplay.orientation;

			if (CanAttack)
			{
				if (PrimaryAttack)
				{
					kc.Attack(rangeDisplay.centre.position);
				}
				if (SpecialAttack)
				{
					kc.SpecialAttack(rangeDisplay.centre.position);
				}
			}
		}

		public void OnFixedUpdate()
		{

			if (IsMoving)
			{
				Vector2 movementVector = MoveDirection;
				Vector3 position = kc.transform.position;
				Quaternion rotation = Quaternion.identity;

				for (int i = 0; i < movementOverrides.Count; i++)
				{
					movementOverrides[i].ApplyOverride(ref movementVector, ref rotation, ref position);
				}

				kc.MovementVector = movementVector;
				kc.MovementRotation = Quaternion.Slerp(kc.transform.rotation, rotation, Time.fixedDeltaTime * 2f);
			}
		}

		// Subscribtions
		public void OnEnemiesInRange()
		{
			rangeDisplay.gameObject.SetActive(true);
		}

		public void OnEnemiesOutOfRange()
		{
			rangeDisplay.gameObject.SetActive(false);
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
			List < PlayerRespawnArea > respawnLocations = GlobalLevelData.RespawnAreas;
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

		public void SubscribeMovementOverride(IControllerMovementOverride controllerOverride)
		{
			if (!movementOverrides.Contains(controllerOverride))
			{
				movementOverrides.Add(controllerOverride);
			}
		}

		public void UnsubscribMovementOverride(IControllerMovementOverride controllerOverride)
		{
			if (movementOverrides.Contains(controllerOverride))
			{
				movementOverrides.Remove(controllerOverride);
			}
		}



		// Action properties
		private bool PrimaryAttack
		{
			get 
			{
				bool pressed = Mouse.current.leftButton.isPressed;
				return pressed; 
			}
		}

		private bool SpecialAttack
		{
			get
			{
				bool pressed = Mouse.current.rightButton.isPressed;
				return pressed;
			}
		}

		private bool CanAttack => rangeDisplay.gameObject.activeInHierarchy;

		// Movement properties
		private bool IsMoving => MoveDirection != Vector2.zero;
		private float Horizontal
		{
			get
			{
				var keyboard = Keyboard.current;
				var horizontal = 0f;
				if (keyboard.dKey.isPressed)
				{
					horizontal += 1f;
				}
				if (keyboard.aKey.isPressed)
				{
					horizontal -= 1f;
				}
				return horizontal;
			}
		}
		private float Vertical
		{
			get
			{
				var keyboard = Keyboard.current;
				var vertical = 0f;

				if (keyboard.wKey.isPressed)
				{
					vertical += 1f;
				}

				if (keyboard.sKey.isPressed)
				{
					vertical -= 1f;
				}
				return vertical;
			}
		}
		private Vector2 MoveDirection
		{
			get
			{
				Vector2 value = new Vector2(Horizontal, Vertical).normalized;
				return value;
			}
		}

	}
}