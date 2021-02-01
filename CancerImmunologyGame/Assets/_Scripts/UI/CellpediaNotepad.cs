using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CellpediaUI
{
	public class CellpediaNotepad : MonoBehaviour
	{

		[Header("Notepad items")]
		[SerializeField]
		private TMP_Text cellDescription = null;
		[SerializeField]
		private TMP_Text cellName = null;
		[SerializeField]
		private Image note1drawing = null;
		[SerializeField]
		private Image note2drawing = null;
		[SerializeField]
		private Image note3drawing = null;
		[SerializeField]
		private GameObject note3 = null;

		internal void SetVisual (CellDescription cd)
		{
			cellDescription.text = cd.description;
			cellName.text = cd.cellname;
			note1drawing.sprite = cd.note1;
			note2drawing.sprite = cd.note2;
			if (cd.note3 == null)
			{
				note3.gameObject.SetActive(false);
			} else
			{
				note3.gameObject.SetActive(true);
				note3drawing.sprite = cd.note3;
			}
		}

	}

}