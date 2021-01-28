using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

namespace Bloodflow
{
	public class BloodflowEntrance : MonoBehaviour
	{
		[Header("Attributes")]
		[SerializeField]
		public PathCreator pathCreator = null;
		public VertexPath path = null;
		public Vector3 eulerRotationOfNormals = Vector3.zero;

		public Rigidbody2D rb = null;


		public float distance = 0.0f;


		void Awake()
		{
			Initialise();
		}

		void Update()
		{

		}



		public void Initialise()
		{

			path = pathCreator.path;
			eulerRotationOfNormals = pathCreator.bezierPath.FlipNormals ? new Vector3(0.0f, 0.0f, 90.0f) : new Vector3(0.0f, 0.0f, -90.0f);

			distance = 0.0f;
			transform.position = path.GetPointAtDistance(distance, EndOfPathInstruction.Stop);
		}

		public float speed = 3.0f;
		public float flow_speed = 5.0f;

		void FixedUpdate()
		{

			// FLOW MOVEMENTS
			var timeAtClosestPoint = path.GetClosestTimeOnPath(transform.position);
			var closestDirection = path.GetDirection(timeAtClosestPoint);
			Debug.Log(closestDirection);

			Vector2 flowMove = new Vector2(closestDirection.x, closestDirection.y) * flow_speed;

			// PLAYER MOVEMENT
			Vector3 movement = Vector3.zero;
			// Collect input 
			movement.x = Input.GetAxisRaw("Horizontal");
			movement.y = Input.GetAxisRaw("Vertical");
			//kc.MovementVector = movement;

			// Damping if both axis are pressed. sqare root of 2.
			if (Mathf.Abs(movement.x) == 1 && Mathf.Abs(movement.y) == 1)
			{
				movement = movement * 0.73f;
			}

			// Resolve force conflicts by projection
			Vector2 normal = path.GetNormal(timeAtClosestPoint);
			// project onto normal (unit vector)
			Vector2 projection = Vector2.Dot(movement, normal) / Vector2.SqrMagnitude(normal) * normal;
			projection = projection.normalized * speed;
			// normalize
			//projection.normalized


			// update position
			rb.MovePosition(rb.position + flowMove + projection.normalized);

			//Vector2 move = movement * speed * Time.fixedDeltaTime;
			//rb.MovePosition(move + rb.position);
		}
	}


}