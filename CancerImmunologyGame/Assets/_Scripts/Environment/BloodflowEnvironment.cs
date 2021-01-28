using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

namespace Bloodflow
{
	public class BloodflowEnvironment : MonoBehaviour
	{
		[Header("Attributes")]
		[SerializeField]
		private PathCreator pathCreator = null;
		private VertexPath path = null;
		[SerializeField]
		private float flow_speed = 7.0f;

		[Header("Debug (Read Only)")]
		[SerializeField]
		private List<KillerCell> killerCells = new List<KillerCell>();
		internal List<KillerCell> KillerCells => killerCells;

#if BLOODFLOW_ROTATION
		[SerializeField]
		private float rotationMultiplier = 0.0f;
#endif


		internal void Initialise()
		{
			path = pathCreator.path;

#if BLOODFLOW_ROTATION
			rotationMultiplier = pathCreator.bezierPath.FlipNormals ? -1.0f : 1.0f;
#endif
		}

		internal void OnFixedUpdate()
		{

			foreach (KillerCell kc in killerCells)
			{

				// Collect needed data
				float time = path.GetClosestTimeOnPath(kc.transform.position);
				Vector2 direction = path.GetDirection(time);
#if BLOODFLOW_ROTATION
				Vector2 constraintNormal = path.GetNormal(time) * rotationMultiplier;
#else
				Vector2 constraintNormal = path.GetNormal(time);
#endif
				// Calculate flow movement
				Vector2 flowMoveVector = new Vector2(direction.x, direction.y) * flow_speed * Time.deltaTime;
				kc.FlowVector = flowMoveVector;

				Vector2 movementVector = kc.MovementVector;

#if BLOODFLOW_ROTATION

				// Obtain correct rotation
				var rotationAngle = Mathf.Atan2(constraintNormal.y, constraintNormal.x) * Mathf.Rad2Deg;
				var rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);
				kc.CorrectRotation = rotation;

				// Constraint only in Left - Right axis, then rotate
				movementVector.y = 0.0f;
				movementVector = kc.transform.rotation * movementVector;
				kc.MovementVector = movementVector.normalized;

#else

				Vector2 projection = Vector2.Dot(movementVector, constraintNormal) * constraintNormal;
				// normalize
				kc.MovementVector = projection.normalized;

#endif
			}
		}
	}
}