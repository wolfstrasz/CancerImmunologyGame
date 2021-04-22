using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using ImmunotherapyGame.Player;
using ImmunotherapyGame.Core;

namespace ImmunotherapyGame.Bloodflow
{
	public class BloodflowEnvironment : MonoBehaviour, IControllerMovementOverride
	{
		private VertexPath path = null;
		[Header("Attributes")]
		[SerializeField] private PathCreator pathCreator = null;
		[SerializeField] private float flow_speed = 7.0f;
		[SerializeField] private float rotation_speed = 2.0f;
		[SerializeField][ReadOnly] private float fixNormal = 0.0f;

		internal void Awake()
		{
			path = pathCreator.path;
			fixNormal = pathCreator.bezierPath.FlipNormals ? -1.0f : 1.0f;
		}

		public void ApplyOverride(ref Vector2 movementVector, ref Quaternion newRotation, ref Vector3 position)
		{
			// Find normal at position
			float time = path.GetClosestTimeOnPath(position);
			Debug.Log(time);
			Vector2 direction = path.GetDirection(time);
			Debug.Log(direction);

			Vector2 constraintNormal = path.GetNormal(time) * fixNormal;

			// Calculate flow movement
			Vector2 flowMoveVector = direction * flow_speed * Time.fixedDeltaTime;

			// Calculate rotation
			var rotationAngle = Mathf.Atan2(constraintNormal.y, constraintNormal.x) * Mathf.Rad2Deg;
			newRotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);

			// Update movement vector
			movementVector.y = 0.0f;
			movementVector = newRotation * movementVector;
			movementVector += flowMoveVector;
		}
	}
}