using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using ImmunotherapyGame.Player;

namespace ImmunotherapyGame.Tutorials
{
	[RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
	public class TutorialPopup : MonoBehaviour
	{
		private SpriteRenderer render = null;

		[SerializeField]
		private Light2D spotlight = null;
		[SerializeField]
		private bool isVisisble = false;
		[SerializeField]
		internal bool triggered = false;

		void Start()
		{
			gameObject.SetActive(false);
			render = GetComponent<SpriteRenderer>();
			render.enabled = false;
			spotlight.enabled = false;
		}

		internal void Activate()
		{
			gameObject.SetActive(true);
			//render.enabled = isVisisble;
			spotlight.enabled = isVisisble;
			triggered = false;
		}

		private void OnTriggerEnter2D(Collider2D collider)
		{
			if (collider.gameObject == PlayerController.Instance.gameObject)
			{
				Debug.Log("PopUpCollision");
				render.enabled = false;
				spotlight.enabled = false;
				gameObject.SetActive(false);
				triggered = true;
			}
		}
	}
}
