using UnityEngine.UI;
using UnityEngine;
using TMPro;


namespace ImmunotherapyGame
{
    [RequireComponent (typeof(Slider))]
    public class NumberedSlider : MonoBehaviour
    {
        private Slider slider = null;
        [SerializeField] private TMP_Text sliderTextValue = null;

		private void Awake()
		{
            slider = GetComponent<Slider>();
            slider.onValueChanged.AddListener(delegate { UpdateScrollbarValue(); });
            sliderTextValue.text = Mathf.FloorToInt(slider.value * 100f).ToString();
        }

		private void UpdateScrollbarValue()
		{
            sliderTextValue.text = Mathf.FloorToInt(slider.value * 100f).ToString();
		}
    }
}
