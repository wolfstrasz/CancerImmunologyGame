using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using ImmunotherapyGame.Cancers;
namespace ImmunotherapyGame.Player
{
	public class PlayerAutoAimer : MonoBehaviour
	{
		[Header ("Auto-aim")]
		[SerializeField]
		KillerSense killerSense = null;
		[SerializeField]
		internal PlayerController owner = null;

		// Update is called once per frame
		internal void OnUpdate()
		{
			if (killerSense != owner.KC.Sense)
			{
				killerSense = owner.KC.Sense;
			}

			if (!(killerSense.EvilCellsInRange.Count > 0))
			{
				return;
			}

			EvilCell closestCell = killerSense.EvilCellsInRange[0];
			float minDist = Vector3.SqrMagnitude(closestCell.transform.position - transform.position);

			for (int i = 1; i < killerSense.EvilCellsInRange.Count; ++i)
			{
				float dist = Vector3.SqrMagnitude(killerSense.EvilCellsInRange[i].transform.position - transform.position);
				if (dist < minDist)
				{
					minDist = dist;
					closestCell = killerSense.EvilCellsInRange[i];
				}
			}

			// Obtain auto-aim pointer position
			Vector3 worldPosition = closestCell.transform.position;
			worldPosition.z = 0.0f;

			// Obtain 3D direction
			Vector3 direction = worldPosition - transform.position;
			direction.Normalize();

			// Calculate 2D rotation
			float rotationAngle = ((Mathf.Atan2(direction.y, direction.x)) * Mathf.Rad2Deg);
			owner.CrosshairRotation = Quaternion.Euler(0.0f, 0.0f, rotationAngle);
		}

	}
}