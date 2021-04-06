using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Collider2D))]
public class AIGraphObstacle : MonoBehaviour
{
	private Vector3 prevPosition = Vector3.zero;

	private Collider2D coll;

	// Graph Updating
	[SerializeField]
	private float unitsError = 0.5f;
	[SerializeField]
	private float checkTime = 0.2f;
	[SerializeField]
	private float lastCheckTime = -10f;

	Bounds prevBounds;

	void Awake()
	{
		coll = GetComponent<Collider2D>();
		prevBounds = coll.bounds;
		
		if (AstarPath.active != null)
		{
			DoUpdateGraphs();
		}
	}

	void OnDisable()
	{
		coll.enabled = false;
		DoUpdateGraphs();
	}

	void OnEnable()
	{
		if (AstarPath.active)
		{
			coll.enabled = true;
			DoUpdateGraphs();
		}
	}
	// Update is called once per frame
	void Update()
	{
		if (AstarPath.active == null) return; // DO I NEED THIS?

		lastCheckTime -= Time.deltaTime;
		if (lastCheckTime <= 0.0f)
		{

			// The current bounds of the collider
			Vector3 posDifference = transform.position - prevPosition;

			// If the difference between the previous bounds and the new bounds is greater than some value, update the graphs
			if (posDifference.sqrMagnitude > (unitsError * unitsError))
			{
				// Update the graphs as soon as possible
				DoUpdateGraphs();
				prevPosition = transform.position;
			}

			lastCheckTime = checkTime;
		}
	}



	/// <summary>
	/// Update the graphs around this object.
	/// Note: The graphs will not be updated immediately since the pathfinding threads need to be paused first.
	/// If you want to guarantee that the graphs have been updated then call AstarPath.active.FlushGraphUpdates()
	/// after the call to this method.
	/// </summary>
	public void DoUpdateGraphs()
	{
		if (AstarPath.active == null) return;
		Bounds newBounds = coll.bounds;
		// Send two update request to update the nodes inside the 'prevBounds' and 'newBounds' volumes
		AstarPath.active.UpdateGraphs(newBounds);
		AstarPath.active.UpdateGraphs(prevBounds);
		prevBounds = newBounds;
	}

}
