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
		[SerializeField] private Image note1drawing = null;
		[SerializeField] private GameObject note2 = null;
 		[SerializeField] private Image note2drawing = null;
		[SerializeField] private GameObject note3 = null;
		[SerializeField] private Image note3drawing = null;


		internal void Initialise()
		{

		}

		internal void OnOpen(CellpediaObject cd)
		{
			SetVisual(cd);
		}

		internal void OnClose()
		{

		}
		
		internal void SetVisual (CellpediaObject cd)
		{
			cellName.text = cd.cellname;
			cellDescription.text = cd.description;

			note1drawing.sprite = cd.noteSprite1;
			note1.SetActive(note1drawing.sprite != null);

			note2drawing.sprite = cd.noteSprite2;
			note2.SetActive(note1drawing.sprite != null);

			note3drawing.sprite = cd.noteSprite3;
			note3.SetActive(note1drawing.sprite != null);
		}

	}

}