using UnityEngine;
using ImmunotherapyGame.Player;

namespace ImmunotherapyGame.Audio
{
	public class BackgroundMusicPlayer : MonoBehaviour
	{

		[SerializeField]
		private BackgroundMusic.BackgroundMusicType type = BackgroundMusic.BackgroundMusicType.NORMAL;

		void OnTriggerEnter2D(Collider2D collider)
		{
			if (collider.GetComponent<PlayerController>() != null)
			{
				BackgroundMusic.Instance.PlayMusic(type);
			}
		}
	}
}