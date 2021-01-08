using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class RegulatoryParticle : CellParticle
{
	[Header("Regulatory Cell Particle Attributes")]
    public float exhaust_dmg = 10.0f;

	protected override void OnReachTarget()
	{
		target.ReceiveExhaustion(exhaust_dmg);
		OnDeathEffect();
	}

}
