using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ImmunotherapyGame.CellpediaSystem
{
	public class CellpediaPopup : MonoBehaviour
	{
		[SerializeField] private TMP_Text text = null;
		[SerializeField] private Image cellImage = null;
		[SerializeField] private CanvasGroup alphaGroup = null;
		[SerializeField] private float timeToHide = 3.0f;
		[SerializeField] private float timeBeforeHiding = 3.0f;

		internal void SetInfo(CellpediaCellDescription cellDescription)
		{
			text.text = cellDescription.cellName + " Unlocked!";
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
