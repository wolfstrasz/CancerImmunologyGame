using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DCInteractArea : AreaOfEffect
{
	[SerializeField]
	DendriticCell owner = null;

	protected override void OnActivation()
	{
		owner.StartInteraction();
	}

	protected override void OnDeactivation()
	{
		return;
	}

	protected override void OnEffectStatus()
	{
		return;
	}

}
