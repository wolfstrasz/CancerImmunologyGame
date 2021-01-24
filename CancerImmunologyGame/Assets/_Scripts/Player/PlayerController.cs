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
			if (Input.GetKey(KeyCode.Keypad7))
			{
				kc.ReceiveHealth(-0.1f);
			}
			if (Input.GetKey(KeyCode.Keypad9))
			{
				kc.ReceiveHealth(+0.1f);
			}
			if (Input.GetKey(KeyCode.Keypad4))
			{
				kc.ReceiveExhaustion(-0.1f);
			}
			if (Input.GetKey(KeyCode.Keypad6))
			{
				kc.ReceiveExhaustion(+0.1f);
			}
			PlayerUI.Instance.OnUpdate();

			if (isPlayerRespawning)
			{
				WaitForCameraToFocusAfterRespawn();
				return;
			}

			if (GlobalGameData.isGameplayPaused || kc.IsBusy || !GlobalGameData.areControlsEnabled)
				return;

			if (kc.IsDead)
			{
				Respawn();
				return;
			}

			if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.E))
			{
				kc.Attack();
			}

		}

		// Physics update
		public void OnFixedUpdate()
		{
			if (isPlayerRespawning) return;

			if (GlobalGameData.isGameplayPaused || kc.IsBusy || !GlobalGameData.areControlsEnabled)
			{
				kc.MovementVector = new Vector2(0.0f, 0.0f);
				return;
			}

			// Collect input 
			movement.x = Input.GetAxisRaw("Horizontal");
			movement.y = Input.GetAxisRaw("Vertical");
			kc.MovementVector = movement;

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
		//	rb.isKinematic = true;
			movement = new Vector2(0.0f, 0.0f);
			isPlayerRespawning = true;
			gameObject.transform.position = GlobalGameData.GetClosestSpawnLocation(gameObject.transform.position);

			SmoothCamera.Instance.isCameraFocused = false;
		}

		private void WaitForCameraToFocusAfterRespawn()
		{
			if (SmoothCamera.Instance.isCameraFocused && SmoothCamera.Instance.focusTarget == this.gameObject)
			{
				isPlayerRespawning = false;
				kc.IsDead = false;
		//		rb.isKinematic = false;
			}
		}
	}
}