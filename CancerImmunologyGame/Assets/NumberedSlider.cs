using UnityEngine.UI;
using UnityEngine;
using TMPro;


namespace ImmunotherapyGame
{
    public class NumberedSlider : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text sliderTextValue = null;
        [SerializeField]
        private Slider slider = null;

        public void UpdateScrollbarValue()
		{
            sliderTextValue.text = Mathf.FloorToInt(slider.value * 100f).ToString();
		}
    }
}
