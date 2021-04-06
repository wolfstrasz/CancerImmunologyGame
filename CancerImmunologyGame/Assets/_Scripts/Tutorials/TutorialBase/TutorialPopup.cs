using UnityEngine;
using Player;

namespace ImmunotherapyGame.Tutorials
{
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
			if (collider.gameObject == PlayerController.Instance.gameObject)
			{
				gameObject.SetActive(false);
				Debug.Log("PopUpCollision");
				owner.Notify();
			}
		}
	}
}
