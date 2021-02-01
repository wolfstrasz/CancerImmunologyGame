using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using CellpediaUI;

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
		}

		protected override bool OnUpdate()
		{
			return true;
		}


	}
}
