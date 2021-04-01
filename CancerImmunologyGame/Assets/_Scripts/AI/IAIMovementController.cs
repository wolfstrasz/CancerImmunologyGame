using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public interface IAIMovementController : IAIKillerCellController
{
	Seeker GetSeeker();
	void UpdateMovementDirection(Vector3 directionVector);
	float GetAcceptableDistanceFromTarget();
}

public interface IAIKillerCellController
{
	KillerCell GetControlledCell();
	Transform GetControlledCellTransform();

	GameObject GetTarget();
	Transform GetTargetTransform();

	void SetTarget(GameObject target);
}
