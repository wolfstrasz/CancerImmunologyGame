using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Player
{
	public class PlayerController : Singleton<PlayerController>
	{
		[SerializeField]
		KillerCell kc = null;
		[SerializeField]
		private Vector3 HeartOutroPosition = new Vector3(0.0f, 0.0f, 0.0f);
		private bool heartOutro = false;
		private bool heartOutroEnd = false;
		private bool heartOutroCamera = false;

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

		public void OnCameraOutroFinished()
		{
			heartOutroEnd = true;
			heartOutroCamera = false;
		}

		// input
		public void OnUpdate()
		{

			if (heartOutro)
			{
				if (Vector3.SqrMagnitude(transform.position - HeartOutroPosition) > 1.0f)
					transform.position = Vector3.Lerp(transform.position, HeartOutroPosition, (float)System.Math.Pow((1.0f - 0.943f), Time.unscaledDeltaTime));
				else
				{
					heartOutro = false;
					heartOutroCamera = true;
					SmoothCamera.Instance.StartHeartOutro();

				}
				return;
			}

			if (heartOutroCamera)
			{
				return;
			}

			if (heartOutroEnd)
			{
				if (Vector3.SqrMagnitude(transform.position - kc.transform.position) > 1.0f)
					transform.position = Vector3.Lerp(transform.position, kc.transform.position, (float)System.Math.Pow((1.0f - 0.943f), Time.unscaledDeltaTime));
				else
				{
					transform.position = kc.transform.position;
					heartOutroEnd = false;
					kc.IsKinematic = false;
				}
				return;
			}
		

	
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

		public void StartHeartMovement()
		{
			heartOutro = true;
			kc.IsKinematic = true;
		}
	}
}