using UnityEngine.UI;
using UnityEngine;
using TMPro;


namespace ImmunotherapyGame
{
    public class NumberedScrollbar : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text scrollbarTextValue = null;
        [SerializeField]
        private Scrollbar scrollbar = null;

        public void UpdateScrollbarValue()
		{
            scrollbarTextValue.text = Mathf.FloorToInt(scrollbar.value * 100f).ToString();
		}
    }
}
