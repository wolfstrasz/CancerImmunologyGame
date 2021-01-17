using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Cellpedia : Singleton<Cellpedia>
{

	[SerializeField]
	List<Petridish> petridishes = new List<Petridish>();
	[SerializeField]
	Button rotatorBtn = null;
	bool rotatorLocked = false;

	[SerializeField]
	List<string> texts = new List<string>();
	int textIndex = 0;

	[SerializeField]
	TMP_Text text = null;

	public void Close()
	{
		gameObject.SetActive(false);
	}

	public void Open()
	{
		gameObject.SetActive(true);
	}

	public void NextPetridish()
	{
		if (!rotatorLocked)
		{
			rotatorLocked = true;
			foreach (Petridish dish in petridishes)
			{
				dish.ShiftLeft();
			}
			StartCoroutine(WaitToUnlock());

		}
	}

	IEnumerator WaitToUnlock()
	{
		yield return new WaitForSeconds(2.2f);

		textIndex++;
		textIndex %= texts.Count;
		text.text = texts[textIndex];

		rotatorLocked = false;
	}


}
