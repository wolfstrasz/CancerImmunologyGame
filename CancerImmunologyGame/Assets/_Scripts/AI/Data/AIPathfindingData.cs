using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Pathfinding.RVO;


namespace ImmunotherapyGame.AI
{
    [System.Serializable]
    public class AIPathfindingData 
    {
		[Header("Pathfinding Data")]
		public Seeker pathSeeker;
		public RVOController rvoController;
		public float repathRate;
		public float movementLookAhead;
		public float slowdownDistance;
		public AIGraphObstacle graphObstacle;

		public void SetObstacleActive(bool isActive)
		{
			graphObstacle.SetActive(isActive);
			rvoController.locked = isActive;
		}
	}
}
