using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ResourceBar : MonoBehaviour
{
	protected static string additionalText = "%";
	[SerializeField]
	protected Slider slider;
	[SerializeField]
	protected TMP_Text valueText;

	public void SetMaxValue(float maxValue)
	{
		slider.maxValue = maxValue;
		valueText.text = Mathf.Ceil(slider.value / maxValue * 100.0f).ToString() + additionalText;
	}

	public void SetValue(float value)
	{
		slider.value = value;
		valueText.text = Mathf.Ceil(value / slider.maxValue * 100.0f).ToString() + additionalText;
	}
}
