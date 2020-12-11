using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Player
{
	public class PlayerController : Singleton<PlayerController>
	{
		public Transform ArteryStartLoc;
		public Transform ToTeleportFromLoc;

		public GameObject headSystem = null;
		public ParticleSystem attackEffect = null;
		public GameObject AttackEffectPrefab;

		public Animator animator = null;
		public float rotOFfset = 0.2f;

		HashSet<CancerCell> cancerCells = new HashSet<CancerCell>();
		public float speed = 4.0f;
		public Rigidbody2D rb;

		Vector2 movement;
		private bool isPlayerRespawning = false;
		private bool attackIsOnCooldown = false;

		private bool isInPowerUpAnimation = false;
		private bool isInAttackAnimation = false;

		// NEW ATTACK MECHANIC
		public float attack_speed = 1.0f;
		public float attack_anim_time = 0.4f;
		public float power_up_time = 0.8f;
		public bool powered_up = false;
		public void StartPowerUp()
		{
			animator.SetTrigger("PowerUp");
			attack_speed = 2.0f;
			animator.speed = attack_speed;
			powered_up = true;
			StartCoroutine(WaitForPowerUpAnim());
		}

		IEnumerator WaitForPowerUpAnim()
		{
			isInPowerUpAnimation = true;
			attackIsOnCooldown = true;
			yield return new WaitForSeconds(power_up_time);
			isInPowerUpAnimation = false;
			attackIsOnCooldown = false;
		}

		public void Initialise()
		{
			GlobalGameData.player = gameObject;
		}

		void StopPowerUp()
		{
			attack_speed = 1.0f;
			animator.SetTrigger("PowerUpFinished");
			animator.speed = attack_speed;
			powered_up = false;
		}


		// input
		public void OnUpdate()
		{
			if (GlobalGameData.isPaused || isInPowerUpAnimation || isInAttackAnimation)
			{
				movement = new Vector2(0.0f, 0.0f);
				return;
			}
			if (isPlayerRespawning) return;

			//if (GlobalGameData.Instance.powerUp == 0.0f)
			//{
			//    StopPowerUp();
			//}

			if (Vector3.Distance(gameObject.transform.position, ToTeleportFromLoc.position) <= 2.0f)
			{
				gameObject.transform.position = ArteryStartLoc.position;
			}

			//animator.SetFloat("ExhaustionRate", GlobalGameData.Instance.exhaustion / GlobalGameData.Instance.maxExhaustion);
			if (!GlobalGameData.areControlsEnabled)
			{
				movement = new Vector2(0.0f, 0.0f);
				return;
			}

			movement.x = Input.GetAxisRaw("Horizontal");
			movement.y = Input.GetAxisRaw("Vertical");

			Debug.Log(movement);
			if (Input.GetKey(KeyCode.Keypad7))
			{
				GlobalGameData.AddHealth(-0.1f);
			}
			if (Input.GetKey(KeyCode.Keypad9))
			{
				GlobalGameData.AddHealth(+0.1f);
			}
			if (Input.GetKey(KeyCode.Keypad4))
			{
				GlobalGameData.AddExhaustion(-0.1f);
			}
			if (Input.GetKey(KeyCode.Keypad6))
			{
				GlobalGameData.AddExhaustion(+0.1f);
			}
			if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
			{
				AttackCancerCells();
			}


			//if(GlobalGameData.Instance.health <= 0.0f || GlobalGameData.Instance.exhaustion == GlobalGameData.Instance.maxExhaustion)
			//{
			//    StartCoroutine(PlayerRespawn());

			//}
		}

		IEnumerator PlayerRespawn()
		{
			rb.isKinematic = true;
			movement = new Vector2(0.0f, 0.0f);
			isPlayerRespawning = true;
			gameObject.transform.position = ArteryStartLoc.position;
			//GlobalGameData.Instance.SetHealth( GlobalGameData.Instance.maxHealth);
			GlobalGameData.SetExhaustion(0.0f);

			yield return new WaitForSeconds(1.0f);
			isPlayerRespawning = false;
			rb.isKinematic = false;
		}

		// physics update
		public void OnFixedUpdate()
		{

			if (isPlayerRespawning) return;

			if (Mathf.Abs(movement.x) == 1 && Mathf.Abs(movement.y) == 1)
			{
				movement = movement * 0.74f;
			}

			Vector2 move = movement * speed * Time.fixedDeltaTime;
			if (powered_up)
			{
				rb.MovePosition(move + rb.position);
				return;
			}

			Vector2 dampedMove = move;
			//Vector2 dampedMove = move * (GlobalGameData.Instance.maxExhaustion - GlobalGameData.Instance.exhaustion) / GlobalGameData.Instance.maxExhaustion;
			Vector2 finalPos = dampedMove + rb.position;
			rb.MovePosition(finalPos);
		}

		private void AttackCancerCells()
		{

			//     if (!attackIsOnCooldown)
			//     {
			//         // Find closest
			//         float minDist = 100000.0f;
			//         CancerCell closestCell = null;

			//foreach (var cell in GlobalGameData.Instance.allCancerCells)
			//{

			//	float dist = Vector3.Distance(transform.position, cell.transform.position);
			//	if (dist < minDist)
			//	{
			//		minDist = dist;
			//		closestCell = cell;
			//	}
			//}

			//if (minDist > 1.5f) return;

			//         // hit it
			//         animator.SetTrigger("Attacks");
			//         GlobalGameData.Instance.AddExhaustion(7.5f);

			//         StartCoroutine(RunEffect(closestCell));
			//         StartCoroutine(AttackCooldown());
			//     }
		}

		IEnumerator RunEffect(CancerCell closestCell)
		{

			yield return new WaitForSeconds(attack_anim_time / attack_speed / 2);
			Vector3 diff = closestCell.transform.position - transform.position;
			diff.Normalize();

			float rot_z = ((Mathf.Atan2(diff.y, diff.x) + rotOFfset) * Mathf.Rad2Deg);
			// headSystem.transform.rotation = Quaternion.Euler(0f, 0f, rot_z);

			GameObject newEffect = Instantiate(AttackEffectPrefab, transform.position, Quaternion.Euler(0f, 0f, rot_z));
			newEffect.GetComponent<ParticleSystem>().Play();

			closestCell.HitCell();
			//attackEffect.Play();
			// rotate so it attack it

		}

		IEnumerator AttackCooldown()
		{
			isInAttackAnimation = false;
			attackIsOnCooldown = true;
			yield return new WaitForSeconds(attack_anim_time / attack_speed);
			isInAttackAnimation = true;
			yield return new WaitForSeconds(attack_anim_time / attack_speed);
			attackIsOnCooldown = false;
		}


		public void AddCloseCancerCell(GameObject go)
		{
			cancerCells.Add(go.GetComponent<CancerCell>());
		}

		public void RemoveCloseCancerCell(GameObject go)
		{
			cancerCells.Remove(go.GetComponent<CancerCell>());

		}
	}
}