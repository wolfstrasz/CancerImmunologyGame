﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Cells
{
	public class HelperTCell : Cell
	{
		// Spawning particles functionality
		[Header("Functionality info (Read Only)")]
		[SerializeField]
		private bool hasSpotBeenReserved = false;
		[SerializeField]
		private bool shouldHeal = false;

		[Header("Particle Spawning")]
		[SerializeField]
		private GameObject particlePrefab = null;
		[SerializeField]
		private float timeBetweenSpawns = 2.0f;
		[SerializeField]
		private float particlesLifetime = 1f;
		[SerializeField]
		private float timeBeforeNextSpawn = 0.0f;

		[Header("Booking spots")]
		[SerializeField]
		private float spreadAngleInDegrees = 60.0f;
		[SerializeField]
		private float bookingSpotDistance = 1.0f;
		[SerializeField]
		private int forcedFreeSpots = 1;
		[SerializeField]
		private List<GameObject> bookingSpots = null;
		[SerializeField]
		private List<GameObject> freeBookingSpots = null;

		[Header("Simulation of booking spot rotation")]
		[SerializeField]
		private float timeToPassForFullRotation = 5f;
		private float timePassedForFullRotation = 0f;

		[SerializeField]
		private List<Transform> patrolPoints = new List<Transform>();
		[SerializeField]
		private int patrolIndex = 0;
		[SerializeField]
		private float speed = 2;

		public override bool isImmune => true;

		private void Start()
		{
			GenerateBookingSpots();
		}


		private void Update()
		{
			FakeRotateBookingSpots();



			timeBeforeNextSpawn -= Time.deltaTime;
			if (timeBeforeNextSpawn < 0f)
			{
				timeBeforeNextSpawn = 0f;
			}


			if (shouldHeal && timeBeforeNextSpawn <= 0f)
			{
				
				foreach(GameObject spot in bookingSpots)
				{
					Vector3 spawnDirection = (spot.transform.position - transform.position).normalized;
					HelperCellParticle particle = Instantiate(particlePrefab, transform.position, Quaternion.identity).GetComponent<HelperCellParticle>();
					particle.SetParticleData(spawnDirection, particlesLifetime);
				}

				// TODO: Add spawn sound;
				timeBeforeNextSpawn = timeBetweenSpawns;
			}
		}


		private void FixedUpdate()
		{

			if (!hasSpotBeenReserved)
			{
				MoveToNextPatrolPoint();
			}
		}

		private void MoveToNextPatrolPoint()
		{
			Vector3 direction = (patrolPoints[patrolIndex].position - transform.position).normalized;

			transform.position += direction * Time.deltaTime * speed;

			if (Vector3.SqrMagnitude(transform.position - patrolPoints[patrolIndex].position) < 2.0f)
   			{
				patrolIndex++;
				patrolIndex %= patrolPoints.Count;
			}
		}

		/// --------------------------------------
		/// BOOKING SPOTS
		private void GenerateBookingSpots()
		{
			// Reserve space
			int bookingSpotCount = Mathf.RoundToInt(360f / spreadAngleInDegrees) + 1;
			bookingSpots = new List<GameObject>(bookingSpotCount);
			freeBookingSpots = new List<GameObject>(bookingSpotCount);

			float initialRotationOffset = Random.Range(0f, spreadAngleInDegrees);
			float spawnAngleDegrees = initialRotationOffset;
			Vector3 spawnDirection = Vector3.right;

			// Spawn booking spots
			bookingSpotCount = 0;
			float fullRotation = 360 + initialRotationOffset - spreadAngleInDegrees;
			while (spawnAngleDegrees <= fullRotation)
			{
				// Calculate booking spot position
				Vector3 spawnPosition = Quaternion.Euler(0f, 0f, spawnAngleDegrees) * spawnDirection * bookingSpotDistance;

				// Create booking spot
				GameObject newBookSpot = new GameObject("BookingSpot" + bookingSpotCount.ToString());
				newBookSpot.transform.SetParent(this.transform, false);
				newBookSpot.transform.localPosition = spawnPosition;
				bookingSpots.Add(newBookSpot);
				freeBookingSpots.Add(newBookSpot);

				bookingSpotCount++;
				spawnAngleDegrees += spreadAngleInDegrees;
			}
		}

		private void FakeRotateBookingSpots()
		{
			timePassedForFullRotation += Time.deltaTime;
			if (timePassedForFullRotation > timeToPassForFullRotation)
				timePassedForFullRotation -= timeToPassForFullRotation;
		}

		private void FastSimulateBookingSpotRotations()
		{
			float angleToRotate = (timePassedForFullRotation / timeToPassForFullRotation) * 360f;
			Quaternion eulerRotation = Quaternion.Euler(0f, 0f, angleToRotate);

			foreach (GameObject spot in bookingSpots)
			{
				spot.transform.localPosition = eulerRotation * spot.transform.localPosition;
			}
		}

		public GameObject ReserveBookingSpot()
		{
			if (!hasSpotBeenReserved)
			{
				FastSimulateBookingSpotRotations();
			}

			if (freeBookingSpots.Count > forcedFreeSpots)
			{
				int spotID = Random.Range(0, freeBookingSpots.Count);

				GameObject spotToReserve = freeBookingSpots[spotID];
				freeBookingSpots.RemoveAt(spotID);
				hasSpotBeenReserved = true;
				return spotToReserve;
			}
			return null;
		}

		public void FreeBookingSpot(GameObject bookingSpotToFree)
		{
			Debug.Log("Releasing booking spot");
			freeBookingSpots.Add(bookingSpotToFree);
			if (freeBookingSpots.Count == bookingSpots.Count)
				hasSpotBeenReserved = false;
		}

		public void StartHealingOnCellsNearby()
		{
			if (!hasSpotBeenReserved)
			{
				FastSimulateBookingSpotRotations();
				hasSpotBeenReserved = true;
			}
			shouldHeal = true;
		}

		public void TryToStopHealingOnCellLeaving()
		{
			shouldHeal = false;

			if (freeBookingSpots.Count == bookingSpots.Count)
			{
				hasSpotBeenReserved = false;
			}
		}

		public override void HitCell(float amount)
		{
			Debug.LogWarning("Helper cell got hit but it is not implemented!");
		}

		public override void ExhaustCell(float amount)
		{
			Debug.LogWarning("Helper cell got hit but it is not implemented!");

		}
	}

}