using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HelperTCellArea : AreaOfEffect
{
	private bool isEffectDeactivated = true;

	protected override void OnActivation()
	{
		isEffectDeactivated = false;
	}

	protected override void OnDeactivation()
	{
		isEffectDeactivated = true;
	}

	protected override void OnEffectStatus()
	{
		if (GlobalGameData.isPaused) return;
		if (isEffectDeactivated) return;
		GlobalGameData.AddHealth(+0.35f); // Must change to global scriptable object values
		GlobalGameData.AddExhaustion(-0.30f); // Must change to global scriptable object values
	}

}
