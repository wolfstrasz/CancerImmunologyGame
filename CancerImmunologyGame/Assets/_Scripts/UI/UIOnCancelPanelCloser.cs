using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ImmunotherapyGame.UI
{
    public class UIOnCancelPanelCloser : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> panelsToClose = new List<GameObject>();
        [SerializeField]
        private List<UIMenuNode> nodesToListen = new List<UIMenuNode>();

		private void OnEnable()
		{
			foreach (var node in nodesToListen)
			{
				node.onCancelCall += delegate { ClosePanels(); };
			}
		}

		private void OnDisable()
		{
			foreach (var node in nodesToListen)
			{
				node.onCancelCall -= delegate { ClosePanels(); };
			}
		}

		public void ClosePanels()
		{
			foreach (var panel in panelsToClose)
			{
				panel.SetActive(false);
			}
		}
	}
}
