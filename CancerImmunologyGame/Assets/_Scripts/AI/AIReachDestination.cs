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
		// Data obtained from controller
		private IAIMovementController controller;
		private Transform targetToReach;
		private Transform targetToMove;
		private float distanceSq;

		// Data to work with
		private Path path = null;
		private AIPathState aiPathState;
		private int currentWaypoint = 0;
		private List<Vector3> vectorPath = new List<Vector3>();
		private float repathRate = 1f;
		private float timePassedForRepath = 0f;
		private float movementLookAhead = 0f;
		private float slowdownDistance = 0f;
		RVOController rvoController = null;

		protected enum AIPathState { NEEDTOSEARCH, FOUND, ERROR, SEARCHING, ENDED }

		public AIReachDestination(string name, BehaviourTree owner, IAIMovementController controller) : base(name, owner, "AIReachDestination")
		{
			this.controller = controller;
			rvoController = controller.RVOController;
			repathRate = controller.RepathRate;
			movementLookAhead = controller.MovementLookAhead;
			slowdownDistance = controller.SlowdownDistance;
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
			Vector3 currentTargetToMovePosition = targetToMove.position;

			if (Vector3.SqrMagnitude(currentTargetToMovePosition - targetToReach.position) <= distanceSq)
			{
				//rvoController.locked = true;
				nodeState = NodeStates.SUCCESS;
				aiPathState = AIPathState.ENDED;
				timePassedForRepath = 0.0f;
				path.Release(this);
				path = null;

				controller.GraphObstacle.SetActive(true);
				rvoController.locked = true;

				return nodeState;
			}

			if (vectorPath == null || vectorPath.Count == 0)
			{
				//rvoController.lockWhenNotMoving = true;
				//rvoController.locked = true;
				float speed = controller.ControlledCell.movementSpeed;
				rvoController.SetTarget(currentTargetToMovePosition, speed, speed);
			}
			else
			{
				rvoController.locked = false;
				controller.GraphObstacle.SetActive(false);
				//rvoController.lockWhenNotMoving = false;
				//rvoController.locked = false;

				// Check in a loop if we are close enough to the current waypoint to switch to the next one.
				// We do this in a loop because many waypoints might be close to each other and we may reach
				// several of them in the same frame.
				float distanceToNextWaypointSQ = Vector3.SqrMagnitude(currentTargetToMovePosition - path.vectorPath[currentWaypoint]);
				while (distanceToNextWaypointSQ < movementLookAhead * movementLookAhead && currentWaypoint != (vectorPath.Count - 1))
				{
					currentWaypoint++;
					distanceToNextWaypointSQ = Vector3.SqrMagnitude(currentTargetToMovePosition - path.vectorPath[currentWaypoint]);
				}

				// Obtain path point at MovementLookAhead distance
				// Current path segment goes from vectorPath[wp-1] to vectorPath[wp]
				// We want to find the point on that segment that is 'moveNextDist' from our current position.
				// This can be visualized as finding the intersection of a circle with radius 'moveNextDist'
				// centered at our current position with that segment.
				var p1 = (currentWaypoint > 0) ? vectorPath[currentWaypoint - 1] : vectorPath[0];
				var p2 = vectorPath[currentWaypoint];

				// Calculate the intersection with the circle. This involves some math.
				var t = VectorMath.LineCircleIntersectionFactor(currentTargetToMovePosition, p1, p2, movementLookAhead);

				// Clamp to a point on the segment
				t = Mathf.Clamp01(t);
				Vector3 intersectionWaypoint = Vector3.Lerp(p1, p2, t);

				//Vector3 intersectionWaypoint = vectorPath[currentWaypoint];
				//Debug.Log("Controller of: " + controller.ControlledCell.gameObject.name + " has found current waypoint to be: " + intersectionWaypoint);
				// Obtain the remaining distance to goal
				float remainingDistance = (intersectionWaypoint - currentTargetToMovePosition).magnitude + (intersectionWaypoint - p2).magnitude;
				//float remainingDistance = (intersectionWaypoint - currentTargetToMovePosition).magnitude;
				for (int i = currentWaypoint; i < vectorPath.Count - 1; i++)
				{
					remainingDistance += (vectorPath[i + 1] - vectorPath[i]).magnitude;
				}

				// Obtain weight vector for local avoidance, where remainingDistance is the weight, 
				var direction = (intersectionWaypoint - currentTargetToMovePosition).normalized;
				var rvoTarget = direction * remainingDistance + currentTargetToMovePosition;
				var desiredSpeed = Mathf.Clamp01(remainingDistance / slowdownDistance) * controller.ControlledCell.movementSpeed;

				rvoController.SetTarget(rvoTarget, controller.ControlledCell.movementSpeed, controller.ControlledCell.movementSpeed);

			}

			// Process RVO data

			// Get a processed movement delta from the rvo controller and move the character.
			// This is based on information from earlier frames.
			var movementDelta = rvoController.CalculateMovementDelta(Time.deltaTime);

			// Transform into raw direction vector (non-normalized) that afterwards the cell will use to move to the same position
			// after AIController applies it
			Vector2 raw;
			raw.x = movementDelta.x;// / controller.ControlledCell.movementSpeed;
			raw.y = movementDelta.y;// / controller.ControlledCell.movementSpeed;
			controller.MovementDirection = raw.normalized;

			nodeState = NodeStates.RUNNING;
			return nodeState;
		}


		protected override NodeStates OnEvaluateAction()
		{
			if (controller.Target == null)
			{
				nodeState = NodeStates.FAILURE;
				return nodeState;
			}


			if (!(aiPathState == AIPathState.SEARCHING))
			{

				timePassedForRepath -= Time.deltaTime;
				if (targetToReach != controller.Target.transform)
				{
					Debug.Log("Different target to reach and controller target. -> Will Search for new path");
					timePassedForRepath = repathRate;
					aiPathState = AIPathState.NEEDTOSEARCH;
				}
				if (timePassedForRepath <= 0f)
				{
					Debug.Log("Repathing");
					timePassedForRepath = repathRate;
					aiPathState = AIPathState.NEEDTOSEARCH;
				}
			}

			if (aiPathState == AIPathState.NEEDTOSEARCH)
			{
				Debug.Log("Need to search for new path!");
				targetToMove = controller.ControlledCell.transform;
				if (targetToMove == null)
				{
					ActionNodeLogWarning("Did not receive a targed to move in movement data");
					nodeState = NodeStates.FAILURE;
					return nodeState;
				}

				targetToReach = controller.Target.transform;
				if (targetToReach == null)
				{
					ActionNodeLogWarning("Did not receive a targed to reach in movement data");
					nodeState = NodeStates.FAILURE;
					return nodeState;
				}

				distanceSq = controller.AcceptableDistanceFromTarget;
				distanceSq *= distanceSq;

				if (Vector3.SqrMagnitude(targetToReach.position - targetToMove.position) < distanceSq)
				{
					Debug.Log("Already reached target position");

					nodeState = NodeStates.SUCCESS;
					aiPathState = AIPathState.ENDED;
					controller.GraphObstacle.SetActive(true);
					rvoController.locked = true;
					return nodeState;
				}
				else
				{
					Debug.Log("Searching for new path");

					controller.GraphObstacle.SetActive(false);
					rvoController.locked = false;
					aiPathState = AIPathState.SEARCHING;
					controller.PathSeeker.StartPath(targetToMove.position, targetToReach.position, OnPathComplete);
					nodeState = NodeStates.RUNNING;
				}

				return nodeState;
			}

			if (targetToMove == null || targetToReach == null)
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
				targetToReach = null;
				nodeState = NodeStates.SUCCESS;
				controller.GraphObstacle.SetActive(true);
				rvoController.locked = true;
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
