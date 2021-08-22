using UnityEngine;
using UnityEngine.InputSystem;

using System.Collections.Generic;
using ImmunotherapyGame.Cancers;
using ImmunotherapyGame.Core;
using ImmunotherapyGame.Audio;


namespace ImmunotherapyGame.Player
{
	[RequireComponent(typeof(PlayerInput))]
	public class PlayerController : Singleton<PlayerController>, IControllerMovementOverridable
	{
		[SerializeField] private PlayerData playerData = null;
		[SerializeField] private KillerCell controlledCell = null;

		[Header("Aiming")]
		[SerializeField] private GameObject crosshairPivot = null;
		[SerializeField] private GameObject crosshairPrimary = null;
		[SerializeField] private GameObject crosshairSecondary = null;

		[SerializeField] private Transform crosshair = null;
		protected Quaternion crosshairRotation = Quaternion.identity;

		[Header("Debug")]
		[SerializeField] private List<IControllerMovementOverride> movementOverrides = new List<IControllerMovementOverride>();

		private bool InitiatePrimaryAttack { get; set; }
		private bool InitiateSpecialAttack { get; set; }
		private bool CanUsePrimary => crosshairPrimary.activeInHierarchy;
		private bool CanUseSecondary => crosshairSecondary.activeInHierarchy;

		private Vector2 MoveDirection { get; set; }


		protected override void Awake()
		{
			base.Awake();
			playerData.CurrentCell = controlledCell;
			controlledCell.onDeathEvent += OnCellDeath;
		}


		// input
		public void OnUpdate()
		{
			transform.position = controlledCell.transform.position;
			transform.rotation = controlledCell.transform.rotation;
			crosshairPivot.transform.rotation = crosshairRotation;

			var rotationZ = crosshairRotation.eulerAngles.z;
			controlledCell.FlipSpriteLocalTransform = !(rotationZ >= 90f && rotationZ <= 270f);

			// Update CrosshairVisibilities
			crosshairPrimary.SetActive(controlledCell.PrimaryAbilityCaster.HasTargetsInRange);
			crosshairSecondary.SetActive(controlledCell.CanUseSecondaryAttack);

			if (CanUsePrimary)
			{
				if (InitiatePrimaryAttack)
				{
					controlledCell.UsePrimaryAttack(crosshair.gameObject);
				} else
				{
					controlledCell.StopPrimaryAttack();
				}
				
			}

			if (CanUseSecondary)
			{
				if (InitiateSpecialAttack)
				{
					controlledCell.UseSecondaryAttack(crosshair.gameObject);
					InitiateSpecialAttack = false;

				}
			}

		}

		public void OnFixedUpdate()
		{
			Vector2 movementVector = MoveDirection;
			Vector3 position = controlledCell.transform.position;
			Quaternion rotation = Quaternion.identity;

			for (int i = 0; i < movementOverrides.Count; i++)
			{
				movementOverrides[i].ApplyOverride(ref movementVector, ref rotation, ref position);
			}

			controlledCell.MovementVector = movementVector;
			controlledCell.MovementRotation = Quaternion.Slerp(controlledCell.transform.rotation, rotation, Time.fixedDeltaTime * 2f);
		}

		/// <summary>
		/// Callback to move the player cell back to the closest nearby respawn location.
		/// </summary>
		/// <param name="cell"></param>
		public void OnCellDeath(Cell cell)
		{ 
			if (GlobalLevelData.RespawnAreas == null || GlobalLevelData.RespawnAreas.Count == 0)
			{
				Debug.LogWarning("Zero respawn areas found on map. Respawning at same position");
				return;
			}

			// Find closest spawn location
			List <PlayerRespawnArea> respawnLocations = GlobalLevelData.RespawnAreas;
			Vector3 closestRespawnLocation = respawnLocations[0].Position;
			float minDistance = Vector3.Distance(transform.position, respawnLocations[0].Position);

			foreach (var area in respawnLocations)
			{
				float distance = Vector3.Distance(transform.position, area.Position);
				if (distance <= minDistance)
				{
					minDistance = distance;
					closestRespawnLocation = area.Position;
				}
			}

			// Transport cell and heal TODO: Move to cell doing it.
			controlledCell.transform.position = closestRespawnLocation;
			transform.position = closestRespawnLocation;
			controlledCell.Respawn();

		}

		public void SubscribeMovementOverride(IControllerMovementOverride controllerOverride)
		{
			if (!movementOverrides.Contains(controllerOverride))
			{
				movementOverrides.Add(controllerOverride);
			}
		}

		public void UnsubscribeMovementOverride(IControllerMovementOverride controllerOverride)
		{
			if (movementOverrides.Contains(controllerOverride))
			{
				movementOverrides.Remove(controllerOverride);
			}
		}

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

			if (direction != Vector2.zero)
			{
				// Calculate rotation
				float rotationAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
				crosshairRotation = Quaternion.Euler(0.0f, 0.0f, rotationAngle);
			}
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
			crosshairRotation = Quaternion.Euler(0.0f, 0.0f, rotationAngle);
		}

		void OnDisplayAim(InputValue value)
		{
			if (value.isPressed)
			{
				crosshairPivot.SetActive(!crosshairPivot.activeInHierarchy);
			}
		}

	}
}