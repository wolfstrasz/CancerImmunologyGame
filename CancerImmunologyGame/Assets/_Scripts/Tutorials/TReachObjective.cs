using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorials {
	public class TReachObjective : TutorialEvent
	{
		[SerializeField]
		private GameObject popupPrefab = null;
		[SerializeField]
		private float size = 1.0f;
		[SerializeField]
		private bool isVisible = false;

		private TutorialPopup popup = null;
		private bool notifiedFromPopup = false;

		internal void Notify()
		{
			notifiedFromPopup = true;
		}

		protected override void OnEndEvent()
		{
			Destroy(popup);
		}

		protected override void OnStartEvent()
		{
			popup = Instantiate(popupPrefab, this.transform).GetComponent<TutorialPopup>();
			popup.SetAttributes(size, isVisible, this);
		}

		protected override bool OnUpdate()
		{
			return notifiedFromPopup;
		}
	}
}
