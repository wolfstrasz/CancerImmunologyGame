using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using ImmunotherapyGame.Core;
using ImmunotherapyGame.UI;

namespace ImmunotherapyGame.CellpediaSystem
{
	
	[RequireComponent (typeof(Selectable))]
	public class PetridishButton : UIMenuNode, ISelectHandler, IDeselectHandler, IPointerEnterHandler, ISubmitHandler, IMoveHandler
	{
		internal static PetridishButton lastSubmitted = null;
		internal static List<PetridishButton> allPetridishButtons = null;
		[ReadOnly]
		private int id = -1;
		[ReadOnly]
		internal bool isActivated = false;
		[ReadOnly]
		internal CellpediaObject cellObject = null;

		[Header("Petridish")]
		[SerializeField]
		private Image cellImage = null;

		[Header("Scaling On Select")]
		[SerializeField]
		private Vector3 scaling = new Vector3(1.0f, 1.0f, 1.0f);
		[SerializeField]
		private Vector3 initialScaling = new Vector3(1.0f, 1.0f, 1.0f);

		internal void Initialise(CellpediaObject cellObject, int id)
		{
			this.id = id;
			this.cellObject = cellObject;
			cellImage.sprite = cellObject.sprite;
			cellImage.color = Color.black;
			transform.localScale = initialScaling;
			Deactivate();
		}

		internal void Activate()
		{
			isActivated = true;
			cellImage.color = Color.white;
		}

		internal void Deactivate()
		{
			isActivated = false;
			cellImage.color = Color.black;
		}

		internal void SubmitCellData()
		{
			if (lastSubmitted != this)
			{
				bool success = Cellpedia.Instance.microscope.NextPetridish(cellObject);
				if (success)
				{

					if (lastSubmitted != null)
					{
						lastSubmitted.cellImage.gameObject.SetActive(true);
					}

					cellImage.gameObject.SetActive(false);
					lastSubmitted = this;
				}
			} else
			{
				cellImage.gameObject.SetActive(false);
			}
		}

		// BUTTON FUNCTIONALITY
		
		// When selected.
		public override void OnSelect(BaseEventData eventData)
		{
			if (!isActivated) return;
			base.OnSelect(eventData);
			gameObject.transform.localScale = scaling;
		}

		public override void OnDeselect(BaseEventData eventData)
		{
			base.OnDeselect(eventData);
			if (!isActivated) return;
			gameObject.transform.localScale = initialScaling;
		}

		public override void OnPointerEnter(PointerEventData eventData)
		{
			if (!isActivated) return;
			base.OnPointerEnter(eventData);
		}

		public void OnSubmit(BaseEventData eventData)
		{
			if (!isActivated || lastSubmitted == this) return;
			SubmitCellData();
		}

		public void OnMove(AxisEventData eventData)
		{
			if (eventData.moveDir == MoveDirection.Left)
			{
				int count = allPetridishButtons.Count;
				int searchId = id - 1;

				if (searchId < 0)
					searchId = count - 1;

				PetridishButton buttonToSelect = allPetridishButtons[searchId];

				while (!buttonToSelect.isActivated && buttonToSelect != this)
				{
					--searchId;
					if (searchId < 0)
					{
						searchId = count - 1;
					}
					buttonToSelect = allPetridishButtons[searchId];
				}

				if (buttonToSelect != this)
				{
					EventSystem.current.SetSelectedGameObject(buttonToSelect.gameObject);
				}
			}
			else if (eventData.moveDir == MoveDirection.Right)
			{
				int count = allPetridishButtons.Count;
				int searchId = id + 1;

				if (searchId >= count)
					searchId = 0;

				PetridishButton buttonToSelect = allPetridishButtons[searchId];

				while (!buttonToSelect.isActivated && buttonToSelect != this)
				{
					++searchId;
					if (searchId >= count)
					{
						searchId = 0;
					}
					buttonToSelect = allPetridishButtons[searchId];
				}

				if (buttonToSelect != this)
				{
					EventSystem.current.SetSelectedGameObject(buttonToSelect.gameObject);
				}
			}
		}

		protected override void OnPointerLeftClick(PointerEventData eventData)
		{
			if (!isActivated || lastSubmitted == this) return;
			SubmitCellData();
		}
	}
}