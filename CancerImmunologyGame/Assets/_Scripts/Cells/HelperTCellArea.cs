using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

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
		if (GlobalGameData.isGameplayPaused) return;
		if (isEffectDeactivated) return;
		PlayerUI.Instance.AddHealth(+0.35f); // Must change to global scriptable object values
		PlayerUI.Instance.AddExhaustion(-0.30f); // Must change to global scriptable object values
	}

}
