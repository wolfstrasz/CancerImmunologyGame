using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cells;
using Cancers;

namespace Player
{
	public class PlayerController : Singleton<PlayerController> , ICellController, ICancerDeathListener
	{

		// TODO: REMOVE THIS FROM HERE
		[SerializeField]
		private Vector3 HeartOutroPosition = new Vector3(0.0f, 0.0f, 0.0f);
		private bool heartOutro = false;
		private bool heartOutroEnd = false;
		private bool heartOutroCamera = false;
		// ---------------------------------------

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


		public KillerCell KC => kc;

		public void Initialise()
		{
			PlayerUI.Instance.Initialise();
			PlayerUI.Instance.SetPlayerInfo(kc);
			kc.Sense.controller = this;
			kc.controller = this;
			rangeDisplay.Initialise(kc);
		}


		void OnTriggerEnter2D(Collider2D collider)
		{
			Cancer cancer = collider.GetComponent<Cancer>();
			if (cancer != null)
			{
				BackgroundMusic.Instance.PlayBattleMusic();
				cancer.SubscribeListener(this);
			}
		}

		void OnTriggerExit2D(Collider2D collider)
		{
			Cancer cancer = collider.GetComponent<Cancer>();
			if (cancer != null)
			{

				BackgroundMusic.Instance.PlayNormalMusic();
				cancer.UnsubscribeListener(this);
			}
		}

		//TODO : REMOVE THIS
		public void OnCameraOutroFinished()
		{
			heartOutroEnd = true;
			heartOutroCamera = false;
		}

		public void StartHeartMovement()
		{
			heartOutro = true;
			areControlsEnabled = false;
		}
		//----------------------------

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
					areControlsEnabled = true;

					//kc.IsKinematic = false;
				}
				return;
			}
			// -----------------------------------------

			// TODO: manage it better
			transform.position = kc.transform.position;
			transform.rotation = kc.transform.rotation;

			PlayerUI.Instance.OnUpdate();
			rangeDisplay.OnUpdate();

#if PLAYER_DEBUG
			DebugInput();
#endif

			if (canAttack)
			{
				kc.SpriteOrientation = rangeDisplay.orientation;
				if (Input.GetKey(KeyCode.Mouse0))
				{
					kc.Attack(rangeDisplay.centre.position);
				}
			}
			else
			{
				kc.StopAttack();
				kc.SpriteOrientation = Quaternion.identity;
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

		public void OnCancerDeath()
		{
			BackgroundMusic.Instance.PlayNormalMusic();
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

			kc.AddHealth(KillerCell.maxHealth);
			kc.AddEnergy(KillerCell.maxEnergy);
			kc.gameObject.transform.position = closestRespawnLocation;
			gameObject.transform.position = closestRespawnLocation;

			SmoothCamera.Instance.SetNewFocus(this.gameObject, true);

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