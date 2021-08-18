using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.UI;
using UnityEngine.SceneManagement;

namespace ImmunotherapyGame.UI
{
    public class InterfaceControlPanel: MonoBehaviour
    {
        /// <summary>
        /// User interfaces with Levels < 0 request Gameplay Pause and with Levels >= 0 request Game Pause
        /// </summary>
        [SerializeField] public int sortLevel;
        [SerializeField] private Canvas canvas = null;
        [SerializeField] private bool shouldCloseOnSceneChange = true;
        [SerializeField][ReadOnly] private bool spamStop = false;
        public UIMenuNode initialControlNode;

        [Header("Cancel listeners")]
        public List<UIMenuNode> nodesToListen = new List<UIMenuNode>();


        // Events
        public delegate void OnOpenInterface();
        public OnOpenInterface onOpenInterface;

        public delegate void OnCloseInterface();
        public OnCloseInterface onCloseInterface;

		public void Start()
		{
            //Debug.Log("Panel level index: " + gameObject.name + " = " + level);
            canvas = gameObject.GetComponent<Canvas>();
            canvas.sortingOrder = sortLevel;
		}

		public void LateUpdate()
		{
            spamStop = false;
		}

		public void Open()
		{
            if (!spamStop)
            {
                Debug.Log(gameObject.name + " requests OPEN");
                InterfaceManager.Instance.RequestOpen(this);
            }
		}

        public void Close()
		{
            if (!spamStop)
            {
                Debug.Log(gameObject.name + " requests CLOSE");
                InterfaceManager.Instance.RequestClose(this);
            }
		}

        public bool IsOpened => gameObject.activeInHierarchy;

		private void OnEnable()
		{
            foreach (var node in nodesToListen)
            {
                node.onCancelCall += delegate { Close(); };
            }
            spamStop = true;

            SceneManager.activeSceneChanged += OnSceneChange;
        }

		private void OnDisable()
		{
            foreach (var node in nodesToListen)
            {
                node.onCancelCall -= delegate { Close(); };
            }
            spamStop = false;
            SceneManager.activeSceneChanged -= OnSceneChange;
        }

        public void OnSceneChange(Scene id1, Scene id2)
		{
            if (shouldCloseOnSceneChange)
			{
                gameObject.SetActive(false);
			}
        }

		// Menu Initial Navigation Controll
		internal InterfaceControlPanel LastLowerInterfacePanel;
    }
}
