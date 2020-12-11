using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public abstract class AreaOfEffect : MonoBehaviour
{

	private void Update()
	{
		OnEffectStatus();
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		PlayerController pc = collider.gameObject.GetComponent<PlayerController>();
		if (pc != null)
		{
			OnActivation();
		}
	}

	private void OnTriggerExit2D(Collider2D collider)
	{
		PlayerController pc = collider.gameObject.GetComponent<PlayerController>();
		if (pc != null)
		{
			OnDeactivation();
		}
	}

	protected abstract void OnActivation();

	protected abstract void OnDeactivation();

	protected abstract void OnEffectStatus();

}
