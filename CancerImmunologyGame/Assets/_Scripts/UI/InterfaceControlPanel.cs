using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.UI;

namespace ImmunotherapyGame.UI
{
    public class InterfaceControlPanel: MonoBehaviour
    {
        /// <summary>
        /// User interfaces with Levels < 0 request Gameplay Pause and with Levels >= 0 request Game Pause
        /// </summary>
        public int level;
        [SerializeField]
        private bool shouldCloseOnSceneChange = true;
        public UIMenuNode initialControlNode;

        public delegate void OnOpenInterface();
        public OnOpenInterface onOpenInterface;

        public delegate void OnCloseInterface();
        public OnCloseInterface onCloseInterface;

        [Header("Cancel listeners")]
        public List<UIMenuNode> nodesToListen = new List<UIMenuNode>();
        [SerializeField] private Canvas canvas = null;

		public void Start()
		{
            Debug.Log("Panel level index: " + gameObject.name + " = " + level);
            canvas = gameObject.GetComponent<Canvas>();
            canvas.sortingOrder = level;
		}

		public void Open()
		{
            Debug.Log(gameObject.name + " requests OPEN");
            InterfaceManager.Instance.RequestOpen(this);
		}

        public void Close()
		{
            Debug.Log(gameObject.name + " requests CLOSE");
            InterfaceManager.Instance.RequestClose(this);
		}

        public bool IsOpened => gameObject.activeInHierarchy;

		private void OnEnable()
		{
            foreach (var node in nodesToListen)
            {
                node.onCancelCall += delegate { Close(); };
            }
        }

		private void OnDisable()
		{
            foreach (var node in nodesToListen)
            {
                node.onCancelCall -= delegate { Close(); };
            }
            if (shouldCloseOnSceneChange)
			{
                gameObject.SetActive(false);
			}
        }


		// Menu Initial Navigation Controll
		internal InterfaceControlPanel LastLowerInterfacePanel;
    }
}
