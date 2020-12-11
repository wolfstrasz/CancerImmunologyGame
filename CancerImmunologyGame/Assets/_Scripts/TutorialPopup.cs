using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorials {
	public class TutorialPopup : MonoBehaviour
	{

		[SerializeField]
		private GameObject visual = null;

		private TReachObjective owner = null;


		internal void SetAttributes(float size, bool isVisible, TReachObjective _owner)
		{
			transform.localScale = Vector3.one * size;
			visual.SetActive(isVisible);
			owner = _owner;
		}

		private void OnTriggerEnter2D(Collider2D collider)
		{
			if (collider.gameObject == GlobalGameData.player)
			{
				collider.enabled = false;
				gameObject.SetActive(false);
				Debug.Log("PopUpCollision");
				owner.Notify();
			}
		}
	}
}
