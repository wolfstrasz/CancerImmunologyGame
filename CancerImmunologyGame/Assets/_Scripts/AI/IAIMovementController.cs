using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Cells;

// Final Interface
public interface IAIKillerCellController : 
	IAIMovementController, 
	IAIHelperCellInteractor
{
}

// Complex interfaces
public interface IAIMovementController : 
	IAICellController, IAITargetHandler
{
	Seeker PathSeeker { get; set; }
	Vector2 MovementDirection { get; set; }
}


public interface IAIHelperCellInteractor : 
	IAICellController, IAITargetHandler
{
	HelperTCell BookedHelperTCell { get; set; }
	GameObject BookingSpot { get; set; }
}


// Base interfaces
public interface IAITargetHandler : IAICellController
{
	GameObject Target { get; set; }
	float AcceptableDistanceFromTarget { get; set; }
}

public interface IAICellController
{
	KillerCell ControlledCell { get; set; }
}