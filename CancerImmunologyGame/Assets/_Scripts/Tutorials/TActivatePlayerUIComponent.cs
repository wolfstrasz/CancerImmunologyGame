using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace Tutorials
{
	public class TActivatePlayerUIComponent : TutorialEvent
	{
		[Header("Attributes")]
		[SerializeField]
		PlayerUIPanels panel = PlayerUIPanels.IMMUNOTHERAPY;

		protected override void OnEndEvent()
		{
		}

		protected override void OnStartEvent()
		{
			if (panel == PlayerUIPanels.IMMUNOTHERAPY)
			{
				PlayerUI.Instance.ActivateImmunotherapyPanel();
			}
			else if (panel == PlayerUIPanels.PLAYER_INFO)
			{
				PlayerUI.Instance.ActivatePlayerInfoPanel();
			}
			else if (panel == PlayerUIPanels.MICROSCOPE)
			{
				PlayerUI.Instance.ActivateMicroscopePanel();
			}
		}

		protected override bool OnUpdate()
		{
			return true;
		}


	}
}
