using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using ImmunotherapyGame.Audio;
namespace ImmunotherapyGame.UI
{
	public class UIInitialNavigationController : MonoBehaviour/*, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerClickHandler*/
	{
		[SerializeField]
		private GameObject initialiserGameObject;
		[SerializeField]
		private UIMenuNode initialiserNode;

		private GameObject previouslySelectedGameObject;

		void OnEnable()
		{
			Debug.Log(gameObject + " will request Nav Control");
			if (initialiserGameObject == null)
			{
				Debug.LogError(gameObject + " initial control node does not have a initialiser gameobject. Assign one!");
			} else
			{
				previouslySelectedGameObject = EventSystem.current.currentSelectedGameObject;
				EventSystem.current.SetSelectedGameObject(null);
				EventSystem.current.SetSelectedGameObject(initialiserGameObject);
				initialiserNode.OnSelectView = true;
			}
		}

		void OnDisable()
		{
			if (previouslySelectedGameObject != null && EventSystem.current != null)
			{
				EventSystem.current.SetSelectedGameObject(null);
				EventSystem.current.SetSelectedGameObject(previouslySelectedGameObject);
			}
		}

	}


}
