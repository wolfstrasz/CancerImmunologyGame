using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using ImmunotherapyGame.Player;

namespace ImmunotherapyGame.Tutorials
{
	[RequireComponent(typeof(Collider2D))]
	public class TutorialPopup : MonoBehaviour
	{
		[SerializeField] private bool isVisisble = false;
		[SerializeField] private Color colour;
		[SerializeField] private GameObject visual = null;
		[SerializeField] private SpriteRenderer sprite1 = null;
		[SerializeField] private SpriteRenderer sprite2 = null;
		[SerializeField] [ReadOnly] internal bool wasTriggered = false;

		void Start()
		{
			gameObject.SetActive(false);
			visual.SetActive(false);
			sprite1.color = colour;
			sprite2.color = colour;
		}

		internal void Activate()
		{
			gameObject.SetActive(true);
			visual.SetActive(isVisisble);
			wasTriggered = false;
		}

		private void OnEnable()
		{
			visual.SetActive(isVisisble);
		}

		private void OnTriggerEnter2D(Collider2D collider)
		{
			if (collider.gameObject == PlayerController.Instance.gameObject)
			{
				Debug.Log("PopUpCollision");
				visual.SetActive(false);
				gameObject.SetActive(false);
				wasTriggered = true;
			}
		}
	}
}
