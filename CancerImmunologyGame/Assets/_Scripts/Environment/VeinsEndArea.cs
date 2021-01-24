using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

[RequireComponent(typeof(Collider2D))]
public class VeinsEndArea : MonoBehaviour
{
	[SerializeField]
	private GameObject arteryStart = null;

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject == GlobalGameData.player)
		{
			PlayerController.Instance.StartHeartMovement();
			GlobalGameData.player.transform.position = arteryStart.transform.position;
		}
	}
}
