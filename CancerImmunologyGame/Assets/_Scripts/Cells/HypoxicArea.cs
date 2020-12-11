using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class HypoxicArea : AreaOfEffect
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
		PlayerUI.Instance.AddHealth(-0.02f); // Must change to global scriptable object values
	}

}
