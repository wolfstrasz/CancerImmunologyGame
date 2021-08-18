using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.EventSystems;

using ImmunotherapyGame.Core;
using ImmunotherapyGame.GameManagement;

namespace ImmunotherapyGame.UI
{
	public class InterfaceManager : Singleton<InterfaceManager>
	{
		[SerializeField]
		private int LevelRequirementForFullPause = 0;

		private List<InterfaceControlPanel> allOpenedInterfacePanels = new List<InterfaceControlPanel>();
		private InterfaceControlPanel currentInterfaceOpened = null;

		private void OnEnable()
		{
			SceneManager.activeSceneChanged += OnSceneUnloaded;
		}

		public void Initialise()
		{
		}

		private void OnDisable()
		{
			SceneManager.activeSceneChanged -= OnSceneUnloaded;
		}

		private void ClosePanel (InterfaceControlPanel interfacePanelToClose)
		{
			// Debug.Log("Interface Manager: Closing Panel = " + interfacePanelToClose.gameObject.name);
			GameManager.Instance.RequestGameStateUnpause(interfacePanelToClose.gameObject);

			interfacePanelToClose.gameObject.SetActive(false);

			if (interfacePanelToClose.onCloseInterface != null)
			{
				interfacePanelToClose.onCloseInterface();
			}

			interfacePanelToClose.LastLowerInterfacePanel = null;
			
			allOpenedInterfacePanels.Remove(interfacePanelToClose);
		}

		private void OpenPanel (InterfaceControlPanel interfacePanelToOpen, InterfaceControlPanel lowerLevelInterfaceObject)
		{
			if (!interfacePanelToOpen.gameObject.activeInHierarchy)
			{
				//Debug.Log("Interface Manager: Opening Panel = " + interfacePanelToOpen.gameObject.name);
				if (interfacePanelToOpen.sortLevel >= LevelRequirementForFullPause)
					GameManager.Instance.RequestGameStatePause(GameStatePauseRequestType.FULL, interfacePanelToOpen.gameObject);
				else
					GameManager.Instance.RequestGameStatePause(GameStatePauseRequestType.GAMEPLAY, interfacePanelToOpen.gameObject);
				// Open menu
				interfacePanelToOpen.gameObject.SetActive(true);
				if (interfacePanelToOpen.onOpenInterface != null)
					interfacePanelToOpen.onOpenInterface();
			}
			else
			{
				// Debug.Log("Interface Manager: Going back to Panel = " + interfacePanelToOpen.gameObject.name);
			}
		
			// if we are sending a null => do not reasign (functionality is used for going down the menu tree)
			if (lowerLevelInterfaceObject != null) 
				interfacePanelToOpen.LastLowerInterfacePanel = lowerLevelInterfaceObject;

			// Update event system selected node to the new menu's initiliser node
			EventSystem.current.SetSelectedGameObject(null);

			if (interfacePanelToOpen.initialControlNode != null && interfacePanelToOpen.initialControlNode.gameObject != null)
			{
				EventSystem.current.SetSelectedGameObject(interfacePanelToOpen.initialControlNode.gameObject);
				interfacePanelToOpen.initialControlNode.OnSelectView = true;
			}

			// Set the menu to current
			currentInterfaceOpened = interfacePanelToOpen;
			allOpenedInterfacePanels.Add(currentInterfaceOpened);
		}

        public void RequestOpen(InterfaceControlPanel interfaceToOpen)
		{
			// Debug.Log("Interface Manager: " + interfaceToOpen.gameObject.name + " request to open");
			if (currentInterfaceOpened == interfaceToOpen)
			{
				Debug.Log("Interface Manager: Will not open same interface");
				return;
			}

			if (currentInterfaceOpened == null)
			{
				// Debug.Log("Interface Manager: " + "On null");
				// Nothing to reassign
				OpenPanel(interfaceToOpen, null); 
				return;
			}
            
			if (interfaceToOpen.sortLevel < currentInterfaceOpened.sortLevel)
			{
				// Debug.Log("Interface Manager: " + " need higher level ");
				return;
			} 

			// If both are on the same level, close previous one
			if (interfaceToOpen.sortLevel == currentInterfaceOpened.sortLevel)
			{
				// Debug.Log("Interface Manager: " + " equal level");

				// Remember previous menu data
				InterfaceControlPanel lowerInterface = currentInterfaceOpened.LastLowerInterfacePanel;
				
				ClosePanel(currentInterfaceOpened);
				OpenPanel(interfaceToOpen, lowerInterface);
				return;
			}

			if (interfaceToOpen.sortLevel > currentInterfaceOpened.sortLevel)
			{
				// Debug.Log("Interface Manager: " + " higher level");

				OpenPanel(interfaceToOpen, currentInterfaceOpened);
				return;
			}
		}

        public void RequestClose(InterfaceControlPanel interfaceToClose)
		{
			if (currentInterfaceOpened == interfaceToClose)
			{
				// remember lower menu
				InterfaceControlPanel lowerInterface = currentInterfaceOpened.LastLowerInterfacePanel;

				// Close menu
				ClosePanel(currentInterfaceOpened);
				currentInterfaceOpened = null;

				if (lowerInterface != null)
				{
					// Nothing to reassign
					OpenPanel(lowerInterface, null);
				}
			}
		}

		public void OnSceneUnloaded(Scene currentScene, Scene nextScene)
		{
			Debug.Log("SCENE UNLOADED");
			allOpenedInterfacePanels.Clear();
			currentInterfaceOpened = null;
		}

	}
}
