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

		private void OnTriggerEnter2D(Collider2D collider)
		{

			audio.Play();
			render.enabled = false;
			glow.SetActive(false);
		}

		internal void Remove()
		{
			Destroy(this.gameObject);
		}

	}
}