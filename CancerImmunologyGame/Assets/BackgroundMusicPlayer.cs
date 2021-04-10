using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.Player;

public class BackgroundMusicPlayer : MonoBehaviour
{

	[SerializeField]
	private BackgroundMusic.BackgroundMusicType type = BackgroundMusic.BackgroundMusicType.NORMAL;

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.GetComponent<PlayerController>()!= null)
		{
			BackgroundMusic.Instance.PlayMusic(type);
		}
	}
}
