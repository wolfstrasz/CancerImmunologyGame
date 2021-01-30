using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
	public class PlayerFOV : MonoBehaviour
	{
		[SerializeField]
		private float fov = 90.0f;
		public int rayCount = 2;
		public float angle = 0f;
		private float angleIncrease;
		public float viewDistance = 50f;
		public int sortOrder = 10000;
		public MeshRenderer render = null;

		// COUNTER CLOCKWISE
		// Start is called before the first frame update
		void Start()
		{
			Mesh mesh = new Mesh();
			GetComponent<MeshFilter>().mesh = mesh;
			Vector3 origin = Vector3.zero;
			// Define stats

			 float angleIncrease = fov / rayCount;
			// Setup vertices uv, triangles
			Vector3[] vertices = new Vector3[rayCount + 1 + 1];
			Vector2[] uv = new Vector2[vertices.Length];
			int[] triangles = new int[rayCount * 3];

			vertices[0] = origin;
			int vertexIndex = 1;
			int triangleIndex = 0;

			for (int i = 0; i <= rayCount; ++i)
			{
				Vector3 vertex = origin + Utils.GetVectorFromAngle(angle) * viewDistance;
				vertices[vertexIndex] = vertex;

				if (i > 0)
				{
					triangles[triangleIndex + 0] = 0;
					triangles[triangleIndex + 1] = vertexIndex - 1;
					triangles[triangleIndex + 2] = vertexIndex;
					triangleIndex += 3;
				}
				++vertexIndex;
				angle -= angleIncrease;
			}

			mesh.vertices = vertices;
			mesh.uv = uv;
			mesh.triangles = triangles;
			render.sortingOrder = sortOrder;

		}

		// Update is called once per frame
		void Update()
		{

		}
	}
}