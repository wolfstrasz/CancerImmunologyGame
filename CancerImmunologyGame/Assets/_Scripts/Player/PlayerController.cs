using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Player
{
	public class PlayerController : Singleton<PlayerController>
	{
		public GameObject AttackEffectPrefab = null;

		public Animator animator = null;
		public float rotOFfset = 0.2f;

		public float speed = 4.0f;
		public Rigidbody2D rb = null;

		Vector2 movement = Vector2.zero;
		private bool isPlayerRespawning = false;
		private bool isInPowerUpAnimation = false;
		private bool isInAttackAnimation = false;

		internal bool isInPowerUpMode = false;
	
		public void Initialise()
		{
			GlobalGameData.player = gameObject;
		}
		
		// input
		public void OnUpdate()
		{

			if (isPlayerRespawning)
			{
				WaitForCameraToFocusAfterRespawn();
				return;
			}

			if (GlobalGameData.isPaused || isInPowerUpAnimation || isInAttackAnimation || !GlobalGameData.areControlsEnabled)
			{
				movement = new Vector2(0.0f, 0.0f);
				return;
			}

			// Collect input 
			movement.x = Input.GetAxisRaw("Horizontal");
			movement.y = Input.GetAxisRaw("Vertical");

			if (PlayerUI.Instance.PowerUp <= 0.0f)
			{
				ExitPowerUpMode();
			}

			if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
			{
				AttackCancerCells();
			}

		}

		// Physics update
		public void OnFixedUpdate()
		{
			if (isPlayerRespawning) return;

			// WHY DID I DO THIS I NEED TO FIND OUT !!!
			if (Mathf.Abs(movement.x) == 1 && Mathf.Abs(movement.y) == 1)
			{
				movement = movement * 0.74f;
			}
			///////////////////////////////////////////

			Vector2 move = movement * speed * Time.fixedDeltaTime;
			if (isInPowerUpMode)
			{
				rb.MovePosition(move + rb.position);
			}
			else
			{
				rb.MovePosition(move * PlayerUI.Instance.GetExhaustRatio() + rb.position);
			}
		}


		// Power up functionality
		internal void EnterPowerUpMode()
		{
			isInPowerUpMode = true;
			isInPowerUpAnimation = true;
			animator.SetTrigger("PowerUp");
			animator.speed = 2.0f;
		}

		internal void ExitPowerUpMode()
		{
			isInPowerUpMode = false;
			animator.speed = 1.0f;
		}

		public void OnFinishPowerUpAnimation()
		{
			isInPowerUpAnimation = false;
		}

		// Respawning functionality
		internal void Respawn()
		{

			rb.isKinematic = true;
			movement = new Vector2(0.0f, 0.0f);
			isPlayerRespawning = true;
			gameObject.transform.position = GlobalGameData.GetClosestSpawnLocation(gameObject.transform.position);

			SmoothCamera.Instance.isCameraFocused = false;
		}

		private void WaitForCameraToFocusAfterRespawn()
		{
			if (SmoothCamera.Instance.isCameraFocused && SmoothCamera.Instance.focusTarget == this.gameObject)
			{
				PlayerUI.Instance.ResetData();
				isPlayerRespawning = false;
				rb.isKinematic = false;
			}
		}



		// Attack functionality
		private CancerCell closestCell = null;
		private void AttackCancerCells()
		{
			isInAttackAnimation = true;
			// Find closest cancer cell
			// Need to change to Cancer optimisation
			float minDist = 100000.0f;
			closestCell = null;

			foreach (var cell in FindObjectsOfType<CancerCell>())
			{
				float dist = Vector3.Distance(transform.position, cell.transform.position);
				if (dist < minDist)
				{
					minDist = dist;
					closestCell = cell;
				}
			}

			if (minDist > 1.5f) return;

			animator.SetTrigger("Attacks");
		}

		public void OnAttackEffect()
		{
			Vector3 diff = closestCell.transform.position - transform.position;
			diff.Normalize();

			float rot_z = ((Mathf.Atan2(diff.y, diff.x) + rotOFfset) * Mathf.Rad2Deg);

			GameObject newEffect = Instantiate(AttackEffectPrefab, transform.position, Quaternion.Euler(0f, 0f, rot_z));
			newEffect.GetComponent<ParticleSystem>().Play();
			PlayerUI.Instance.AddExhaustion(7.5f);

			closestCell.HitCell();
		}

		public void OnAttackFinished()
		{
			isInAttackAnimation = false;
		}
	}
}