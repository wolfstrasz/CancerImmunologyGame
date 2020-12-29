using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperCellParticle : CellParticle
{
	[Header("Helper Cell Particle Attributes")]
	[SerializeField]
	private float healthRegeneration = 0.35f;
	[SerializeField]
	private float exhaustRegeneration = 0.30f;

	protected override void OnReachTarget()
	{
		Debug.Log("Reached targer");
		target.ReceiveHealth(healthRegeneration);
		target.ReceiveExhaustion(-exhaustRegeneration);
		Destroy(gameObject);
	}
}

