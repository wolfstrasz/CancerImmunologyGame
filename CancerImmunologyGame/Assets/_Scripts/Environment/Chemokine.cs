using UnityEngine;

namespace ImmunotherapyGame.Chemokines
{
	public class Chemokine : MonoBehaviour
	{

		[SerializeField]
		private new AudioSource audio = null;
		[SerializeField]
		private SpriteRenderer render = null;
		[SerializeField]
		private GameObject glow = null;
		private bool shouldHide = false;

		private void OnEnable()
		{
			shouldHide = false;
		}

		private void Update()
		{
			if (shouldHide)
			{
				if (!audio.isPlaying)
					gameObject.SetActive(false);
			}
		}

		private void OnTriggerEnter2D(Collider2D collider)
		{
			audio.Play();
			render.enabled = false;
			glow.SetActive(false);
			shouldHide = true;
		}

		internal void Remove()
		{
			Destroy(gameObject);
		}

	}
}