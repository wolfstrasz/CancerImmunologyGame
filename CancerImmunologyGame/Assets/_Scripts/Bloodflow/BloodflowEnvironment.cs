using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using ImmunotherapyGame.Core;
using ImmunotherapyGame.Player;

namespace ImmunotherapyGame.Bloodflow
{
	public class BloodflowEnvironment : Singleton<BloodflowEnvironment>, IControllerMovementOverride
	{
		[Header("Attributes")]
		[SerializeField] private List<PathCreator> allPathCreators = new List<PathCreator>();
		[SerializeField] private float flow_speed = 7.0f;
		[SerializeField] private bool flipNormal = true;

		[Header("Debug")]
		[SerializeField] [ReadOnly] internal GameObject overridableObject;
		[SerializeField] [ReadOnly] int count = 0;
		[SerializeField] [ReadOnly] int currentPathIndex = -1;
		[SerializeField] [ReadOnly] private List<VertexPath> allPaths = new List<VertexPath>();

		private float FlippedNormal => flipNormal ? -1.0f : 1.0f;
		private VertexPath currentPath { get; set; }

		private void OnEnable()
		{
			CreatePaths();
			GlobalLevelData.BloodflowEnvironments.Add(this);
		}

		private void OnDisable()
		{
			GlobalLevelData.BloodflowEnvironments.Remove(this);
		}

		public void OnFixedUpdate()
		{
			if (overridableObject != null)
			{
				// Find Closest path
				SetClosestPath();
			}
		}

		public void CreatePaths()
		{
			allPaths.Clear();
			for (int i = 0; i < allPathCreators.Count; ++i)
			{
				allPaths.Add(allPathCreators[i].path);
			}
			count = 0;
		}

		private void SetClosestPath()
		{
			float minDist = 1000f;
			for (int i = 0; i< allPaths.Count; ++i)
			{
				float dist = (allPaths[i].GetClosestPointOnPath(overridableObject.transform.position) - overridableObject.transform.position).magnitude;
				if (dist < minDist)
				{
					currentPath = allPaths[i];
					minDist = dist;
					currentPathIndex = i;
				}
			}
		}


		public void ApplyOverride(ref Vector2 movementVector, ref Quaternion newRotation, ref Vector3 position)
		{
			if (allPathCreators.Count <= 0)
			{
				Debug.LogWarning("Bloodflow Environment has 0 assigned path creators");
				return;
			}

			// Find normal at position
			float time = currentPath.GetClosestTimeOnPath(position);
			Debug.Log(time);
			Vector2 direction = currentPath.GetDirection(time);
			Debug.Log(direction);

			Vector2 constraintNormal = currentPath.GetNormal(time) * FlippedNormal;

			// Calculate flow movement
			Vector2 flowMoveVector = direction * flow_speed * Time.fixedDeltaTime;

			// Calculate rotation
			var rotationAngle = Mathf.Atan2(constraintNormal.y, constraintNormal.x) * Mathf.Rad2Deg;
			newRotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);

			// Update movement vector
			movementVector.y = 0.0f;
			movementVector = newRotation * movementVector;
			movementVector += flowMoveVector;
		}


		internal void AddOverridable(GameObject overridable)
		{
			if (overridableObject != overridable)
			{
				if (overridableObject != null)
				{
					overridableObject.GetComponent<IControllerMovementOverridable>().UnsubscribeMovementOverride(this);
					currentPathIndex = -1;

				}
				overridableObject = overridable;
				count = 1;
				overridableObject.GetComponent<IControllerMovementOverridable>().SubscribeMovementOverride(this);
				SetClosestPath();
			}
			else
			{
				++count;
			}
		}

		internal void RemoveOverridable(GameObject overridable)
		{
			if (overridableObject == overridable)
			{
				--count;
				if (count == 0)
				{
					overridableObject.GetComponent<IControllerMovementOverridable>().UnsubscribeMovementOverride(this);
					overridableObject = null;
					currentPathIndex = -1;
				}
			}
		}
	}
}