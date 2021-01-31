using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cancers;

namespace Player
{
	public class PlayerRangeDisplay : MonoBehaviour
	{
		[SerializeField]
		private RectTransform outerCircle = null;
		[SerializeField]
		private Image fovImage = null;

		[SerializeField]
		private float range = 1.0f;
		[SerializeField]
		private float fov = 90.0f;

		internal void Initialise(float _range, float _fov)
		{
			fov = _fov;
			range = _range;
			fovImage.fillAmount = _fov / 360.0f;
			outerCircle.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, _fov / 2.0f);
			outerCircle.localScale = new Vector3(_range, _range, 1.0f);
		}


		// Update is called once per frame
		void Update()
		{
#if UNITY_EDITOR
			fovImage.fillAmount = fov / 360.0f;
			outerCircle.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, fov / 2.0f);
			outerCircle.localScale = new Vector3(range, range, 1.0f);
#endif

			Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			worldPosition.z = 0.0f;

			Vector3 diff = worldPosition - transform.position;
			diff.Normalize();
			float spawnRotationAngle = ((Mathf.Atan2(diff.y, diff.x)) * Mathf.Rad2Deg);
			transform.rotation = Quaternion.Euler(0.0f, 0.0f, spawnRotationAngle);
			
		}

	}
}