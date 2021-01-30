using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class RegulatoryParticle : CellParticle
{
	[Header("Regulatory Cell Particle Attributes")]
    public float energyDmg = -5.0f;

	protected override void OnReachTarget()
	{
		target.AddEnergy(energyDmg);
		OnDeathEffect();
	}

}
