using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CellpediaUI
{
	public class Petridish : MonoBehaviour
	{
		private static Petridish nextPetridish = null;

		[Header("Links")]
		[SerializeField]
		private RectTransform trans = null;
		[SerializeField]
		private RectTransform parentTransform = null;
		[SerializeField]
		private GameObject cellVisual = null;
		[SerializeField]
		private Animator cellVisualAnimator;
		[SerializeField]
		private Image cellVisualSprite = null;

		[Header("Attributes")]
		[SerializeField]
		private bool isLeaving = false;
		[SerializeField]
		private static float timetopass = 2.0f;
		[SerializeField]
		private float scalingFactor = 0.75f;

		[Header("Debugging")]
		[SerializeField]
		private float xStartCoord = 0.0f;
		[SerializeField]
		private static float xShiftCoord = 0.0f;
		[SerializeField]
		internal bool isShifting = false;
		[SerializeField]
		private float timepassed = 0.0f;

		public static float Timetopass { get => timetopass; set => timetopass = value; }

		// Start is called before the first frame update
		void Start()
		{
			xShiftCoord = parentTransform.rect.width;
			if (isLeaving)
			{
				xStartCoord = 0.0f;
			}
			else
			{
				xStartCoord = xShiftCoord;
				nextPetridish = this;
			}
			trans.localScale = new Vector3(scalingFactor, scalingFactor, 1.0f) ;
			
			trans.localPosition = new Vector2(xStartCoord, 0.0f);
		}


		// Update is called once per frame
		void Update()
		{
			if (!isShifting) return;

			if (isShifting && timepassed < Timetopass)
			{
				timepassed += Time.deltaTime;
				if (timepassed > Timetopass)
					timepassed = Timetopass;


				trans.localPosition = new Vector2(xStartCoord - (timepassed / Timetopass) * xShiftCoord, 0.0f);
				return;
			}

			Reset();
		}

		private void Reset()
		{
			isShifting = false;
			timepassed = 0.0f;

			if (isLeaving)
			{
				xStartCoord = xShiftCoord;
				nextPetridish = this;
			}
			else
			{
				xStartCoord = 0.0f;
			}
			isLeaving = !isLeaving;
			trans.localPosition = new Vector2(xStartCoord, 0.0f);
		}

		internal void ShiftLeft()
		{
			timepassed = 0.0f;
			isShifting = true;
		}

		internal void SetVisual(CellDescription cd)
		{
			Debug.Log("Setting visual for: " + gameObject.name);
			float scaleValue = 0.5f * cd.scale;
			cellVisual.transform.localScale = new Vector3(scaleValue, scaleValue, 1.0f);
			//cellVisualAnimator.SetTrigger(cd.animatorTrigger);
			cellVisualAnimator.Play(cd.animatorTrigger);
			cellVisualSprite.sprite = cd.sprite;
		}

	}
}