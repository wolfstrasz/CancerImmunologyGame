using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegulatorySense : AreaOfEffect
{

	public RegulatoryCell rc = null;

	public void InitialiseRC()
	{
		rc.gameObject.SetActive(true);
		rc.isShooting = true;
		rc.StartShooting();
		rc.StartMoving();
	}

	protected override void OnActivation()
	{
		Debug.Log("Regulatory sensed player coming");
		rc.isShooting = true;
		rc.StartShooting();
		rc.StartMoving();
	}

	protected override void OnDeactivation()
	{
		rc.isShooting = false;
		rc.StopShooting();
		rc.StopMoving();
		Debug.Log("Regulatory sensed player leaving");
	}

	protected override void OnEffectStatus()
	{
		return;
	}
}
