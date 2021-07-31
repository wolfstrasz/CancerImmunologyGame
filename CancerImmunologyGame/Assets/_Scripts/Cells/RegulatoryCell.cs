using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

using ImmunotherapyGame.Abilities;
using ImmunotherapyGame.Core;

namespace ImmunotherapyGame
{

    public class RegulatoryCell : Cell
    {
        [Header("Skills")]
		[SerializeField] private RangedAbilityCaster primaryCaster = null;
		[SerializeField] private RangedAbilityCaster chargeCaster = null;
		[SerializeField] private GameObject chargePathPivot = null;
        [SerializeField] [ReadOnly] private Vector3 chargeAtTargetDirection;
        [SerializeField] [ReadOnly] private GameObject chargeAtTarget;

        [Header ("Patrol movement")]
        [SerializeField] private PathCreator path = null;
        [SerializeField] [ReadOnly] private VertexPath pathToFollow = null;
        [SerializeField] [ReadOnly] private float distanceTravelled = 0.0f;
        [SerializeField] [ReadOnly] private Vector3 closestPathPosition;

        private float pathLengthDistance { get; set; }

        [Header("State info")]
        [SerializeField] [ReadOnly] private bool isPreparingForCharge = false;
        [SerializeField] [ReadOnly] private bool isCharging = false;
        [SerializeField] [ReadOnly] private bool isReturningFromCharge = false;
        [SerializeField] [ReadOnly] private float chargeAttackTimeLeft = 0f;
        [SerializeField] private float chargeAttackTime = 1f;

        private bool isBusy => isPreparingForCharge || isCharging || isReturningFromCharge;
        public override bool isImmune => true;

		void Awake()
        {
            if (path != null)
            {
                pathToFollow = path.path;
                pathLengthDistance = pathToFollow.length;
            }
        }

        public override void OnUpdate()
        {
            if (primaryCaster != null && primaryCaster.HasTargetsInRange && !isBusy)
			{
				if (primaryCaster.CanCastAbility(CurrentEnergy))
				{
					GameObject targetToShoot = Utils.GetRandomGameObject(primaryCaster.TargetsInRange);
                    primaryCaster.CastAbility(targetToShoot);
				}
			}

			if (chargeCaster != null && chargeCaster.HasTargetsInRange && !isBusy)
			{
				// Add secondary skill here!
				if (chargeCaster.CanCastAbility(CurrentEnergy))
				{
                    chargeAtTarget = Utils.GetRandomGameObject(chargeCaster.TargetsInRange);

					isPreparingForCharge = true;
                    animator.SetTrigger("PrepareForCharge");

                    chargeAtTargetDirection = (chargeAtTarget.transform.position - transform.position).normalized;
                    render.flipX = chargeAtTargetDirection.x < 0.0f;
                    Utils.LookAt2D(chargePathPivot.transform, chargePathPivot.transform.position + chargeAtTargetDirection);
				}
			}

		}

        public void OnChargeExecution()
		{
   //         if (chargeCaster.CanCastAbility(CurrentEnergy))
			//{
            chargeCaster.CastAbility(gameObject);
			//}

            animator.SetTrigger("Charge");
            isPreparingForCharge = false;
            isCharging = true;
            chargeAttackTimeLeft = chargeAttackTime;
		}

        private void OnChargeFinish()
		{
            animator.SetTrigger("ReturnFromCharge");

            isReturningFromCharge = true;
            isCharging = false;

            if (pathToFollow != null)
			{
                closestPathPosition = pathToFollow.GetClosestPointOnPath(transform.position);
			} else
			{
                closestPathPosition = transform.position;
			}

        }

        public void OnFixedUpdate()
        {

            if (!isBusy && path != null)
			{
                MoveByPath();
			} 
            else if (isPreparingForCharge)
			{
                
			} 
            else if (isCharging)
			{
                MoveToChargeTarget();
			}
            else if (isReturningFromCharge)
			{
                MoveToPath();
			}
        }


        private void MoveByPath()
        {
            distanceTravelled += Time.fixedDeltaTime * CurrentSpeed;
            if (distanceTravelled > pathLengthDistance) distanceTravelled -= pathLengthDistance;
            Vector3 newPos = pathToFollow.GetPointAtDistance(distanceTravelled, EndOfPathInstruction.Loop);
            transform.position = newPos;

            // Check for flipping
            Vector3 direction = newPos - transform.position;
            render.flipX = direction.x < 0.0f;
        }


        private void MoveToPath()
		{
            Vector3 distanceVector = closestPathPosition - transform.position;
            Vector3 direction = distanceVector.normalized;
            Vector3 movement = direction * Time.fixedDeltaTime * CurrentSpeed;

            // Check for flipping
            render.flipX = direction.x < 0.0f;

            if (movement.magnitude < distanceVector.magnitude)
			{

                transform.position += movement;
			}
            else
			{
                transform.position = closestPathPosition;

                if (pathToFollow != null)
				{
                    float newDistanceAlongPath = pathToFollow.GetClosestDistanceAlongPath(closestPathPosition);
                    distanceTravelled = newDistanceAlongPath;
                }
     
                isReturningFromCharge = false;
			}
		}

        private void MoveToChargeTarget()
		{
            chargeAttackTimeLeft -= Time.fixedDeltaTime;
            Vector3 movement = chargeAtTargetDirection * Time.fixedDeltaTime * CurrentSpeed;

            // Check for flipping
            render.flipX = chargeAtTargetDirection.x < 0.0f;

            transform.position += movement;
            if (chargeAttackTimeLeft < 0f)
			{
                OnChargeFinish();
            }
        }

	}

}