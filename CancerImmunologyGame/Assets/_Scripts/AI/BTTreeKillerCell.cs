using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeBase;
using Pathfinding;

public class BTTreeKillerCell : BehaviourTree
{
	[Header ("Killer Cell AI")]
	public KillerCell cell;
	public Transform healTarget = null;
	public Transform baseTarget = null;
	public AIDestinationSetter setter = null;
	public AIPath path = null;

	void Start()
	{

		
	}

	public override void ResetTree()
	{

	}
}
