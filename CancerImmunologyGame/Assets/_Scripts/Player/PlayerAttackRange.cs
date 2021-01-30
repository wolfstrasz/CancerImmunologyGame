using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cancers;

namespace Player
{
	public class PlayerAttackRange : MonoBehaviour, ICancerCellObserver
	{
		[SerializeField]
		private PlayerAttackRangeSettings settings = null;
		[SerializeField]
		private GameObject canvas = null;
		[SerializeField]
		private List<CancerCell> ccs = new List<CancerCell>();

		[SerializeField]
		private Image lowerFill = null;
		[SerializeField]
		private Image upperFill = null;
		[SerializeField]
		private RectTransform imageZ = null;
		[SerializeField]
		private RectTransform canvasTransform = null;

		internal bool CanAttack => canvas.activeInHierarchy;
		public Quaternion orientation { get => transform.rotation; }
		void OnTriggerEnter2D(Collider2D collider)
		{
			Debug.Log(collider.gameObject);
			CancerCellBody ccBody = collider.GetComponent<CancerCellBody>();
			if (ccBody != null)
			{
				Debug.Log("Collided with cc body");
				ccs.Add(ccBody.owner);
				ccBody.owner.AddObserver(this);
				canvas.SetActive(true);
			}
		}

		void OnTriggerExit2D(Collider2D collider)
		{
			CancerCellBody ccBody = collider.GetComponent<CancerCellBody>();
			if (ccBody != null)
			{
				Debug.Log("UN-Collided with cc body");

				ccs.Remove(ccBody.owner);
				ccBody.owner.RemoveObserver(this);
				if (ccs.Count <= 0)
				{
					canvas.SetActive(false);
				}
			}
		}

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

			Vector3 diff = worldPosition - transform.position;
			diff.Normalize();
			float spawnRotationAngle = ((Mathf.Atan2(diff.y, diff.x)) * Mathf.Rad2Deg);
			transform.rotation = Quaternion.Euler(0.0f, 0.0f, spawnRotationAngle);
			
		}

		public void NotifyOfDeath(CancerCell cc)
		{
			ccs.Remove(cc);
			if (ccs.Count <= 0)
			{
				canvas.SetActive(false);
			}
		}
	}
}