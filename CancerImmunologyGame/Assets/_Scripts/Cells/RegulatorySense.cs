using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegulatorySense : AreaOfEffect
{
	public RegulatoryCell rc = null;

	protected override void OnActivation()
	{
		Debug.Log("Regulatory sensed player coming");
		rc.isShooting = true;
		rc.StartShooting();
	}

	protected override void OnDeactivation()
	{
		rc.isShooting = false;
		rc.StopShooting();
		Debug.Log("Regulatory sensed player leaving");
	}

	protected override void OnEffectStatus()
	{
		return;
	}
}
