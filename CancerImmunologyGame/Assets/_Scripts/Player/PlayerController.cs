using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cells;

namespace Player
{
	public class PlayerController : Singleton<PlayerController> , ICellController
	{
		[SerializeField]
		KillerCell kc = null;
		KillerSense kcSense = null;

		[SerializeField]
		private Vector3 HeartOutroPosition = new Vector3(0.0f, 0.0f, 0.0f);
		private bool heartOutro = false;
		private bool heartOutroEnd = false;
		private bool heartOutroCamera = false;

		// Movement
		[SerializeField]
		private bool isPlayerRespawning = false;
		private Vector2 movementVector = Vector2.zero;

		[Header("Range functionality")]
		[SerializeField]
		private PlayerRangeDisplay rangeDisplay = null;
		private bool canAttack = false;

		public void Initialise()
		{
			PlayerUI.Instance.Initialise();
			PlayerUI.Instance.SetPlayerInfo(kc);
			kc.controller = this;
			kcSense = kc.Sense;
			kcSense.controller = this;
			rangeDisplay.Initialise(kc.Range, kc.Fov);

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
					GlobalGameData.areControlsEnabled = true;

					//kc.IsKinematic = false;
				}
				return;
			}
		

	
			transform.position = kc.transform.position;
			//if (canAttack)
			//	kc.orientation = rangeDisplay.orientation;

#if BLOODFLOW_ROTATION

			transform.rotation = kc.transform.rotation;
#else
#endif

#if !REMOVE_PLAYER_DEBUG
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
			{
				return;
			}


			//if (Input.GetKey(KeyCode.Mouse0) && rangeDisplay.CanAttack)
			//{
			//	kc.Attack();
			//} else
			//{
			//	kc.StopAttack();
			//}

		}

		// Physics update
		public void OnFixedUpdate()
		{
			if (GlobalGameData.isGameplayPaused || kc.IsBusy || !GlobalGameData.areControlsEnabled)
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

		// Respawning functionality
		internal void Respawn()
		{
			SmoothCamera.Instance.isCameraFocused = false;
			isPlayerRespawning = true;
			//kc.IsKinematic = true;

			// Update cell position and controller
			kc.gameObject.transform.position = GlobalGameData.GetClosestSpawnLocation(kc.gameObject.transform.position);
			gameObject.transform.position = kc.gameObject.transform.position;
		}

		private void WaitForCameraToFocusAfterRespawn()
		{
			if (SmoothCamera.Instance.isCameraFocused && kc.IsDead)
			{
				//kc.IsKinematic = false;
				isPlayerRespawning = false;
				kc.Respawn();
			}
		}

		public void StartHeartMovement()
		{
			heartOutro = true;
			GlobalGameData.areControlsEnabled = false;
			//kc.IsKinematic = true;
		}

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
	}
}