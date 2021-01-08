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
		private CancerCell closestCell = null;

		[SerializeField]
		private GameObject AttackEffectPrefab = null;
		[SerializeField]
		private Animator animator = null;
		[SerializeField]
		private float rotOFfset = 0.2f;

		// Movement
		[SerializeField]
		private float speed = 4.0f;
		[SerializeField]
		private Rigidbody2D rb = null;
		private Vector2 movement = Vector2.zero;

		// control states
		[SerializeField]
		private bool queuePowerUp = false;
		[SerializeField]
		private bool isPlayerRespawning = false;
		[SerializeField]
		private bool isInPowerUpAnimation = false;
		[SerializeField]
		private bool isInAttackAnimation = false;
		[SerializeField]
		internal bool isInPowerUpMode = false;



		public void Initialise()
		{
			allotherKCs = new List<KillerCell>(FindObjectsOfType<KillerCell>());
			allotherKCs.Remove(kc);
			PlayerUI.Instance.Initialise(animator);
			GlobalGameData.player = gameObject;
			kc.Initialise();
		}


		// hardcoded
		List<KillerCell> allotherKCs = new List<KillerCell>();
		int KCindex = 0;

		// input
		public void OnUpdate()
		{
			PlayerUI.Instance.OnUpdate();

			// hard-code should be removed

			if (Input.GetKeyDown(KeyCode.Tab))
			{
				if (allotherKCs.Count == 0) return;

				Vector3 nextPosition = allotherKCs[KCindex].transform.position;
				allotherKCs[KCindex].transform.position = transform.position;
				KCindex = (++KCindex) % allotherKCs.Count;

				transform.position = nextPosition;
			}
			if (isPlayerRespawning)
			{
				WaitForCameraToFocusAfterRespawn();
				return;
			}

			//Debug.Log(GlobalGameData.isGameplayPaused  + " " + isInPowerUpAnimation + " " + isInAttackAnimation + " " + (!GlobalGameData.areControlsEnabled));

			if (GlobalGameData.isGameplayPaused || isInPowerUpAnimation || isInAttackAnimation || !GlobalGameData.areControlsEnabled)
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

			if (queuePowerUp) return;

			if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.E))
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
				rb.MovePosition(move * PlayerUI.Instance.GetSlowDown() + rb.position);
			}
		}


		// Power up functionality
		internal void EnterPowerUpMode()
		{
			if (isInAttackAnimation)
			{
				isInPowerUpMode = true;
				queuePowerUp = true;
				return;
			}

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

		private List<CancerCell> cancerCellsInRange = new List<CancerCell>();
		private void AttackCancerCells()
		{
			cancerCellsInRange = kc.GetCancerCellsInRange();

			if (cancerCellsInRange.Count == 0) return;
			isInAttackAnimation = true;
			// Find closest cancer cell
			// Need to change to Cancer optimisation
			float minDist = 100000.0f;
			closestCell = null;

			foreach (var cell in cancerCellsInRange)
			{
				if (cell.CellInDivision()) continue;

				float dist = Vector3.Distance(transform.position, cell.transform.position);
				if (dist < minDist)
				{
					minDist = dist;
					closestCell = cell;
				}
			}

			if (closestCell == null)
			{
				isInAttackAnimation = false;
				return;
			}

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

			bool killedTheCell = closestCell.HitCell();
			if (killedTheCell)
			{
				cancerCellsInRange.Remove(closestCell);
			}
		}

		public void OnAttackFinished()
		{
			isInAttackAnimation = false;
			if (queuePowerUp)
			{
				EnterPowerUpMode();
				queuePowerUp = false;
			}
		}
	}
}