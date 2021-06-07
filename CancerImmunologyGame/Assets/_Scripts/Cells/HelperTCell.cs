using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using ImmunotherapyGame.Abilities;
namespace ImmunotherapyGame
{
	public class HelperTCell : Cell
	{
		[Header("Healing")]
		[SerializeField]
		private AbilityCaster healCaster = null;

		[Header("AI Booking")]
		[SerializeField]
		private int forcedFreeSpots = 1;
		[SerializeField]
		private float spreadAngleInDegrees = 60.0f;
		[SerializeField]
		private float bookingSpotDistance = 1.0f;

		[SerializeField][ReadOnly]
		private bool shouldHeal = false;
		[SerializeField][ReadOnly]
		private bool hasSpotBeenReserved = false;
		[SerializeField][ReadOnly]
		private List<GameObject> bookingSpots = null;
		[SerializeField][ReadOnly]
		private List<GameObject> freeBookingSpots = null;

		[Header("Simulation of booking spot rotation")]
		[SerializeField]
		private float timeToPassForFullRotation = 5f;
		[SerializeField][ReadOnly]
		private float timePassedForFullRotation = 0f;

		[Header("Patrol Movement")]
		[SerializeField]
		private List<Transform> patrolPoints = new List<Transform>();
		[SerializeField][ReadOnly]
		private int patrolIndex = 0;

		public override bool isImmune => true;

		protected override void Start()
		{
			base.Start();
			GenerateBookingSpots();
		}


		public void OnUpdate()
		{
			shouldHeal = healCaster.HasTargetsInRange;

			if (shouldHeal && healCaster.CanCastAbility(CurrentEnergy))
			{
				animator.SetBool("OnPrimarySkill", true);
				// TODO: Make animation
			} else
			{
				animator.SetBool("OnPrimarySkill", false);
			}

		}

		public void OnPrimarySkillExecution()
		{
			if (healCaster.CanCastAbility(CurrentEnergy))
			{
				float energyCost = healCaster.CastAbilityOnAllTargetsInRange();
				ApplyEnergyAmount(-energyCost);
			}
		}

		public void OnFixedUpdate()
		{
			if (!(hasSpotBeenReserved || shouldHeal))
			{
				if (patrolPoints.Count > 1)
					MoveToNextPatrolPoint();

				FakeRotateBookingSpots();
			}
		}

		private void MoveToNextPatrolPoint()
		{
			Vector3 direction = (patrolPoints[patrolIndex].position - transform.position).normalized;
			transform.position += direction * Time.deltaTime * CurrentSpeed;
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
			timePassedForFullRotation += Time.fixedDeltaTime;
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

			if (!gameObject.activeInHierarchy)
			{
				return null;
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
	}

}