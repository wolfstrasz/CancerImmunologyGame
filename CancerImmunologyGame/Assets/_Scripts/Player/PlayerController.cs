using UnityEngine;
using UnityEngine.InputSystem;

using System.Collections.Generic;
using ImmunotherapyGame.Cancers;
using ImmunotherapyGame.Core;
using ImmunotherapyGame.Audio;


namespace ImmunotherapyGame.Player
{
	[RequireComponent(typeof(PlayerInput))]
	public class PlayerController : Singleton<PlayerController>, ICancerDeathObserver, IControllerMovementOverridable
	{
		[SerializeField] private PlayerData playerData = null;
		[SerializeField] private ImmunotherapyCaster immunotherapyCaster = null;

		[SerializeField]
		private KillerCell controlledCell = null;
		[Header("Aiming")]
		[SerializeField]
		private GameObject crosshairCanvas = null;
		[SerializeField]
		private Transform crosshair = null;
		internal Quaternion CrosshairRotation { get; set; }

		[Header("Debug (Read Only)")]
		[SerializeField]
		private List<IControllerMovementOverride> movementOverrides = new List<IControllerMovementOverride>();

		[SerializeField]
		private List<Cancer> cancersNearby = new List<Cancer>();

		public KillerCell ControlledCell => controlledCell;

		PlayerControls playerControls = null;

		protected override void Awake()
		{
			base.Awake();
			if (controlledCell != null)
			{
				transform.position = controlledCell.transform.position;
				transform.rotation = controlledCell.transform.rotation;
				playerData.CurrentCell = controlledCell;
			}

			CrosshairRotation = Quaternion.identity;
			playerData.CurrentCaster = immunotherapyCaster;
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

			transform.position = controlledCell.transform.position;
			transform.rotation = controlledCell.transform.rotation;
			crosshairCanvas.transform.rotation = CrosshairRotation;

			if (CanAttack)
			{
				if (InitiatePrimaryAttack)
				{
					controlledCell.UsePrimaryAttack(crosshair.gameObject);
				} else
				{
					controlledCell.StopPrimaryAttack();
				}
				if (InitiateSpecialAttack)
				{
					controlledCell.SecondaryAttack(crosshair.gameObject);
				}
			}
		}

		public void OnFixedUpdate()
		{

			//if (IsMoving)
			//{
				Vector2 movementVector = MoveDirection;
				Vector3 position = controlledCell.transform.position;
				Quaternion rotation = Quaternion.identity;

				for (int i = 0; i < movementOverrides.Count; i++)
				{
					movementOverrides[i].ApplyOverride(ref movementVector, ref rotation, ref position);
				}

				controlledCell.MovementVector = movementVector;
				controlledCell.MovementRotation = Quaternion.Slerp(controlledCell.transform.rotation, rotation, Time.fixedDeltaTime * 2f);
			//}
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

			if (GlobalLevelData.RespawnAreas == null || GlobalLevelData.RespawnAreas.Count == 0)
			{
				Debug.LogWarning("Zero respawn areas found on map. Respawning at same position");
				return;
			}

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

			controlledCell.gameObject.transform.position = closestRespawnLocation;
			gameObject.transform.position = closestRespawnLocation;

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


		void OnImmunotherapy(InputValue value)
		{
			Debug.Log("Player: On Special Ability Use");
			if (!immunotherapyCaster.IsOnCooldown)
			{
				immunotherapyCaster.CastAbility();
			}
		}

	}
}