using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using ImmunotherapyGame.Core;

namespace ImmunotherapyGame.UI
{
	[RequireComponent(typeof(Selectable))]
	public class UIInitialControlNode : MonoBehaviour/*, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerClickHandler*/
	{
		[SerializeField]
		private GameObject initialiserGameObject;
		[SerializeField]
		private Selectable initialiserSelectable;

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
			}
			//RequestUINavigationControl(this);
		}

		void OnDisable()
		{
			if (previouslySelectedGameObject != null)
			{
				EventSystem.current.SetSelectedGameObject(null);
				EventSystem.current.SetSelectedGameObject(previouslySelectedGameObject);
				EventSystem.current.gameObject.GetComponent<Selectable>(); //.OnSelect(new BaseEventData(EventSystem.current));
			}
			//FreeUINavigationControl(this);
		}

	}

}
