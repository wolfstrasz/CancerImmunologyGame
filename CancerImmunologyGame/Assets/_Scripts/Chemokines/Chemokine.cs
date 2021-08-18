using UnityEngine;

namespace ImmunotherapyGame.Chemokines
{
	public class Chemokine : MonoBehaviour
	{
		[SerializeField] private new AudioSource audio = null;
		[SerializeField] private SpriteRenderer render = null;
		[SerializeField] private GameObject glow = null;
		[SerializeField] private Collider2D coll = null;
		[SerializeField] [ReadOnly] private bool isCollected = false;

		private void OnEnable()
		{
			isCollected = false;
			render.enabled = true;
			glow.SetActive(true);
			coll.enabled = true;
		}

		private void Update()
		{
			if (isCollected)
			{
				if (!audio.isPlaying)
				{
					gameObject.SetActive(false);
				}
			}
		}

		private void OnTriggerEnter2D(Collider2D collider)
		{
			audio.Play();
			Hide();
			isCollected = true;
		}

		internal void Hide()
		{
			render.enabled = false;
			glow.SetActive(false);
			coll.enabled = false;
		}
	}
}