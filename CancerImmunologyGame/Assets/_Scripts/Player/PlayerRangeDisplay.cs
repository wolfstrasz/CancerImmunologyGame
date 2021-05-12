using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using ImmunotherapyGame.Cancers;
namespace ImmunotherapyGame.Player
{
	public class PlayerRangeDisplay : MonoBehaviour
	{
		[SerializeField]
		private RectTransform outerCircle = null;
		[SerializeField]
		private Image fovImage = null;
		[SerializeField]
		internal Transform centre = null;

		[Header("Debug (Read Only)")]
		[SerializeField]
		private float range = 1.0f;
		[SerializeField]
		private float fov = 90.0f;
		[SerializeField]
		private float dull = 0.8f;

		internal Quaternion orientation => transform.rotation;
		KillerSense killerSense = null;
		Vector3 worldPosition = Vector3.zero;

		internal void Initialise(KillerCell kc)
		{
			killerSense = kc.Sense;
			fov = kc.Fov;
			range = kc.Range;
			fovImage.fillAmount = kc.Fov / 360.0f;
			outerCircle.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, kc.Fov / 2.0f);
			outerCircle.localScale = new Vector3(kc.Range * dull, kc.Range * dull, 1.0f);
		}


		// Update is called once per frame
		internal void OnUpdate()
		{
#if UNITY_EDITOR
			fovImage.fillAmount = fov / 360.0f;
			outerCircle.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, fov / 2.0f);
			outerCircle.localScale = new Vector3(range * dull, range * dull, 1.0f);
#endif


			if (GlobalGameData.autoAim)
			{
				if (!(killerSense.EvilCellsInRange.Count > 0)) return;

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

				worldPosition = closestCell.transform.position;
			}
			else
			{
				worldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
				//worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			}
			
			worldPosition.z = 0.0f;

			Vector3 diff = worldPosition - transform.position;
			diff.Normalize();
			float spawnRotationAngle = ((Mathf.Atan2(diff.y, diff.x)) * Mathf.Rad2Deg);
			transform.rotation = Quaternion.Euler(0.0f, 0.0f, spawnRotationAngle);
			
		}



	}
}