using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Player
{
	public class PlayerController : Singleton<PlayerController>
	{
		[SerializeField]
		KillerCell kc = null;

		// Attack
		private Vector2 movement = Vector2.zero;

		[SerializeField]
		private bool isPlayerRespawning = false;

		public void Initialise()
		{
			kc.Initialise();
			PlayerUI.Instance.Initialise();
			PlayerUI.Instance.kc = kc;

			GlobalGameData.player = kc.gameObject;
		}

		// input
		public void OnUpdate()
		{
			transform.position = kc.transform.position;

#if !REMOVE_PLAYER_DEBUG
			if (Input.GetKeyDown(KeyCode.Keypad7))
			{
				kc.ReceiveHealth(-20.0f);
			}
			if (Input.GetKeyDown(KeyCode.Keypad9))
			{
				kc.ReceiveHealth(+20.0f);
			}
			if (Input.GetKeyDown(KeyCode.Keypad4))
			{
				kc.ReceiveExhaustion(-20.0f);
			}
			if (Input.GetKeyDown(KeyCode.Keypad6))
			{
				kc.ReceiveExhaustion(+20.0f);
			}
#endif
			PlayerUI.Instance.OnUpdate();


			if (isPlayerRespawning)
			{
				WaitForCameraToFocusAfterRespawn();
				return;
			}
   
			if (kc.IsDead)
			{
				Respawn();
				return;
			}

			if (GlobalGameData.isGameplayPaused || kc.IsBusy || !GlobalGameData.areControlsEnabled)
				return;

			if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.E))
			{
				kc.Attack();
			}

		}

		// Physics update
		public void OnFixedUpdate()
		{
			if (GlobalGameData.isGameplayPaused || kc.IsBusy || !GlobalGameData.areControlsEnabled)
			{
				kc.ClearForces();
				movement = Vector2.zero;
				return;
			}

			// Collect input 
			movement.x = Input.GetAxisRaw("Horizontal");
			movement.y = Input.GetAxisRaw("Vertical");
			//kc.MovementVector = movement;

			// Damping if both axis are pressed. sqare root of 2.
			if (Mathf.Abs(movement.x) == 1 && Mathf.Abs(movement.y) == 1)
			{
				movement = movement * 0.74f;
			}

			kc.Move(movement);
		}

		// Respawning functionality
		internal void Respawn()
		{
			SmoothCamera.Instance.isCameraFocused = false;
			isPlayerRespawning = true;
			kc.IsKinematic = true;

			// Update cell position and controller
			kc.gameObject.transform.position = GlobalGameData.GetClosestSpawnLocation(kc.gameObject.transform.position);
			gameObject.transform.position = kc.gameObject.transform.position;
		}

		private void WaitForCameraToFocusAfterRespawn()
		{
			if (SmoothCamera.Instance.isCameraFocused && kc.IsDead)
			{
				kc.IsKinematic = false;
				isPlayerRespawning = false;
				kc.Respawn();
			}
		}
	}
}