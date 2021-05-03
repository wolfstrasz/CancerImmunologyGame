
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

using TMPro;

using ImmunotherapyGame.LevelManagement;

namespace ImmunotherapyGame.UI
{
    public class LevelSelectButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [Header("Menu Button Attributes")]
        [SerializeField]
        private Vector3 scaling = new Vector3(1.0f, 1.0f, 1.0f);
        [SerializeField]
        private GameObject completedOverlay = null;
        [SerializeField]
        private GameObject lockedOverlay = null;
        [SerializeField]
        private TMP_Text text = null;
        [SerializeField]
        private int sceneIndex = 0;

		public void OnPointerClick(PointerEventData eventData)
		{
            Debug.Log("OnClick");
            if (lockedOverlay.activeSelf) return;
            // TODO: Play Audio
            gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            LevelSelectScreen.Instance.Close();
            SceneManager.LoadScene(sceneIndex);
        }

		public void OnPointerEnter(PointerEventData eventData)
		{
            Debug.Log("OnEnter");
            if (lockedOverlay.activeSelf) return;

            // TODO: Play Audio
            gameObject.transform.localScale = scaling;
        }

		public void OnPointerExit(PointerEventData eventData)
		{
            Debug.Log("OnExit");

            gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }

		internal void UpdateData(LevelDataObject data)
		{
            completedOverlay.SetActive(data.isCompleted);
            lockedOverlay.SetActive(data.isLocked);
            sceneIndex = data.sceneIndex;
            text.text = data.levelIndex.ToString();
		}
    }
}
