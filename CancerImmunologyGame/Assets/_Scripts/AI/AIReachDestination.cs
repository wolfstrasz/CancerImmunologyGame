using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeBase;
using Pathfinding;
public class AIReachDestination : BTActionNode
{
	// Data obtained from controller
	private IAIMovementController controller;
	private Transform targetToReach;
	private Transform targetToMove;
	private float distanceSq;

	// Data to work with
	private Path path = null;
	private AIPathState pathState;
	private int currentWaypoint = 0;


	protected enum AIPathState { EMPTY, FOUND, ERROR, SEARCHING, ENDED }

	public AIReachDestination(string name, BehaviourTree owner, IAIMovementController controller) : base (name, owner, "AIReachDestination")
	{ 
		this.controller = controller;
	}

	protected override NodeStates OnEvaluateAction()
	{

		// PathState has been found 
		if (pathState == AIPathState.FOUND)
		{
			if (path == null)
			{
				Debug.LogError("Path is null but path was found!");
				nodeState = NodeStates.RUNNING;
				return nodeState;
			}

			// Check in a loop if we are close enough to the current waypoint to switch to the next one.
			// We do this in a loop because many waypoints might be close to each other and we may reach
			// several of them in the same frame.
			while (true)
			{

				float distanceToNextWaypointSQ = Vector3.SqrMagnitude(targetToMove.position - path.vectorPath[currentWaypoint]);
				if (distanceToNextWaypointSQ <= distanceSq)
				{

					currentWaypoint++;
					if (currentWaypoint >= path.vectorPath.Count)
					{
						pathState = AIPathState.ENDED;
						nodeState = NodeStates.SUCCESS;
						return nodeState;
					}
				}
				else
				{
					break;
				}
			}

			// MOVE THE CELL
			// Normalize it so that it has a length of 1 world unit
			Vector3 dir = (path.vectorPath[currentWaypoint] - targetToMove.position).normalized;
			controller.MovementDirection = dir;
			nodeState = NodeStates.RUNNING;
			return nodeState;
		}

		if (pathState == AIPathState.SEARCHING)
		{
			nodeState = NodeStates.RUNNING;
			return nodeState;
		}

		if (pathState == AIPathState.ENDED)
		{
			nodeState = NodeStates.SUCCESS;
			return nodeState;
		}

		if (pathState == AIPathState.EMPTY)
		{
			targetToMove = controller.ControlledCell.transform;
			if (targetToMove == null)
			{
				ActionNodeLogWarning("Did not receive a targed to move in movement data");
			}

			targetToReach = controller.Target.transform;
			if (targetToReach == null)
			{
				ActionNodeLogWarning("Did not receive a targed to reach in movement data");
			}

			distanceSq = controller.AcceptableDistanceFromTarget;
			distanceSq *= distanceSq;

			Debug.Log("HOWDY: " + targetToReach.gameObject.name + " -> " + targetToReach.localPosition + " " + targetToReach.position);
			if (Vector3.SqrMagnitude(targetToReach.position - targetToMove.position) < distanceSq)
			{
				nodeState = NodeStates.SUCCESS;
				pathState = AIPathState.ENDED;
				return nodeState;
			} else
			{
				pathState = AIPathState.SEARCHING;
				controller.PathSeeker.StartPath(targetToMove.position, targetToReach.position, OnPathComplete);
				nodeState = NodeStates.RUNNING;
			}
			
			return nodeState;
		}

		if (pathState == AIPathState.ERROR)
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
		currentWaypoint = 0;
		pathState = AIPathState.EMPTY;
		path = null;
		targetToMove = null;
		targetToReach = null;
		distanceSq = 1.0f;
	}


	public void OnPathComplete(Path p)
	{
		if (!p.error)
		{
			path = p;
			currentWaypoint = 0;
			pathState = AIPathState.FOUND; 
		}
		else
		{
			path = null;
			pathState = AIPathState.ERROR;
		}
	}

}
