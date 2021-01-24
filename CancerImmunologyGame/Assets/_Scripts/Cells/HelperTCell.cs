using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Cells
{
	public class HelperTCell : Cell
	{
		[SerializeField]
		private HelperSense sense = null;
		[SerializeField]
		private bool isActivated = false;
		[SerializeField]
		private GameObject body = null;
		[SerializeField]
		private GameObject sprite = null;

		// Spawning particles functionality
		[Header("ParticleSpawning")]
		[SerializeField]
		private GameObject particle = null;
		[SerializeField]
		private float timeBetweenSpawns = 2.0f;
		[SerializeField]
		private float spreadDistance = 1.5f;
		[SerializeField]
		private float spreadAngle = 30.0f;
		[SerializeField]
		private bool spawning = false;
		[SerializeField]
		private float spawnTimePassed = 0.0f;


		public void Activate()
		{
			body.SetActive(true);
			sprite.SetActive(true);
			sense.gameObject.SetActive(true);
			GetComponent<Animator>().enabled = true;
			isActivated = true;
		}

		void Update()
		{
			if (!isActivated) return;

			if (GlobalGameData.isGameplayPaused) return;

			if (!spawning && sense.KillerCells.Count > 0)
			{
				spawning = true;
				spawnTimePassed = 0.0f;
				foreach (KillerCell kc in sense.KillerCells)
				{
					SpawnParticle(kc);
				}
			}

			if (spawning)
			{
				spawnTimePassed += Time.deltaTime * GlobalGameData.gameplaySpeed;
				if (spawnTimePassed > timeBetweenSpawns)
				{
					spawnTimePassed = 0.0f;
					foreach (KillerCell kc in sense.KillerCells)
					{
						SpawnParticle(kc);
					}
				}
			}
		}

		private void SpawnParticle(KillerCell kc)
		{
			Vector3 direction = (kc.transform.position - transform.position).normalized;

			float rotation = Random.Range(-spreadAngle, spreadAngle);

			Vector3 spreadVector = Quaternion.Euler(0.0f, 0.0f, rotation) * direction * spreadDistance;

			GameObject particleObj = Instantiate(particle, transform.position, Quaternion.identity);

			HelperCellParticle hcParticle = particleObj.GetComponent<HelperCellParticle>();

			hcParticle.Initialise(transform.position + spreadVector, kc);

		}


	}

}