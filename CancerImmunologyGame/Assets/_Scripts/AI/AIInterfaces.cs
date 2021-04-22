using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Pathfinding.RVO;
using ImmunotherapyGame.Cancers;

namespace ImmunotherapyGame.AI
{
	// Final Interface
	public interface IAIKillerCellController :
	IAIMovementController,
	IAIHelperCellInteractor,
	IAICancerCellInteractor,
	IAICancerInteractor
	{
	}

	// Complex interfaces
	public interface IAIMovementController :
		IAICellController, IAITargetHandler
	{
		Seeker PathSeeker { get; set; }
		Vector2 MovementDirection { get; set; }
		float RepathRate { get; }
		float MovementLookAhead { get; }
		float SlowdownDistance { get; }
		RVOController RVOController { get; }
		GameObject GraphObstacle { get; }
	}


	public interface IAICancerInteractor :
		IAICancerCellInteractor
	{
		GameObject BasePoint { get; set; }
		List<GameObject> ControlPoints { get; set; }
		List<Cancer> TargetCancers { get; set; }
	}

	public interface IAIHelperCellInteractor :
		IAICellController, IAITargetHandler
	{
		HelperTCell BookedHelperTCell { get; set; }
		GameObject BookingSpot { get; set; }
	}

	public interface IAICancerCellInteractor : IAICellController, IAITargetHandler
	{
		EvilCell TargetedEvilCell { get; set; }
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
}