using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

namespace ImmunotherapyGame.Bloodflow
{
	public class BloodCell : MonoBehaviour
	{
		[Header("Path Following Attributes")]
		[SerializeField] private float speed = 7.0f;
		[SerializeField] public float rotationSpeed = 90.0f;

		[Header("Debug")]
		[SerializeField][ReadOnly] private float distanceTravelled = 0.0f;
		[SerializeField][ReadOnly] private float endDistance = 0.0f;

		private VertexPath vertexPath = null;

		internal void Initialise(VertexPath path, float startDistance, float endDistance, Sprite sprite)
		{
			GetComponent<SpriteRenderer>().sprite = sprite;
			transform.Rotate(0, 0, Random.Range(0.0f, 360.0f));
			vertexPath = path;
			distanceTravelled = startDistance;
			transform.position = vertexPath.GetPointAtDistance(startDistance);
			this.endDistance = endDistance;
		}

		internal void OnFixedUpdate()
		{
			transform.Rotate(0, 0, rotationSpeed * Time.fixedDeltaTime);

			if (distanceTravelled >= endDistance)
				distanceTravelled -= endDistance;


			distanceTravelled += speed * Time.fixedDeltaTime;
			transform.position = vertexPath.GetPointAtDistance(distanceTravelled);
		}
	}
}