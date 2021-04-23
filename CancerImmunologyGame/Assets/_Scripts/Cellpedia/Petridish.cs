using UnityEngine;
using UnityEngine.UI;

namespace ImmunotherapyGame.CellpediaSystem
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
		private Animator cellVisualAnimator = null;
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

			if (isShifting && timepassed < timetopass)
			{
				timepassed += Time.deltaTime;
				if (timepassed > timetopass)
					timepassed = timetopass;


				trans.localPosition = new Vector2(xStartCoord - (timepassed / timetopass) * xShiftCoord, 0.0f);
				return;
			}

			Reset();
		}

		internal void SkipAnimation()
		{
			if (isShifting)
			{
				timepassed = timetopass;
				trans.localPosition = new Vector2(xStartCoord - xShiftCoord, 0.0f);
				Reset();
				return;
			}
		}

		internal  void Reset()
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
			float scaleValue = scalingFactor * cd.scale;
			cellVisual.transform.localScale = new Vector3(scaleValue, scaleValue, 1.0f);
			//cellVisualAnimator.SetTrigger(cd.animatorTrigger);
			cellVisualAnimator.Play(cd.animatorTrigger);
			cellVisualSprite.sprite = cd.sprite;
		}

	}
}