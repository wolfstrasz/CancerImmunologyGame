using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ImmunotherapyGame.CellpediaSystem
{
	public class CellpediaNotepad : MonoBehaviour
	{

		[Header("Notepad items")]
		[SerializeField] private TMP_Text cellDescription = null;
		[SerializeField] private TMP_Text cellName = null;
		[SerializeField] private GameObject note1 = null;
		[SerializeField] private Image notedrawing1 = null;
		[SerializeField] private GameObject note2 = null;
 		[SerializeField] private Image notedrawing2 = null;
		[SerializeField] private GameObject note3 = null;
		[SerializeField] private Image notedrawing3 = null;


		internal void OnOpen(CellpediaObject cd)
		{
			SetVisual(cd);
		}
		
		internal void SetVisual (CellpediaObject cd)
		{
			cellName.text = cd.cellname;
			cellDescription.text = cd.description;

			notedrawing1.sprite = cd.noteSprite1;
			note1.SetActive(notedrawing1.sprite != null);

			notedrawing2.sprite = cd.noteSprite2;
			note2.SetActive(notedrawing2.sprite != null);

			notedrawing3.sprite = cd.noteSprite3;
			note3.SetActive(notedrawing3.sprite != null);
		}

	}

}