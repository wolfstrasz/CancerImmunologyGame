using UnityEngine;
using UnityEngine.InputSystem;

using System.Collections.Generic;
using ImmunotherapyGame.Cancers;
using ImmunotherapyGame.Core;
using ImmunotherapyGame.Audio;


namespace ImmunotherapyGame.Player
{
	[RequireComponent(typeof(PlayerInput))]
	public class PlayerController : Singleton<PlayerController> , ICellController, ICancerDeathObserver, IControllerMovementOverridable
	{
		[SerializeField]
		private KillerCell kc = null;
		[Header("Aiming")]
		[SerializeField]
		private GameObject crosshairCanvas = null;
		[SerializeField]
		private Transform crosshair = null;
		internal Quaternion CrosshairRotation { get; set; }

		[Header("AutoAim")]
		[SerializeField]
		private GameObject autoAimerPrefab = null;
		[SerializeField]
		private bool useAutoAim = false;
		[SerializeField]
		private PlayerAutoAimer autoAimer = null;


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
			transform.rotation = kc.transform.rotation;
			CrosshairRotation = Quaternion.identity;
			// Auto aiming
			useAutoAim = GlobalGameData.autoAim;
			if (useAutoAim && autoAimer == null)
			{
				autoAimer = Instantiate(autoAimerPrefab, transform).GetComponent<PlayerAutoAimer>();
				autoAimer.owner = this;
			}
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

			transform.position = kc.transform.position;
			transform.rotation = kc.transform.rotation;
			if (autoAimer != null)
				autoAimer.OnUpdate();
			crosshairCanvas.transform.rotation = CrosshairRotation;
			PlayerUI.Instance.OnUpdate();

			if (CanAttack)
			{
				if (InitiatePrimaryAttack)
				{
					kc.Attack(crosshair.position);
				}
				if (InitiateSpecialAttack)
				{
					kc.SpecialAttack(crosshair.position);
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
			crosshairCanvas.SetActive(true);
		}

		public void OnEnemiesOutOfRange()
		{
			crosshairCanvas.SetActive(false);
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


		private bool InitiatePrimaryAttack { get; set; }
		private bool InitiateSpecialAttack { get; set; }
		private bool CanAttack => crosshairCanvas.activeInHierarchy;

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
		private Vector2 MoveDirection { get; set; }

		// Action Callbacks
		void OnMovement(InputValue value)
		{
			var direction = value.Get<Vector2>();
			MoveDirection = new Vector2(direction.x, direction.y);
		}

		void OnPrimaryAttack(InputValue value)
		{
			InitiatePrimaryAttack = value.isPressed;
		}

		void OnSecondaryAttack(InputValue value)
		{
			InitiateSpecialAttack = value.isPressed;
		}

		void OnAim(InputValue value)
		{
			// Get 2D direction
			Vector2 direction = value.Get<Vector2>();

			// Calculate rotation
			float rotationAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			CrosshairRotation = Quaternion.Euler(0.0f, 0.0f, rotationAngle);
		}

		void OnMouseAim(InputValue value)
		{
			// Obtain pointer position
			Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
			worldPosition.z = 0.0f;

			// Get 2D direction
			Vector3 diff = worldPosition - transform.position;
			diff.Normalize();

			// Calculate rotation
			float rotationAngle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
			CrosshairRotation = Quaternion.Euler(0.0f, 0.0f, rotationAngle);
		}
	}
}