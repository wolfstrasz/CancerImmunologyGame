using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

namespace BloodcellAnimation
{
	public class BloodCell : MonoBehaviour
	{
		[Header("Functional Linking")]
		[SerializeField]
		private List<Sprite> bloodCellSprites;
		[SerializeField]
		private SpriteRenderer render = null;


		[Header("Path Following Attributes")]
		[SerializeField]
		private EndOfPathInstruction endOfPathInstruction = EndOfPathInstruction.Loop;
		[SerializeField]
		private float speed = 8.0f;
		[SerializeField]
		public float rotationSpeed = 90.0f;

		[Header("Debugging")]
		[SerializeField]
		private VertexPath vertexPath = null;
		[SerializeField]
		private float distanceTravelled = 0.0f;
		[SerializeField]
		private float distanceToEnd = 0.0f;
		[SerializeField]
		private bool isMoving = false;

		internal void SetData(VertexPath vp, float startDistance, float endDistance)
		{
			vertexPath = vp;
			distanceTravelled = startDistance;
			transform.position = vertexPath.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
			distanceToEnd = endDistance;
			isMoving = true;
		}

		// Update is called once per frame
		void Awake()
		{
			render = GetComponent<SpriteRenderer>();
			render.sprite = bloodCellSprites[Random.Range(0, bloodCellSprites.Count)];
			transform.Rotate(0, 0, Random.Range(0.0f, 360.0f));
		}

		void Update()
		{
			if (!isMoving) return;
			transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

			if (distanceTravelled >= distanceToEnd)
				distanceTravelled = 0.0f;

			if (vertexPath != null)
			{
				distanceTravelled += speed * Time.deltaTime;
				transform.position = vertexPath.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
				//transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
			}
		}
	}
}