using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;


namespace ImmunotherapyGame
{
    public class MoveByPath : MonoBehaviour
    {
        [SerializeField] private GameObject objectToMove;
        [SerializeField] private PathCreator path = null;
        [SerializeField] private float speed = 2f;
        [SerializeField] [ReadOnly] private float distanceTravelled = 0f;
        [SerializeField] [ReadOnly] private float pathLengthDistance = 0f;
        private VertexPath pathToFollow = null;

		public void OnEnable()
		{
            distanceTravelled = 0f;
            pathToFollow = path.path;
			pathLengthDistance = pathToFollow.length;
            objectToMove.transform.position = pathToFollow.GetPointAtDistance(distanceTravelled, EndOfPathInstruction.Loop);
		}

		public void FixedUpdate()
		{
            distanceTravelled += Time.fixedDeltaTime * speed;
            if (distanceTravelled < pathLengthDistance)
            {
                Vector3 newPos = pathToFollow.GetPointAtDistance(distanceTravelled, EndOfPathInstruction.Loop);
                objectToMove.transform.position = newPos;
            }

        }

	}
}
