using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ImmunotherapyGame
{
	public class ResourceBar : MonoBehaviour
	{
		protected static string percentCharacter = "%";
		[SerializeField] protected Slider slider;
		[SerializeField] protected TMP_Text valueText;
		[SerializeField] protected Color barColour;
		[SerializeField] protected Image fillImage;
		[SerializeField] protected Image fillBackgroundImage;

		protected void Awake()
		{
			fillImage.color = barColour;
			fillBackgroundImage.color = barColour;
		}

		public void SetColour (Color colour)
		{
			barColour = colour;
			fillImage.color = colour;
			fillBackgroundImage.color = colour;
		}

		public void SetMaxValue(float maxValue)
		{
			slider.maxValue = maxValue;
			valueText.text = Mathf.Ceil(slider.value / maxValue * 100.0f).ToString() + percentCharacter;
		}

		public void SetValue(float value)
		{
			slider.value = value;
			valueText.text = Mathf.Ceil(value / slider.maxValue * 100.0f).ToString() + percentCharacter;
		}

		public void SetInverseValue(float value)
		{
			float inverseValue = slider.maxValue - value;
			slider.value = inverseValue;
			valueText.text = Mathf.Ceil(inverseValue / slider.maxValue * 100.0f).ToString() + percentCharacter;
		}
	}
}