using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellHealthBar : MonoBehaviour
{

	[SerializeField]
	private Slider slider;
	[SerializeField]
	private bool autoHide = false;
	[SerializeField]
	private float autoHideScale = 1.0f;
	[SerializeField]
	private Image image = null;

	public float MaxHealth
	{
		get => slider.maxValue;
		set => slider.maxValue = value;
	}

	public float Health { 
		get => slider.value;
		set
		{
			slider.value = value;
			if (autoHide)
			{
				image.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
			}

		}

	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			Health -= 1.0f;
		}

		if (autoHide)
		{
			float alpha = image.color.a;
			if (alpha > 0.0f)
			{
				alpha -= autoHideScale * Time.deltaTime;
				image.color = new Color(1.0f, 1.0f, 1.0f, alpha);
			}
		}
	}


}
