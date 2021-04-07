using UnityEngine;

namespace ImmunotherapyGame.Tutorials
{
	[RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
	public class TutorialPopup : MonoBehaviour
	{
		private SpriteRenderer render = null;

		[SerializeField]
		private bool isVisisble = false;
		internal bool triggered = false;


		void Start()
		{
			gameObject.SetActive(false);
			render = GetComponent<SpriteRenderer>();
		}

		internal void Activate()
		{
			gameObject.SetActive(true);
			render.enabled = isVisisble;
			triggered = false;
		}

		private void OnTriggerEnter2D(Collider2D collider)
		{
			Debug.Log("PopUpCollision");
			render.enabled = false;
			gameObject.SetActive(false);
			triggered = true;
		}
	}
}
