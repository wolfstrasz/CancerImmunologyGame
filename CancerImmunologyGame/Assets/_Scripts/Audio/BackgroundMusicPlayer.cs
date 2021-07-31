using UnityEngine;
using ImmunotherapyGame.Player;

namespace ImmunotherapyGame.Audio
{
	[RequireComponent(typeof(Collider2D))]
	public class BackgroundMusicPlayer : MonoBehaviour
	{
		[SerializeField]
		private MusicType type;
		private bool alreadySubscribed = false;

		private void OnTriggerEnter2D(Collider2D collider)
		{
			if (collider.GetComponent<PlayerController>() != null)
			{
				if (!alreadySubscribed)
				{
					BackgroundMusic.Instance.Subscribe(type);
					alreadySubscribed = true;
				}
			}
		}

		private void OnTriggerExit2D(Collider2D collider)
		{
			if (collider.GetComponent<PlayerController>() != null)
			{
				if (alreadySubscribed)
				{
					BackgroundMusic.Instance.Unsubscribe(type);
					alreadySubscribed = false;
				}
			}
		}

		private void OnDisable()
		{
			if (alreadySubscribed)
			{
				BackgroundMusic.Instance.Unsubscribe(type);
				alreadySubscribed = false;
			}
		}
	}
}