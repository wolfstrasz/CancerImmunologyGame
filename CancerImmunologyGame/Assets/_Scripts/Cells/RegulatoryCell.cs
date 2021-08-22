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
        [SerializeField] [ReadOnly] private Vector3 chargeDirection;
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
            if (isBusy)
			{
                return;
			}
            if (primaryCaster != null && primaryCaster.CanCastAbility(CurrentEnergy))
            {
                ExecutePrimaryAttack();
            }
            if (chargeCaster != null  && chargeCaster.CanCastAbility(CurrentEnergy))
            {
                ExecuteChargeAttack();
            }

        }

        public void OnFixedUpdate()
        {
            if (!isBusy && path != null)
            {
                FollowPath();
            }
            else if (isCharging)
            {
                ChargeAtTarget();
            }
            else if (isReturningFromCharge)
            {
                GoBackToPath();
            }
        }

        private void ExecutePrimaryAttack()
		{
            GameObject targetToShoot = Utils.GetRandomGameObject(primaryCaster.TargetsInRange);
            primaryCaster.CastAbility(targetToShoot);
        }

        private void ExecuteChargeAttack()
		{
            chargeAtTarget = Utils.GetRandomGameObject(chargeCaster.TargetsInRange);
            chargeDirection = (chargeAtTarget.transform.position - transform.position).normalized;
            render.flipX = chargeDirection.x < 0.0f;
            Utils.LookAt2D(chargePathPivot.transform, chargePathPivot.transform.position + chargeDirection);

            isPreparingForCharge = true;
            animator.SetTrigger("PrepareForCharge");
        }

        private void OnChargeFinish()
        {
            isReturningFromCharge = true;
            isCharging = false;
            animator.SetTrigger("ReturnFromCharge");
        }

        private void FollowPath()
        {
            distanceTravelled += Time.fixedDeltaTime * CurrentSpeed;
            if (distanceTravelled > pathLengthDistance) distanceTravelled -= pathLengthDistance;
            Vector3 newPos = pathToFollow.GetPointAtDistance(distanceTravelled, EndOfPathInstruction.Loop);
            transform.position = newPos;

            // Check for flipping
            Vector3 direction = newPos - transform.position;
            render.flipX = direction.x < 0.0f;
        }

        private void ChargeAtTarget()
        {
            chargeAttackTimeLeft -= Time.fixedDeltaTime;
            Vector3 movement = chargeDirection * Time.fixedDeltaTime * CurrentSpeed;
            transform.position += movement;

            if (chargeAttackTimeLeft <= 0f)
            {
                OnChargeFinish();
            }
        }

        private void GoBackToPath()
        {
            if (pathToFollow != null)
            {
                closestPathPosition = pathToFollow.GetClosestPointOnPath(transform.position);
                Vector3 distanceVector = closestPathPosition - transform.position;
                Vector3 directionVector = distanceVector.normalized;
                Vector3 movementVector = directionVector * Time.fixedDeltaTime * CurrentSpeed;

                // Check for flipping
                render.flipX = directionVector.x < 0.0f;

                if (movementVector.magnitude < distanceVector.magnitude)
                {
                    transform.position += movementVector;
                }
                else
                {
                    transform.position = closestPathPosition;
                    distanceTravelled = pathToFollow.GetClosestDistanceAlongPath(closestPathPosition);
                    isReturningFromCharge = false;
                }
            }
            else
            {
                closestPathPosition = transform.position;
                Debug.LogWarning("Regulatory Cell did not find PathToFollow (null). Regulatory cell will stay still.");
                isReturningFromCharge = false;
            }
        }

        public void ForceChargeStop()
        {
            if (isCharging)
            {
                OnChargeFinish();
            }
        }

        // Animation Callbacks
        public void OnChargeExecution()
		{
            chargeCaster.CastAbility(gameObject);
            chargeAttackTimeLeft = chargeAttackTime;
            isPreparingForCharge = false;
            isCharging = true;
            animator.SetTrigger("Charge");
		}
     
        protected override void OnCellDeath() { }
    }

}