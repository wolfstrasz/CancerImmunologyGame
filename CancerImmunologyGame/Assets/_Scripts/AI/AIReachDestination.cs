using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeBase;
using Pathfinding;
using Pathfinding.RVO;

namespace ImmunotherapyGame.AI
{
	public class AIReachDestination : BTActionNode
	{
		private AIPathfindingData pathfindingData;
		private AITargetingData targetData;
		private AIControlledCellData controlledCellData;

		// Controller and objects cached data
		private GameObject targetObjectToReach;
		private GameObject targetObjectToMove;

		// AStar handling: attributes (obtained from controller
		private float acceptableDistanceFromTargetSq;

		// AStar handling: data from search
		private Path path = null;
		private int currentWaypoint = 0;
		private List<Vector3> vectorPath = new List<Vector3>();
		private float timePassedForRepath = 0f;

		// AStar handling: State machine
		private AIPathState aiPathState;
		protected enum AIPathState { NEEDTOSEARCH, FOUND, ERROR, SEARCHING, ENDED }

		public AIReachDestination(string name, BehaviourTree owner, AIControlledCellData controlledCellData, AIPathfindingData pathfindingData, AITargetingData targetData) : base(name, owner, "AIReachDestination")
		{
			this.controlledCellData = controlledCellData;
			this.pathfindingData = pathfindingData;
			this.targetData = targetData;
		}


		private NodeStates MoveTarget()
		{
			if (path == null)
			{
				Debug.LogError("Path is null but path was found!");
				nodeState = NodeStates.RUNNING;
				return nodeState;
			}

			// Cache the value for more optimised use
			Vector3 targetObjectToMovePosition = targetObjectToMove.transform.position;

			if (Vector3.SqrMagnitude(targetObjectToMovePosition - targetObjectToReach.transform.position) <= acceptableDistanceFromTargetSq)
			{
				//rvoController.locked = true;
				nodeState = NodeStates.SUCCESS;
				aiPathState = AIPathState.ENDED;
				timePassedForRepath = 0.0f;
				path.Release(this);
				path = null;

				pathfindingData.graphObstacle.SetActive(true);
				pathfindingData.rvoController.locked = true;

				return nodeState;
			}

			if (vectorPath == null || vectorPath.Count == 0)
			{
				//rvoController.lockWhenNotMoving = true;
				//rvoController.locked = true;
				float speed = controlledCellData.speed;
				pathfindingData.rvoController.SetTarget(targetObjectToMovePosition, speed, speed);
			}
			else
			{
				pathfindingData.rvoController.locked = false;
				pathfindingData.graphObstacle.SetActive(false);
				//rvoController.lockWhenNotMoving = false;
				//rvoController.locked = false;

				// Check in a loop if we are close enough to the current waypoint to switch to the next one.
				// We do this in a loop because many waypoints might be close to each other and we may reach
				// several of them in the same frame.
				float distanceToNextWaypointSQ = Vector3.SqrMagnitude(targetObjectToMovePosition - path.vectorPath[currentWaypoint]);
				while (distanceToNextWaypointSQ < pathfindingData.movementLookAhead * pathfindingData.movementLookAhead && currentWaypoint != (vectorPath.Count - 1))
				{
					currentWaypoint++;
					distanceToNextWaypointSQ = Vector3.SqrMagnitude(targetObjectToMovePosition - path.vectorPath[currentWaypoint]);
				}

				// Obtain path point at MovementLookAhead distance
				// Current path segment goes from vectorPath[wp-1] to vectorPath[wp]
				// We want to find the point on that segment that is 'moveNextDist' from our current position.
				// This can be visualized as finding the intersection of a circle with radius 'moveNextDist'
				// centered at our current position with that segment.
				var p1 = (currentWaypoint > 0) ? vectorPath[currentWaypoint - 1] : vectorPath[0];
				var p2 = vectorPath[currentWaypoint];

				// Calculate the intersection with the circle.
				var t = VectorMath.LineCircleIntersectionFactor(targetObjectToMovePosition, p1, p2, pathfindingData.movementLookAhead);

				// Clamp to a point on the segment
				t = Mathf.Clamp01(t);
				Vector3 intersectionWaypoint = Vector3.Lerp(p1, p2, t);

				// Obtain the remaining distance to goal
				float remainingDistance = (intersectionWaypoint - targetObjectToMovePosition).magnitude + (intersectionWaypoint - p2).magnitude;
				//float remainingDistance = (intersectionWaypoint - currentTargetToMovePosition).magnitude;
				for (int i = currentWaypoint; i < vectorPath.Count - 1; i++)
				{
					remainingDistance += (vectorPath[i + 1] - vectorPath[i]).magnitude;
				}

				// Obtain weight vector for local avoidance, where remainingDistance is the weight, 
				var direction = (intersectionWaypoint - targetObjectToMovePosition).normalized;
				var rvoTarget = direction * remainingDistance + targetObjectToMovePosition;
				var desiredSpeed = Mathf.Clamp01(remainingDistance / pathfindingData.slowdownDistance) * controlledCellData.speed;

				pathfindingData.rvoController.SetTarget(rvoTarget, controlledCellData.speed, controlledCellData.speed);

			}

			// Process RVO data

			// Get a processed movement delta from the rvo controller and move the character.
			// This is based on information from earlier frames.
			var movementDelta = pathfindingData.rvoController.CalculateMovementDelta(Time.deltaTime);

			// Transform into raw direction vector (non-normalized) that afterwards the cell will use to move to the same position
			// after AIController applies it
			Vector2 raw;
			raw.x = movementDelta.x;// / controller.ControlledCell.movementSpeed;
			raw.y = movementDelta.y;// / controller.ControlledCell.movementSpeed;
			controlledCellData.movementDirection = raw.normalized;

			nodeState = NodeStates.RUNNING;
			return nodeState;
		}


		protected override NodeStates OnEvaluateAction()
		{
			// Fail on no target
			if (targetData.currentTarget == null)
			{
				nodeState = NodeStates.FAILURE;
				return nodeState;
			}

			// Check if we need to find a new path (Repathing)
			if (!(aiPathState == AIPathState.SEARCHING))
			{
				timePassedForRepath -= Time.deltaTime;
				if (targetObjectToReach != targetData.currentTarget)
				{
					Debug.Log("Different target to reach and controller target. -> Will Search for new path");
					timePassedForRepath = pathfindingData.repathRate;
					aiPathState = AIPathState.NEEDTOSEARCH;
				}
				if (timePassedForRepath <= 0f)
				{
					Debug.Log("Repathing");
					timePassedForRepath = pathfindingData.repathRate;
					aiPathState = AIPathState.NEEDTOSEARCH;
				}
			}

			if (aiPathState == AIPathState.NEEDTOSEARCH)
			{
				Debug.Log("Need to search for new path!");

				if (controlledCellData.controlledObject == null)
				{
					ActionNodeLogWarning("Controller is missing a controlled cell");
					nodeState = NodeStates.FAILURE;
					return nodeState;
				}

				targetObjectToMove = controlledCellData.controlledObject;

				if (targetObjectToMove == null)
				{
					ActionNodeLogWarning("Did not receive a targed to move in movement data");
					nodeState = NodeStates.FAILURE;
					return nodeState;
				}

				targetObjectToReach = targetData.currentTarget;
				if (targetObjectToReach == null)
				{
					ActionNodeLogWarning("Did not receive a targed to reach in movement data");
					nodeState = NodeStates.FAILURE;
					return nodeState;
				}

				// Calculate acceptable distance
				acceptableDistanceFromTargetSq = targetData.acceptableDistanceFromCurrentTarget * targetData.acceptableDistanceFromCurrentTarget;

				// Check if we are already there
				if (Vector3.SqrMagnitude(targetObjectToReach.transform.position - targetObjectToMove.transform.position) < acceptableDistanceFromTargetSq)
				{
					Debug.Log("Already reached target position");

					// Make cell an obstacle
					pathfindingData.SetObstacleActive(true);
				

					// Update states
					aiPathState = AIPathState.ENDED;
					nodeState = NodeStates.SUCCESS;
					return nodeState;
				}
				else
				{
					Debug.Log("Searching for new path");
					// Stop cell from being an obstacle
					pathfindingData.SetObstacleActive(false);

					// Request path
					pathfindingData.pathSeeker.StartPath(targetObjectToMove.transform.position, targetObjectToReach.transform.position, OnPathComplete);

					// Update State
					aiPathState = AIPathState.SEARCHING;
					nodeState = NodeStates.RUNNING;
				}

				return nodeState;
			}

			// Safe guard check
			if (targetObjectToMove == null || targetObjectToReach == null)
			{
				nodeState = NodeStates.FAILURE;
				return nodeState;
			}

			// PathState has been found 
			if (aiPathState == AIPathState.FOUND)
			{
				return MoveTarget();
			}

			if (aiPathState == AIPathState.SEARCHING)
			{
				nodeState = NodeStates.RUNNING;
				return nodeState;
			}

			if (aiPathState == AIPathState.ENDED)
			{
				targetObjectToReach = null;

				// Enable cell as obstacle in graph
				pathfindingData.SetObstacleActive(true);
			
				nodeState = NodeStates.SUCCESS;
				return nodeState;
			}

			if (aiPathState == AIPathState.ERROR)
			{
				nodeState = NodeStates.FAILURE;
				return nodeState;
			}

			// Safe to report failure even if it this code is not reachable
			nodeState = NodeStates.FAILURE;
			return nodeState;
		}

		protected override void OnResetTreeNode()
		{

		}


		public void OnPathComplete(Path p)
		{

			aiPathState = AIPathState.FOUND;

			if (path != null) path.Release(this);
			path = p;
			p.Claim(this);

			if (p.error)
			{
				currentWaypoint = 0;
				vectorPath = null;
				aiPathState = AIPathState.ERROR;
			}

			// else
			currentWaypoint = 0;
			vectorPath = p.vectorPath;

			aiPathState = AIPathState.FOUND;


		}

	}
}
