using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class VeinsEndArea : MonoBehaviour
{
	[SerializeField]
	private GameObject arteryStart = null;

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject == GlobalGameData.player)
		{
			GlobalGameData.player.transform.position = arteryStart.transform.position;
		}
	}
}
