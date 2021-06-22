using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ImmunotherapyGame.CellpediaSystem
{
	public class CellpediaPopup : MonoBehaviour
	{
		[SerializeField]
		private CanvasGroup alphaGroup = null;
		[SerializeField]
		private float timeBeforeHiding = 3.0f;
		[SerializeField]
		private float timeToHide = 3.0f;
		[SerializeField]
		private TMP_Text text = null;
		[SerializeField]
		private Image cellImage = null;

		internal void SetInfo(CellpediaObject cellDescription)
		{
			text.text = cellDescription.cellname + " Unlocked!";
			cellImage.sprite = cellDescription.sprite;
			cellImage.gameObject.transform.localScale = cellDescription.spriteUIScaleVector;
		}

		// Update is called once per frame
		void Update()
		{

			if (timeBeforeHiding >= 0.0f)
			{
				timeBeforeHiding -= Time.deltaTime;
			}
			else
			{
				alphaGroup.alpha -= Time.deltaTime / timeToHide;

				if (alphaGroup.alpha <= 0.0f)
				{
					Destroy(gameObject);
				}
			}
		}
	}
}
