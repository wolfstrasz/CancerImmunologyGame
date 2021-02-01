using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CellpediaUI
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
		[SerializeField]
		private GameObject particleEffectPrefab = null;

		internal void SetInfo(CellDescription cellDescription)
		{
			text.text = cellDescription.cellname + " Unlocked!";
			cellImage.sprite = cellDescription.sprite;
			cellImage.gameObject.transform.localScale = cellDescription.scaleVector;

			//if (particleEffectPrefab != null)
			//{
			//	CellpediaPopupEffect goEffect = Instantiate(particleEffectPrefab, cellImage.transform.position, Quaternion.identity, Cellpedia.Instance.PopupLayout).GetComponent<CellpediaPopupEffect>();
			//	goEffect.SetData(Cellpedia.Instance.MicroscopeButtonPosition, cellDescription, cellImage.gameObject.GetComponent<RectTransform>());
			//}
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
