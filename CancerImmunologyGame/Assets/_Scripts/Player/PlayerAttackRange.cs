using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
	public class PlayerAttackRange : MonoBehaviour
	{
		[SerializeField]
		private PlayerAttackRangeSettings settings = null;
		[SerializeField]
		private Image lowerFill = null;
		[SerializeField]
		private Image upperFill = null;
		[SerializeField]
		private RectTransform imageZ = null;
		[SerializeField]
		private RectTransform canvasTransform = null;

		internal void SetNewSettings(PlayerAttackRangeSettings settings)
		{
			this.settings = settings;
			lowerFill.fillAmount = 1.0f - settings.fovCutoff;
			upperFill.fillAmount = 1.0f - settings.fovCutoff;
			imageZ.anchorMin = new Vector2(settings.widthCentre, settings.heightCutoff);
			imageZ.anchorMax = new Vector2(settings.width, 1.0f - settings.heightCutoff);
			transform.localScale = new Vector3(settings.scaleFactor, settings.scaleFactor, 1.0f);
			canvasTransform.localPosition = new Vector3(settings.offsetFromCell, 0.0f, 0.0f);
		}


		internal void Initialise()
		{
			lowerFill.fillAmount = 1.0f - settings.fovCutoff;
			upperFill.fillAmount = 1.0f - settings.fovCutoff;
			imageZ.anchorMin = new Vector2(settings.widthCentre, settings.heightCutoff);
			imageZ.anchorMax = new Vector2(settings.width, 1.0f - settings.heightCutoff);
			transform.localScale = new Vector3(settings.scaleFactor, settings.scaleFactor, 1.0f);
			canvasTransform.localPosition = new Vector3(settings.offsetFromCell, 0.0f, 0.0f);

		}
		// Update is called once per frame
		void Update()
		{
			canvasTransform.localPosition = new Vector3(settings.offsetFromCell, 0.0f, 0.0f);
#if UNITY_EDITOR
			lowerFill.fillAmount = 1.0f - settings.fovCutoff;
			upperFill.fillAmount = 1.0f - settings.fovCutoff;
			imageZ.anchorMin = new Vector2 (settings.widthCentre, settings.heightCutoff);
			imageZ.anchorMax = new Vector2(settings.width, 1.0f - settings.heightCutoff);
			transform.localScale = new Vector3(settings.scaleFactor, settings.scaleFactor, 1.0f);
#endif

			Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			worldPosition.z = 0.0f;

			Debug.Log(worldPosition);
			Vector3 diff = worldPosition - transform.position;
			diff.Normalize();
			float spawnRotationAngle = ((Mathf.Atan2(diff.y, diff.x)) * Mathf.Rad2Deg);
			transform.rotation = Quaternion.Euler(0.0f, 0.0f, spawnRotationAngle);
			
		}
	}
}